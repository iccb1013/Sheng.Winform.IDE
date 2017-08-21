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
using System.Drawing;
using System.Reflection;
using System.Xml;
using System.ComponentModel;
using Sheng.SailingEase.Drawing;
namespace Sheng.SailingEase.ComponentModel.Design
{
    class ColorChooseCell : DataGridViewTextBoxCell, IPropertyGirdCell
    {
        Bitmap showColorBitmap;
        public ColorChooseCell()
        {
            this.showColorBitmap = new Bitmap(24, 17);
        }
        public override void InitializeEditingControl(int rowIndex, object
              initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            this._oldValue = this.Value;
            this._oldValueInitialize = true;
            base.InitializeEditingControl(rowIndex, initialFormattedValue,
                    dataGridViewCellStyle);
            ColorChooseCellEditingControl ctl =
                    DataGridView.EditingControl as ColorChooseCellEditingControl;
            if (this.Value != null)
            {
                ctl.ColorValue = this.Value.ToString();
            }
            else
            {
                ctl.ColorValue = String.Empty;
            }
        }
        public override Type EditType
        {
            get
            {
                return typeof(ColorChooseCellEditingControl);
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
        protected override void Paint(System.Drawing.Graphics graphics, System.Drawing.Rectangle clipBounds, 
            System.Drawing.Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, 
            object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle 
            advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, 
                formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
            graphics.FillRectangle(Brushes.White, cellBounds.X+1, cellBounds.Y+1, cellBounds.Width-2, cellBounds.Height-2);
            if (this.Value != null && this.Value.ToString() != String.Empty)
            {
                int textLocationX = cellBounds.X + 28;
                int textLocationY = cellBounds.Y + 4;
                graphics.DrawString(this.Value.ToString().Split('.')[1],
                    cellStyle.Font, Brushes.Black, new Point(textLocationX, textLocationY));
                Color color = ColorRepresentationHelper.GetColorByValue(this.Value.ToString());
                Graphics showColorGraphics = Graphics.FromImage(showColorBitmap);
                showColorGraphics.Clear(color);
                showColorGraphics.DrawRectangle(Pens.Black, 0, 0, this.showColorBitmap.Width - 1, this.showColorBitmap.Height - 1);
                graphics.DrawImage(this.showColorBitmap, cellBounds.X + 2, cellBounds.Y + 2);
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
            if (this.Value == null)
            {
                return string.Empty;
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
