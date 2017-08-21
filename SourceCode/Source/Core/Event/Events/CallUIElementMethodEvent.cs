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
using Sheng.SailingEase.Core;
using Sheng.SailingEase.ComponentModel;
using Sheng.SailingEase.Kernal;
namespace Sheng.SailingEase.Core
{
    [Serializable]
    [EventProvide("调用对象方法", 0x000076,"调用指定元素提供的方法")]
    public class CallUIElementMethodEvent:EventBase
    {
        [NonSerialized]
        private EventTypesAbstract _eventTypesAdapter = EventTypes.Instance;
        protected EventTypesAbstract EventTypesAdapter
        {
            get { return this._eventTypesAdapter; }
            set { this._eventTypesAdapter = value; }
        }
        private EnumCallUIElementMethodTargetForm _targetWindow = EnumCallUIElementMethodTargetForm.Current;
        public EnumCallUIElementMethodTargetForm TargetWindow
        {
            get
            {
                return this._targetWindow;
            }
            set
            {
                this._targetWindow = value;
            }
        }
        private EventBase _callEvent;
        public EventBase CallEvent
        {
            get
            {
                return this._callEvent;
            }
            set
            {
                this._callEvent = value;
            }
        }
        private string _formElement;
        public string FormElement
        {
            get
            {
                return this._formElement;
            }
            set
            {
                this._formElement = value;
            }
        }
        private UIElementEntityProvideAttribute _formElementControlType;
        public UIElementEntityProvideAttribute FormElementControlType
        {
            get { return this._formElementControlType; }
            set { this._formElementControlType = value; }
        }
        private EventProvideAttribute _event;
        public EventProvideAttribute Event
        {
            get
            {
                return this._event;
            }
            set
            {
                this._event = value;
            }
        }
        public override WindowEntity HostFormEntity
        {
            get
            {
                return base.HostFormEntity;
            }
            set
            {
                base.HostFormEntity = value;
                if (this.CallEvent != null)
                    this.CallEvent.HostFormEntity = value;
            }
        }
        public CallUIElementMethodEvent()
        {
        }
        public override string ToXml()
        {
            SEXElement xmlDoc = SEXElement.Parse(base.ToXml());
            xmlDoc.AppendChild(String.Empty, "TargetWindow", (int)this.TargetWindow);
            xmlDoc.AppendChild(String.Empty, "FormElement", this.FormElement);
            xmlDoc.AppendChild(String.Empty, "FormElementControlType", this.FormElementControlType.Code);
            xmlDoc.AppendChild(String.Empty, "EventCode", this.Event.Code);
            xmlDoc.AppendInnerXml(String.Empty, this.CallEvent.ToXml());
            return xmlDoc.ToString();
        }
        public override void FromXml(string strXml)
        {
            base.FromXml(strXml);
            SEXElement xmlDoc = SEXElement.Parse(strXml);
            this.TargetWindow = (EnumCallUIElementMethodTargetForm)xmlDoc.GetInnerObject<int>("/TargetWindow", 0);
            this.FormElement = xmlDoc.GetInnerObject("/FormElement");
            this.FormElementControlType = UIElementEntityTypes.Instance.GetProvideAttribute(xmlDoc.GetInnerObject<int>("/FormElementControlType", -1));
            this.Event = EventTypes.Instance.GetProvideAttribute(xmlDoc.GetInnerObject<int>("/EventCode", -1));
            this.CallEvent = this.EventTypesAdapter.CreateInstance(
                Convert.ToInt32(xmlDoc.SelectSingleNode("/Event").Attribute("EventCode").Value));
            this.CallEvent.FromXml(xmlDoc.SelectSingleNode("/Event").ToString());
            this.CallEvent.HostFormEntity = this.HostFormEntity;
        }
        public enum EnumCallUIElementMethodTargetForm
        {
            [LocalizedDescription("EnumCallEntityMethodTargetForm_Current")]
            Current = 0,
            [LocalizedDescription("EnumCallEntityMethodTargetForm_Caller")]
            Caller = 1
        }
    }
}
