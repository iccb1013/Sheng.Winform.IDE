/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Instrumentation
{
    [InstrumentationListener(typeof(ExceptionHandlingInstrumentationListener))]
	public class ExceptionHandlingInstrumentationProvider
	{
        [InstrumentationProvider("ExceptionHandlerExecuted")]
		public event EventHandler<EventArgs> exceptionHandlerExecuted;
		[InstrumentationProvider("ExceptionHandled")]
		public event EventHandler<EventArgs> exceptionHandled;
		[InstrumentationProvider("ExceptionHandlingErrorOccurred")]
		public event EventHandler<ExceptionHandlingErrorEventArgs> exceptionHandlingErrorOccurred;
		public void FireExceptionHandledEvent()
		{
			if (exceptionHandled != null) exceptionHandled(this, new EventArgs());
		}
		public void FireExceptionHandlerExecutedEvent()
		{
			if (exceptionHandlerExecuted != null) exceptionHandlerExecuted(this, new EventArgs());
		}
		public void FireExceptionHandlingErrorOccurred(string errorMessage)
		{
			if (exceptionHandlingErrorOccurred != null) exceptionHandlingErrorOccurred(this, new ExceptionHandlingErrorEventArgs(errorMessage));
		}
	}
}
