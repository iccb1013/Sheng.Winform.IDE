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
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Kernal;
using System.Xml.Linq;
namespace Sheng.SailingEase.Core
{
    public class MenuEntity : EntityBase, IEventSupport
    {
        private EventTypesAbstract _eventTypesAdapter = Sheng.SailingEase.Core.EventTypes.Instance;
        protected EventTypesAbstract EventTypesAdapter
        {
            get { return this._eventTypesAdapter; }
            set { this._eventTypesAdapter = value; }
        }
        private byte _layer;
        public byte Layer
        {
            get
            {
                return this._layer;
            }
            set
            {
                this._layer = value;
            }
        }
        private string _parentId;
        public string ParentId
        {
            get
            {
                return this._parentId;
            }
            set
            {
                this._parentId = value;
            }
        }
        private string _text = String.Empty;
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
        public const string Property_Menus = "Menus";
        private MenuEntityCollection _menus = new MenuEntityCollection();
        public MenuEntityCollection Menus
        {
            get { return this._menus; }
            set { this._menus = value; }
        }
        public MenuEntity()
            : base()
        {
            this.XmlRootName = "Menu";
        }
        public override string ToXml()
        {
            SEXElement xmlDoc = SEXElement.Parse(base.ToXml());
            xmlDoc.AppendChild(String.Empty, "Text", this.Text);
            xmlDoc.AppendChild(String.Empty, "Layer", this.Layer);
            xmlDoc.AppendChild(String.Empty, "ParentId", this.ParentId);
            xmlDoc.AppendChild("Events");
            foreach (EventBase eve in this.Events)
            {
                xmlDoc.AppendInnerXml("/Events", eve.ToXml());
            }
            return xmlDoc.ToString();
        }
        public override void FromXml(string strXml)
        {
            base.FromXml(strXml);
            SEXElement xmlDoc = SEXElement.Parse(strXml);
            this.Text = xmlDoc.GetInnerObject("/Text");
            this.Layer = xmlDoc.GetInnerObject<byte>("/Layer", 0);
            this.ParentId = xmlDoc.GetInnerObject("/ParentId");
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
        private EventCollection events;
        public EventCollection Events
        {
            get
            {
                if (events == null)
                    events = new EventCollection(null, this);
                return this.events;
            }
            set
            {
                this.events = value;
            }
        }
        public virtual EventTypeCollection EventProvide
        {
            get { throw new NotImplementedException(); }
        }
        private static MenuEventTimes _eventTimes;
        public List<EventTimeAbstract> EventTimeProvide
        {
            get
            {
                if (_eventTimes == null)
                {
                    _eventTimes = new MenuEventTimes();
                }
                return _eventTimes.Times;
            }
        }
        public string GetEventTimeName(int code)
        {
            if (_eventTimes == null)
            {
                _eventTimes = new MenuEventTimes();
            }
            return _eventTimes.GetEventName(code);
        }
        public void EventUpdate(object sender)
        {
            if (this.EventUpdated != null)
                EventUpdated(sender, this);
        }
    }
}
