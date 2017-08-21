/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners
{
	public abstract class CustomTraceListener : TraceListener
	{
		private ILogFormatter formatter;
		protected CustomTraceListener()
		{
		}
		public ILogFormatter Formatter
		{
			get { return this.formatter; }
			set { this.formatter = value; }
		}
	}
}
