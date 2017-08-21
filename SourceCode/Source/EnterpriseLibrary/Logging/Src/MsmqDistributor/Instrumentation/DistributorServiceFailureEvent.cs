/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Instrumentation
{
	public class DistributorServiceFailureEvent : DistributorServiceEvent
	{
		private string failureMessage;
		private Exception failureException;
		public DistributorServiceFailureEvent(string failureMessage, Exception failureException)
		{
			this.failureMessage = failureMessage;
			this.failureException = failureException;
		}
		public string FailureMessage
		{
			get { return failureMessage; }
		}
		public string FailureException
		{
			get { return failureException == null ? string.Empty : failureException.ToString(); }
		}
	}
}
