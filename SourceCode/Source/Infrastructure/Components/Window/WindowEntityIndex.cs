/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Kernal;
using System.Diagnostics;
using System.Collections;
namespace Sheng.SailingEase.Infrastructure
{
    public class WindowEntityIndex : IEntityIndex
    {
        private bool _initialized = false;
        IWindowComponentService _windowComponentService = ServiceUnity.WindowComponentService;
        private string _id;
        public string Id
        {
            get
            {
                if (_initialized)
                    return Window.Id;
                else
                    return _id;
            }
            protected set
            {
                _id = value;
            }
        }
        public const string Property_Name = "Name";
        private string _name;
        public string Name
        {
            get
            {
                if (_initialized)
                    return Window.Name;
                else
                    return _name;
            }
            protected set
            {
                _name = value;
            }
        }
        private string _code;
        public string Code
        {
            get
            {
                if (_initialized)
                    return Window.Code;
                else
                    return _code;
            }
            protected set
            {
                _code = value;
            }
        }
        private bool _system;
        public bool System
        {
            get
            {
                if (_initialized)
                    return Window.Sys;
                else
                    return _system;
            }
            protected set
            {
                _system = value;
            }
        }
        private string _parentFolderId;
        public string ParentFolderId
        {
            get
            {
                if (_initialized)
                    return Window.FolderId;
                else
                    return _parentFolderId;
            }
            protected set
            {
                _parentFolderId = value;
            }
        }
        private WindowEntity _window;
        public WindowEntity Window
        {
            get
            {
                if (_initialized == false)
                {
                    _window = _windowComponentService.GetWindowEntity(_id);
                    _initialized = true;
                }
                return _window;
            }
        }
        public EntityBase Entity
        {
            get { return Window; }
        }
        public Type EntityType
        {
            get { return typeof(WindowEntity); }
        }
        public WindowEntityIndex()
        {
        }
        public WindowEntityIndex(WindowEntity entity)
        {
            _window = entity;
            _initialized = true;
        }
        public void FromXml(string xml)
        {
            Debug.Assert(String.IsNullOrEmpty(xml) == false, "xml 为 空");
            if (String.IsNullOrEmpty(xml)) { return; }
            SEXElement element = SEXElement.Parse(xml);
            this.Id = element.Attribute("Id").Value;
            this.Name = element.Attribute("Name").Value;
            this.Code = element.Attribute("Code").Value;
            this.System = element.GetAttributeObject<bool>("Sys", false);
            this.ParentFolderId = element.Attribute("Folder").Value;
        }
    }
    public class FolderEntityIndex : IEntityIndex
    {
        private bool _initialized = false;
        IWindowComponentService _windowComponentService = ServiceUnity.WindowComponentService;
        private string _id;
        public string Id
        {
            get
            {
                if (_initialized)
                    return Folder.Id;
                else
                    return _id;
            }
            protected set
            {
                _id = value;
            }
        }
        public const string Property_Name = "Name";
        private string _name;
        public string Name
        {
            get
            {
                if (_initialized)
                    return Folder.Name;
                else
                    return _name;
            }
            protected set
            {
                _name = value;
            }
        }
        public string Code
        {
            get
            {
                return null;
            }
        }
        public bool System
        {
            get
            {
                return false;
            }
        }
        private string _parentFolderId;
        public string ParentFolderId
        {
            get
            {
                if (_initialized)
                    return Folder.Parent;
                else
                    return _parentFolderId;
            }
            protected set
            {
                _parentFolderId = value;
            }
        }
        private WindowFolderEntity _folder;
        public WindowFolderEntity Folder
        {
            get
            {
                if (_initialized == false)
                {
                    _folder = _windowComponentService.GetFolderEntity(_id);
                    _initialized = true;
                }
                return _folder;
            }
        }
        public EntityBase Entity
        {
            get { return null; }
        }
        public Type EntityType
        {
            get { return typeof(WindowFolderEntity); }
        }
        public const string Property_Items = "Items";
        private List<IEntityIndex> _items = new List<IEntityIndex>();
        public List<IEntityIndex> Items
        {
            get { return _items; }
            set { _items = value; }
        }
        public FolderEntityIndex()
        {
        }
        public FolderEntityIndex(WindowFolderEntity entity)
        {
            _folder = entity;
            _initialized = true;
        }
        public void FromXml(string xml)
        {
            Debug.Assert(String.IsNullOrEmpty(xml) == false, "xml 为 空");
            if (String.IsNullOrEmpty(xml)) { return; }
            SEXElement element = SEXElement.Parse(xml);
            this.Id = element.Attribute("Id").Value;
            this.Name = element.Attribute("Name").Value;
            this.ParentFolderId = element.Attribute("Parent").Value;
        }
    }
}
