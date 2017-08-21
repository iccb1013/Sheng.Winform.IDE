using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Unity;
using Sheng.SailingEase.Components.DataEntityComponent.Localisation;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.IDataBaseProvide;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Kernal;

namespace Sheng.SailingEase.Components.DataEntityComponent
{
   

    /// <summary>
    /// 控制数据实体的存储
    /// </summary>
    class DataEntityArchive
    {
        #region XPATH

        /// <summary>
        /// Index/DataEntity
        /// 索引文件中的DataEntity节点
        /// </summary>
        const string XPATH_Index_DataEntity = "/DataEntity";

        /// <summary>
        ///  Index/DataEntity/Entity[@Id='{0}']
        ///  从索引文件中选出指定实体索引
        /// </summary>
        const string XPATH_Index_SelectDataEntity = "/DataEntity/Entity[@Id='{0}']";

       
        /// <summary>
        ///  Index/DataEntity/Entity[@Code='{0}']
        ///  从索引文件中选出指定实体索引 ，根据code，忽略code大小写
        /// </summary>
        const string XPATH_Index_SelectDataEntity_ByCode_IgnoreCase =
            "/DataEntity/Entity[translate(@Code,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')=translate('{0}','ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')]";

        /// <summary>
        /// Index/DataEntity/Entity
        /// 从索引文件中选出所有Entity节点
        /// </summary>
        const string XPATH_Index_DataEntity_Entity = "/DataEntity/Entity";

        /// <summary>
        /// Entity/Items/Item[@Code='{0}']
        /// 从数据实体XML中选出指定的数据项  ，根据code，忽略code大小写
        /// </summary>
        const string XPATH_DataEntity_SelectItem_ByCode_IgnoreCase =
            "/Items/Item[translate(@Code,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')=translate('{0}','ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')]";

        /// <summary>
        /// Entity/Items/Item[@Id='{0}']
        ///  从数据实体XML中选出指定的数据项
        /// </summary>
        const string XPATH_DataEntity_SelectItem = "/Items/Item[@Id='{0}']";

        /// <summary>
        /// Entity/Items
        /// 数据实体XML中的Items节点
        /// </summary>
        const string XPATH_DataEntity_Items = "/Items";

        #endregion

        #region 私有成员

        ICachingService _cachingService = ServiceUnity.CachingService;
        IUnityContainer _container = ServiceUnity.Container;
        IEventAggregator _eventAggregator = ServiceUnity.EventAggregator;
        IPackageService _packageService = ServiceUnity.PackageService;

        /// <summary>
        /// 数据实体整个XML
        /// 数据实体目前只使用一个XML，所有数据实体，数据项皆存在这个XML里
        /// </summary>
        XElement _indexXml;

        #endregion

        #region 公开属性

        private static InstanceLazy<DataEntityArchive> _instance =
           new InstanceLazy<DataEntityArchive>(() => new DataEntityArchive());
        public static DataEntityArchive Instance
        {
            get { return _instance.Value; }
        }

        #endregion

        #region 构造

        private DataEntityArchive()
        {
            SubscribeEvent();
        }

        #endregion

        #region 公开方法

        #region 数据实体

        /// <summary>
        /// 添加一个数据实体
        /// </summary>
        /// <param name="dataEntity"></param>
        public void Add(DataEntityDev dataEntity)
        {
            Debug.Assert(dataEntity != null, "dataEntity 为 null");

            if (dataEntity == null)
                return;

            //添加索引信息
            _indexXml.XPathSelectElement(XPATH_Index_DataEntity).Add(ArchiveHelper.GetEntityArchiveIndex(dataEntity));

            SaveIndexFile();

            string xml = dataEntity.ToXml();
            XElement xElement = XElement.Parse(xml);

            //添加数据实体文件
            _packageService.Current.AddFileContent(xml, Path.Combine(Constant.PACKAGE_DATAENTITY_FOLDER, dataEntity.Id));

            _cachingService.Add(dataEntity.Id, xElement);

            //发布事件
            DataEntityEventArgs args = new DataEntityEventArgs(dataEntity);
            _eventAggregator.GetEvent<DataEntityAddedEvent>().Publish(args);
        }

        //TODO:更名为remove
        /// <summary>
        /// 删除数据实体
        /// </summary>
        /// <param name="id"></param>
        public void Delete(DataEntityDev dataEntity)
        {
            Debug.Assert(dataEntity != null, "dataEntity 为 null");

            if (dataEntity == null)
                return;

            //移除索引信息
            XElement element = _indexXml.XPathSelectElement(String.Format(XPATH_Index_SelectDataEntity, dataEntity.Id));
            Debug.Assert(element != null, "删除数据实体索引时未找到指定数据实体的索引记录");
            if (element != null)
            {
                element.Remove();
                SaveIndexFile();

                //移除数据实体文件
                _packageService.Current.DeleteFile(Path.Combine(Constant.PACKAGE_DATAENTITY_FOLDER, dataEntity.Id));

                _cachingService.Remove(dataEntity.Id);

                //发布事件
                DataEntityEventArgs args = new DataEntityEventArgs(dataEntity);
                _eventAggregator.GetEvent<DataEntityRemovedEvent>().Publish(args);
            }
        }

        /// <summary>
        /// 更新数据实体
        /// 实现方式是删除旧节点后重新添加，避免在这里再次操作XML具体内容
        /// </summary>
        /// <param name="dataEntity"></param>
        public void Update(DataEntityDev dataEntity)
        {
            Debug.Assert(dataEntity != null, "dataEntity 为 null");

            if (dataEntity == null)
                return;

            //更新索引信息
            XElement element = _indexXml.XPathSelectElement(String.Format(XPATH_Index_SelectDataEntity, dataEntity.Id));
            Debug.Assert(element != null, "更新数据实体索引时未找到指定数据实体的索引记录");
            if (element != null)
            {
                element.ReplaceWith(ArchiveHelper.GetEntityArchiveIndex(dataEntity));
                SaveIndexFile();

                string xml = dataEntity.ToXml();
                XElement xElement = XElement.Parse(xml);

                //更新数据实体文件
                _packageService.Current.AddFileContent(xml, Path.Combine(Constant.PACKAGE_DATAENTITY_FOLDER, dataEntity.Id));

                _cachingService.Add(dataEntity.Id, xElement);

                //发布事件
                DataEntityEventArgs args = new DataEntityEventArgs(dataEntity);
                _eventAggregator.GetEvent<DataEntityUpdatedEvent>().Publish(args);
            }
        }

        /// <summary>
        /// 提交
        /// 根据dataEntity的Id进行判断，如果已存在，调用update，如果不存在，调用add
        /// </summary>
        /// <param name="dataEntity"></param>
        public void Commit(DataEntityDev dataEntity)
        {
            Debug.Assert(dataEntity != null, "dataEntity 为 null");

            if (dataEntity == null)
                return;

            if (EntityExistById(dataEntity.Id))
            {
                Update(dataEntity);
            }
            else
            {
                Add(dataEntity);
            }
        }

        /// <summary>
        /// 获取数据实体
        /// 如果数据实体不存在，返回null
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataEntityDev GetDataEntity(string id)
        {
            //首先尝试从缓存中获取
            XElement cachingElement = (XElement)_cachingService.GetData(id);
            string strEntity;
            if (cachingElement == null)
            {
                string file = Path.Combine(Constant.PACKAGE_DATAENTITY_FOLDER, id);
                bool fileExist = _packageService.Current.Container(file);

                Debug.Assert(fileExist, "数据实体文件不存在");
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

            DataEntityDev dataEntity = new DataEntityDev(false);
            dataEntity.FromXml(strEntity);

            return dataEntity;
        }

        /// <summary>
        /// 获取数据实体列表
        /// </summary>
        /// <returns></returns>
        public List<DataEntity> GetDataEntityList()
        {
            XElement[] entityElements = _indexXml.XPathSelectElements(XPATH_Index_DataEntity_Entity).ToArray();

            List<DataEntity> list = new List<DataEntity>();

            foreach (XElement element in entityElements)
            {
                DataEntityDev entity = GetDataEntity(element.Attribute("Id").Value);
                if (entity != null)
                    list.Add(entity);
            }

            return list;
        }

        //TODO:改名,让名子反应出查询是通过code查的
        /// <summary>
        /// 判断指定代码的数据实体是否存在
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool EntityExist(string code)
        {
            Debug.Assert(String.IsNullOrEmpty(code) == false, "code 为 空 或为 Null");

            XElement element = _indexXml.XPathSelectElement(
                String.Format(XPATH_Index_SelectDataEntity_ByCode_IgnoreCase, code));
            if (element == null)
                return false;
            else
                return true;

            
        }

        #endregion

        #region 数据项

        /// <summary>
        /// 添加一个数据项
        /// </summary>
        /// <param name="dataItemEntity"></param>
        /// <param name="parentId"></param>
        public void AddDataItem(DataItemEntityDev dataItemEntity, string dataEntityId)
        {
            if (dataItemEntity == null || String.IsNullOrEmpty(dataEntityId))
            {
                Debug.Assert(false, "参数异常");
                return;
            }

            //判断指定的数据实体是否存在
            bool entityExist = EntityExistById(dataEntityId);

            Debug.Assert(entityExist, "数据实体不存在");
            if (entityExist == false)
                return;

            //拿数据实体文件
            string file = Path.Combine(Constant.PACKAGE_DATAENTITY_FOLDER, dataEntityId);

            //在数据实体XML中加上数据项节点
            //尝试从缓存中获取
            XElement entityElement = (XElement)_cachingService.GetData(dataEntityId);
            if (entityElement == null)
            {
                bool fileExist = _packageService.Current.Container(file);
                Debug.Assert(fileExist, "数据实体文件不存在");
                if (fileExist == false)
                    return;

                string strEntity = _packageService.Current.GetFileContent(file);
                entityElement = XElement.Parse(strEntity);
                _cachingService.Add(dataEntityId, entityElement);
            }

            //这里直接操作了缓存中的XElement，无需更新缓存了
            entityElement.XPathSelectElement(XPATH_DataEntity_Items).Add(XElement.Parse(dataItemEntity.ToXml()));

            //保存文件
            _packageService.Current.AddFileContent(entityElement.ToString(), file);

            //发布事件
            DataItemEntityEventArgs args = new DataItemEntityEventArgs(dataItemEntity);
            _eventAggregator.GetEvent<DataItemEntityAddedEvent>().Publish(args);
        }

        //TODO:改成remove
        /// <summary>
        /// 删除数据项
        /// </summary>
        /// <param name="id"></param>
        public void DeleteDataItem(DataItemEntityDev dataItemEntity)
        {
            if (dataItemEntity == null || dataItemEntity.Owner == null)
            {
                Debug.Assert(false, "参数异常");
                return;
            }

            string dataEntityId = dataItemEntity.Owner.Id;

            //判断指定的数据实体是否存在
            bool entityExist = EntityExistById(dataEntityId);

            Debug.Assert(entityExist, "数据实体不存在");
            if (entityExist == false)
                return;

            //拿数据实体文件
            string file = Path.Combine(Constant.PACKAGE_DATAENTITY_FOLDER, dataEntityId);
            
            //在数据实体XML中删除数据项节点
            //尝试从缓存中获取
            XElement entityElement = (XElement)_cachingService.GetData(dataEntityId);
            if (entityElement == null)
            {
                bool fileExist = _packageService.Current.Container(file);
                Debug.Assert(fileExist, "数据实体文件不存在");
                if (fileExist == false)
                    return;

                string strEntity = _packageService.Current.GetFileContent(file);
                entityElement = XElement.Parse(strEntity);
                _cachingService.Add(dataEntityId, entityElement);
            }

            //直接更新了缓存中的对像，无需再更新缓存
            entityElement.XPathSelectElement(String.Format(XPATH_DataEntity_SelectItem, dataItemEntity.Id)).Remove();

            //保存文件
            _packageService.Current.AddFileContent(entityElement.ToString(), file);

            //发布事件
            DataItemEntityEventArgs args = new DataItemEntityEventArgs(dataItemEntity);
            _eventAggregator.GetEvent<DataItemEntityRemovedEvent>().Publish(args);
        }

        /// <summary>
        /// 更新数据项
        /// </summary>
        /// <param name="dataItemEntity"></param>
        public void UpdateDataItem(DataItemEntityDev dataItemEntity)
        {
            if (dataItemEntity == null || dataItemEntity.Owner == null)
            {
                Debug.Assert(false, "参数异常");
                return;
            }

            string dataEntityId = dataItemEntity.Owner.Id;

            //判断指定的数据实体是否存在
            bool entityExist = EntityExistById(dataEntityId);

            Debug.Assert(entityExist, "数据实体不存在");
            if (entityExist == false)
                return;

            //拿数据实体文件
            string file = Path.Combine(Constant.PACKAGE_DATAENTITY_FOLDER, dataEntityId);
           
            //在数据实体XML中更新数据项节点
            //尝试从缓存中获取
            XElement entityElement = (XElement)_cachingService.GetData(dataEntityId);
            if (entityElement == null)
            {
                bool fileExist = _packageService.Current.Container(file);
                Debug.Assert(fileExist, "数据实体文件不存在");
                if (fileExist == false)
                    return;

                string strEntity = _packageService.Current.GetFileContent(file);
                entityElement = XElement.Parse(strEntity);
                _cachingService.Add(dataEntityId, entityElement);
            }

            //直接更新缓存中的对象，无需更新缓存了
            entityElement.XPathSelectElement(String.Format(XPATH_DataEntity_SelectItem, dataItemEntity.Id)).ReplaceWith(XElement.Parse(dataItemEntity.ToXml()));

            //保存文件
            _packageService.Current.AddFileContent(entityElement.ToString(), file);

            //发布事件
            DataItemEntityEventArgs args = new DataItemEntityEventArgs(dataItemEntity);
            _eventAggregator.GetEvent<DataItemEntityUpdatedEvent>().Publish(args);
        }

        public void CommitDataItem(DataItemEntityDev dataItemEntity, string dataEntityId)
        {
            if (dataItemEntity == null)
                return;

            if (ItemEntityExistById(dataItemEntity.Id, dataEntityId))
            {
                UpdateDataItem(dataItemEntity);
            }
            else
            {
                AddDataItem(dataItemEntity, dataEntityId);
            }
        }

        /// <summary>
        /// 获取数据项
        /// 如果不存在，返回null
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataItemEntity GetDataItemEntity(string id, string dataEntityId)
        {
            if (String.IsNullOrEmpty(id) || String.IsNullOrEmpty(dataEntityId))
            {
                Debug.Assert(false, "参数异常");
                return null;
            }

            DataEntity dataEntity = GetDataEntity(dataEntityId);

            if (dataEntity == null)
            {
                Debug.Assert(false, "没有取到数据实体");
                return null;
            }

            List<DataItemEntity> items =
                (from c in dataEntity.Items.ToList() where c.Id.Equals(id) select c).ToList();

            if (items.Count == 0)
            {
                Debug.Assert(false, "没有取到数据项");
                return null;
            }

            return items[0];

            /*

            //判断指定的数据实体是否存在
            bool entityExist = EntityExistById(dataEntityId);

            Debug.Assert(entityExist, "数据实体不存在");
            if (entityExist == false)
                return null;

            //在数据实体XML中拿出数据项节点
            XElement entityElement = (XElement)_cachingService.GetData(dataEntityId);
            if (entityElement == null)
            {
                //拿数据实体文件
                string file = Path.Combine(Constant.PACKAGE_DATAENTITY_FOLDER, dataEntityId);
                bool fileExist = _packageService.Current.Container(file);

                Debug.Assert(fileExist, "数据实体文件不存在");
                if (fileExist == false)
                    return null;

                string strEntity = _packageService.Current.GetFileContent(file);
                entityElement = XElement.Parse(strEntity);
                _cachingService.Add(dataEntityId, entityElement);
            }

            XElement itemElement = entityElement.XPathSelectElement(String.Format(XPATH_DataEntity_SelectItem, id));

            if (itemElement == null)
                return null;

            DataItemEntityDev dataItemEntity = new DataItemEntityDev(GetDataEntity(dataEntityId));
            dataItemEntity.FromXml(itemElement.ToString());
            return dataItemEntity;
             * 
             */
        }

        //TODO:改名，在名子上体现出是用code查找的
        /// <summary>
        /// 判断指定代码的数据项是否存在,by code
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool ItemEntityExist(string dataEntityId, string code)
        {
            if (String.IsNullOrEmpty(dataEntityId) || String.IsNullOrEmpty(code))
            {
                Debug.Assert(false, "参数异常");
                return false;
            }

            //判断指定的数据实体是否存在
            bool entityExist = EntityExistById(dataEntityId);

            Debug.Assert(entityExist, "数据实体不存在");
            if (entityExist == false)
                return false;

            //在数据实体XML中拿出数据项节点
            XElement entityElement = (XElement)_cachingService.GetData(dataEntityId);
            if (entityElement == null)
            {
                //拿数据实体文件
                string file = Path.Combine(Constant.PACKAGE_DATAENTITY_FOLDER, dataEntityId);
                bool fileExist = _packageService.Current.Container(file);

                Debug.Assert(fileExist, "数据实体文件不存在");
                if (fileExist == false)
                    return false;

                string strEntity = _packageService.Current.GetFileContent(file);
                entityElement = XElement.Parse(strEntity);
                _cachingService.Add(dataEntityId, entityElement);
            }

            XElement itemElement = entityElement.XPathSelectElement(String.Format(XPATH_DataEntity_SelectItem_ByCode_IgnoreCase, code));

            if (itemElement == null)
                return false;
            else
                return true;
        }

        #endregion

        #endregion

        #region 私有方法

        /// <summary>
        /// 事件订阅
        /// </summary>
        private void SubscribeEvent()
        {
            _eventAggregator.GetEvent<ProjectOpenedEvent>().Subscribe((e) =>
            {
                #region 初始化 IndexXml
                
                //初始化 IndexXml
                if (_packageService.Current.Container(Constant.PACKAGE_DATAENTITY_INDEX_FILE))
                {
                    //读取当前数据实体索引信息
                    string strIndex = _packageService.Current.GetFileContent(Constant.PACKAGE_DATAENTITY_INDEX_FILE);
                    _indexXml = XElement.Parse(strIndex);
                }
                else
                {
                    //如果当前不存在索引文件，创建一个
                    _indexXml = new XElement("Index", new XElement("DataEntity"));
                    SaveIndexFile();
                }

                #endregion

                #region 首次打开的话创建预置数据实体

                if (e.Project.ProjectSummary.FirstRun)
                {
                    #region 创建枚举数据表

                    IDataBase dataBase = DataBaseProvide.Current;

                    //创建枚举表
                    DataEntityDev enumDataEntity = new DataEntityDev(Language.Current.DataEntity_Enum, "Enum", true);

                    enumDataEntity.Items.Add(
                        new DataItemEntityDev(enumDataEntity, Language.Current.DataEntity_Enum_Text, "Text", true, false, dataBase.CreateVarCharField(50)),
                        new DataItemEntityDev(enumDataEntity, Language.Current.DataEntity_Enum_Value, "Value", true, false, dataBase.CreateVarCharField(50)),
                        new DataItemEntityDev(enumDataEntity, Language.Current.DataEntity_Enum_EnumKey, "EnumKey", true, false, dataBase.CreateVarCharField(50)),
                        new DataItemEntityDev(enumDataEntity, Language.Current.DataEntity_Enum_Sort, "Sort", true, false, dataBase.CreateIntField())
                        );

                    Add(enumDataEntity);


                    #endregion

                    #region 创建用户数据表

                    //创建用户表
                    DataEntityDev userDataEntity = new DataEntityDev(Language.Current.DataEntity_User, "User", true);

                    //GroupId暂时允许为null
                    userDataEntity.Items.Add(
                        new DataItemEntityDev(userDataEntity, Language.Current.DataEntity_User_GroupId, "GroupId", true, true, dataBase.CreateIdField()),
                         new DataItemEntityDev(userDataEntity, Language.Current.DataEntity_User_Name, "Name", true, false, dataBase.CreateVarCharField(50)),
                         new DataItemEntityDev(userDataEntity, Language.Current.DataEntity_User_LoginName, "LoginName", true, false, dataBase.CreateVarCharField(50)),
                         new DataItemEntityDev(userDataEntity, Language.Current.DataEntity_User_Password, "Password", true, false, dataBase.CreateVarCharField(50)),
                         new DataItemEntityDev(userDataEntity, Language.Current.DataEntity_User_LastLoginTime, "LastLoginTime", true, true, dataBase.CreateSmallDatetimeField())
                        );

                    Add(userDataEntity);

                    #endregion

                    #region 创建用户组表

                    //创建用户组表
                    DataEntityDev userGroupDataEntity = new DataEntityDev(Language.Current.DataEntity_UserGroup, "UserGroup", true);

                    userGroupDataEntity.Items.Add(
                          new DataItemEntityDev(userGroupDataEntity, Language.Current.DataEntity_UserGroup_Name, "Name", true, false, dataBase.CreateVarCharField(50))
                        );

                    Add(userGroupDataEntity);

                    #endregion
                }

                #endregion
            });

            _eventAggregator.GetEvent<ProjectClosedEvent>().Subscribe((e) =>
            {
                //释放 IndexXml
                _indexXml = null;
            });
        }

        /// <summary>
        /// 根据Id判断实体是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool EntityExistById(string id)
        {
            Debug.Assert(String.IsNullOrEmpty(id) == false, "id 为 空 或为 Null");

            XElement element = _indexXml.XPathSelectElement(String.Format(XPATH_Index_SelectDataEntity, id));
            if (element == null)
                return false;
            else
                return true;
        }

        /// <summary>
        /// 判断指定id的数据项是否存在,by id
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        private bool ItemEntityExistById(string id, string entityId)
        {
            XElement entityElement = (XElement)_cachingService.GetData(entityId);
            if (entityElement == null)
            {
                string file = Path.Combine(Constant.PACKAGE_DATAENTITY_FOLDER, entityId);
                bool fileExist = _packageService.Current.Container(file);

                Debug.Assert(fileExist, "数据实体文件不存在");
                if (fileExist == false)
                    return false;

                string strEntity = _packageService.Current.GetFileContent(file);
                entityElement = XElement.Parse(strEntity);
                _cachingService.Add(entityId, entityElement);
            }

            XElement itemElement = entityElement.XPathSelectElement(String.Format(XPATH_DataEntity_SelectItem, id));
            if (itemElement == null)
                return false;
            else
                return true;
        }

        /// <summary>
        /// 保存索引文件
        /// </summary>
        private void SaveIndexFile()
        {
            Debug.Assert(_indexXml != null, "IndexXml 为 null");

            if (_indexXml != null)
            {
                _packageService.Current.AddFileContent(_indexXml.ToString(), Constant.PACKAGE_DATAENTITY_INDEX_FILE);
            }
        }

        #endregion
    }
}
