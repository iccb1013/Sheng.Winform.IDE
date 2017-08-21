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
using Sheng.SailingEase.Core;
using System.Xml;
using System.ComponentModel;
using Sheng.SailingEase.ComponentModel;
using Sheng.SailingEase.Kernal;
using System.Xml.Linq;
namespace Sheng.SailingEase.Core
{
    public abstract class ToolStripItemAbstract : EntityBase, IEventSupport
    {
        [NonSerialized]
        private EventTypesAbstract _eventTypesAdapter = Sheng.SailingEase.Core.EventTypes.Instance;
        protected EventTypesAbstract EventTypesAdapter
        {
            get { return this._eventTypesAdapter; }
            set { this._eventTypesAdapter = value; }
        }
        public const string Property_GroupId = "GroupId";
        private string _groupId;
        public string GroupId
        {
            get { return _groupId; }
            set { _groupId = value; }
        }
        [NonSerialized]
        private WindowEntity _hostFormEntity;
        public WindowEntity HostFormEntity
        {
            get
            {
                return this._hostFormEntity;
            }
            set
            {
                this._hostFormEntity = value;
                this.Events.HostFormEntity = value;
            }
        }
        private EnumToolStripItemAlignment _aligment = EnumToolStripItemAlignment.Left;
        public EnumToolStripItemAlignment Aligment
        {
            get
            {
                return this._aligment;
            }
            set
            {
                this._aligment = value;
            }
        }
        private string _toolTipText = String.Empty;
       [DefaultValue(StringUnity.EmptyString)]
        [CorePropertyRelator("ToolStripItemAbstract_ToolTipText", "PropertyCatalog_Style", 
            Description = "ToolStripItemAbstract_ToolTipText_Description", XmlNodeName = "ToolTipText")]
        [PropertyTextBoxEditorAttribute()]
        public string ToolTipText
        {
            get
            {
                return this._toolTipText;
            }
            set
            {
                this._toolTipText = value;
            }
        }
        private string _text = String.Empty;
        [DefaultValue(StringUnity.EmptyString)]
        [CorePropertyRelator("ToolStripItemAbstract_Text", "PropertyCatalog_Style", 
            Description = "ToolStripItemAbstract_Text_Description", XmlNodeName = "Text")]
        [PropertyTextBoxEditorAttribute()]
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
        public ToolStripItemAbstract()
        {
            this.XmlRootName = "Element";
            this.Events = new EventCollection(this.HostFormEntity, this);
        }
        public override string ToXml()
        {
            SEXElement xmlDoc = SEXElement.Parse(base.ToXml());
            xmlDoc.AppendAttribute(String.Empty, "GroupId", this.GroupId);
            xmlDoc.AppendChild(String.Empty, "ControlType", ToolStripItemEntityTypesFactory.Instance.GetProvideAttribute(this).Code);
            xmlDoc.AppendChild(String.Empty, "ToolTipText", this.ToolTipText);
            xmlDoc.AppendChild(String.Empty, "Text", this.Text);
            xmlDoc.AppendChild("Events");
            if (this.Events != null)
            {
                foreach (EventBase even in this.Events)
                {
                    xmlDoc.AppendInnerXml("/Events", even.ToXml());
                }
            }
            return xmlDoc.ToString();
        }
        public override void FromXml(string strXml)
        {
            base.FromXml(strXml);
            SEXElement xmlDoc = SEXElement.Parse(strXml);
            this.GroupId = xmlDoc.GetAttributeObject("GroupId");
            this.ToolTipText = xmlDoc.GetInnerObject("/ToolTipText");
            this.Text = xmlDoc.GetInnerObject("/Text");
            EventBase eventBase;
            foreach (XElement eventNode in xmlDoc.SelectNodes("/Events/Event"))
            {
                eventBase = this.EventTypesAdapter.CreateInstance(Convert.ToInt32(eventNode.Attribute("EventCode").Value));
                eventBase.FromXml(eventNode.ToString());
                this.Events.Add(eventBase);
            }
        }
        [field: NonSerialized]
        public event OnEventUpdatedHandler EventUpdated;
        private EventCollection _events;
        public EventCollection Events
        {
            get
            {
                return this._events;
            }
            set
            {
                this._events = value;
            }
        }
        private EventTypeCollection _eventProvide = new EventTypeCollection();
        public virtual EventTypeCollection EventProvide
        {
            get { return _eventProvide; }
        }
        private List<EventTimeAbstract> _eventTimeProvide = new List<EventTimeAbstract>();
        public virtual List<EventTimeAbstract> EventTimeProvide
        {
            get { return _eventTimeProvide; }
        }
        public virtual string GetEventTimeName(int code)
        {
            return String.Empty;
        }
        public void EventUpdate(object sender)
        {
            if (this.EventUpdated != null)
                EventUpdated(sender, this);
        }
        public enum EnumToolStripItemAlignment
        {
            [LocalizedDescription("EnumToolStripItemAlignment_Left")]
            Left = 0,
            [LocalizedDescription("EnumToolStripItemAlignment_Right")]
            Right = 1
        }
        public enum EnumToolStripItemDisplayStyle
        {
            [LocalizedDescription("EnumToolStripItemDisplayStyle_None")]
            None = 0,
            [LocalizedDescription("EnumToolStripItemDisplayStyle_Text")]
            Text = 1,
            [LocalizedDescription("EnumToolStripItemDisplayStyle_Image")]
            Image = 2,
            [LocalizedDescription("EnumToolStripItemDisplayStyle_ImageAndText")]
            ImageAndText = 3
        }
    }
}
