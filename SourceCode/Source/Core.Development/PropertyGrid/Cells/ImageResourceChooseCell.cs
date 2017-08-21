/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Sheng.SailingEase.ComponentModel.Design;
using Sheng.SailingEase.Drawing;
using Sheng.SailingEase.ComponentModel;
using Sheng.SailingEase.Infrastructure;
using System.Diagnostics;
namespace Sheng.SailingEase.Core.Development
{
    [PropertyGridCellProvideAttribute(typeof(PropertyImageResourceChooseEditorAttribute))]
    class ImageResourceChooseCell : DataGridViewTextBoxCell, IPropertyGirdCell
    {
        IResourceComponentService _resourceService = ServiceUnity.ResourceService;
        public ImageResourceChooseCell()
        {
        }
        public override void InitializeEditingControl(int rowIndex, object
              initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            this._oldValue = this.Value;
            this._oldValueInitialize = true;
            base.InitializeEditingControl(rowIndex, initialFormattedValue,
                    dataGridViewCellStyle);
            ImageResourceChooseCellEditingControl ctl =
                    DataGridView.EditingControl as ImageResourceChooseCellEditingControl;
            if (this.Value != null)
            {
                ctl.ResourceName = this.Value.ToString();
            }
            else
            {
                ctl.ResourceName = String.Empty;
            }
        }
        public override Type EditType
        {
            get
            {
                return typeof(ImageResourceChooseCellEditingControl);
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
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle,
                advancedBorderStyle, paintParts);
            graphics.FillRectangle(Brushes.White, cellBounds.X + 1, cellBounds.Y + 1, cellBounds.Width - 2, cellBounds.Height - 2);
            int imageLocationX = cellBounds.X + 2;
            int imageLocationY = cellBounds.Y + 2;
            int imageSizeW = 16;
            int imageSizeH = 16;
            Rectangle imageBounds = new Rectangle(imageLocationX, imageLocationY, imageSizeW, imageSizeH);
            int textLocationX = imageLocationX + imageSizeW + 2;
            int textLocationY = cellBounds.Y + 4;
            Point textLoation = new Point(textLocationX, textLocationY);
            if (this.Value != null && this.Value.ToString() != String.Empty)
            {
                string resourceName = this.Value.ToString();
                Image imageDraw;
                if (_resourceService.Container(resourceName))
                {
                    ImageResourceInfo imageResource = _resourceService.GetImageResource(resourceName);
                    Image image = imageResource.GetImage();
                    imageDraw = DrawingTool.GetScaleImage(image, imageSizeW, imageSizeH);
                    if (image != imageDraw)
                        image.Dispose();
                }
                else
                {
                    imageDraw = DrawingTool.Mark.FileNotFind(imageBounds.Size);
                }
                try
                {
                    graphics.DrawImage(imageDraw, imageBounds);
                }
                catch (Exception exception)
                {
                    Debug.Assert(false, exception.Message);
                    imageDraw = DrawingTool.Mark.FileNotFind(imageBounds.Size);
                    graphics.DrawImage(imageDraw, imageBounds);
                }
                imageDraw.Dispose();
                Font font = new Font(cellStyle.Font, cellStyle.Font.Style | FontStyle.Bold);
                Brush textBrush = new SolidBrush(cellStyle.ForeColor);
                graphics.DrawString(resourceName, font, textBrush, textLoation);
                font.Dispose();
                textBrush.Dispose();
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
