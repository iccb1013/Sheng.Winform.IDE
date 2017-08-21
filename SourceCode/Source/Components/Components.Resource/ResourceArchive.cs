using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using Microsoft.Practices.Composite.Events;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Kernal;

namespace Sheng.SailingEase.Components.ResourceComponent
{
  
    class ResourceArchive
    {
        #region XPath

        /// <summary>
        /// Index/Resource
        /// 索引文件中的DataEntity节点
        /// </summary>
        const string XPATH_Index_Resource = "/Resource";

        /// <summary>
        ///  Index/Resource/File[@Name='{0}']
        ///  从索引文件中选出指定实体索引
        /// </summary>
        const string XPATH_Index_SelectResource = 
            "/Resource/File[translate(@Name,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')=translate('{0}','ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')]";

        #endregion

        #region 私有成员

        ICachingService _cachingService = ServiceUnity.CachingService;
        IEventAggregator _eventAggregator = ServiceUnity.EventAggregator;
        IPackageService _packageService = ServiceUnity.PackageService;

        /// <summary>
        /// 数据实体整个XML
        /// 数据实体目前只使用一个XML，所有数据实体，数据项皆存在这个XML里
        /// </summary>
        XElement _indexXml;

        #endregion

        #region 公开属性

        private static InstanceLazy<ResourceArchive> _instance =
            new InstanceLazy<ResourceArchive>(() => new ResourceArchive());
        public static ResourceArchive Instance
        {
            get { return _instance.Value; }
        }

        #endregion

        #region 构造

        private ResourceArchive()
        {
            SubscribeEvent();
        }

        #endregion

        #region 公开方法

        #region 基本

        /// <summary>
        /// 判断指定的资源文件是否存在,根据资源文件名
        /// </summary>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        public bool Container(string resourceName)
        {
            Debug.Assert(String.IsNullOrEmpty(resourceName) == false, "resourceName 为空");

            if (String.IsNullOrEmpty(resourceName))
                throw new ArgumentNullException();

            return _packageService.Current.Container(
                Path.Combine(Constant.PACKAGE_RESOURCE_FOLDER, resourceName));//Path.GetFileName(resourceName)
        }

        /// <summary>
        /// 添加资源文件
        /// </summary>
        /// <param name="path"></param>
        public void Add(string path)
        {
            Debug.Assert(String.IsNullOrEmpty(path) == false);

            if (String.IsNullOrEmpty(path))
                return;

            Debug.Assert(File.Exists(path));

            if (File.Exists(path) == false)
                return;

            string fileName =  Path.GetFileName(path);
            if (fileName.ToLower() == "index")
            {
                fileName += "1";
            }

            //判断文件是否已经存在
            bool rewrite = Container(fileName);

            //添加文件（如果文件已经存在于包内了，自动覆盖掉了）
            _packageService.Current.AddFile(path,
                Path.Combine(Constant.PACKAGE_RESOURCE_FOLDER, fileName));

            if (rewrite)
            {
                //从缓存中清除原来的资源
                _cachingService.Remove(fileName);
                //从索引文件中删除原索引节点
                _indexXml.XPathSelectElement(String.Format(XPATH_Index_SelectResource, fileName)).Remove();

            }

            //添加索引信息
            _indexXml.XPathSelectElement(XPATH_Index_Resource).Add(GetArchiveIndex(fileName));
            SaveIndexFile();

            //发布事件
            ResourceEventArgs args = new ResourceEventArgs(fileName);
            if (rewrite)
            {
                _eventAggregator.GetEvent<ResourceUpdatedEvent>().Publish(args);
            }
            else
            {
                _eventAggregator.GetEvent<ResourceAddedEvent>().Publish(args);
            }
        }

        /// <summary>
        /// 添加资源文件
        /// </summary>
        /// <param name="path"></param>
        public void Add(string[] pathArray)
        {
            Debug.Assert(pathArray != null);

            if (pathArray == null || pathArray.Length == 0)
                return;

            foreach (string path in pathArray)
            {
                Add(path);
            }
        }

        public void Remove(string resourceName)
        {
            Debug.Assert(String.IsNullOrEmpty(resourceName) == false, "resourceName 为空");

            if (String.IsNullOrEmpty(resourceName))
                throw new ArgumentNullException();

            _packageService.Current.DeleteFile(Path.Combine(Constant.PACKAGE_RESOURCE_FOLDER, resourceName));

            _cachingService.Remove(resourceName);

            //从索引文件中删除索引节点
            _indexXml.XPathSelectElement(String.Format(XPATH_Index_SelectResource, resourceName)).Remove();
            SaveIndexFile();

            ResourceEventArgs args = new ResourceEventArgs(resourceName);
            _eventAggregator.GetEvent<ResourceRemovedEvent>().Publish(args);
        }

        /// <summary>
        /// 获取资源文件的文件流
        /// </summary>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        public Stream GetStream(string resourceName)
        {
            if (String.IsNullOrEmpty(resourceName))
            {
                Debug.Assert(false, "resourceName 为空");
                throw new ArgumentNullException();
            }

            Stream stream = (Stream)_cachingService.GetData(resourceName);
            if (stream == null)
            {
                string file = Path.Combine(Constant.PACKAGE_RESOURCE_FOLDER, resourceName);

                if (_packageService.Current.Container(file) == false)
                {
                    Debug.Assert(false, resourceName + " 不存在");
                    throw new FileNotFoundException();
                }

                stream = _packageService.Current.GetFileStream(file);

                _cachingService.Add(resourceName, stream);
            }

            return stream;
        }

        public List<ResourceInfo> GetResourceList()
        {
            return GetResourceList(null);
        }

        /// <summary>
        /// 获取资源对象列表，根据指定的文件名过滤
        /// 如 *.jpg;*.bmp
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<ResourceInfo> GetResourceList(string filter)
        {
            return GetResourceList<ResourceInfo>(filter);
        }

        public List<T> GetResourceList<T>(string filter) where T : ResourceInfo
        {
            List<T> resourceInfoList = new List<T>();

            //取出当前所有相关的资源文件列表
            List<string> resourceFiles = _packageService.Current.GetFileList(Constant.PACKAGE_RESOURCE_FOLDER, filter);

            foreach (var file in resourceFiles)
            {
                T resourceInfo = ResourceInfoFactory.Instance.GetResourceInfo<T>(
                    Path.GetFileName(file), GetStream(file));

                resourceInfoList.Add(resourceInfo);
            }

            return resourceInfoList;
        }

        public T GetResource<T>(string name) where T : ResourceInfo
        {
            T resourceInfo = ResourceInfoFactory.Instance.GetResourceInfo<T>(name, GetStream(name));
            return resourceInfo;
        }

        #endregion

        #region 扩展

        /// <summary>
        /// 获取当前所有图像资源
        /// </summary>
        /// <returns></returns>
        public List<ImageResourceInfo> GetImageResoruceList()
        {
            return GetResourceList<ImageResourceInfo>(Constant.RESOURCE_IMAGE_FILE_FILTER);
        }

        public ImageResourceInfo GetImageResoruce(string name)
        {
            return GetResource<ImageResourceInfo>(name);
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
                if (_packageService.Current.Container(Constant.PACKAGE_RESOURCE_INDEX_FILE))
                {
                    //读取当前数据实体索引信息
                    string strIndex = _packageService.Current.GetFileContent(Constant.PACKAGE_RESOURCE_INDEX_FILE);
                    _indexXml = XElement.Parse(strIndex);
                }
                else
                {
                    //如果当前不存在索引文件，创建一个
                    _indexXml = new XElement("Index", new XElement("Resource"));
                    SaveIndexFile();
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
        /// 保存索引文件
        /// </summary>
        private void SaveIndexFile()
        {
            Debug.Assert(_indexXml != null, "IndexXml 为 null");

            if (_indexXml != null)
            {
                _packageService.Current.AddFileContent(_indexXml.ToString(), Constant.PACKAGE_RESOURCE_INDEX_FILE);
            }
        }

        private XElement GetArchiveIndex(string fileName)
        {
            Debug.Assert(String.IsNullOrEmpty(fileName) == false, "fileName 为空");

            XElement ele = new XElement("File",
                new XAttribute("Type", "Image"),
                new XAttribute("Name", fileName));

            return ele;
        }

        #endregion

    }
}
