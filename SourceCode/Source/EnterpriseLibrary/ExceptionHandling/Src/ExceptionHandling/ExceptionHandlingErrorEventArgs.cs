/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Instrumentation;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
{
	public class ExceptionHandlingErrorEventArgs : EventArgs
	{
		string message;
		public ExceptionHandlingErrorEventArgs(string message)
		{
			this.message = message;
		}
		public string Message { get { return message; } }
	}
}
