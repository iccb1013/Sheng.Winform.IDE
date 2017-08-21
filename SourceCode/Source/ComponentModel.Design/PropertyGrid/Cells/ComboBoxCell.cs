/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.ComponentModel;
using Sheng.SailingEase.Kernal;
namespace Sheng.SailingEase.ComponentModel.Design
{
    class ComboBoxCell : DataGridViewTextBoxCell, IPropertyGirdCell
    {
        private string _text;
        public string Text
        {
            get
            {
                return this._text;
            }
            set
            {
                this._text = value;
            }
        }
        public override Type EditType
        {
            get
            {
                return typeof(ComboBoxCellEditingControl);
            }
        }
        public override Type ValueType
        {
            get
            {
                return typeof(String);
            }
        }
        public override object DefaultNewRowValue
        {
            get
            {
                return String.Empty;
            }
        }
        public ComboBoxCell()
        {
        }
        public override void InitializeEditingControl(int rowIndex, object
               initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            this._oldValue = this.Value;
            this._oldValueInitialize = true;
            base.InitializeEditingControl(rowIndex, initialFormattedValue,
                    dataGridViewCellStyle);
            ComboBoxCellEditingControl ctl =
                    DataGridView.EditingControl as ComboBoxCellEditingControl;
            ctl.PropertyGirdAttribute = this._propertyRelatorAttribute;
            PropertyComboBoxEditorAttribute editorAttribute = (PropertyComboBoxEditorAttribute)this.PropertyEditorAttribute;
            ctl.DataSource = EnumDescConverter.Get(editorAttribute.Enum);
            if (this.Value != null)
            {
                ctl.SelectedValue = this.Value.ToString();
            }
            else
            {
                ctl.SelectedValue = String.Empty;
            }
        }
        protected override object GetFormattedValue(object value, int rowIndex, ref DataGridViewCellStyle cellStyle,
            System.ComponentModel.TypeConverter valueTypeConverter, System.ComponentModel.TypeConverter formattedValueTypeConverter,
            DataGridViewDataErrorContexts context)
        {
            string strValue = String.Empty;
            if (value != null)
            {
                PropertyComboBoxEditorAttribute editorAttribute = (PropertyComboBoxEditorAttribute)this.PropertyEditorAttribute;
                if (editorAttribute != null)
                {
                    DataRow[] dr = EnumDescConverter.Get(editorAttribute.Enum).Select("Value='" + value + "'");
                    if (dr.Length > 0)
                    {
                        strValue = dr[0]["Text"].ToString();
                    }
                }
                else
                {
                    strValue = value.ToString();
                }
            }
            return base.GetFormattedValue(strValue, rowIndex, ref cellStyle, valueTypeConverter, formattedValueTypeConverter, context);
        }
        protected override bool SetValue(int rowIndex, object value)
        {
           
            bool changed = (base.GetValue(rowIndex) != null && value != null) &&
                base.GetValue(rowIndex).ToString().Equals(value.ToString()) == false;
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
