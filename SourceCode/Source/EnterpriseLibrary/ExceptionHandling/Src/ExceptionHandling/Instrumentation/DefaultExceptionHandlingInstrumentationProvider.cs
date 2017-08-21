/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Instrumentation
{
	[InstrumentationListener(typeof(DefaultExceptionHandlingEventLogger))]
	public class DefaultExceptionHandlingInstrumentationProvider
	{
		[InstrumentationProvider("ExceptionHandlingErrorOccurred")]
		public event EventHandler<DefaultExceptionHandlingErrorEventArgs> exceptionHandlingErrorOccurred;
		public void FireExceptionHandlingErrorOccurred(string policyName, string message)
		{
			if (exceptionHandlingErrorOccurred != null)
				exceptionHandlingErrorOccurred(this, new DefaultExceptionHandlingErrorEventArgs(policyName, message));
		}
	}
}
