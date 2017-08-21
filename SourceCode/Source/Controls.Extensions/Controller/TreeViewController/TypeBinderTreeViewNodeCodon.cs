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
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections;
namespace Sheng.SailingEase.Controls.Extensions
{
    public class TypeBinderTreeViewNodeCodon
    {
        public Type DataBoundType
        {
            get;
            set;
        }
        private bool _actOnSubClass = false;
        public bool ActOnSubClass
        {
            get { return _actOnSubClass; }
            set { _actOnSubClass = value; }
        }
        public string TextMember
        {
            get;
            set;
        }
        public Type ItemType
        {
            get;
            set;
        }
        public string ItemsMember
        {
            get;
            set;
        }
       
        public string RelationParentMember
        {
            get;
            set;
        }
        public string RelationChildMember
        {
            get;
            set;
        }
        public ContextMenuStrip ContextMenuStrip
        {
            get;
            set;
        }
        private int _imageIndex = 0;
        public int ImageIndex
        {
            get { return _imageIndex; }
            set { _imageIndex = value; }
        }
        private int _selectedImageIndex = -1;
        public int SelectedImageIndex
        {
            get { return _selectedImageIndex; }
            set { _selectedImageIndex = value; }
        }
        private Func<object, IList> _getItemsFunc;
        public Func<object, IList> GetItemsFunc
        {
            get { return _getItemsFunc; }
            set
            {
                if (_getItemsFunc != value)
                {
                    _getItemsFunc = value;
                }
            }
        }
        public TypeBinderTreeViewNodeCodon()
        {
        }
        public TypeBinderTreeViewNodeCodon(Type dataBoundType, string textMember)
            : this(dataBoundType, null, textMember, null)
        {
        }
        public TypeBinderTreeViewNodeCodon(Type dataBoundType, Type itemType, string textMember)
            : this(dataBoundType, itemType, textMember, null)
        {
        }
        public TypeBinderTreeViewNodeCodon(Type dataBoundType, Type itemType, string textMember, string itemsMember)
        {
            DataBoundType = dataBoundType;
            ItemType = itemType;
            TextMember = textMember;
            ItemsMember = itemsMember;
        }
        public bool Compatible(object obj)
        {
            if (obj == null)
            {
                Debug.Assert(false, "obj为null");
                throw new ArgumentNullException();
            }
            return Compatible(obj.GetType());
        }
        public bool Compatible(Type type)
        {
            if (type == null)
            {
              
                Debug.Assert(false, "type为null");
                return false;
            }
            bool compatible = type.Equals(DataBoundType) ||
                (_actOnSubClass && type.IsSubclassOf(DataBoundType));
            return compatible;
        }
        public bool UpwardCompatible(object obj)
        {
            if (obj == null)
            {
                Debug.Assert(false, "obj为null");
                throw new ArgumentNullException();
            }
            return UpwardCompatible(obj.GetType());
        }
        public bool UpwardCompatible(Type type)
        {
            if (type == null)
            {
                Debug.Assert(false, "type为null");
                throw new ArgumentNullException();
            }
            bool compatible = type.Equals(DataBoundType) || DataBoundType.IsSubclassOf(type);
            return compatible;
        }
        public override string ToString()
        {
            if (DataBoundType != null)
                return DataBoundType.ToString();
            else
                return base.ToString();
        }
    }
}
