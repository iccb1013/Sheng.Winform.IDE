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
using Sheng.SailingEase.Core.Development;

namespace Sheng.SailingEase.Components.WindowComponent
{
    class WindowArchive : IArchiveService
    {


        #region XPATH

        /// <summary>
        /// Index/Window
        /// 索引文件中的Window节点
        /// </summary>
        const string XPATH_Index_Window = "/Window";

        /// <summary>
        /// Index/Folder
        /// 索引文件中的Folder节点
        const string XPATH_Index_Folder = "/Folder";

        /// <summary>
        /// Folder/Folder[@Id='{0}']
        /// 从索引文件中选出指定文件夹实体索引
        /// </summary>
        const string XPATH_Index_SelectFolder = "/Folder/Folder[@Id='{0}']";

        /// <summary>
        /// Folder/Folder[@ParentId='{0}']
        /// 从索引文件中选出 父目录 为指定文件夹实体索引
        /// </summary>
        const string XPATH_Index_SelectFolder_ByParentId = "/Folder/Folder[@Parent='{0}']";

        /// <summary>
        ///  Index/Window/Entity[@Id='{0}']
        ///  从索引文件中选出指定实体索引
        /// </summary>
        const string XPATH_index_SelectWindow = "/Window/Entity[@Id='{0}']";

        /// <summary>
        ///  Index/Window/Entity
        ///  从索引文件中选出所有指定实体索引
        /// </summary>
        const string XPATH_Index_Window_Entity = "/Window/Entity";

        /// <summary>
        ///  Index/Window/Entity[@FolderId='{0}']
        ///  从索引文件中选出父节点为指定Id的窗体实体索引
        /// </summary>
        const string XPATH_Index_SelectWindow_ByFolderId = "/Window/Entity[@Folder='{0}']";

        /// <summary>
        ///  Index/Window/Entity[@Code='{0}']
        ///  从索引文件中选出指定实体索引 ，根据code，忽略code大小写
        /// </summary>
        const string XPATH_Index_SelectWindow_ByCode_IgnoreCase =
            "/Window/Entity[translate(@Code,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')=translate('{0}','ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')]";

        const string XPATH_Window_SelectElement_ByCode_ByControlType = "/Elements/Element[@Code='{0}'][@ControlType ='{1}']";

        const string XPATH_Window_SelectElement_ByCode = "/Elements/Element[@Code='{0}']";

        const string XPATH_Window_SelectElement_DataColumn_ByCode = "/Elements/Element[@Code='{0}']/Columns/Column[@Code='{1}']";


        #endregion

        #region 私有成员

        ICachingService _cachingService = ServiceUnity.CachingService;
        IEventAggregator _eventAggregator = ServiceUnity.EventAggregator;
        IPackageService _packageService = ServiceUnity.PackageService;
        IWindowElementContainer _windowElementContainer = ServiceUnity.WindowElementContainer;

        /// <summary>
        /// 索引XML
        /// 不需要缓存，因为其一直在内存中
        /// </summary>
        XElement _indexXml;

        #endregion

        #region 公开属性

        private static InstanceLazy<WindowArchive> _instance =
           new InstanceLazy<WindowArchive>(() => new WindowArchive());
        public static WindowArchive Instance
        {
            get { return _instance.Value; }
        }

        #endregion

        #region 构造

        private WindowArchive()
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
                if (_packageService.Current.Container(Constant.PACKAGE_WINDOW_INDEX_FILE))
                {
                    //读取当前数据实体索引信息
                    string strIndex = _packageService.Current.GetFileContent(Constant.PACKAGE_WINDOW_INDEX_FILE);
                    _indexXml = XElement.Parse(strIndex);
                }
                else
                {
                    //如果当前不存在索引文件，创建一个
                    _indexXml = new XElement("Index", 
                        new XElement("Folder"),
                        new XElement("Window"));
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
                _packageService.Current.AddFileContent(_indexXml.ToString(), Constant.PACKAGE_WINDOW_INDEX_FILE);
            }
        }

        /// <summary>
        /// 获取用于存储在XML索引文件中的索引节点
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private XElement GetArchiveIndex(WindowEntity entity)
        {
            Debug.Assert(entity != null, "entity 为 null");

            XElement ele = new XElement("Entity",
                new XAttribute("Id", entity.Id),
                new XAttribute("Name", entity.Name),
                new XAttribute("Code", entity.Code),
                new XAttribute("Sys", entity.Sys),
                new XAttribute("Folder", entity.FolderId));

            return ele;
        }

        /// <summary>
        /// 获取用于存储在XML索引文件中的索引节点
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private XElement GetArchiveIndex(WindowFolderEntity entity)
        {
            Debug.Assert(entity != null, "entity 为 null");

            //文件夹在索引文件中可直接ToXml存储
            return XElement.Parse(entity.ToXml());

            //XElement ele = new XElement("Entity",
            //    new XAttribute("Id", entity.Id),
            //    new XAttribute("Name", entity.Name),
            //    new XAttribute("ParentId", entity.Parent));

            //return ele;
        }

        /// <summary>
        /// 获取所有窗体的 Id
        /// </summary>
        /// <returns></returns>
        private List<string> GetWindowIds()
        {
            List<string> list = new List<string>();

            foreach (XElement element in _indexXml.XPathSelectElements(XPATH_Index_Window_Entity))
            {
                list.Add(element.Attribute("Id").Value);
            }

            return list;
        }

        #endregion

        #region 公开方法

        #region 目录操作

        /// <summary>
        /// 提交
        /// 根据Id进行判断，如果已存在，调用update，如果不存在，调用add
        /// </summary>
        /// <param name="dataEntity"></param>
        public void Commit(WindowFolderEntity entity)
        {
            Debug.Assert(entity != null, "FormFolderEntity 为 null");

            if (entity == null)
                return;

            if (FolderExistById(entity.Id))
            {
                UpdateFolder(entity);
            }
            else
            {
                AddFolder(entity);
            }
        }

        /// <summary>
        /// 添加文件夹
        /// </summary>
        /// <param name="entity"></param>
        public void AddFolder(WindowFolderEntity entity)
        {
            Debug.Assert(entity != null, "FormFolderEntity 为 null");

            if (entity == null)
                return;

            //添加索引信息
            _indexXml.XPathSelectElement(XPATH_Index_Folder).Add(GetArchiveIndex(entity));

            SaveIndexFile();

            //发布事件
            WindowFolderEventArgs args = new WindowFolderEventArgs(entity);
            _eventAggregator.GetEvent<WindowFolderAddedEvent>().Publish(args);
        }

        //原来是通过返回null，如传入了一个空串，来表示获取根目录（根目录实际上不是一个目录）
        //现在改进为查不到抛异常，是不是根目录外部判断处理，不通过返回null隐式的表达其它含义
        /// <summary>
        /// 获取一个文件夹实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WindowFolderEntity GetFolderEntity(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                Debug.Assert(false, "GetFolderEntity 传入的 id 为空");
                return null;
            }

            XElement element = _indexXml.XPathSelectElement(String.Format(XPATH_Index_SelectFolder, id));

            if (element == null)
            {
                Debug.Assert(false, "GetFolderEntity 失败，返回了 null");
            }

            WindowFolderEntity formFolderEntity = new WindowFolderEntity();
            formFolderEntity.FromXml(element.ToString());
            return formFolderEntity;
        }

        public void DeleteFolder(WindowFolderEntity entity)
        {
            DeleteFolder(entity.Id);
        }

        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="id"></param>
        /// <param name="deleteForm"></param>
        public void DeleteFolder(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                Debug.Assert(false, "DeleteFolder 传入的 id 为空");
            }

            //专门在这里递归删除目录是避免反复调用 SaveIndexFile
            ProcessFolderDelete(id);

            SaveIndexFile();
        }

        /// <summary>
        /// 此方法供DeleteFolder调用
        /// 作用是避免每次递归都SaveXml
        /// </summary>
        /// <param name="id"></param>
        /// <param name="deleteForm"></param>
        private void ProcessFolderDelete(string id)
        {
            WindowFolderEntity folderEntity = GetFolderEntity(id);
            Debug.Assert(folderEntity != null, "folderEntity 为 null");

            //递归当前目录下所有子目录，删除之
            WindowFolderEntityCollection folderCollection = GetFolderCollection(id);
            foreach (WindowFolderEntity folder in folderCollection)
            {
                ProcessFolderDelete(folder.Id);
            }

            //删除窗体
            foreach (XElement element in
                from c in _indexXml.XPathSelectElements(String.Format(XPATH_Index_SelectWindow_ByFolderId, id)) select c)
            {
                string formId = element.Attribute("Id").Value;

                //移除索引信息
                element.Remove();

                //移除实体文件
                _packageService.Current.DeleteFile(Path.Combine(Constant.PACKAGE_WINDOW_FOLDER, formId));

                //移除缓存
                _cachingService.Remove(formId);
            }

            //从索引文件中删除本目录
            _indexXml.XPathSelectElement(String.Format(XPATH_Index_SelectFolder, id)).Remove();

            //发布事件
            WindowFolderEventArgs args = new WindowFolderEventArgs(folderEntity);
            _eventAggregator.GetEvent<WindowFolderRemovedEvent>().Publish(args);
        }

        /// <summary>
        /// 更新一个文件夹
        /// </summary>
        /// <param name="formFolderEntity"></param>
        public void UpdateFolder(WindowFolderEntity entity)
        {
            Debug.Assert(entity != null, "FormFolderEntity 为 null");

            if (entity == null)
                return;

            //更新索引信息
            XElement element = _indexXml.XPathSelectElement(String.Format(XPATH_Index_SelectFolder, entity.Id));
            Debug.Assert(element != null, "更新窗体文件夹索引时未找到指定文件夹的索引记录");
            if (element != null)
            {
                element.ReplaceWith(GetArchiveIndex(entity));
                SaveIndexFile();

                //发布事件
                WindowFolderEventArgs args = new WindowFolderEventArgs(entity);
                _eventAggregator.GetEvent<WindowFolderUpdatedEvent>().Publish(args);
            }
        }

        /// <summary>
        /// 获取根目录下的目录对象集合
        /// 等价于 GetFolderCollection(null)
        /// </summary>
        /// <returns></returns>
        public WindowFolderEntityCollection GetFolderCollection()
        {
            return GetFolderCollection(null);
        }

        /// <summary>
        /// 获取隶属于指定目录的子目录集合
        /// </summary>
        /// <param name="folderId"></param>
        /// <returns></returns>
        public WindowFolderEntityCollection GetFolderCollection(string folderId)
        {
            if (folderId == null)
                folderId = String.Empty;

            WindowFolderEntityCollection collection = new WindowFolderEntityCollection();

            foreach (XElement element in
                _indexXml.XPathSelectElements(String.Format(XPATH_Index_SelectFolder_ByParentId, folderId)))
            {
                WindowFolderEntity formFolderEntity = new WindowFolderEntity();
                //FormFolderEntity在往Index文件里存的时候是直接ToXml的
                //所以此处直接FromXml即可
                formFolderEntity.FromXml(element.ToString());

                collection.Add(formFolderEntity);
            }

            return collection;
        }

        public void MoveFolder(string folderId, string targetFolderId)
        {
            //targetFolderId允许为空，等于让指定folder处于根目录下
            if (String.IsNullOrEmpty(folderId))
            {
                Debug.Assert(false, "folderId 为空");
            }

            Debug.Assert(folderId != targetFolderId, "folderId 等于 targetFolderId");

            //更新索引文件中指定目录节点的 ParentId
            XElement element = _indexXml.XPathSelectElement(String.Format(XPATH_Index_SelectFolder, folderId));
            Debug.Assert(element != null, "指定Id的目录不存在");

            string originalParentId = element.Attribute("Parent").Value;
            element.Attribute("Parent").Value = targetFolderId;
            SaveIndexFile();

            //发布事件
            WindowFolderMovedEventArgs args = new WindowFolderMovedEventArgs(folderId, targetFolderId, originalParentId);
            _eventAggregator.GetEvent<WindowFolderMovedEvent>().Publish(args);
        }

        /// <summary>
        /// 获取指定目录对象的完整（Id）路径
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public string GetFolderFullPath(WindowFolderEntity folder)
        {
            if (folder == null)
            {
                Debug.Assert(false, "folder 为 null");
                throw new ArgumentNullException();
            }

            return GetFolderFullPath(folder.Id).TrimStart('/');
        }

        private string GetFolderFullPath(string id)
        {
            //当递归到根目录时，跳出
            if (String.IsNullOrEmpty(id))
                return String.Empty;

            XElement element = _indexXml.XPathSelectElement(String.Format(XPATH_Index_SelectFolder, id));
            string parentId = element.Attribute("Parent").Value;

            return GetFolderFullPath(parentId) + "/" + id;
        }

        /// <summary>
        /// 根据Id判断枚举是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool FolderExistById(string id)
        {
            Debug.Assert(String.IsNullOrEmpty(id) == false, "id 为 空 或为 Null");

            XElement element = _indexXml.XPathSelectElement(String.Format(XPATH_Index_SelectFolder, id));
            if (element == null)
                return false;
            else
                return true;
        }

        #endregion

        #region 窗体操作

        public WindowEntityCollection GetAllWindow()
        {
            WindowEntityCollection list = new WindowEntityCollection();

            foreach (XElement element in _indexXml.XPathSelectElements(XPATH_Index_Window_Entity))
            {
                WindowEntity entity = GetWindowEntity(element.Attribute("Id").Value);
                list.Add(entity);
            }

            return list;
        }

        public WindowEntityCollection GetWindowList(string folderId)
        {
            WindowEntityCollection list = new WindowEntityCollection();

            foreach (XElement element in _indexXml.XPathSelectElements(
                String.Format(XPATH_Index_SelectWindow_ByFolderId, folderId)))
            {
                WindowEntity entity = GetWindowEntity(element.Attribute("Id").Value);
                list.Add(entity);
            }

            return list;
        }

        /// <summary>
        /// 提交
        /// 根据Id进行判断，如果已存在，调用update，如果不存在，调用add
        /// </summary>
        /// <param name="dataEntity"></param>
        public void Commit(WindowEntity entity)
        {
            Debug.Assert(entity != null, "FormEntity 为 null");

            if (entity == null)
                return;

            if (CheckExistById(entity.Id))
            {
                Update(entity);
            }
            else
            {
                Add(entity);
            }
        }

        public void Add(WindowEntity entity)
        {
            Debug.Assert(entity != null, "FormEntity 为 null");

            if (entity == null)
                return;

            //添加索引信息
            _indexXml.XPathSelectElement(XPATH_Index_Window).Add(GetArchiveIndex(entity));

            SaveIndexFile();

            string xml = entity.ToXml();
            XElement xElement = XElement.Parse(xml);

            //添加实体文件
            _packageService.Current.AddFileContent(xml,
                Path.Combine(Constant.PACKAGE_WINDOW_FOLDER, entity.Id));

            _cachingService.Add(entity.Id, xElement);

            //发布事件
            WindowEventArgs args = new WindowEventArgs(entity);
            _eventAggregator.GetEvent<WindowAddedEvent>().Publish(args);
        }

        public void Delete(WindowEntity entity)
        {
            Debug.Assert(entity != null, "FormEntity 为 null");

            if (entity == null)
                return;

            Delete(entity.Id);
        }

        public void Delete(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                Debug.Assert(false, "id 为 空");
                return;
            }

            //移除索引信息
            XElement element = _indexXml.XPathSelectElement(String.Format(XPATH_index_SelectWindow, id));
            Debug.Assert(element != null, "删除窗体索引时未找到指定窗体的索引记录");
            if (element != null)
            {
                element.Remove();
                SaveIndexFile();

                //移除实体文件
                _packageService.Current.DeleteFile(Path.Combine(Constant.PACKAGE_WINDOW_FOLDER, id));

                //移除缓存
                _cachingService.Remove(id);

                //发布事件
                WindowRemovedEventArgs args = new WindowRemovedEventArgs(id);
                _eventAggregator.GetEvent<WindowRemovedEvent>().Publish(args);
            }
        }

        public void Update(WindowEntity entity)
        {
            Debug.Assert(entity != null, "FormEntity 为 null");

            if (entity == null)
                return;

            //更新索引信息
            XElement element = _indexXml.XPathSelectElement(String.Format(XPATH_index_SelectWindow, entity.Id));
            Debug.Assert(element != null, "更新窗体索引时未找到指定窗体的索引记录");
            if (element != null)
            {
                element.ReplaceWith(GetArchiveIndex(entity));
                SaveIndexFile();

                string xml = entity.ToXml();
                XElement xElement = XElement.Parse(xml);

                //更新实体文件
                _packageService.Current.AddFileContent(xml,
                    Path.Combine(Constant.PACKAGE_WINDOW_FOLDER, entity.Id));

                //更新缓存
                _cachingService.Add(entity.Id, xElement);

                //发布事件
                WindowEventArgs args = new WindowEventArgs(entity);
                _eventAggregator.GetEvent<WindowUpdatedEvent>().Publish(args);
            }
        }

        public WindowEntity GetWindowEntity(string id)
        {
            //首先尝试从缓存中获取
            XElement cachingElement = (XElement)_cachingService.GetData(id);
            string strEntity;
            if (cachingElement == null)
            {
                string file = Path.Combine(Constant.PACKAGE_WINDOW_FOLDER, id);
                bool fileExist = _packageService.Current.Container(file);

                Debug.Assert(fileExist, "窗体文件不存在");
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

            WindowEntity entity = _windowElementContainer.CreateWindowEntity();
            entity.FromXml(strEntity);

            return entity;
        }

        /// <summary>
        /// 将指定的窗体移动到指定的目录下
        /// </summary>
        /// <param name="windowId"></param>
        /// <param name="folderId"></param>
        public void MoveWindow(string windowId, string folderId)
        {
            //folderId 允许为空，等于说把窗体移到根目录
            if (String.IsNullOrEmpty(windowId))
            {
                Debug.Assert(false, "formId 为空");
            }

            //此处均直接操作XML，不实例化对象，性能优先

            //更新索引文件中指定窗体节点的 Folder
            XElement element = _indexXml.XPathSelectElement(String.Format(XPATH_index_SelectWindow, windowId));
            Debug.Assert(element != null, "指定Id的窗体不存在");

            string originalFolderId = element.Attribute("Folder").Value;
            element.Attribute("Folder").Value = folderId;
            SaveIndexFile();

            string file = Path.Combine(Constant.PACKAGE_WINDOW_FOLDER, windowId);

            //更新窗体实体文件中的 Folder
            //首先尝试从缓存中获取
            XElement entityElement = (XElement)_cachingService.GetData(windowId);
            string strEntity;
            if (entityElement == null)
            {
                bool fileExist = _packageService.Current.Container(file);

                Debug.Assert(fileExist, "窗体文件不存在");
                if (fileExist == false)
                    return;

                strEntity = _packageService.Current.GetFileContent(file);
                entityElement = XElement.Parse(strEntity);
                _cachingService.Add(windowId, entityElement);
            }

            entityElement.XPathSelectElement("/Folder").Value = folderId;

            //保存实体文件
            _packageService.Current.AddFileContent(entityElement.ToString(), file);

            //发布事件
            WindowMovedEventArgs args = new WindowMovedEventArgs(windowId, folderId, originalFolderId);
            _eventAggregator.GetEvent<WindowMovedEvent>().Publish(args);

        }

        /// <summary>
        /// 根据code判断窗体是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckExistByCode(string code)
        {
            Debug.Assert(String.IsNullOrEmpty(code) == false, "code 为 空 或为 Null");

            XElement element = _indexXml.XPathSelectElement(
                String.Format(XPATH_Index_SelectWindow_ByCode_IgnoreCase, code));
            if (element == null)
                return false;
            else
                return true;
        }

        /// <summary>
        /// 根据Id判断枚举是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckExistById(string id)
        {
            Debug.Assert(String.IsNullOrEmpty(id) == false, "id 为 空 或为 Null");

            XElement element = _indexXml.XPathSelectElement(String.Format(XPATH_index_SelectWindow, id));
            if (element == null)
                return false;
            else
                return true;
        }

        /// <summary>
        /// 检查指定的窗体元素是否存在,使用code,(在所有窗体范围内)
        /// </summary>
        /// <param name="code"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool CheckElementExistByCode(string code, UIElementEntityProvideAttribute type)
        {
            //得到所有窗体id列表，迭代，从缓存中取出XML，使用XPath进行查询

            List<string> idList =  GetWindowIds();
            foreach (var id in idList)
            {
                XElement xml = (XElement)_cachingService.GetData(id);
                if (xml == null)
                {
                    //这里不用再缓存了 GetWindowEntity方法就做了缓存
                    xml = XElement.Parse(GetWindowEntity(id).ToXml());
                }
                if (xml.XPathSelectElement(String.Format(
                    XPATH_Window_SelectElement_ByCode_ByControlType,code,type.Code)) == null)
                    continue;
                else
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 检查指定的窗体元素是否存在,使用code,(在所有窗体范围内)
        /// </summary>
        /// <param name="code"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool CheckElementExistByCode(string code)
        {
            List<string> idList = GetWindowIds();
            foreach (var id in idList)
            {
                XElement xml = (XElement)_cachingService.GetData(id);
                if (xml == null)
                {
                    //这里不用再缓存了 GetWindowEntity方法就做了缓存
                    xml = XElement.Parse(GetWindowEntity(id).ToXml());
                }
                if (xml.XPathSelectElement(String.Format(
                    XPATH_Window_SelectElement_ByCode, code)) == null)
                    continue;
                else
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 检查指定的数据列表数据列是否存在,使用code(在所有窗体范围内,在指定的数据列表范围内)
        /// </summary>
        /// <param name="dataListCode"></param>
        /// <param name="columnCode"></param>
        /// <returns></returns>
        public bool CheckDataColumnExistByCode(string dataListCode, string columnCode)
        {
            List<string> idList = GetWindowIds();
            foreach (var id in idList)
            {
                XElement xml = (XElement)_cachingService.GetData(id);
                if (xml == null)
                {
                    //这里不用再缓存了 GetWindowEntity方法就做了缓存
                    xml = XElement.Parse(GetWindowEntity(id).ToXml());
                }
                if (xml.XPathSelectElement(String.Format(
                    XPATH_Window_SelectElement_DataColumn_ByCode, dataListCode, columnCode)) == null)
                    continue;
                else
                    return true;
            }
            return false;
        }
        
        #endregion

        #region IEntityIndex

        /// <summary>
        /// 获取指定目录下的窗体Index对象集合
        /// </summary>
        /// <param name="folderId"></param>
        /// <returns></returns>
        public List<IEntityIndex> GetWindowIndexList(string folderId)
        {
            List<IEntityIndex> list = new List<IEntityIndex>();

            foreach (XElement element in _indexXml.XPathSelectElements(
                String.Format(XPATH_Index_SelectWindow_ByFolderId, folderId)))
            {
                WindowEntityIndex entity = new WindowEntityIndex();
                entity.FromXml(element.ToString());
                list.Add(entity);
            }

            return list;
        }

        public List<IEntityIndex> GetIndexList()
        {
            return GetIndexList(null);
        }

        //此处需要把Folder封装在Index里面了
        /// <summary>
        /// 获取指定目录下的目录和窗体的Index集合
        /// 如果folderId为空串，则从根目录获取
        /// 目录将进行递归，将获取所有子目录下的目录和窗体
        /// </summary>
        /// <param name="folderId"></param>
        /// <returns></returns>
        public List<IEntityIndex> GetIndexList(string folderId)
        {
            List<IEntityIndex> list = new List<IEntityIndex>();

            foreach (XElement element in _indexXml.XPathSelectElements(
                String.Format(XPATH_Index_SelectFolder_ByParentId, folderId)))
            {
                FolderEntityIndex entity = new FolderEntityIndex();
                entity.FromXml(element.ToString());
                entity.Items = GetIndexList(entity.Id);
                list.Add(entity);
            }

            list.AddRange(GetWindowIndexList(folderId));

            return list;
        }

        #endregion

        #endregion

        #region IArchiveService 成员

        Type IArchiveService.CoverType
        {
            get { return typeof(WindowEntity); }
        }

        bool IArchiveService.ActOnSubClass
        {
            get { return true; }
        }

        void IArchiveService.Save(object obj)
        {
            WindowEntity entity = obj as WindowEntity;
            Debug.Assert(entity != null, "entity为null");
            if (entity != null)
                Commit(entity);
        }

        void IArchiveService.Delete(object obj)
        {
            WindowEntity entity = obj as WindowEntity;
            Debug.Assert(entity != null, "entity为null");
            if (entity != null)
                Delete(entity);
        }

        #endregion
    }
}
