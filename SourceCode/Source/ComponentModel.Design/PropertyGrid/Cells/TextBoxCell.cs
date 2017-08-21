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
using System.Xml;
using System.Text.RegularExpressions;
using System.ComponentModel;
using Sheng.SailingEase.ComponentModel.Design.Localisation;
namespace Sheng.SailingEase.ComponentModel.Design
{
    class TextBoxCell : DataGridViewTextBoxCell, IPropertyGirdCell
    {
        public override Type EditType
        {
            get
            {
                return typeof(TextBoxCellEditingControl);
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
        public TextBoxCell()
        {
        }
        public override void InitializeEditingControl(int rowIndex, object
              initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            this._oldValue = this.Value;
            this._oldValueInitialize = true;
            base.InitializeEditingControl(rowIndex, initialFormattedValue,
                    dataGridViewCellStyle);
            TextBoxCellEditingControl ctl =
                    DataGridView.EditingControl as TextBoxCellEditingControl;
            ctl.PropertyGirdAttribute = _propertyRelatorAttribute;
            PropertyTextBoxEditorAttribute editorAttribute = (PropertyTextBoxEditorAttribute)this.PropertyEditorAttribute;
            ctl.MaxLength = editorAttribute.MaxLength;
            if (this.Value != null)
            {
                ctl.Text = this.Value.ToString();
            }
            else
            {
                ctl.Text = String.Empty;
            }
        }
        protected override bool SetValue(int rowIndex, object value)
        {
           
            if (this.DataGridView != null && ((PropertyGridDataGridView)this.DataGridView).SelectedObjectSeting == false)
            {
                PropertyTextBoxEditorAttribute editorAttribute = (PropertyTextBoxEditorAttribute)this.PropertyEditorAttribute;
                if (value == null || value.ToString() == String.Empty)
                {
                    if (editorAttribute.AllowEmpty == false)
                    {
                        this.ErrorText = Language.Current.PropertyGrid_ErrorText_NullValueInefficacy;
                        return false;
                    }
                    return base.SetValue(rowIndex, value);
                }
                try
                {
                    Convert.ChangeType(value, editorAttribute.TypeCode);
                }
                catch (Exception ex)
                {
                    this.ErrorText = ex.Message;
                    return false;
                }
                if (String.IsNullOrEmpty(editorAttribute.Regex) == false)
                {
                    if (value != null)
                    {
                        Regex r = new Regex(editorAttribute.Regex, RegexOptions.Singleline);
                        Match m = r.Match(value.ToString());
                        if (m.Success == false)
                        {
                            this.ErrorText = editorAttribute.RegexMsg;
                            return false;
                        }
                    }
                }
                bool changed = (base.GetValue(rowIndex) != null && value != null) && base.GetValue(rowIndex).ToString().Equals(value) == false;
                PropertyGridValidateResult validateResult = this.Owner.ValidateValue(this.OwnerRow.PropertyName, value, changed);
                if (validateResult.Success == false)
                {
                    this.ErrorText = validateResult.Message;
                    return false;
                }
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
            if (this.Value == null)
            {
                return String.Empty;
            }
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
