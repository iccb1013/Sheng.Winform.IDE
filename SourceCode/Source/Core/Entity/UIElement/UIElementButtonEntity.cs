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
namespace Sheng.SailingEase.Core
{
    [Serializable]
    [NormalUIElementEntityProvide("FormElementButtonEntity", 0x000069)]
    public class UIElementButtonEntity : UIElement
    {
        public UIElementButtonEntity()
            : base()
        {
        }
        public override void FromXml(string strXml)
        {
            base.FromXml(strXml);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(strXml);
        }
        public override string  ToXml()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(base.ToXml());
            return xmlDoc.OuterXml;
        }
        private static UIElementButtonEventTimes _eventTimes;
        public override List<EventTimeAbstract> EventTimeProvide
        {
            get
            {
                if (_eventTimes == null)
                {
                    _eventTimes = new UIElementButtonEventTimes();
                }
                return _eventTimes.Times;
            }
        }
        public override string GetEventTimeName(int code)
        {
            if (_eventTimes == null)
            {
                _eventTimes = new UIElementButtonEventTimes();
            }
            return _eventTimes.GetEventName(code);
        }
    }
}
