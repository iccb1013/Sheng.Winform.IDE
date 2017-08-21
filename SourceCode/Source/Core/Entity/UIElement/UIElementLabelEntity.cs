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
using System.Text;
using System.Xml;
using Sheng.SailingEase.ComponentModel;
using Sheng.SailingEase.Kernal;
namespace Sheng.SailingEase.Core
{
    [Serializable]
    [NormalUIElementEntityProvide("FormElementLabelEntity", 0x000067)]
    public class UIElementLabelEntity:UIElement
    {
        private bool _autoSize = false;
        [DefaultValue(false)]
        [CorePropertyRelator("FormElementLabelEntity_AutoSize", "PropertyCatalog_Style", Description = "FormElementLabelEntity_AutoSize_Description", XmlNodeName = "AutoSize",ReadOnly=true)]
        [PropertyBooleanEditorAttribute()]
        public bool AutoSize
        {
            get
            {
                return this._autoSize;
            }
            set
            {
                this._autoSize = value;
            }
        }
        private EnumContentAlignment _textAlign = EnumContentAlignment.TopLeft;
        [DefaultValue(EnumContentAlignment.TopLeft)]
        [CorePropertyRelator("FormElementLabelEntity_TextAlign", "PropertyCatalog_Style", Description = "FormElementLabelEntity_TextAlign_Description", XmlNodeName = "TextAlign")]
        [PropertyComboBoxEditorAttribute(Enum = typeof(EnumContentAlignment))]
        public EnumContentAlignment TextAlign
        {
            get
            {
                return this._textAlign;
            }
            set
            {
                this._textAlign = value;
            }
        }
        public UIElementLabelEntity()
            : base()
        {
        }
        public override void FromXml(string strXml)
        {
            base.FromXml(strXml);
            SEXElement xmlDoc = SEXElement.Parse(strXml);
            this.AutoSize = xmlDoc.GetInnerObject<bool>("/AutoSize", true);
            this.TextAlign = (EnumContentAlignment)xmlDoc.GetInnerObject<int>("/TextAlign", 0);
        }
        public override string ToXml()
        {
            SEXElement xmlDoc = SEXElement.Parse(base.ToXml());
            xmlDoc.AppendChild(String.Empty, "AutoSize", this.AutoSize);
            xmlDoc.AppendChild(String.Empty, "TextAlign", (int)this.TextAlign);
            return xmlDoc.ToString();
        }
    }
}
