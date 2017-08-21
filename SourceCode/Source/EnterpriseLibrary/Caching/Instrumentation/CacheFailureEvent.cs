/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation
{
	public class CacheFailureEvent : CacheEvent
	{
		private string errorMessage;
		private string exceptionMessage;
		public CacheFailureEvent(string instanceName, string errorMessage, string exceptionMessage)
			: base(instanceName)
		{
			this.errorMessage = errorMessage;
			this.exceptionMessage = exceptionMessage;
		}
		public string ErrorMessage
		{
			get { return errorMessage; }
		}
		public string ExceptionMessage
		{
			get { return exceptionMessage; }
		}
	}
}
