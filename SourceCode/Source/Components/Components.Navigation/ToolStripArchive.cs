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
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Core.Development;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Kernal;

namespace Sheng.SailingEase.Components.NavigationComponent
{
    class ToolStripArchive : IArchiveService
    {
        #region XPATH

        /// <summary>
        /// Index/Pages
        /// 索引文件中的Folder节点
        const string XPATH_Index_Pages = "/Pages";

        /// <summary>
        /// Pages/Page[@Id='{0}']
        /// 从索引文件中选出指定文件夹实体索引
        /// </summary>
        const string XPATH_Index_SelectPage = "/Pages/Page[@Id='{0}']";

        /// <summary>
        /// Pages/Page
        /// 从索引文件中选出所有页
        /// </summary>
        const string XPATH_Index_Pages_Page = "/Pages/Page";

        /// <summary>
        ///  Index/ToolStrip/Entity[@Code='{0}']
        ///  从索引文件中选出指定实体索引 ，根据code，忽略code大小写
        /// </summary>
        const string XPATH_Index_SelectPage_ByCode_IgnoreCode =
            "/Pages/Page[translate(@Code,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')=translate('{0}','ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')]";

        /// <summary>
        /// Index/Groups
        /// 索引文件中的Folder节点
        const string XPATH_Index_Groups = "/Groups";

        /// <summary>
        /// /Groups/Group[@Id='{0}']
        /// 从索引文件中选出指定文件夹实体索引
        /// </summary>
        const string XPATH_Index_SelectGroup = "/Groups/Group[@Id='{0}']";

        /// <summary>
        /// /Groups/Group[@PageId='{0}']
        /// 从索引文件中选出 父目录 为指定文件夹实体索引
        /// </summary>
        const string XPATH_Index_SelectGroup_ByPageId = "/Groups/Group[@PageId='{0}']";

        /// <summary>
        ///  Index/ToolStrip/Entity[@Code='{0}']
        ///  从索引文件中选出指定实体索引 ，根据code，忽略code大小写
        /// </summary>
        const string XPATH_Index_SelectGroup_ByCode_IgnoreCode =
            "/Groups/Group[translate(@Code,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')=translate('{0}','ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')]";

        /// <summary>
        /// Index/ToolStrip
        /// 索引文件中的Dictionary节点
        /// </summary>
        const string XPATH_Index_ToolStrip = "/ToolStrip";
       
        /// <summary>
        ///  Index/ToolStrip/Entity[@Id='{0}']
        ///  从索引文件中选出指定实体索引
        /// </summary>
        const string XPATH_Index_SelectToolStrip = "/ToolStrip/Entity[@Id='{0}']";

        /// <summary>
        ///  ToolStrip/Entity[@GroupId='{0}']
        ///  从索引文件中指定分组下的所有项
        /// </summary>
        const string XPATH_Index_SelectToolStrip_ByGroupId = "/ToolStrip/Entity[@GroupId='{0}']";

        ///// <summary>
        /////  Index/ToolStrip/Entity[@ParentId='{0}']
        /////  从索引文件中选出父节点为指定Id的实体索引
        ///// </summary>
        //const string XPATH_INDEX_SELECTENTITY_BYPARENTID = "/ToolStrip/Entity[@ParentId='{0}']";

        /// <summary>
        /// Index/ToolStrip/Entity
        /// 从索引文件中选出所有Entity节点
        /// </summary>
        const string XPATH_Index_ToolStrip_Entity = "/ToolStrip/Entity";

        /// <summary>
        ///  Index/ToolStrip/Entity[@Code='{0}']
        ///  从索引文件中选出指定实体索引 ，根据code，忽略code大小写
        /// </summary>
        const string XPATH_Index_SelectToolStrip_ByCode_IgnoreCode =
            "/ToolStrip/Entity[translate(@Code,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')=translate('{0}','ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')]";

        #endregion

        #region 私有成员

        ICachingService _cachingService = ServiceUnity.CachingService;
        IUnityContainer _container = ServiceUnity.Container;
        IEventAggregator _eventAggregator = ServiceUnity.Container.Resolve<IEventAggregator>();
        IPackageService _packageService = ServiceUnity.Container.Resolve<IPackageService>();
        ToolStripItemEntityTypesFactory _toolStripItemEntityTypesFactory = ToolStripItemEntityTypesFactory.Instance;

        /// <summary>
        /// 索引XML
        /// </summary>
        XElement _indexXml;

        #endregion

        #region 公开属性

        private static InstanceLazy<ToolStripArchive> _instance =
           new InstanceLazy<ToolStripArchive>(() => new ToolStripArchive());
        public static ToolStripArchive Instance
        {
            get { return _instance.Value; }
        }

        #endregion

        #region 构造

        private ToolStripArchive()
        {
            SubscribeEvent();
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
                if (_packageService.Current.Container(Constant.PACKAGE_NAVIGATION_TOOLSTRIP_INDEX_FILE))
                {
                    //读取当前数据实体索引信息
                    string strIndex = _packageService.Current.GetFileContent(Constant.PACKAGE_NAVIGATION_TOOLSTRIP_INDEX_FILE);
                    _indexXml = XElement.Parse(strIndex);
                }
                else
                {
                    //如果当前不存在索引文件，创建一个
                    _indexXml = new XElement("Index", 
                        new XElement("ToolStrip"),
                        new XElement("Pages"),
                        new XElement("Groups"));
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
                _packageService.Current.AddFileContent(_indexXml.ToString(), Constant.PACKAGE_NAVIGATION_TOOLSTRIP_INDEX_FILE);
            }
        }

        /// <summary>
        /// 获取用于存储在XML索引文件中的索引节点
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private XElement GetArchiveIndex(ToolStripItemAbstract entity)
        {
            Debug.Assert(entity != null, "entity 为 null");

            XElement ele = new XElement("Entity",
                new XAttribute("Id", entity.Id),
                new XAttribute("Name", entity.Name),
                new XAttribute("Code", entity.Code),
                new XAttribute("GroupId", entity.GroupId),
                new XAttribute("Sys", entity.Sys));
            //new XAttribute("ParentId", entity.ParentId));
            //计划将来工具栏项目支持多级

            return ele;
        }

        #endregion

        #region 公开方法

        #region Page 操作

        /// <summary>
        /// 提交
        /// 根据MenuEntity的Id进行判断，如果已存在，调用update，如果不存在，调用add
        /// </summary>
        /// <param name="dataEntity"></param>
        public void Commit(ToolStripPageEntity entity)
        {
            Debug.Assert(entity != null, "enumEntity 为 null");

            if (entity == null)
                return;

            if (PageExistById(entity.Id))
            {
                UpdatePage(entity);
            }
            else
            {
                AddPage(entity);
            }
        }

        /// <summary>
        /// 添加一个Ribbon页
        /// </summary>
        /// <param name="pageEntity"></param>
        public void AddPage(ToolStripPageEntity entity)
        {
            Debug.Assert(entity != null, "entity 为 null");

            if (entity == null)
                return;

            //添加索引信息
            _indexXml.XPathSelectElement(XPATH_Index_Pages).Add(XElement.Parse(entity.ToXml()));

            SaveIndexFile();

            //发布事件
            ToolStripPageEventArgs args = new ToolStripPageEventArgs(entity);
            _eventAggregator.GetEvent<ToolStripPageAddedEvent>().Publish(args);
        }

        /// <summary>
        /// 删除一个Ribbon页，以及其下所有分组和项
        /// </summary>
        /// <param name="id"></param>
        public void RemovePage(ToolStripPageEntity entity)
        {
            Debug.Assert(entity != null, "entity 为 null");

            if (entity == null)
                return;

            //查找其下所有分组，连带删除其下所有分组下的所有项
            ToolStripGroupEntityCollection groups = GetGroupCollection(entity.Id);
            foreach (var groupItem in groups)
            {
                RemoveGroup(groupItem);
            }

            //删除此页
            XElement element = _indexXml.XPathSelectElement(String.Format(XPATH_Index_SelectPage, entity.Id));
            if (element != null)
            {
                element.Remove();
            }

            SaveIndexFile();

            ToolStripPageEventArgs args = new ToolStripPageEventArgs(entity);
            _eventAggregator.GetEvent<ToolStripPageRemovedEvent>().Publish(args);
        }

        public void UpdatePage(ToolStripPageEntity entity)
        {
            Debug.Assert(entity != null, "ToolStripPageEntity 为 null");

            if (entity == null)
                return;

            //更新索引信息
            XElement element = _indexXml.XPathSelectElement(String.Format(XPATH_Index_SelectPage, entity.Id));
            Debug.Assert(element != null, "更新窗体文件夹索引时未找到指定文件夹的索引记录");
            if (element != null)
            {
                element.ReplaceWith(XElement.Parse(entity.ToXml()));
                SaveIndexFile();

                //发布事件
                ToolStripPageEventArgs args = new ToolStripPageEventArgs(entity);
                _eventAggregator.GetEvent<ToolStripPageUpdatedEvent>().Publish(args);
            }
        }

        public ToolStripPageEntity GetPageEntity(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                Debug.Assert(false, "GetPageEntity 传入的 id 为空");
                return null;
            }

            XElement element = _indexXml.XPathSelectElement(String.Format(XPATH_Index_SelectPage, id));

            if (element == null)
            {
                Debug.Assert(false, "GetFolderEntity 失败，返回了 null");
            }

            ToolStripPageEntityDev toolStripPageEntity = new ToolStripPageEntityDev();
            toolStripPageEntity.FromXml(element.ToString());
            return toolStripPageEntity;
        }

        public ToolStripPageEntityCollection GetPageCollection()
        {
            ToolStripPageEntityCollection collection = new ToolStripPageEntityCollection();

            foreach (XElement element in _indexXml.XPathSelectElements(XPATH_Index_Pages_Page))
            {
                ToolStripPageEntityDev toolStripPageEntity = new ToolStripPageEntityDev();
                //FormFolderEntity在往Index文件里存的时候是直接ToXml的
                //所以此处直接FromXml即可
                toolStripPageEntity.FromXml(element.ToString());

                collection.Add(toolStripPageEntity);
            }

            return collection;
        }

        /// <summary>
        /// 将指定的菜单页移动到另一个菜单项之前
        /// 原 MoveUp
        /// </summary>
        /// <param name="code"></param>
        /// <param name="beforeCode"></param>
        public void MovePageBefore(string id, string beforeId)
        {
            if (id.Equals(beforeId))
            {
                Debug.Assert(false, "id 和 beforeId 相同");
                return;
            }

            //移动索引文件中的顺序即可
            //因为现在菜单实体的XML全部分开存放了，不存在顺序问题，顺序是由index文件中的顺序确定的

            XElement element = _indexXml.XPathSelectElement(String.Format(XPATH_Index_SelectPage, id));
            XElement beforeElement = _indexXml.XPathSelectElement(String.Format(XPATH_Index_SelectPage, beforeId));

            Debug.Assert(element != null && beforeId != null, "指定的节点没有找到");

            if (element == null || beforeId == null)
                return;

            XElement elementNew = new XElement(element);
            beforeElement.AddBeforeSelf(elementNew);
            element.Remove();

            SaveIndexFile();

            //发布事件
            ToolStripPageMoveBeforeEventArgs args = new ToolStripPageMoveBeforeEventArgs(id, beforeId);
            _eventAggregator.GetEvent<ToolStripPageMoveBeforeEvent>().Publish(args);
        }

        /// <summary>
        /// 将指定的菜单页移动到另一个菜单项之后
        /// 原 MoveDown
        /// </summary>
        /// <param name="code"></param>
        /// <param name="afterCode"></param>
        public void MovePageAfter(string id, string afterId)
        {
            if (id.Equals(afterId))
            {
                Debug.Assert(false, "id 和 afterId 相同");
                return;
            }

            XElement element = _indexXml.XPathSelectElement(String.Format(XPATH_Index_SelectPage, id));
            XElement afterElement = _indexXml.XPathSelectElement(String.Format(XPATH_Index_SelectPage, afterId));

            Debug.Assert(element != null && afterId != null, "指定的节点没有找到");

            if (element == null || afterId == null)
                return;

            XElement elementNew = new XElement(element);
            afterElement.AddAfterSelf(elementNew);
            element.Remove();

            SaveIndexFile();

            //发布事件
            ToolStripPageMoveAfterEventArgs args = new ToolStripPageMoveAfterEventArgs(id, afterId);
            _eventAggregator.GetEvent<ToolStripPageMoveAfterEvent>().Publish(args);
        }

        /// <summary>
        /// 根据Id判断枚举是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool PageExistById(string id)
        {
            Debug.Assert(String.IsNullOrEmpty(id) == false, "id 为 空 或为 Null");

            XElement element = _indexXml.XPathSelectElement(String.Format(XPATH_Index_SelectPage, id));
            if (element == null)
                return false;
            else
                return true;
        }

        public bool PageExistByCode(string code)
        {
            Debug.Assert(String.IsNullOrEmpty(code) == false, "code 为 空 或为 Null");

            XElement element = _indexXml.XPathSelectElement(
                String.Format(XPATH_Index_SelectPage_ByCode_IgnoreCode, code));
            if (element == null)
                return false;
            else
                return true;
        }

        #endregion

        #region Group 操作

        /// <summary>
        /// 提交
        /// 根据MenuEntity的Id进行判断，如果已存在，调用update，如果不存在，调用add
        /// </summary>
        /// <param name="dataEntity"></param>
        public void Commit(ToolStripGroupEntity entity)
        {
            Debug.Assert(entity != null, "enumEntity 为 null");

            if (entity == null)
                return;

            if (GroupExistById(entity.Id))
            {
                UpdateGroup(entity);
            }
            else
            {
                AddGroup(entity);
            }
        }

        /// <summary>
        /// 添加一个Ribbon页
        /// </summary>
        /// <param name="pageEntity"></param>
        public void AddGroup(ToolStripGroupEntity entity)
        {
            Debug.Assert(entity != null, "entity 为 null");

            if (entity == null)
                return;

            //添加索引信息
            _indexXml.XPathSelectElement(XPATH_Index_Groups).Add(XElement.Parse(entity.ToXml()));

            SaveIndexFile();

            //发布事件
            ToolStripGroupEventArgs args = new ToolStripGroupEventArgs(entity);
            _eventAggregator.GetEvent<ToolStripGroupAddedEvent>().Publish(args);
        }

        /// <summary>
        /// 删除一个Ribbon分组，以及其下所有项
        /// </summary>
        /// <param name="id"></param>
        public void RemoveGroup(ToolStripGroupEntity entity)
        {
            Debug.Assert(entity != null, "entity 为 null");

            if (entity == null)
                return;

            //查找并删除其下所有项，并删除
                //避免发布过多删除项事件，所以不调用 Delete(item);
            XElement[] entityElements = _indexXml.XPathSelectElements(
                String.Format(XPATH_Index_SelectToolStrip_ByGroupId, entity.Id)).ToArray();
            foreach (XElement itemElement in entityElements)
            {
                string itemId = itemElement.Attribute("Id").Value;

                //移除索引信息
                itemElement.Remove();

                //移除实体文件
                _packageService.Current.DeleteFile(Path.Combine(Constant.PACKAGE_NAVIGATION_TOOLSTRIP_FOLDER, itemId));

                //移除缓存
                _cachingService.Remove(itemId);
            }

            //删除该分组
            XElement element = _indexXml.XPathSelectElement(String.Format(XPATH_Index_SelectGroup, entity.Id));
            if (element != null)
            {
                element.Remove();
            }

            SaveIndexFile();

            //发布事件
            ToolStripGroupEventArgs args = new ToolStripGroupEventArgs(entity);
            _eventAggregator.GetEvent<ToolStripGroupRemovedEvent>().Publish(args);
        }

        public void UpdateGroup(ToolStripGroupEntity entity)
        {
            Debug.Assert(entity != null, "ToolStripGroupEntity 为 null");

            if (entity == null)
                return;

            //更新索引信息
            XElement element = _indexXml.XPathSelectElement(String.Format(XPATH_Index_SelectGroup, entity.Id));
            Debug.Assert(element != null, "更新窗体文件夹索引时未找到指定文件夹的索引记录");
            if (element != null)
            {
                element.ReplaceWith(XElement.Parse(entity.ToXml()));
                SaveIndexFile();

                //发布事件
                ToolStripGroupEventArgs args = new ToolStripGroupEventArgs(entity);
                _eventAggregator.GetEvent<ToolStripGroupUpdatedEvent>().Publish(args);
            }
        }

        public ToolStripGroupEntity GetGroupEntity(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                Debug.Assert(false, "GetGroupEntity 传入的 id 为空");
                return null;
            }

            XElement element = _indexXml.XPathSelectElement(String.Format(XPATH_Index_SelectGroup, id));

            if (element == null)
            {
                Debug.Assert(false, "GetGroupEntity 失败，返回了 null");
            }

            ToolStripGroupEntityDev toolStripGroupEntity = new ToolStripGroupEntityDev();
            toolStripGroupEntity.FromXml(element.ToString());
            return toolStripGroupEntity;
        }

        public ToolStripGroupEntityCollection GetGroupCollection(string pageId)
        {
            ToolStripGroupEntityCollection collection = new ToolStripGroupEntityCollection();

            foreach (XElement element in _indexXml.XPathSelectElements(String.Format(XPATH_Index_SelectGroup_ByPageId,pageId)))
            {
                ToolStripGroupEntityDev toolStripGroupEntity = new ToolStripGroupEntityDev();
                //FormFolderEntity在往Index文件里存的时候是直接ToXml的
                //所以此处直接FromXml即可
                toolStripGroupEntity.FromXml(element.ToString());

                collection.Add(toolStripGroupEntity);
            }

            return collection;
        }

        /// <summary>
        /// 将指定的菜单页移动到另一个菜单项之前
        /// 原 MoveUp
        /// </summary>
        /// <param name="code"></param>
        /// <param name="beforeCode"></param>
        public void MoveGroupBefore(string id, string beforeId)
        {
            if (id.Equals(beforeId))
            {
                Debug.Assert(false, "id 和 beforeId 相同");
                return;
            }

            //移动索引文件中的顺序即可
            //因为现在菜单实体的XML全部分开存放了，不存在顺序问题，顺序是由index文件中的顺序确定的

            XElement element = _indexXml.XPathSelectElement(String.Format(XPATH_Index_SelectGroup, id));
            XElement beforeElement = _indexXml.XPathSelectElement(String.Format(XPATH_Index_SelectGroup, beforeId));

            Debug.Assert(element != null && beforeId != null, "指定的节点没有找到");

            if (element == null || beforeId == null)
                return;

            XElement elementNew = new XElement(element);
            beforeElement.AddBeforeSelf(elementNew);
            element.Remove();

            SaveIndexFile();

            //发布事件
            ToolStripGroupMoveBeforeEventArgs args = new ToolStripGroupMoveBeforeEventArgs(id, beforeId);
            _eventAggregator.GetEvent<ToolStripGroupMoveBeforeEvent>().Publish(args);
        }

        /// <summary>
        /// 将指定的菜单页移动到另一个菜单项之后
        /// 原 MoveDown
        /// </summary>
        /// <param name="code"></param>
        /// <param name="afterCode"></param>
        public void MoveGroupAfter(string id, string afterId)
        {
            if (id.Equals(afterId))
            {
                Debug.Assert(false, "id 和 afterId 相同");
                return;
            }

            XElement element = _indexXml.XPathSelectElement(String.Format(XPATH_Index_SelectGroup, id));
            XElement afterElement = _indexXml.XPathSelectElement(String.Format(XPATH_Index_SelectGroup, afterId));

            Debug.Assert(element != null && afterId != null, "指定的节点没有找到");

            if (element == null || afterId == null)
                return;

            XElement elementNew = new XElement(element);
            afterElement.AddAfterSelf(elementNew);
            element.Remove();

            SaveIndexFile();

            //发布事件
            ToolStripGroupMoveAfterEventArgs args = new ToolStripGroupMoveAfterEventArgs(id, afterId);
            _eventAggregator.GetEvent<ToolStripGroupMoveAfterEvent>().Publish(args);
        }

        /// <summary>
        /// 根据Id判断枚举是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool GroupExistById(string id)
        {
            Debug.Assert(String.IsNullOrEmpty(id) == false, "id 为 空 或为 Null");

            XElement element = _indexXml.XPathSelectElement(String.Format(XPATH_Index_SelectGroup, id));
            if (element == null)
                return false;
            else
                return true;
        }

        public bool GroupExistByCode(string code)
        {
            Debug.Assert(String.IsNullOrEmpty(code) == false, "code 为 空 或为 Null");

            XElement element = _indexXml.XPathSelectElement(
                String.Format(XPATH_Index_SelectGroup_ByCode_IgnoreCode, code));
            if (element == null)
                return false;
            else
                return true;
        }

        #endregion

        #region Item 操作

        /// <summary>
        /// 添加一个菜单项
        /// </summary>
        /// <param name="entity"></param>
        public void Add(ToolStripItemAbstract entity)
        {
            Debug.Assert(entity != null, "entity 为 null");

            if (entity == null)
                return;

            //添加索引信息
            _indexXml.XPathSelectElement(XPATH_Index_ToolStrip).Add(GetArchiveIndex(entity));

            string xml = entity.ToXml();
            XElement xElement = XElement.Parse(xml);

            SaveIndexFile();

            //添加数据实体文件
            _packageService.Current.AddFileContent(xml,
                Path.Combine(Constant.PACKAGE_NAVIGATION_TOOLSTRIP_FOLDER, entity.Id));

            _cachingService.Add(entity.Id, xElement);

            //发布事件
            ToolStripEventArgs args = new ToolStripEventArgs(entity);
            _eventAggregator.GetEvent<ToolStripItemAddedEvent>().Publish(args);
        }

        /// <summary>
        /// 删除菜单项，及其所有子项
        /// </summary>
        /// <param name="code"></param>
        public void Delete(ToolStripItemAbstract entity)
        {
            Debug.Assert(entity != null, "entity 为 null");

            if (entity == null)
                return;

            //移除索引信息
            XElement element = _indexXml.XPathSelectElement(String.Format(XPATH_Index_SelectToolStrip, entity.Id));
            Debug.Assert(element != null, "删除主工具栏索引时未找到指定主菜单的索引记录");
            if (element != null)
            {
                element.Remove();
                SaveIndexFile();

                //移除实体文件
                _packageService.Current.DeleteFile(Path.Combine(Constant.PACKAGE_NAVIGATION_TOOLSTRIP_FOLDER, entity.Id));

                _cachingService.Remove(entity.Id);

                //发布事件
                ToolStripEventArgs args = new ToolStripEventArgs(entity);
                _eventAggregator.GetEvent<ToolStripItemRemovedEvent>().Publish(args);
            }
        }

        /// <summary>
        /// 获取一个工具栏项实例（根据Id）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ToolStripItemAbstract GetEntity(string id)
        {
            XElement entityElement = (XElement)_cachingService.GetData(id);
            string strEntity;
            if (entityElement == null)
            {
                string file = Path.Combine(Constant.PACKAGE_NAVIGATION_TOOLSTRIP_FOLDER, id);
                bool fileExist = _packageService.Current.Container(file);

                Debug.Assert(fileExist, "工具栏项文件不存在");
                if (fileExist == false)
                    return null;

                strEntity = _packageService.Current.GetFileContent(file);
                entityElement = XElement.Parse(strEntity);
                _cachingService.Add(id, entityElement);
            }
            else
            {
                strEntity = entityElement.ToString();
            }

            int controlType = Convert.ToInt32(entityElement.Element("ControlType").Value);

            ToolStripItemAbstract entity = _toolStripItemEntityTypesFactory.CreateInstance(controlType);
            if (entity == null)
            {
                Debug.Assert(false, "_toolStripItemsContainer.Create(controlType) 失败，返回了null");
                throw new Exception();
            }
            entity.FromXml(strEntity);

            return entity;
        }

        /// <summary>
        /// 更新一个主工具栏项目
        /// </summary>
        /// <param name="formEntity"></param>
        public void Update(ToolStripItemAbstract entity)
        {
            Debug.Assert(entity != null, "ToolStripItemAbstract 为 null");

            if (entity == null)
                return;

            //更新索引信息
            XElement element = _indexXml.XPathSelectElement(String.Format(XPATH_Index_SelectToolStrip, entity.Id));
            Debug.Assert(element != null, "更新主工具栏项目索引时未找到指定主菜单的索引记录");
            if (element != null)
            {
                element.ReplaceWith(GetArchiveIndex(entity));
                SaveIndexFile();

                string xml = entity.ToXml();
                XElement xElement = XElement.Parse(xml);

                //更新实体文件
                _packageService.Current.AddFileContent(xml,
                    Path.Combine(Constant.PACKAGE_NAVIGATION_TOOLSTRIP_FOLDER, entity.Id));

                _cachingService.Add(entity.Id, xElement);

                //发布事件
                ToolStripEventArgs args = new ToolStripEventArgs(entity);
                _eventAggregator.GetEvent<ToolStripItemUpdatedEvent>().Publish(args);
            }
        }

        /// <summary>
        /// 将指定的菜单项移动到另一个菜单项之前
        /// 原 MoveUp
        /// </summary>
        /// <param name="code"></param>
        /// <param name="beforeCode"></param>
        public void MoveBefore(string id, string beforeId)
        {
            if (id.Equals(beforeId))
            {
                Debug.Assert(false, "id 和 beforeId 相同");
                return;
            }

            //移动索引文件中的顺利即可
            //因为现在菜单实体的XML全部分开存放了，不存在顺序问题，顺序是由index文件中的顺序确定的

            XElement element = _indexXml.XPathSelectElement(String.Format(XPATH_Index_SelectToolStrip, id));
            XElement beforeElement = _indexXml.XPathSelectElement(String.Format(XPATH_Index_SelectToolStrip, beforeId));

            Debug.Assert(element != null && beforeId != null, "指定的节点没有找到");

            if (element == null || beforeId == null)
                return;

            XElement elementNew = new XElement(element);
            beforeElement.AddBeforeSelf(elementNew);
            element.Remove();

            SaveIndexFile();

            //发布事件
            ToolStripItemMoveBeforeEventArgs args = new ToolStripItemMoveBeforeEventArgs(id, beforeId);
            _eventAggregator.GetEvent<ToolStripItemMoveBeforeEvent>().Publish(args);
        }

        /// <summary>
        /// 将指定的菜单项移动到另一个菜单项之后
        /// 原 MoveDown
        /// </summary>
        /// <param name="code"></param>
        /// <param name="afterCode"></param>
        public void MoveAfter(string id, string afterId)
        {
            if (id.Equals(afterId))
            {
                Debug.Assert(false, "id 和 afterId 相同");
                return;
            }

            XElement element = _indexXml.XPathSelectElement(String.Format(XPATH_Index_SelectToolStrip, id));
            XElement afterElement = _indexXml.XPathSelectElement(String.Format(XPATH_Index_SelectToolStrip, afterId));

            Debug.Assert(element != null && afterId != null, "指定的节点没有找到");

            if (element == null || afterId == null)
                return;

            XElement elementNew = new XElement(element);
            afterElement.AddAfterSelf(elementNew);
            element.Remove();

            SaveIndexFile();

            //发布事件
            ToolStripItemMoveAfterEventArgs args = new ToolStripItemMoveAfterEventArgs(id, afterId);
            _eventAggregator.GetEvent<ToolStripItemMoveAfterEvent>().Publish(args);
        }

        /// <summary>
        /// 获取工具栏所有项
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public ToolStripItemEntityCollection GetEntityList()
        {
            XElement[] entityElements = _indexXml.XPathSelectElements(XPATH_Index_ToolStrip_Entity).ToArray();

            ToolStripItemEntityCollection list = new ToolStripItemEntityCollection();

            foreach (XElement element in entityElements)
            {
                ToolStripItemAbstract entity = GetEntity(element.Attribute("Id").Value);
                if (entity != null)
                    list.Add(entity);
            }

            return list;
        }

        /// <summary>
        /// 获取指定分组下的所有项
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public ToolStripItemEntityCollection GetEntityList(string groupId)
        {
            XElement[] entityElements = _indexXml.XPathSelectElements(
                String.Format(XPATH_Index_SelectToolStrip_ByGroupId, groupId)).ToArray();

            ToolStripItemEntityCollection list = new ToolStripItemEntityCollection();

            foreach (XElement element in entityElements)
            {
                ToolStripItemAbstract entity = GetEntity(element.Attribute("Id").Value);
                if (entity != null)
                    list.Add(entity);
            }

            return list;
        }

        /// <summary>
        /// 提交
        /// 根据MenuEntity的Id进行判断，如果已存在，调用update，如果不存在，调用add
        /// </summary>
        /// <param name="dataEntity"></param>
        public void Commit(ToolStripItemAbstract entity)
        {
            Debug.Assert(entity != null, "enumEntity 为 null");

            if (entity == null)
                return;

            if (EntityExistById(entity.Id))
            {
                Update(entity);
            }
            else
            {
                Add(entity);
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

            XElement element = _indexXml.XPathSelectElement(String.Format(XPATH_Index_SelectToolStrip, id));
            if (element == null)
                return false;
            else
                return true;
        }

        public bool CheckExistByCode(string code)
        {
            Debug.Assert(String.IsNullOrEmpty(code) == false, "code 为 空 或为 Null");

            XElement element = _indexXml.XPathSelectElement(
                String.Format(XPATH_Index_SelectToolStrip_ByCode_IgnoreCode, code));
            if (element == null)
                return false;
            else
                return true;
        }

        #endregion

        #endregion

        #region IArchiveService 成员

        Type IArchiveService.CoverType
        {
            get { return typeof(ToolStripItemAbstract); }
        }

        bool IArchiveService.ActOnSubClass
        {
            get { return true; }
        }

        void IArchiveService.Save(object obj)
        {
            ToolStripItemAbstract entity = obj as ToolStripItemAbstract;
            Debug.Assert(entity != null, "entity为null");
            if (entity != null)
                Commit(entity);
        }

        void IArchiveService.Delete(object obj)
        {
            ToolStripItemAbstract entity = obj as ToolStripItemAbstract;
            Debug.Assert(entity != null, "entity为null");
            if (entity != null)
                Delete(entity);
        }

        #endregion
    }
} 
