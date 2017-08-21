/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Diagnostics;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners.Tests
{
	public class ExceptionThrowingMockTraceListener : TraceListener
	{
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
		{
			throw new Exception("exception while tracing");
		}
		public override void Write(string message)
		{
			throw new Exception("The method or operation is not implemented.");
		}
		public override void WriteLine(string message)
		{
			throw new Exception("The method or operation is not implemented.");
		}
	}
}
