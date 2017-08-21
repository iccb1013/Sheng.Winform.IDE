/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
{
	public class DefaultExceptionHandlingErrorEventArgs : EventArgs
	{
		private readonly string policyName;
		private readonly string message;
		public DefaultExceptionHandlingErrorEventArgs(string policyName, string message)
		{
			this.policyName = policyName;
			this.message = message;
		}
		public string PolicyName { get { return policyName; } }
		public string Message { get { return message; } }
	}
}
