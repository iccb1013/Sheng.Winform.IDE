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
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Kernal;
using Sheng.SailingEase.Core;

namespace Sheng.SailingEase.Components.NavigationComponent
{
    class MenuStripArchive : IArchiveService
    {
        #region XPATH

        /// <summary>
        /// Index/MenuStrip
        /// 索引文件中的MenuStrip节点
        /// </summary>
        const string XPATH_Index_MenuStrip = "/MenuStrip";

        /// <summary>
        ///  Index/MenuStrip/Entity[@Id='{0}']
        ///  从索引文件中选出指定实体索引
        /// </summary>
        const string XPATH_Index_SelectMenuStrip = "/MenuStrip/Entity[@Id='{0}']";

        /// <summary>
        ///  Index/MenuStrip/Entity[@ParentId='{0}']
        ///  从索引文件中选出父节点为指定Id的实体索引
        /// </summary>
        const string XPATH_Index_SelectMenuStrip_ByParentId = "/MenuStrip/Entity[@ParentId='{0}']";

        /// <summary>
        ///  Index/MenuStrip/Entity[@Code='{0}']
        ///  从索引文件中选出指定实体索引 ，根据code，忽略code大小写
        /// </summary>
        const string XPATH_Index_SelectMenuStrip_ByCode_IgnoreCase =
            "/MenuStrip/Entity[translate(@Code,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')=translate('{0}','ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')]";

        #endregion

        #region 私有成员

        ICachingService _cachingService = ServiceUnity.CachingService;
        IUnityContainer _container = ServiceUnity.Container;
        IEventAggregator _eventAggregator = ServiceUnity.Container.Resolve<IEventAggregator>();
        IPackageService _packageService = ServiceUnity.Container.Resolve<IPackageService>();

        /// <summary>
        /// 索引XML
        /// </summary>
        XElement _indexXml;

        #endregion

        #region 公开属性

        private static InstanceLazy<MenuStripArchive> _instance =
           new InstanceLazy<MenuStripArchive>(() => new MenuStripArchive());
        public static MenuStripArchive Instance
        {
            get { return _instance.Value; }
        }

        #endregion

        #region 构造

        private MenuStripArchive()
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
                if (_packageService.Current.Container(Constant.PACKAGE_NAVIGATION_MENUSTRIP_INDEX_FILE))
                {
                    //读取当前数据实体索引信息
                    string strIndex = _packageService.Current.GetFileContent(Constant.PACKAGE_NAVIGATION_MENUSTRIP_INDEX_FILE);
                    _indexXml = XElement.Parse(strIndex);
                }
                else
                {
                    //如果当前不存在索引文件，创建一个
                    _indexXml = new XElement("Index", new XElement("MenuStrip"));
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
                _packageService.Current.AddFileContent(_indexXml.ToString(), Constant.PACKAGE_NAVIGATION_MENUSTRIP_INDEX_FILE);
            }
        }

        /// <summary>
        /// 获取用于存储在XML索引文件中的索引节点
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private XElement GetArchiveIndex(MenuEntity entity)
        {
            Debug.Assert(entity != null, "entity 为 null");

            XElement ele = new XElement("Entity",
                new XAttribute("Id", entity.Id),
                new XAttribute("Name", entity.Name),
                new XAttribute("Code", entity.Code),
                new XAttribute("Sys", entity.Sys),
                new XAttribute("ParentId", entity.ParentId));

            return ele;
        }

        #endregion

        #region 公开方法

        /// <summary>
        /// 添加一个菜单项
        /// </summary>
        /// <param name="entity"></param>
        public void Add(MenuEntity entity)
        {
            Debug.Assert(entity != null, "MenuEntity 为 null");

            if (entity == null)
                return;

            //添加索引信息
            _indexXml.XPathSelectElement(XPATH_Index_MenuStrip).Add(GetArchiveIndex(entity));

            SaveIndexFile();

            string xml = entity.ToXml();
            XElement xElement = XElement.Parse(xml);

            //添加数据实体文件
            _packageService.Current.AddFileContent(xml, 
                Path.Combine(Constant.PACKAGE_NAVIGATION_MENUSTRIP_FOLDER, entity.Id));

            _cachingService.Add(entity.Id, xElement);

            //发布事件
            MenuStripEventArgs args = new MenuStripEventArgs(entity);
            _eventAggregator.GetEvent<MenuStripItemAddedEvent>().Publish(args);
        }

        /// <summary>
        /// 获取父级菜单Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetParentId(string id)
        {
            Debug.Assert(String.IsNullOrEmpty(id) == false, "id 为空");

            if (String.IsNullOrEmpty(id))
                return String.Empty;

            XElement element = _indexXml.XPathSelectElement(String.Format(XPATH_Index_SelectMenuStrip, id));
            Debug.Assert(element != null, "未找到指定主菜单的索引记录");

            if (element == null)
                return String.Empty;
            else
            {
                return element.Attribute("ParentId").Value;
            }
        }

        /// <summary>
        /// 获取菜单的路径，如“文件/打开”，包括其自身的完整路径
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string GetPath(string id)
        {
            //顶级
            if (String.IsNullOrEmpty(id))
                return string.Empty;

            StringBuilder path = new StringBuilder();

            GetParentPath(id, path);

            return path.ToString();
        }

        /// <summary>
        /// 通过递归查找菜单项的完整路径
        /// </summary>
        /// <param name="code"></param>
        /// <param name="xmlDoc"></param>
        /// <param name="path"></param>
        private void GetParentPath(string id, StringBuilder path)
        {
            XElement element = _indexXml.XPathSelectElement(String.Format(XPATH_Index_SelectMenuStrip, id));
            path.Insert(0, element.Attribute("Name").Value + "/");

            XElement elementParent = _indexXml.XPathSelectElement(
                String.Format(XPATH_Index_SelectMenuStrip, element.Element("ParentId").Value));

            if (elementParent == null)
                return;

            GetParentPath(elementParent.Attribute("Id").Value, path);
        }

        /// <summary>
        /// 获取菜单的层级
        /// 顶层节点为0
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public int GetLayer(string id)
        {
            if (String.IsNullOrEmpty(id))
                return -1;

            int layer = -1;

            GetLayerCount(id, ref layer);

            return layer;
        }

        /// <summary>
        /// 通过递归累计菜单项的层级
        /// </summary>
        /// <param name="code"></param>
        /// <param name="layer"></param>
        private void GetLayerCount(string id, ref int layer)
        {
            XElement element = _indexXml.XPathSelectElement(String.Format(XPATH_Index_SelectMenuStrip, id));
            if (element == null)
                return;

            layer++;

            if (element.Attribute("ParentId").Value == String.Empty)
                return;

            XElement elementParent = _indexXml.XPathSelectElement(
                String.Format(XPATH_Index_SelectMenuStrip, element.Attribute("ParentId").Value));

            GetLayerCount(elementParent.Attribute("Id").Value, ref layer);
        }

        /// <summary>
        /// 将指定的菜单项移动到另一个菜单项之前
        /// </summary>
        /// <param name="code"></param>
        /// <param name="beforeCode"></param>
        public void MoveBefore(string id, string beforeId)
        {
            //移动索引文件中的顺利即可
            //因为现在菜单实体的XML全部分开存放了，不存在顺序问题，顺序是由index文件中的顺序确定的

            XElement element = _indexXml.XPathSelectElement(String.Format(XPATH_Index_SelectMenuStrip, id));
            XElement beforeElement = _indexXml.XPathSelectElement(String.Format(XPATH_Index_SelectMenuStrip, beforeId));

            Debug.Assert(element != null && beforeId != null, "指定的节点没有找到");

            if (element == null || beforeId == null)
                return;

            XElement elementNew = new XElement(element);
            beforeElement.AddBeforeSelf(elementNew);
            element.Remove();

            SaveIndexFile();

            //发布事件
            MenuStripItemMoveBeforeEventArgs args = new MenuStripItemMoveBeforeEventArgs(id, beforeId);
            _eventAggregator.GetEvent<MenuStripItemMoveBeforeEvent>().Publish(args);
        }

        /// <summary>
        /// 将指定的菜单项移动到另一个菜单项之后
        /// </summary>
        /// <param name="code"></param>
        /// <param name="afterCode"></param>
        public void MoveAfter(string id, string afterId)
        {
            XElement element = _indexXml.XPathSelectElement(String.Format(XPATH_Index_SelectMenuStrip, id));
            XElement afterElement = _indexXml.XPathSelectElement(String.Format(XPATH_Index_SelectMenuStrip, afterId));

            Debug.Assert(element != null && afterId != null, "指定的节点没有找到");

            if (element == null || afterId == null)
                return;

            XElement elementNew = new XElement(element);
            afterElement.AddAfterSelf(elementNew);
            element.Remove();

            SaveIndexFile();

            //发布事件
            MenuStripItemMoveAfterEventArgs args = new MenuStripItemMoveAfterEventArgs(id, afterId);
            _eventAggregator.GetEvent<MenuStripItemMoveAfterEvent>().Publish(args);
        }

        /// <summary>
        /// 删除菜单项，及其所有子项
        /// </summary>
        /// <param name="code"></param>
        public void Delete(MenuEntity entity)
        {
            Debug.Assert(entity != null, "menuEntity 为 null");

            if (entity == null)
                return;

            //移除索引信息
            XElement element = _indexXml.XPathSelectElement(String.Format(XPATH_Index_SelectMenuStrip, entity.Id));
            Debug.Assert(element != null, "删除主菜单索引时未找到指定主菜单的索引记录");
            if (element != null)
            {
                element.Remove();
                SaveIndexFile();

                //移除实体文件
                _packageService.Current.DeleteFile(Path.Combine(Constant.PACKAGE_NAVIGATION_MENUSTRIP_FOLDER, entity.Id));

                _cachingService.Remove(entity.Id);

                //通过递归删除所有子菜单项
                DeleteChild(entity.Id);

                //发布事件
                MenuStripEventArgs args = new MenuStripEventArgs(entity);
                _eventAggregator.GetEvent<MenuStripItemRemovedEvent>().Publish(args);
            }
        }

        /// <summary>
        /// 通过递归删除指定id下的所有子菜单项
        /// 这里参数用string id，而不是用实体对象的原因是递归删除时，只到xml里取到id即可
        /// 无需初始化实体对象
        /// </summary>
        /// <param name="menuEntity"></param>
        private void DeleteChild(string id)
        {
            //到index索引文件里找ParentId==menuEntity.Id的，删除之，并删除相应的实体文件
            //继续到index索引文件里找ParentId为刚才删除的项的id的，如此递归

            Debug.Assert(String.IsNullOrEmpty(id) == false, "id 为空");

            //选出父节点为指定 Id 的所有子节点
            foreach (XElement element in
                _indexXml.XPathSelectElements(String.Format(XPATH_Index_SelectMenuStrip_ByParentId, id)))
            {
                //移除index文件中的节点
                element.Remove();

                string deletedId = element.Attribute("Id").Value;

                //移除实体文件
                _packageService.Current.DeleteFile(Path.Combine(Constant.PACKAGE_NAVIGATION_MENUSTRIP_FOLDER, deletedId));

                _cachingService.Remove(deletedId);

                DeleteChild(deletedId);
            }
           
            //全部删除后，保存index文件
            SaveIndexFile();
        }

        /// <summary>
        /// 获取一个菜单项实例（根据Id）
        /// 包括其所有子项
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MenuEntity GetMenuEntity(string id)
        {
            XElement cachingElement = (XElement)_cachingService.GetData(id);
            string strEntity;
            if (cachingElement == null)
            {
                string file = Path.Combine(Constant.PACKAGE_NAVIGATION_MENUSTRIP_FOLDER, id);
                bool fileExist = _packageService.Current.Container(file);

                Debug.Assert(fileExist, "菜单文件不存在");
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

            MainMenuEntityDev entity = new MainMenuEntityDev();
            entity.FromXml(strEntity);
            entity.Menus.AddRange(GetMenuEntityList(entity.Id).ToArray());

            return entity;
        }

        /// <summary>
        /// 更新（编辑）一个菜单项
        /// </summary>
        /// <param name="entity"></param>
        public void Update(MenuEntity entity)
        {
            Debug.Assert(entity != null, "mainMenuEntity 为 null");

            if (entity == null)
                return;

            //更新索引信息
            XElement element = _indexXml.XPathSelectElement(String.Format(XPATH_Index_SelectMenuStrip, entity.Id));
            Debug.Assert(element != null, "更新主菜单索引时未找到指定主菜单的索引记录");
            if (element != null)
            {
                element.ReplaceWith(GetArchiveIndex(entity));
                SaveIndexFile();

                string xml = entity.ToXml();
                XElement xElement = XElement.Parse(xml);

                //更新实体文件
                _packageService.Current.AddFileContent(xml, 
                    Path.Combine(Constant.PACKAGE_NAVIGATION_MENUSTRIP_FOLDER, entity.Id));

                _cachingService.Add(entity.Id, xElement);

                //发布事件
                MenuStripEventArgs args = new MenuStripEventArgs(entity);
                _eventAggregator.GetEvent<MenuStripItemUpdatedEvent>().Publish(args);
            }
        }

        /// <summary>
        /// 获取顶层菜单集合
        /// 并递归构建菜单的所有子级
        /// </summary>
        /// <returns></returns>
        public MenuEntityCollection GetMainMenuEntityCollection()
        {
            //先从索引文件中取出所有ParentId==""的顶层菜单
            //然后递归构建所有子级菜单

            MenuEntityCollection collection = new MenuEntityCollection();

            foreach (XElement element in _indexXml.XPathSelectElements(
                String.Format(XPATH_Index_SelectMenuStrip_ByParentId, String.Empty)))
            {
                MainMenuEntityDev menu = (MainMenuEntityDev)GetMenuEntity(element.Attribute("Id").Value);
                collection.Add(menu);
            }

            return collection;
        }

        /// <summary>
        /// 提交
        /// 根据MenuEntity的Id进行判断，如果已存在，调用update，如果不存在，调用add
        /// </summary>
        /// <param name="dataEntity"></param>
        public void Commit(MenuEntity entity)
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
        /// 获取菜单项列表
        /// </summary>
        /// <returns></returns>
        public List<MenuEntity> GetMenuEntityList()
        {
            return GetMenuEntityList(String.Empty);
        }

        /// <summary>
        /// 获取属于指定菜单（Id）的子菜单项列表
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<MenuEntity> GetMenuEntityList(string parentId)
        {
            XElement[] entityElements = _indexXml.XPathSelectElements(
                String.Format(XPATH_Index_SelectMenuStrip_ByParentId,parentId)).ToArray();

            List<MenuEntity> list = new List<MenuEntity>();

            foreach (XElement element in entityElements)
            {
                MainMenuEntityDev entity = (MainMenuEntityDev)GetMenuEntity(element.Attribute("Id").Value);
                if (entity != null)
                    list.Add(entity);
            }

            return list;
        }

        /// <summary>
        /// 根据Id判断枚举是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool EntityExistById(string id)
        {
            Debug.Assert(String.IsNullOrEmpty(id) == false, "id 为 空 或为 Null");

            XElement element = _indexXml.XPathSelectElement(String.Format(XPATH_Index_SelectMenuStrip, id));
            if (element == null)
                return false;
            else
                return true;
        }

        /// <summary>
        /// 检查指定的主菜单项是否存在by code
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckExistByCode(string code)
        {
            Debug.Assert(String.IsNullOrEmpty(code) == false, "code 为 空 或为 Null");

            XElement element = _indexXml.XPathSelectElement(
                String.Format(XPATH_Index_SelectMenuStrip_ByCode_IgnoreCase, code));
            if (element == null)
                return false;
            else
                return true;
        }

        #endregion

        #region IArchiveService 成员

        Type IArchiveService.CoverType
        {
            get { return typeof(MenuEntity); }
        }

        bool IArchiveService.ActOnSubClass
        {
            get { return true; }
        }

        void IArchiveService.Save(object obj)
        {
            MenuEntity entity = obj as MenuEntity;
            Debug.Assert(entity != null, "entity为null");
            if (entity != null)
                Commit(entity);
        }

        void IArchiveService.Delete(object obj)
        {
            MenuEntity entity = obj as MenuEntity;
            Debug.Assert(entity != null, "entity为null");
            if (entity != null)
                Delete(entity);
        }

        #endregion
    }
}
