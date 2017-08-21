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
using System.Diagnostics;
using Sheng.SailingEase.Kernal;
using System.Xml.Linq;
namespace Sheng.SailingEase.Core
{
    [Serializable]
    public abstract class EventBase : BaseObject, IXmlable
    {
        private string _xmlRootName = "Event";
        protected string XmlRootName
        {
            get
            {
                return this._xmlRootName;
            }
            set
            {
                this._xmlRootName = value;
            }
        }
        [NonSerialized]
        private WindowEntity _hostFormEntity;
        
        [ObjectCompareAttribute(Ignore = true)]
        public virtual WindowEntity HostFormEntity
        {
            get
            {
                return this._hostFormEntity;
            }
            set
            {
                this._hostFormEntity = value;
            }
        }
        [NonSerialized]
        private EntityBase _hostEntity;
        [ObjectCompareAttribute(Ignore = true)]
        public EntityBase HostEntity
        {
            get { return _hostEntity; }
            set { _hostEntity = value; }
        }
        private string _id;
        public string Id
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
            }
        }
        private string _name;
        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }
        private string _code;
        public string Code
        {
            get
            {
                return this._code;
            }
            set
            {
                this._code = value;
            }
        }
        private int _eventTime;
        public int EventTime
        {
            get
            {
                return this._eventTime;
            }
            set
            {
                this._eventTime = value;
            }
        }
        private bool _sys = false;
        public bool Sys
        {
            get
            {
                return this._sys;
            }
            set
            {
                this._sys = value;
            }
        }
        public string EventName
        {
            get
            {
                return EventTypes.Instance.GetName(this);
            }
        }
        public string EventTimeName
        {
            get
            {
                IEventSupport eventSupport = this.HostEntity as IEventSupport;
                Debug.Assert(eventSupport != null, "Event �� HostEntity Ϊ Null ��û��ʵ�� IEventSupport");
                if (eventSupport != null)
                    return eventSupport.GetEventTimeName(EventTime);
                else
                    return String.Empty;
            }
        }
        public override object Clone()
        {
            EventBase newEvent = base.Clone() as EventBase;
            newEvent.HostFormEntity = this.HostFormEntity;
            newEvent.HostEntity = this.HostEntity;
            return newEvent;
        }
        public virtual string ToXml()
        {
            SEXElement xmlDoc = new SEXElement(XmlRootName);
            xmlDoc.AppendAttribute(String.Empty, "Id", this.Id);
            xmlDoc.AppendAttribute(String.Empty, "EventCode", EventTypes.Instance.GetProvideAttribute(this).Code);
            xmlDoc.AppendAttribute(String.Empty, "EventTime", this.EventTime);
            xmlDoc.AppendAttribute(String.Empty, "Name", this.Name);
            xmlDoc.AppendAttribute(String.Empty, "Code", this.Code);
            xmlDoc.AppendAttribute(String.Empty, "Sys", this.Sys);
            return xmlDoc.ToString();
        }
        public virtual void FromXml(string strXml)
        {
            SEXElement xmlDoc = SEXElement.Parse(strXml);
            this.Id = xmlDoc.GetAttributeObject("Id");
            this.Name = xmlDoc.GetAttributeObject("Name");
            this.Code = xmlDoc.GetAttributeObject("Code");
            this.Sys = xmlDoc.GetAttributeObject<bool>("Sys", false);
            this.EventTime = xmlDoc.GetAttributeObject<int>("EventTime", 0);
        }
    }
}
