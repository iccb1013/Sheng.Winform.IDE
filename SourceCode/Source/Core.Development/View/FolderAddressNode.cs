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
using Sheng.SailingEase.Controls.SEAdressBar;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Localisation;
namespace Sheng.SailingEase.Core.Development
{
    class FolderAddressNode : IAddressNode
    {
        private IWindowComponentService _windowComponentService;
        private IAddressNode parent = null;
        private IAddressNode[] children = null;
        private WindowFolderEntity _entity;
        public WindowFolderEntity Entity
        {
            get { return _entity; }
        }
        public FolderAddressNode()
        {
            _windowComponentService = ServiceUnity.Container.Resolve<IWindowComponentService>();
            GenerateRootNode();
        }
        public FolderAddressNode(WindowFolderEntity entity, FolderAddressNode parent)
        {
            _windowComponentService = ServiceUnity.Container.Resolve<IWindowComponentService>();
            this._entity = entity;
            this.parent = parent;
        }
        private void GenerateRootNode()
        {
            if (children != null)
                return;
            this._entity = null;
            this.parent = null;
            CreateChildNodes();
        }
        private void InitChild()
        {
            WindowFolderEntityCollection subFolders = _windowComponentService.GetFolderCollection(this.UniqueID.ToString());
            children = new FolderAddressNode[subFolders.Count];
            for (int i = 0; i < subFolders.Count; i++)
            {
                children[i] = new FolderAddressNode((WindowFolderEntity)subFolders[i], this);
            }
        }
        public IAddressNode Parent
        {
            get
            {
                if (this.parent == null && this.Entity != null)
                {
                    if (String.IsNullOrEmpty(this.Entity.Parent))
                    {
                        this.parent = new FolderAddressNode();
                    }
                    else
                    {
                        WindowFolderEntity folderEntity = _windowComponentService.GetFolderEntity(this.Entity.Parent);
                        if (folderEntity != null)
                            this.parent = new FolderAddressNode(folderEntity, null);
                    }
                }
                return this.parent;
            }
            set { this.parent = value; }
        }
        public string DisplayName
        {
            get
            {
                if (_entity != null)
                    return _entity.Name;
                else
                    return "<Root>";
            }
        }
        public System.Drawing.Bitmap Icon
        {
            get { return IconsLibrary.Folder; }
        }
        public string UniqueID
        {
            get
            {
                if (_entity != null)
                    return _entity.Id;
                else
                    return String.Empty;
            }
        }
        public SEAddressBarDropDown DropDownMenu
        {
            get;
            set;
        }
        public IAddressNode[] Children
        {
            get
            {
                if (this.children == null)
                {
                    UpdateChildNodes();
                }
                return this.children;
            }
        }
        public void CreateChildNodes()
        {
            if (children == null)
            {
                InitChild();
            }
        }
        public void UpdateChildNodes()
        {
            InitChild();
        }
        public IAddressNode GetChild(string uniqueID)
        {
            foreach (IAddressNode node in this.children)
            {
                if (node.UniqueID == uniqueID)
                    return node;
            }
            return null;
        }
        public IAddressNode Clone()
        {
            return new FolderAddressNode(this.Entity, (FolderAddressNode)this.parent);
        }
    }
}
