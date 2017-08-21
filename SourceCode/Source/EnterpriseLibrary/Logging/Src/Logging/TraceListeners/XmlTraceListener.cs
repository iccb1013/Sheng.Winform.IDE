/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Xml.XPath;
using System.Xml.Serialization;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners
{
	public class XmlTraceListener : XmlWriterTraceListener
	{
		public XmlTraceListener(string filename) : base(filename) { }
		public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
		{
			object actualData = data;
			if (data is XmlLogEntry)
			{
				XmlLogEntry logEntryXml = data as XmlLogEntry;
				if (logEntryXml.Xml != null)
				{
					actualData = logEntryXml.Xml;
				}
				else
				{
					actualData = GetXml(logEntryXml);
				}
			}
			else if (data is LogEntry)
			{
				actualData = GetXml(data as LogEntry);
			}
			base.TraceData(eventCache, source, eventType, id, actualData);
		}
		internal virtual XPathNavigator GetXml(LogEntry logEntry)
		{
			return new XPathDocument(new StringReader(new XmlLogFormatter().Format(logEntry))).CreateNavigator();
		}
	}
}
