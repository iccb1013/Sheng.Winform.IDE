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
using System.Xml;
using Sheng.SailingEase.ComponentModel;
using Sheng.SailingEase.Core;
using System.ComponentModel;
using Sheng.SailingEase.Kernal;
namespace Sheng.SailingEase.Core
{
    [ToolStripItemEntityProvide("ToolStripButtonEntity", 0x00006C)]
    public class ToolStripButtonEntity : ToolStripItemAbstract//, IEventSupport
    {
        private string _image;
        [CorePropertyRelator("ToolStripButtonEntity_ImagePath", "PropertyCatalog_Style", 
            Description = "ToolStripButtonEntity_ImagePath_Description", XmlNodeName = "ImagePath")]
        [PropertyImageResourceChooseEditorAttribute()]
        public string Image
        {
            get
            {
                return this._image;
            }
            set
            {
                this._image = value;
            }
        }
        private EnumToolStripItemDisplayStyle _displayStyle = EnumToolStripItemDisplayStyle.Image;
        public EnumToolStripItemDisplayStyle DisplayStyle
        {
            get
            {
                return this._displayStyle;
            }
            set
            {
                this._displayStyle = value;
            }
        }
        private EnumTextImageRelation _textImageRelation = EnumTextImageRelation.ImageBeforeText;
        public EnumTextImageRelation TextImageRelation
        {
            get
            {
                return this._textImageRelation;
            }
            set
            {
                this._textImageRelation = value;
            }
        }
        public ToolStripButtonEntity()
            : base()
        {
        }
        public override void FromXml(string strXml)
        {
            base.FromXml(strXml);
            SEXElement xmlDoc = SEXElement.Parse(strXml);
            this.Image = xmlDoc.GetInnerObject("/Image");
        }
        public override string ToXml()
        {
            SEXElement xmlDoc = SEXElement.Parse(base.ToXml());
            xmlDoc.AppendChild(String.Empty, "Image", this.Image);
            return xmlDoc.ToString();
        }
        private static ToolStripButtonEventTimes _eventTimes;
        public override List<EventTimeAbstract> EventTimeProvide
        {
            get
            {
                if (_eventTimes == null)
                {
                    _eventTimes = new ToolStripButtonEventTimes();
                }
                return _eventTimes.Times;
            }
        }
        public override string GetEventTimeName(int code)
        {
            if (_eventTimes == null)
            {
                _eventTimes = new ToolStripButtonEventTimes();
            }
            return _eventTimes.GetEventName(code);
        }
    }
}
