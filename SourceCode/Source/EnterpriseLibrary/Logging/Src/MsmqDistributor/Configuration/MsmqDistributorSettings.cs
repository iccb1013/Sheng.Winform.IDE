/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Configuration
{
	public class MsmqDistributorSettings : SerializableConfigurationSection
	{
		private const string msmqPathProperty = "msmqPath";
		private const string queueTimerIntervalProperty = "queueTimerInterval";
		private const string serviceNameProperty = "serviceName";
		public static MsmqDistributorSettings GetSettings(IConfigurationSource configurationSource)
		{
			return configurationSource.GetSection(SectionName) as MsmqDistributorSettings;
		}
		[ConfigurationProperty(msmqPathProperty, IsRequired= true)]
		public string MsmqPath
		{
			get { return (string)base[msmqPathProperty]; }
			set { base[msmqPathProperty] = value; }
		}
		[ConfigurationProperty(queueTimerIntervalProperty, IsRequired= false)]
		public int QueueTimerInterval
		{
			get { return (int)base[queueTimerIntervalProperty]; }
			set { base[queueTimerIntervalProperty] = value; }
		}
		[ConfigurationProperty(serviceNameProperty, IsRequired= true)]
		public string ServiceName
		{
			get { return (string)base[serviceNameProperty]; }
			set { base[serviceNameProperty] = value; }
		}
		public static string SectionName
		{
			get { return "msmqDistributorSettings"; }
		}
	}
}
