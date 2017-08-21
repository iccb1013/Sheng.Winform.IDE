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
using Sheng.SailingEase.ComponentModel.Design;
using Sheng.SailingEase.ComponentModel;
namespace Sheng.SailingEase.Core.Development
{
    [PropertyGridCellProvideAttribute(typeof(PropertyAnchorEditorAttribute))]
    class AnchorCell : DataGridViewTextBoxCell, IPropertyGirdCell
    {
        public AnchorCell()
        {
        }
        public override Type EditType
        {
            get
            {
                return typeof(AnchorCellEditingControl);
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
        public override void InitializeEditingControl(int rowIndex, object
              initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            this._oldValue = this.Value;
            this._oldValueInitialize = true;
            base.InitializeEditingControl(rowIndex, initialFormattedValue,
                    dataGridViewCellStyle);
            AnchorCellEditingControl ctl =
                    DataGridView.EditingControl as AnchorCellEditingControl;
            if (this.Value != null)
            {
                ctl.AnchorText = this.Value.ToString();
            }
            else
            {
                ctl.AnchorText = String.Empty;
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
            StringBuilder strPropertXml = new StringBuilder();
            strPropertXml.Append("<" + xmlNodeName + ">");
            string strValue = String.Empty;
            if (this.Value != null)
            {
                strValue = this.Value.ToString();
            }
            strPropertXml.Append("<Top>");
            if (strValue.IndexOf("Top") >= 0)
            {
                strPropertXml.Append(true.ToString());
            }
            else
            {
                strPropertXml.Append(false.ToString());
            }
            strPropertXml.Append("</Top>");
            strPropertXml.Append("<Left>");
            if (strValue.IndexOf("Left") >= 0)
            {
                strPropertXml.Append(true.ToString());
            }
            else
            {
                strPropertXml.Append(false.ToString());
            }
            strPropertXml.Append("</Left>");
            strPropertXml.Append("<Right>");
            if (strValue.IndexOf("Right") >= 0)
            {
                strPropertXml.Append(true.ToString());
            }
            else
            {
                strPropertXml.Append(false.ToString());
            }
            strPropertXml.Append("</Right>");
            strPropertXml.Append("<Bottom>");
            if (strValue.IndexOf("Bottom") >= 0)
            {
                strPropertXml.Append(true.ToString());
            }
            else
            {
                strPropertXml.Append(false.ToString());
            }
            strPropertXml.Append("</Bottom>");
            strPropertXml.Append("</" + xmlNodeName + ">");
            return strPropertXml.ToString();
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
            UIElementAnchor temp = new UIElementAnchor()
            {
                Top = false,
                Left = false,
                Right = false,
                Bottom = false
            };
            if (this.Value == null)
            {
                return temp;
            }
            string[] strAnchorArray = this.Value.ToString().Split(',');
            foreach (string str in strAnchorArray)
            {
                switch (str)
                {
                    case "Top":
                        temp.Top = true;
                        break;
                    case "Right":
                        temp.Right = true;
                        break;
                    case "Bottom":
                        temp.Bottom = true;
                        break;
                    case "Left":
                        temp.Left = true;
                        break;
                }
            }
            return temp;
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
