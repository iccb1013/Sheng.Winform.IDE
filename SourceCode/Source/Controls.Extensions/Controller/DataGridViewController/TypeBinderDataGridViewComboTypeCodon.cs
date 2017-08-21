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
    public class TypeBinderDataGridViewComboTypeCodon<T> : ITypeBinderDataGridViewTypeCodon
    {
        private List<TypeBinderDataGridViewTypeCodon> _codons = new List<TypeBinderDataGridViewTypeCodon>();
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
            get
            {
                Debug.Assert(false, "不应该调用到这里");
                return null;
            }
        }
        public string ItemsMember
        {
            get
            {
                Debug.Assert(false, "不应该调用到这里");
                return null;
            }
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
        public TypeBinderDataGridViewComboTypeCodon()
        {
            DataBoundType = typeof(T);
        }
        public void AddCodon(TypeBinderDataGridViewTypeCodon codon)
        {
            if (codon == null)
            {
                Debug.Assert(false, "codon 为 null");
                throw new ArgumentNullException();
            }
            if (_codons.Contains(codon))
            {
                Debug.Assert(false, "_typeBinderDataGridViewTypeCodons 重复添加:" + codon.ToString());
                return;
            }
            Type dataBoundType = codon.DataBoundType;
            Debug.Assert(GetCodon(dataBoundType) == null,
                "_typeBinderDataGridViewTypeCodons 重复添加类型:" + codon.ToString());
            bool compatible = false;
            if (DataBoundType.IsClass)
            {
                compatible = dataBoundType.Equals(DataBoundType) || dataBoundType.IsSubclassOf(DataBoundType);
            }
            else if (DataBoundType.IsInterface)
            {
                compatible = dataBoundType == DataBoundType || 
                    dataBoundType.GetInterface(DataBoundType.ToString()) != null;
            }
            if (compatible == false)
            {
                Debug.Assert(false, "指定的 codon 所绑定的对象类型不是该 ComboCodon 绑定类型的子类型:" + codon.ToString());
            }
            _codons.Add(codon);
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
            if (type.IsClass)
            {
                compatible = type.Equals(DataBoundType) || DataBoundType.IsSubclassOf(type);
            }
            else if (type.IsInterface)
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
            BindingListEx<T> bindingList = new BindingListEx<T>();
            return bindingList;
        }
        public IBindingListEx InitializeBindingList(IList list)
        {
            BindingListEx<T> bindingList = new BindingListEx<T>(list.Cast<T>().ToList());
            return bindingList;
        }
        private TypeBinderDataGridViewTypeCodon GetCodon(Type type)
        {
            if (type == null)
                return null;
            foreach (var code in _codons)
            {
                if (code.DataBoundType == null)
                    continue;
                if (code.Compatible(type))
                    return code;
            }
            return null;
        }
    }
}
