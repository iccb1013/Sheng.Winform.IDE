/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using Sheng.SailingEase.Controls.TreeViewGrid;
using Sheng.SailingEase.Infrastructure;
namespace Sheng.SailingEase.Core.Development
{

    class GlobalFormElementTreeModel : ITreeModel
    {
        private IWindowComponentService _windowComponentService;
        private UIElementEntityTypeCollection _allowFormElementControlType = new UIElementEntityTypeCollection();
        public UIElementEntityTypeCollection AllowFormElementControlType
        {
            get
            {
                return this._allowFormElementControlType;
            }
            set
            {
                this._allowFormElementControlType = value;
            }
        }
        public GlobalFormElementTreeModel(UIElementEntityTypeCollection allowFormElementControlType)
        {
            _windowComponentService = ServiceUnity.Container.Resolve<IWindowComponentService>();
            this.AllowFormElementControlType = allowFormElementControlType;
        }
        private TreePath GetPath(GlobalFormElementTreeItemBase item)
        {
            Stack<object> stack = new Stack<object>();
            while (!(item is FolderItem))
            {
                stack.Push(item);
                item = item.Parent;
            }
            return new TreePath(stack.ToArray());
        }
        public System.Collections.IEnumerable GetChildren(TreePath treePath)
        {
            if (treePath.IsEmpty())
            {
                foreach (WindowFolderEntity folder in _windowComponentService.GetFolderCollection())
                {
                    FolderItem item = new FolderItem(folder);
                    yield return item;
                }
                foreach (WindowEntity form in _windowComponentService.GetWindowList(String.Empty))
                {
                    FormItem item = new FormItem(form,null);
                    yield return item;
                }
            }
            else
            {
                List<GlobalFormElementTreeItemBase> items = new List<GlobalFormElementTreeItemBase>();
                GlobalFormElementTreeItemBase parent = treePath.LastNode as GlobalFormElementTreeItemBase;
                if (parent != null)
                {
                    if (parent is FolderItem)
                    {
                        FolderItem folderItem = (FolderItem)parent;
                        WindowFolderEntity formFolderEntity = folderItem.Folder;
                        if (formFolderEntity != null)
                        {
                            foreach (WindowEntity formEntityItem in _windowComponentService.GetWindowList(formFolderEntity.Id))
                            {
                                GlobalFormElementTreeItemBase elementItem = new FormItem(formEntityItem, parent);
                                items.Add(elementItem);
                            }
                        }
                    }
                    WindowEntity formEntity = parent.Entity as WindowEntity;
                    if (formEntity != null)
                    {
                        foreach (UIElement formElementItem in  formEntity.GetFormElement()) 
                        {
                            if (this.AllowFormElementControlType.Allowable(formElementItem))
                            {
                                GlobalFormElementTreeItemBase elementItem = new FormElementItem(formElementItem, parent);
                                items.Add(elementItem);
                            }
                            else
                            {
                                foreach (UIElement innerElement in formElementItem.GetInnerElement())
                                {
                                    if (this.AllowFormElementControlType.Allowable(innerElement))
                                    {
                                        GlobalFormElementTreeItemBase elementItem = new FormElementItem(formElementItem, parent);
                                        items.Add(elementItem);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    UIElement formElement = parent.Entity as UIElement;
                    if (formElement != null)
                    {
                        foreach (UIElement formElementItem in formElement.GetInnerElement())
                        {
                            if (this.AllowFormElementControlType.Allowable(formElementItem) == false)
                                continue;
                            GlobalFormElementTreeItemBase elementItem = new FormElementItem(formElementItem, parent);
                            items.Add(elementItem);
                        }
                    }
                    foreach (GlobalFormElementTreeItemBase item in items)
                        yield return item;
                }
                else
                    yield break;
            }
        }
        public bool IsLeaf(TreePath treePath)
        {
            return false;
        }
        public event EventHandler<TreeModelEventArgs> NodesChanged;
        internal void OnNodesChanged(TreeModelEventArgs args)
        {
            if (NodesChanged != null)
                NodesChanged(this, args);
        }
        public event EventHandler<TreePathEventArgs> StructureChanged;
        public void OnStructureChanged(TreePathEventArgs args)
        {
            if (StructureChanged != null)
                StructureChanged(this, args);
        }
        public event EventHandler<TreeModelEventArgs> NodesInserted;
        internal void OnNodeInserted(GlobalFormElementTreeItemBase parent, int index, Node node)
        {
            if (NodesInserted != null)
            {
                TreeModelEventArgs args = new TreeModelEventArgs(GetPath(parent), new int[] { index }, new object[] { node });
                NodesInserted(this, args);
            }
        }
        public event EventHandler<TreeModelEventArgs> NodesRemoved;
        internal void OnNodeRemoved(GlobalFormElementTreeItemBase parent, int index, Node node)
        {
            if (NodesRemoved != null)
            {
                TreeModelEventArgs args = new TreeModelEventArgs(GetPath(parent), new int[] { index }, new object[] { node });
                NodesRemoved(this, args);
            }
        }
    }
}
