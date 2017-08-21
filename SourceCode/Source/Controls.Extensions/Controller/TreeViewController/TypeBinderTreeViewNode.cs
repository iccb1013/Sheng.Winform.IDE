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
using Sheng.SailingEase.Kernal;
namespace Sheng.SailingEase.Controls.Extensions
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
                if (_codon != null)
                {
                    SetText();
                    ImageIndex = SelectedImageIndex = value.ImageIndex;
                    if (value.SelectedImageIndex >= 0)
                        SelectedImageIndex = value.SelectedImageIndex;
                }
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
        private IList _items;
        public IList Items
        {
            get
            {
                if (_codon == null)
                {
                    Debug.Assert(_codon != null, "_codon 为 null");
                    return null;
                }
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
                    if (_items == null)
                        _items = _codon.GetItemsFunc(_dataBoundItem);
                    return _items;
                }
                else
                    return null;
            }
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
        public object RelationParentMemberValue
        {
            get { return GetMemberValue(Codon.RelationParentMember); }
        }
        public object RelationChildMemberValue
        {
            get { return GetMemberValue(Codon.RelationChildMember); }
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
        private new void Remove()
        {
            Debug.Assert(false, "删除节点必须调用 controller 中的相关方法。");
        }
        private void SetText()
        {
            object textValue = GetMemberValue(TextMember);
            if (textValue == null)
                this.Text = String.Empty;
            else
                this.Text = textValue.ToString();
        }
        private object GetMemberValue(string member)
        {
            if (String.IsNullOrEmpty(member) || _dataBoundItem == null)
            {
                return null;
            }
            object result = ReflectionPool.GetPropertyValue(_dataBoundItem, member);
            return result;
        }
    }
}
