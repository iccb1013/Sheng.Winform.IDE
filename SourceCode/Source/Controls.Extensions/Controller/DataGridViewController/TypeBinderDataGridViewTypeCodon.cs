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
using Sheng.SailingEase.Kernal;
namespace Sheng.SailingEase.Controls.Extensions
{
    public class TypeBinderDataGridViewTypeCodon : ITypeBinderDataGridViewTypeCodon
    {
        public Type DataBoundType
        {
            get;
            private set;
        }
        private bool _actOnSubClass = false;
        public bool ActOnSubClass
        {
            get { return _actOnSubClass; }
            set { _actOnSubClass = value; }
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
        public ContextMenuStrip ContextMenuStrip
        {
            get;
            set;
        }
        public List<DataGridViewColumn> Columns
        {
            get;
            set;
        }
        public TypeBinderDataGridViewTypeCodon(Type dataBoundType)
            : this(dataBoundType, null, null)
        {
        }
        public TypeBinderDataGridViewTypeCodon(Type dataBoundType, Type itemType,
            string itemsMember)
        {
            DataBoundType = dataBoundType;
            ItemType = itemType;
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
            bool compatible = false;
            if (DataBoundType.IsClass)
            {
                compatible = type.Equals(DataBoundType) ||
                    (_actOnSubClass && type.IsSubclassOf(DataBoundType));
            }
            else if (DataBoundType.IsInterface)
            {
                compatible = type == DataBoundType || type.GetInterface(DataBoundType.ToString()) != null;
            }
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
            bool compatible = false;
            if (DataBoundType.IsClass)
            {
                compatible = type.Equals(DataBoundType) || DataBoundType.IsSubclassOf(type);
            }
            else if (DataBoundType.IsInterface)
            {
                compatible = DataBoundType == type || DataBoundType.GetInterface(type.ToString()) != null;
            }
            return compatible;
        }
        public override string ToString()
        {
            if (DataBoundType != null)
                return DataBoundType.ToString();
            else
                return base.ToString();
        }
        public IBindingListEx InitializeBindingList()
        {
            BindingListEx<object> bindingList = new BindingListEx<object>();
            return bindingList;
        }
        public IBindingListEx InitializeBindingList(IList list)
        {
            BindingListEx<object> bindingList = new BindingListEx<object>(list.Cast<object>().ToList());
            return bindingList;
        }
    }
}
