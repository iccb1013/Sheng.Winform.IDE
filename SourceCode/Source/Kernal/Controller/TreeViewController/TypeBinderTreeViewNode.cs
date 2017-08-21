/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Sheng.SailingEase.Kernal
{
    class TypeBinderTreeViewNode : TreeNode, ITypeBinderTreeViewNode
    {
        public TypeBinderTreeViewNode()
        {
        }
        private object _dataBoundItem;
        public object DataBoundItem
        {
            get { return _dataBoundItem; }
            set
            {
                _dataBoundItem = value;
                this.Text = value.ToString();
            }
        }
        private TypeBinderTreeViewNodeCodon _codon;
        public TypeBinderTreeViewNodeCodon Codon
        {
            get { return _codon; }
            set
            {
                _codon = value;
                Debug.Assert(value != null, "Codon为null");
                SetText();
                ImageIndex = SelectedImageIndex = value.ImageIndex;
                if (value.SelectedImageIndex >= 0)
                    SelectedImageIndex = value.SelectedImageIndex;
            }
        }
        public Type ItemType
        {
            get
            {
                if (Codon == null) return null;
                return Codon.ItemType;
            }
        }
        public IList Items
        {
            get
            {
                Debug.Assert(_dataBoundItem != null, "_dataBoundItem 为 null");
                if (_dataBoundItem == null)
                    return null;
                if (String.IsNullOrEmpty(ItemsMember) == false)
                {
                    object listObject = ReflectionPool.GetPropertyValue(_dataBoundItem, ItemsMember);
                    if (listObject == null)
                        return null;
                    return listObject as IList;
                }
                else if (_codon.GetItemsFunc != null)
                {
                    return _codon.GetItemsFunc(_dataBoundItem);
                }
                else
                    return null;
            }
        }
        private IList _ownerList;
        public IList OwnerList
        {
            get { return _ownerList; }
            set { _ownerList = value; }
        }
        public string TextMember
        {
            get
            {
                if (Codon == null) return null;
                return Codon.TextMember;
            }
        }
        public string ItemsMember
        {
            get
            {
                if (Codon == null) return null;
                return Codon.ItemsMember;
            }
        }
        public void Update()
        {
            SetText();
        }
        public void Update(object obj)
        {
            if (_dataBoundItem == null || obj == null)
            {
                Debug.Assert(false, "_dataBoundItem 或 obj 为 null");
                return;
            }
            ReflectionPool.SetPropertyValue(_dataBoundItem, TextMember, 
                ReflectionPool.GetPropertyValue(obj, TextMember));
            SetText();
        }
        public TreeNode Node
        {
            get { return this; }
        }
        public new ITypeBinderTreeViewNode PrevNode
        {
            get
            {
                if (Node.PrevNode == null)
                    return null;
                return Node.PrevNode as ITypeBinderTreeViewNode;
            }
        }
        public new ITypeBinderTreeViewNode NextNode
        {
            get
            {
                if (Node.NextNode == null)
                    return null;
                return Node.NextNode as ITypeBinderTreeViewNode;
            }
        }
        public new ITypeBinderTreeViewNode Parent
        {
            get
            {
                if (Node.Parent == null)
                    return null;
                return Node.Parent as ITypeBinderTreeViewNode;
            }
        }
        public void AddChild(ITypeBinderTreeViewNode node)
        {
            IList list = this.Items;
            if (list == null || node.DataBoundItem == null)
                return;
            if (list.Contains(node.DataBoundItem) == false)
                list.Add(node.DataBoundItem);
            if (this.Nodes.Contains(node.Node) == false)
                this.Nodes.Add(node.Node);
        }
        public new void Remove()
        {
            base.Remove();
            if (this.OwnerList != null && OwnerList.Contains(this.DataBoundItem))
            {
                OwnerList.Remove(this.DataBoundItem);
            }
        }
        private void SetText()
        {
            if (String.IsNullOrEmpty(TextMember) || _dataBoundItem == null)
                return ;
            object textObject = ReflectionPool.GetPropertyValue(_dataBoundItem, TextMember);
            if (textObject == null)
                return;
            this.Text = textObject.ToString();
        }
    }
}
