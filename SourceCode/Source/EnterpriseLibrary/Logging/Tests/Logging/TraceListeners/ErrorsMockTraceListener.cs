/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners.Tests
{
	public class ErrorsMockTraceListener : TraceListener
	{
		public object tracedData = null;
		public string tracedSource = null;
		public TraceEventType tracedEventType = TraceEventType.Information;
		private static List<LogEntry> entries = new List<LogEntry>();
		public ErrorsMockTraceListener()
			: this(string.Empty)
		{
		}
		public ErrorsMockTraceListener(string name)
		{
			this.Name = name;
		}
		public static void Reset()
		{
			entries.Clear();
		}
		public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
		{
			tracedData = data;
			tracedSource = source;
			tracedEventType = eventType;
			entries.Add(data as LogEntry);
		}
		public override void Write(string message)
		{
			throw new Exception("The method or operation is not implemented.");
		}
		public override void WriteLine(string message)
		{
			throw new Exception("The method or operation is not implemented.");
		}
		public static List<LogEntry> Entries
		{
			get { return entries; }
			set { entries = value; }
		}
		public static LogEntry LastEntry
		{
			get { return entries.Count > 0 ? entries[entries.Count - 1] : null; }
		}
	}
}
