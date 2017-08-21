/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
namespace Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation
{
	public class CacheFailureEventArgs : EventArgs
	{
		private string errorMessage;
		private Exception exception;
        public CacheFailureEventArgs(string errorMessage, Exception exception)
		{
			this.errorMessage = errorMessage;
			this.exception = exception;
		}
		public string ErrorMessage
		{
			get { return errorMessage; }
		}
		public Exception Exception
		{
			get { return exception; }
		}
	}
}
