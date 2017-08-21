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
using Sheng.SailingEase.Kernal;
namespace Sheng.SailingEase.ComponentModel.Design
{
    public class CellFactory
    {
        private Dictionary<Type, Type> _cells = new Dictionary<Type, Type>();
        private static InstanceLazy<CellFactory> _instance =
           new InstanceLazy<CellFactory>(() => new CellFactory());
        public static CellFactory Instance
        {
            get { return _instance.Value; }
        }
        private CellFactory()
        {
            _cells.Add(typeof(PropertyBooleanEditorAttribute), typeof(BooleanCell));
            _cells.Add(typeof(PropertyColorChooseEditorAttribute), typeof(ColorChooseCell));
            _cells.Add(typeof(PropertyComboBoxEditorAttribute), typeof(ComboBoxCell));
            _cells.Add(typeof(PropertyNumericUpDownEditorAttribute), typeof(NumericUpDownCell));
            _cells.Add(typeof(PropertyTextBoxEditorAttribute), typeof(TextBoxCell));
        }
        public IPropertyGirdCell Create(PropertyEditorAttribute attribute)
        {
            Type attributeType = attribute.GetType();
            Debug.Assert(_cells.Keys.Contains(attributeType), "没有为PropertyEditorAttribute映射相应的EditingControl");
            if (_cells.Keys.Contains(attributeType))
            {
                return Activator.CreateInstance(_cells[attributeType]) as IPropertyGirdCell;
            }
            else
            {
                return null;
            }
        }
        public void Register(Type editorAttribute, Type cellType)
        {
            Debug.Assert(editorAttribute != null && cellType != null);
            if (editorAttribute == null || cellType == null)
                return;
            Debug.Assert(_cells.Keys.Contains(editorAttribute) == false, "已经有了指定editorAttribute的单元格");
            if (_cells.Keys.Contains(editorAttribute))
                return;
            _cells.Add(editorAttribute, cellType);
        }
    }
}
