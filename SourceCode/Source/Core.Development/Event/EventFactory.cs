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
using System.Data;
using System.Diagnostics;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Kernal;
namespace Sheng.SailingEase.Core.Development
{
    static class EventFactory
    {
        public static EventBase GetEventDevByXml(string strXml)
        {
            SEXElement xmlDoc = SEXElement.Parse(strXml);
            EventBase even = EventDevTypes.Instance.CreateInstance(Convert.ToInt32(xmlDoc.GetAttributeObject("Event", "EventCode")));
            even.FromXml(strXml);
            return even;
        }
        public static DataTable GetDataTable(EventCollection list,EntityBase entity)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("Event");
            dt.Columns.Add("EventTime");
            dt.Columns.Add("EventTimeName");
            dt.Columns.Add("Name");
            dt.Columns.Add("Code");
            dt.Columns.Add("Warning", typeof(bool));
            if (list == null)
                return dt;
            DataRow dr;
            foreach (EventBase even in list)
            {
                dr = dt.NewRow();
                dr["Id"] = even.Id;
                dr["Event"] = EventDevTypes.Instance.GetName(even);
                dr["EventTime"] = even.EventTime;
                dr["EventTimeName"] = (entity as IEventSupport).GetEventTimeName(even.EventTime);
                dr["Name"] = even.Name;
                dr["Code"] = even.Code;
                IWarningable warningable = even as IWarningable;
                if (warningable != null)
                    dr["Warning"] = warningable.Warning.ExistWarning;
                dt.Rows.Add(dr);
            }
            return dt;
        }
        public static EventCollection CopyEvents(EventCollection events)
        {
            EventCollection newEvents = new EventCollection(events.HostFormEntity, events.HostEntity);
            foreach (EventBase _event in events)
            {
                newEvents.Add(EventFactory.GetEventDevByXml(_event.ToXml()));
            }
            return newEvents;
        }
    }
}
