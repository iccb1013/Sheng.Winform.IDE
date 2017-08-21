/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners.Tests
{
	public class MockTraceListener : TraceListener
	{
		public object tracedData = null;
		public string tracedSource = null;
		public TraceEventType tracedEventType = TraceEventType.Information;
		public bool wasDisposed = false;
		private static object traceRequestMonitor = new object();
		private static int processedTraceRequests = 0;
		private static List<LogEntry> entries = new List<LogEntry>();
		private static List<MockTraceListener> instances = new List<MockTraceListener>();
		private static List<Guid> transferGuids = new List<Guid>();
		public MockTraceListener()
			: this("")
		{
		}
		public MockTraceListener(string name)
		{
			this.Name = name;
		}
		public static void Reset()
		{
			lock (traceRequestMonitor)
			{
				entries.Clear();
				instances.Clear();
				transferGuids.Clear();
				processedTraceRequests = 0;
			}
		}
		public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
		{
			lock (traceRequestMonitor)
			{
				tracedData = data;
				tracedSource = source;
				tracedEventType = eventType;
				MockTraceListener.Entries.Add(data as LogEntry);
				MockTraceListener.Instances.Add(this);
				processedTraceRequests++;
			}
		}
		public override void TraceTransfer(TraceEventCache eventCache, string source, int id, string message, Guid relatedActivityId)
		{
			lock (traceRequestMonitor)
			{
				MockTraceListener.transferGuids.Add(relatedActivityId);
			}
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
		}
		public static List<MockTraceListener> Instances
		{
			get { return instances; }
		}
		public static List<Guid> TransferGuids
		{
			get { return transferGuids; }
		}
		public static int ProcessedTraceRequests
		{
			get { lock (traceRequestMonitor) { return processedTraceRequests; } }
		}
		public static LogEntry LastEntry
		{
			get { return entries.Count > 0 ? entries[entries.Count - 1] : null; }
		}
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			wasDisposed = true;
		}
	}
}
