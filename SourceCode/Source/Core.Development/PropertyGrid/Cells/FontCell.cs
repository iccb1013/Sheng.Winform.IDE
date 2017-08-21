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
using System.Windows.Forms;
using System.Xml;
using Sheng.SailingEase.ComponentModel;
using Sheng.SailingEase.ComponentModel.Design;
namespace Sheng.SailingEase.Core.Development
{
     [PropertyGridCellProvideAttribute(typeof(PropertyFontEditorAttribute))]
    class FontCell : DataGridViewTextBoxCell, IPropertyGirdCell
    {
        public FontCell()
        {
        }
        public override void InitializeEditingControl(int rowIndex, object
              initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            this._oldValue = this.Value;
            this._oldValueInitialize = true;
            base.InitializeEditingControl(rowIndex, initialFormattedValue,
                    dataGridViewCellStyle);
            FontCellEditingControl ctl =
                    DataGridView.EditingControl as FontCellEditingControl;
            if (this.Value != null)
            {
                ctl.ElementFont = this.Value as UIElementFont;
            }
            else
            {
                ctl.ElementFont = DefaultNewRowValue as UIElementFont;
            }
        }
        public override Type EditType
        {
            get
            {
                return typeof(FontCellEditingControl);
            }
        }
        public override Type ValueType
        {
            get
            {
                return typeof(UIElementFont);
            }
        }
        public override object DefaultNewRowValue
        {
            get
            {
                UIElementFont temp = new UIElementFont()
                {
                    FontFamily = String.Empty
                };
                return temp;
            }
        }
        protected override bool SetValue(int rowIndex, object value)
        {
            bool changed = (base.GetValue(rowIndex) != null && value != null) && base.GetValue(rowIndex).ToString().Equals(value) == false;
            PropertyGridValidateResult validateResult = this.Owner.ValidateValue(this.OwnerRow.PropertyName, value, changed);
            if (validateResult.Success == false)
            {
                this.ErrorText = validateResult.Message;
                return false;
            }
            return base.SetValue(rowIndex, value);
        }
        public PropertyGridPad Owner
        {
            get;
            set;
        }
        public PropertyGridRow OwnerRow
        {
            get;
            set;
        }
        private PropertyRelatorAttribute _propertyRelatorAttribute;
        public PropertyRelatorAttribute PropertyRelatorAttribute
        {
            get
            {
                return _propertyRelatorAttribute;
            }
            set
            {
                _propertyRelatorAttribute = value;
            }
        }
        private DefaultValueAttribute _defaultValueAttribute;
        public DefaultValueAttribute DefaultValueAttribute
        {
            get
            {
                return _defaultValueAttribute;
            }
            set
            {
                _defaultValueAttribute = value;
            }
        }
        private PropertyEditorAttribute _propertyEditorAttribute;
        public PropertyEditorAttribute PropertyEditorAttribute
        {
            get
            {
                return _propertyEditorAttribute;
            }
            set
            {
                _propertyEditorAttribute = value;
            }
        }
        public string GetPropertyXml(string xmlNodeName)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement xmlElement = xmlDoc.CreateElement(xmlNodeName);
            if (this.Value != null)
            {
                xmlElement.InnerText = this.Value.ToString();
            }
            xmlDoc.AppendChild(xmlElement);
            return xmlDoc.OuterXml;
        }
        public string GetPropertyString()
        {
            if (this.Value == null)
            {
                return String.Empty;
            }
            return this.Value.ToString();
        }
        public void SetPropertyValue(object value)
        {
            this.Value = value;
        }
        public object GetPropertyValue()
        {
            return this.Value;
        }
        private bool _oldValueInitialize = false;
        private object _oldValue = null;
        public object GetPropertyOldValue()
        {
            if (!_oldValueInitialize)
                return this.Value;
            else
                return _oldValue;
        }
    }
}
