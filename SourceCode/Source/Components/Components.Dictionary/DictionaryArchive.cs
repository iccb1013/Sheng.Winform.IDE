using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using Microsoft.Practices.Composite.Events;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Kernal;
using Microsoft.Practices.Unity;

namespace Sheng.SailingEase.Components.DictionaryComponent
{
    class DictionaryArchive
    {
        #region XPATH

        /// <summary>
        /// Index/Dictionary
        /// 索引文件中的Dictionary节点
        /// </summary>
        const string XPATH_Index_Dictionary = "/Dictionary";

        /// <summary>
        ///  Index/Dictionary/Entity[@Id='{0}']
        ///  从索引文件中选出指定实体索引
        /// </summary>
        const string XPATH_Index_SelectDictionary = "/Dictionary/Entity[@Id='{0}']";

        /// <summary>
        ///  Index/DataEntity/Entity[@Code='{0}']
        ///  从索引文件中选出指定实体索引 ，根据code，忽略code大小写
        /// </summary>
        const string XPATH_Index_SelectDictionary_ByCode_IgnoreCase =
            "/Dictionary/Entity[translate(@Code,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')=translate('{0}','ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')]";

        /// <summary>
        /// Index/Dictionary/Entity
        /// 从索引文件中选出所有Entity节点
        /// </summary>
        const string XPATH_Index_Dictionary_Entity = "/Dictionary/Entity";

        /// <summary>
        /// Entity/Items/Item[@Id='{0}']
        ///  从数据实体XML中选出指定的数据项
        /// </summary>
        const string XPATH_Dictionary_SelectItem = "/Items/Item[@Id='{0}']";

        const string XPATH_Dictionary_SelectItem_ByValue = "/Items/Item[@Value='{0}']";

        /// <summary>
        /// Entity/Items
        /// 数据实体XML中的Items节点
        /// </summary>
        const string XPATH_Dictionary_Items = "/Items";


        #endregion

        #region 私有成员

        ICachingService _cachingService = ServiceUnity.CachingService;
        IUnityContainer _container = ServiceUnity.Container;
        IEventAggregator _eventAggregator = ServiceUnity.Container.Resolve<IEventAggregator>();
        IPackageService _packageService = ServiceUnity.Container.Resolve<IPackageService>();

        /// <summary>
        /// 数据实体索引XML
        /// </summary>
        XElement _indexXml;

        #endregion

        #region 公开属性

        private static InstanceLazy<DictionaryArchive> _instance =
           new InstanceLazy<DictionaryArchive>(() => new DictionaryArchive());
        public static DictionaryArchive Instance
        {
            get { return _instance.Value; }
        }

        #endregion

        #region 构造

        private DictionaryArchive()
        {
            SubscribeEvent();
        }

        #endregion

        #region 公开方法

        #region 枚举

        /// <summary>
        /// 添加一个枚举
        /// </summary>
        /// <param name="enumEntity"></param>
        public void Add(EnumEntityDev enumEntity)
        {
            Debug.Assert(enumEntity != null, "enumEntity 为 null");

            if (enumEntity == null)
                return;

            //添加索引信息
            _indexXml.XPathSelectElement(XPATH_Index_Dictionary).Add(ArchiveHelper.GetEntityArchiveIndex(enumEntity));

            SaveIndexFile();

            string xml = enumEntity.ToXml();
            XElement xElement = XElement.Parse(xml);

            //添加数据实体文件
            _packageService.Current.AddFileContent(xml, Path.Combine(Constant.PACKAGE_DICTIONARY_FOLDER, enumEntity.Id));

            _cachingService.Add(enumEntity.Id, xElement);

            //发布事件
            EnumEventArgs args = new EnumEventArgs(enumEntity);
            _eventAggregator.GetEvent<EnumAddedEvent>().Publish(args);
        }

        /// <summary>
        /// 更新枚举
        /// </summary>
        /// <param name="dataEntity"></param>
        public void Update(EnumEntityDev enumEntity)
        {
            Debug.Assert(enumEntity != null, "enumEntity 为 null");

            if (enumEntity == null)
                return;

            //更新索引信息
            XElement element = _indexXml.XPathSelectElement(String.Format(XPATH_Index_SelectDictionary, enumEntity.Id));
            Debug.Assert(element != null, "更新枚举索引时未找到指定枚举的索引记录");
            if (element != null)
            {
                element.ReplaceWith(ArchiveHelper.GetEntityArchiveIndex(enumEntity));
                SaveIndexFile();

                string xml = enumEntity.ToXml();
                XElement xElement = XElement.Parse(xml);

                //更新数据实体文件
                _packageService.Current.AddFileContent(xml, Path.Combine(Constant.PACKAGE_DICTIONARY_FOLDER, enumEntity.Id));

                _cachingService.Add(enumEntity.Id, xElement);

                //发布事件
                EnumEventArgs args = new EnumEventArgs(enumEntity);
                _eventAggregator.GetEvent<EnumUpdatedEvent>().Publish(args);
            }
        }

        //TODO:更名为remove
        /// <summary>
        /// 删除枚举
        /// </summary>
        /// <param name="id"></param>
        public void Delete(EnumEntityDev enumEntity)
        {
            Debug.Assert(enumEntity != null, "enumEntity 为 null");

            if (enumEntity == null)
                return;

            //移除索引信息
            XElement element = _indexXml.XPathSelectElement(String.Format(XPATH_Index_SelectDictionary, enumEntity.Id));
            Debug.Assert(element != null, "删除枚举索引时未找到指定枚举的索引记录");
            if (element != null)
            {
                element.Remove();
                SaveIndexFile();

                //移除实体文件
                _packageService.Current.DeleteFile(Path.Combine(Constant.PACKAGE_DICTIONARY_FOLDER, enumEntity.Id));

                _cachingService.Remove(enumEntity.Id);

                //发布事件
                EnumEventArgs args = new EnumEventArgs(enumEntity);
                _eventAggregator.GetEvent<EnumRemovedEvent>().Publish(args);
            }
        }

        /// <summary>
        /// 提交
        /// 根据enumEntity的Id进行判断，如果已存在，调用update，如果不存在，调用add
        /// </summary>
        /// <param name="dataEntity"></param>
        public void Commit(EnumEntityDev enumEntity)
        {
            Debug.Assert(enumEntity != null, "enumEntity 为 null");

            if (enumEntity == null)
                return;

            if (EntityExistById(enumEntity.Id))
            {
                Update(enumEntity);
            }
            else
            {
                Add(enumEntity);
            }
        }

        /// <summary>
        /// 获取枚举
        /// 可能返回 null
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EnumEntity GetEnumEntity(string id)
        {
            //首先尝试从缓存中获取
            XElement cachingElement = (XElement)_cachingService.GetData(id);
            string strEntity;
            if (cachingElement == null)
            {
                string file = Path.Combine(Constant.PACKAGE_DICTIONARY_FOLDER, id);
                bool fileExist = _packageService.Current.Container(file);

                Debug.Assert(fileExist, "枚举文件不存在");
                if (fileExist == false)
                    return null;

                strEntity = _packageService.Current.GetFileContent(file);
                XElement entityElement = XElement.Parse(strEntity);
                _cachingService.Add(id, entityElement);
            }
            else
            {
                strEntity = cachingElement.ToString();
            }

            EnumEntityDev enumEntity = new EnumEntityDev();
            enumEntity.FromXml(strEntity);

            return enumEntity;
        }

        /// <summary>
        /// 获取枚举列表
        /// </summary>
        /// <returns></returns>
        public List<EnumEntity> GetEnumEntityList()
        {
            XElement[] entityElements = _indexXml.XPathSelectElements(XPATH_Index_Dictionary_Entity).ToArray();

            List<EnumEntity> list = new List<EnumEntity>();

            foreach (XElement element in entityElements)
            {
                EnumEntity entity = GetEnumEntity(element.Attribute("Id").Value);
                if (entity != null)
                    list.Add(entity);
            }

            return list;
        }

        #endregion

        #region 枚举项

        /// <summary>
        /// 添加一个枚举项
        /// </summary>
        /// <param name="dataItemEntity"></param>
        /// <param name="parentId"></param>
        public void AddItem(EnumItemEntityDev enumItemEntity)
        {
            if (enumItemEntity == null || enumItemEntity.Owner == null)
            {
                Debug.Assert(false, "参数异常");
                return;
            }

            string enumId = enumItemEntity.Owner.Id;

            //判断指定的枚举是否存在
            bool entityExist = EntityExistById(enumId);

            Debug.Assert(entityExist, "枚举不存在");
            if (entityExist == false)
                return;

            string file = Path.Combine(Constant.PACKAGE_DICTIONARY_FOLDER, enumId);

            //在数据实体XML中加上数据项节点
            //首先尝试从缓存中获取
            XElement entityElement = (XElement)_cachingService.GetData(enumId);
            string strEntity;
            if (entityElement == null)
            {
                //拿枚举文件
                bool fileExist = _packageService.Current.Container(file);
                Debug.Assert(fileExist, "枚举文件不存在");
                if (fileExist == false)
                    return;

                strEntity = _packageService.Current.GetFileContent(file);
                entityElement = XElement.Parse(strEntity);
                _cachingService.Add(enumId, entityElement);
            }

            entityElement.XPathSelectElement(XPATH_Dictionary_Items).Add(XElement.Parse(enumItemEntity.ToXml()));

            //保存文件
            _packageService.Current.AddFileContent(entityElement.ToString(),file);

            if (enumItemEntity.Owner.Items.Contains(enumItemEntity) == false)
                enumItemEntity.Owner.Items.Add(enumItemEntity);

            //发布事件
            EnumItemEventArgs args = new EnumItemEventArgs(enumItemEntity);
            _eventAggregator.GetEvent<EnumItemEntityAddedEvent>().Publish(args);
        }

        //public EnumItemEntityDev GetEnumItemEntity(string id)
        /// <summary>
        /// 获取枚举项
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EnumItemEntityDev GetItemEntity(string id, EnumEntityDev enumEntity)
        {
            if (String.IsNullOrEmpty(id) || enumEntity == null)
            {
                Debug.Assert(false, "参数异常");
                return null;
            }

            string enumId = enumEntity.Id;

            //判断指定的数据实体是否存在
            bool entityExist = EntityExistById(enumId);

            Debug.Assert(entityExist, "枚举不存在");
            if (entityExist == false)
                return null;

            //在数据实体XML中拿出数据项节点
            XElement entityElement = (XElement)_cachingService.GetData(enumId);
            string strEntity;
            if (entityElement == null)
            {
                //拿数据实体文件
                string file = Path.Combine(Constant.PACKAGE_DICTIONARY_FOLDER, enumId);
                bool fileExist = _packageService.Current.Container(file);

                Debug.Assert(fileExist, "枚举文件不存在");
                if (fileExist == false)
                    return null;

                strEntity = _packageService.Current.GetFileContent(file);
                entityElement = XElement.Parse(strEntity);
                _cachingService.Add(enumId, entityElement);
            }

            XElement itemElement = entityElement.XPathSelectElement(String.Format(XPATH_Dictionary_SelectItem, id));

            if (itemElement == null)
                return null;

            EnumItemEntityDev dataItemEntity = new EnumItemEntityDev(enumEntity);
            dataItemEntity.FromXml(itemElement.ToString());
            return dataItemEntity;
        }

        /// <summary>
        /// 更新数据项
        /// </summary>
        /// <param name="dataItemEntity"></param>
        public void UpdateItem(EnumItemEntityDev enumItemEntity)
        {
            if (enumItemEntity == null || enumItemEntity.Owner == null)
            {
                Debug.Assert(false, "参数异常");
                return;
            }

            string enumId = enumItemEntity.Owner.Id;

            //判断指定的数据实体是否存在
            bool entityExist = EntityExistById(enumId);

            Debug.Assert(entityExist, "枚举不存在");
            if (entityExist == false)
                return;

            string file = Path.Combine(Constant.PACKAGE_DICTIONARY_FOLDER, enumId);

            //在枚举XML中更新数据项节点
            XElement entityElement = (XElement)_cachingService.GetData(enumId);
            string strEntity;
            if (entityElement == null)
            {
                //拿数据实体文件
                bool fileExist = _packageService.Current.Container(file);
                Debug.Assert(fileExist, "枚举文件不存在");
                if (fileExist == false)
                    return;

                strEntity = _packageService.Current.GetFileContent(file);
                entityElement = XElement.Parse(strEntity);
                _cachingService.Add(enumId, entityElement);
            }

            entityElement.XPathSelectElement(String.Format(XPATH_Dictionary_SelectItem, enumItemEntity.Id)).ReplaceWith(XElement.Parse(enumItemEntity.ToXml()));

            //保存文件
            _packageService.Current.AddFileContent(entityElement.ToString(), file);

            //发布事件
            EnumItemEventArgs args = new EnumItemEventArgs(enumItemEntity);
            _eventAggregator.GetEvent<EnumItemEntityUpdatedEvent>().Publish(args);
        }

        //TODO:改成remove
        /// <summary>
        /// 删除枚举项
        /// </summary>
        /// <param name="id"></param>
        public void DeleteItem(EnumItemEntityDev enumItemEntity)
        {
            if (enumItemEntity == null || enumItemEntity.Owner == null)
            {
                Debug.Assert(false, "参数异常");
                return;
            }

            string enumId = enumItemEntity.Owner.Id;

            //判断指定的数据实体是否存在
            bool entityExist = EntityExistById(enumId);

            Debug.Assert(entityExist, "枚举不存在");
            if (entityExist == false)
                return;

            string file = Path.Combine(Constant.PACKAGE_DICTIONARY_FOLDER, enumId);

            //在数据实体XML中删除数据项节点
            XElement entityElement = (XElement)_cachingService.GetData(enumId);
            string strEntity;
            if (entityElement == null)
            {
                //拿数据实体文件
                bool fileExist = _packageService.Current.Container(file);
                Debug.Assert(fileExist, "枚举文件不存在");
                if (fileExist == false)
                    return;

                strEntity = _packageService.Current.GetFileContent(file);
                entityElement = XElement.Parse(strEntity);
                _cachingService.Add(enumId, entityElement);
            }

            entityElement.XPathSelectElement(String.Format(XPATH_Dictionary_SelectItem, enumItemEntity.Id)).Remove();

            //保存文件
            _packageService.Current.AddFileContent(entityElement.ToString(),file);

            if (enumItemEntity.Owner.Items.Contains(enumItemEntity))
                enumItemEntity.Owner.Items.Remove(enumItemEntity);

            //发布事件
            EnumItemEventArgs args = new EnumItemEventArgs(enumItemEntity);
            _eventAggregator.GetEvent<EnumItemEntityRemovedEvent>().Publish(args);
        }

        public void Commit(EnumItemEntityDev enumItemEntity)
        {
            if (enumItemEntity == null || enumItemEntity.Owner == null)
            {
                Debug.Assert(false, "参数异常");
                return;
            }

            if (ItemEntityExistById(enumItemEntity.Id, enumItemEntity.Owner.Id))
            {
                UpdateItem(enumItemEntity);
            }
            else
            {
                AddItem(enumItemEntity);
            }
        }

        /// <summary>
        /// 将指定的枚举项移动到另一个枚举项之前
        /// </summary>
        /// <param name="code"></param>
        /// <param name="beforeCode"></param>
        public void MoveItemUp(string enumId, string id, string beforeId)
        {
            if (String.IsNullOrEmpty(enumId) || String.IsNullOrEmpty(id) || String.IsNullOrEmpty(beforeId))
            {
                Debug.Assert(false, "参数异常");
                return;
            }

            //判断指定的枚举是否存在
            bool entityExist = EntityExistById(enumId);

            Debug.Assert(entityExist, "枚举不存在");
            if (entityExist == false)
                return;

            string file = Path.Combine(Constant.PACKAGE_DICTIONARY_FOLDER, enumId);

            //在数据实体XML中删除数据项节点
            XElement entityElement = (XElement)_cachingService.GetData(enumId);
            string strEntity;
            if (entityElement == null)
            {
                //拿枚举文件
                bool fileExist = _packageService.Current.Container(file);
                Debug.Assert(fileExist, "枚举文件不存在");
                if (fileExist == false)
                    return;

                strEntity = _packageService.Current.GetFileContent(file);
                entityElement = XElement.Parse(strEntity);
                _cachingService.Add(enumId, entityElement);
            }

            //移动枚举项节点
            XElement elementItem = entityElement.XPathSelectElement(String.Format(XPATH_Dictionary_SelectItem, id));
            elementItem.Remove();

            entityElement.XPathSelectElement(String.Format(XPATH_Dictionary_SelectItem, beforeId)).AddBeforeSelf(elementItem);

            //保存文件
            _packageService.Current.AddFileContent(entityElement.ToString(), file);

            //发布事件
            EnumItemMoveAlongEventArgs args = new EnumItemMoveAlongEventArgs(enumId, id, beforeId);
            _eventAggregator.GetEvent<EnumItemMoveAlongEvent>().Publish(args);
        }

        /// <summary>
        /// 将指定的枚举项移动到另一个枚举项之后
        /// </summary>
        /// <param name="code"></param>
        /// <param name="afterCode"></param>
        public void MoveItemDown(string enumId, string id, string afterId)
        {
            if (String.IsNullOrEmpty(enumId) || String.IsNullOrEmpty(id) || String.IsNullOrEmpty(afterId))
            {
                Debug.Assert(false, "参数异常");
                return;
            }

            //判断指定的枚举是否存在
            bool entityExist = EntityExistById(enumId);

            Debug.Assert(entityExist, "枚举不存在");
            if (entityExist == false)
                return;

            string file = Path.Combine(Constant.PACKAGE_DICTIONARY_FOLDER, enumId);
            
            //在数据实体XML中删除数据项节点
            XElement entityElement = (XElement)_cachingService.GetData(enumId);
            string strEntity;
            if (entityElement == null)
            {
                //拿枚举文件
                bool fileExist = _packageService.Current.Container(file);
                Debug.Assert(fileExist, "枚举文件不存在");
                if (fileExist == false)
                    return;

                strEntity = _packageService.Current.GetFileContent(file);
                entityElement = XElement.Parse(strEntity);
                _cachingService.Add(enumId, entityElement);
            }

            //移动枚举项节点
            XElement elementItem = entityElement.XPathSelectElement(String.Format(XPATH_Dictionary_SelectItem, id));
            elementItem.Remove();

            entityElement.XPathSelectElement(String.Format(XPATH_Dictionary_SelectItem, afterId)).AddAfterSelf(elementItem);

            //保存文件
            _packageService.Current.AddFileContent(entityElement.ToString(), file);

            //发布事件
            EnumItemMoveBackEventArgs args = new EnumItemMoveBackEventArgs(enumId, id, afterId);
            _eventAggregator.GetEvent<EnumItemMoveBackEvent>().Publish(args);
        }

        #endregion

        /// <summary>
        /// 检查指定的枚举是否存在by code
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckExistByCode(string code)
        {
            Debug.Assert(String.IsNullOrEmpty(code) == false, "code 为 空 或为 Null");

            XElement element = _indexXml.XPathSelectElement(
                String.Format(XPATH_Index_SelectDictionary_ByCode_IgnoreCase, code));
            if (element == null)
                return false;
            else
                return true;
        }

        /// <summary>
        /// 检查指定的枚举值是否存在(在指定的枚举范围内)
        /// 区分大小写
        /// </summary>
        /// <param name="columnId"></param>
        /// <returns></returns>
        public bool CheckEnumItemExist(string enumId, string enumItemValue)
        {
            if (String.IsNullOrEmpty(enumId) || String.IsNullOrEmpty(enumItemValue))
            {
                Debug.Assert(false, "参数异常");
                return false;
            }

            //判断指定的枚举是否存在
            bool entityExist = EntityExistById(enumId);

            Debug.Assert(entityExist, "枚举不存在");
            if (entityExist == false)
                return false;

            //在数据实体XML中删除数据项节点
            XElement entityElement = (XElement)_cachingService.GetData(enumId);
            string strEntity;
            if (entityElement == null)
            {
                //拿枚举文件
                string file = Path.Combine(Constant.PACKAGE_DICTIONARY_FOLDER, enumId);
                bool fileExist = _packageService.Current.Container(file);

                Debug.Assert(fileExist, "枚举文件不存在");
                if (fileExist == false)
                    return false;

                strEntity = _packageService.Current.GetFileContent(file);
                entityElement = XElement.Parse(strEntity);
                _cachingService.Add(enumId, entityElement);
            }

            XElement element = entityElement.XPathSelectElement(
               String.Format(XPATH_Dictionary_SelectItem_ByValue, enumItemValue));
            if (element == null)
                return false;
            else
                return true;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 事件订阅
        /// </summary>
        private void SubscribeEvent()
        {
            _eventAggregator.GetEvent<ProjectOpenedEvent>().Subscribe((e) =>
            {
                //初始化 IndexXml
                if (_packageService.Current.Container(Constant.PACKAGE_DICTIONARY_INDEX_FILE))
                {
                    //读取当前数据实体索引信息
                    string strIndex = _packageService.Current.GetFileContent(Constant.PACKAGE_DICTIONARY_INDEX_FILE);
                    _indexXml = XElement.Parse(strIndex);
                }
                else
                {
                    //如果当前不存在索引文件，创建一个
                    _indexXml = new XElement("Index", new XElement("Dictionary"));
                    SaveIndexFile();
                }
            });

            _eventAggregator.GetEvent<ProjectClosedEvent>().Subscribe((e) =>
            {
                //释放 IndexXml
                _indexXml = null;
            });
        }

        /// <summary>
        /// 保存索引文件
        /// </summary>
        private void SaveIndexFile()
        {
            Debug.Assert(_indexXml != null, "IndexXml 为 null");

            if (_indexXml != null)
            {
                _packageService.Current.AddFileContent(_indexXml.ToString(), Constant.PACKAGE_DICTIONARY_INDEX_FILE);
            }
        }

        /// <summary>
        /// 根据Id判断枚举是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool EntityExistById(string id)
        {
            Debug.Assert(String.IsNullOrEmpty(id) == false, "id 为 空 或为 Null");

            XElement element = _indexXml.XPathSelectElement(String.Format(XPATH_Index_SelectDictionary, id));
            if (element == null)
                return false;
            else
                return true;
        }

        /// <summary>
        /// 判断指定id的枚举项是否存在,by id
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        private bool ItemEntityExistById(string id, string entityId)
        {
            //在数据实体XML中删除数据项节点
            XElement entityElement = (XElement)_cachingService.GetData(entityId);
            string strEntity;
            if (entityElement == null)
            {
                string file = Path.Combine(Constant.PACKAGE_DICTIONARY_FOLDER, entityId);
                bool fileExist = _packageService.Current.Container(file);

                Debug.Assert(fileExist, "数据实体文件不存在");
                if (fileExist == false)
                    return false;

                strEntity = _packageService.Current.GetFileContent(file);
                entityElement = XElement.Parse(strEntity);
                _cachingService.Add(entityId, entityElement);
            }

            XElement itemElement = entityElement.XPathSelectElement(String.Format(XPATH_Dictionary_SelectItem, id));
            if (itemElement == null)
                return false;
            else
                return true;
        }

        #endregion
    }
}
