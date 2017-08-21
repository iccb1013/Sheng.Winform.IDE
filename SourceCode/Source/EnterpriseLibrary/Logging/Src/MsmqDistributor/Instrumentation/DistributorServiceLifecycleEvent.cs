/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Instrumentation
{
	public class DistributorServiceLifecycleEvent : DistributorServiceEvent
	{
		private string message;
		private bool started;
		public DistributorServiceLifecycleEvent(string message, bool started)
		{
			this.message = message;
			this.started = started;
		}
		public string Message
		{
			get { return message; }
		}
		public bool Started
		{
			get { return started; }
		}
		public string ApplicationBase
		{
			get { return AppDomain.CurrentDomain.SetupInformation.ApplicationBase; }
		}
		public string ApplicationName
		{
			get { return AppDomain.CurrentDomain.SetupInformation.ApplicationName; }
		}
		public string ConfigurationFile
		{
			get { return AppDomain.CurrentDomain.SetupInformation.ConfigurationFile; }
		}
	}
}
