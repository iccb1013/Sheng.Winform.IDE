/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.XPath;
using System.Diagnostics;
using System.IO;
using System.Management.Instrumentation;
namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
	[Serializable]
	public class XmlLogEntry : LogEntry
	{
		public XmlLogEntry() : base() {}
		public XmlLogEntry(object message, ICollection<string> category, int priority, int eventId, TraceEventType severity, string title, IDictionary<string, object> properties) : base(message, category, priority, eventId, severity, title, properties) { }
		private string xmlString = null;
		[NonSerialized]
		private XPathNavigator xml;
		[IgnoreMember]
		public XPathNavigator Xml
		{
			get 
			{
				if (xml == null && !string.IsNullOrEmpty(xmlString))
				{
					TextReader reader = new StringReader(xmlString);
					xml = new XPathDocument(reader).CreateNavigator();
				}
				return xml; 
			}
			set 
			{
				if (xmlString == null && value != null)
				{
					xmlString = value.InnerXml;
				}
				else
				{
					xmlString = null;
				}
				xml = value; 
			}
		}
	}
}
