/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.ComponentModel;
using Sheng.SailingEase.ComponentModel;
using Sheng.SailingEase.Kernal;
namespace Sheng.SailingEase.Core
{
    [Serializable]
    [NormalUIElementEntityProvide("FormElementPictureBoxEntity", 0x000068)]
    public class UIElementPictureBoxEntity : UIElement
    {
        private string _img = String.Empty;
        [DefaultValue(StringUnity.EmptyString)]
        [CorePropertyRelator("FormElementPictureBoxEntity_Img", "PropertyCatalog_Style", 
            Description = "FormElementPictureBoxEntity_Img_Description", XmlNodeName = "Img")]
        [PropertyImageResourceChooseEditorAttribute()]
        public string Img
        {
            get
            {
                return this._img;
            }
            set
            {
                this._img = value;
            }
        }
        private EnumPictureBoxSizeMode _sizeMode = EnumPictureBoxSizeMode.Normal;
        [DefaultValue(EnumPictureBoxSizeMode.Normal)]
        [CorePropertyRelator("FormElementPictureBoxEntity_SizeMode", "PropertyCatalog_Style", 
            Description = "FormElementPictureBoxEntity_SizeMode_Description", XmlNodeName = "SizeMode")]
        [PropertyComboBoxEditorAttribute(Enum = typeof(EnumPictureBoxSizeMode))]
        public EnumPictureBoxSizeMode SizeMode
        {
            get
            {
                return this._sizeMode;
            }
            set
            {
                this._sizeMode = value;
            }
        }
        public UIElementPictureBoxEntity()
            : base()
        {
        }
        public override void FromXml(string strXml)
        {
            base.FromXml(strXml);
            SEXElement xmlDoc = SEXElement.Parse(strXml);
            this.SizeMode = (EnumPictureBoxSizeMode)xmlDoc.GetInnerObject<int>("/SizeMode", 0);
            this.Img = xmlDoc.GetInnerObject("/Img");
        }
        public override string ToXml()
        {
            SEXElement xmlDoc = SEXElement.Parse(base.ToXml());
            xmlDoc.AppendChild(String.Empty, "SizeMode", (int)this.SizeMode);
            xmlDoc.AppendChild(String.Empty, "Img", this.Img);
            return xmlDoc.ToString();
        }
        public enum EnumPictureBoxSizeMode
        {
            [LocalizedDescription("EnumPictureBoxSizeMode_Normal")]
            Normal = 0,
            [LocalizedDescription("EnumPictureBoxSizeMode_StretchImage")]
            StretchImage = 1,
            [LocalizedDescription("EnumPictureBoxSizeMode_AutoSize")]
            AutoSize = 2,
            [LocalizedDescription("EnumPictureBoxSizeMode_Zoom")]
            Zoom = 4
        }
    }
}
