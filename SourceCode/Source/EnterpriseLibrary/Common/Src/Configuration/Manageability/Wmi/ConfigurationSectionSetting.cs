/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Configuration;
using System.Management.Instrumentation;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
	[ManagementEntity]
	public abstract class ConfigurationSectionSetting : ConfigurationSetting
	{
		private static readonly PublishedInstancesManager<ConfigurationSectionSetting, PublishedInstanceKey>
			publishedInstancesManager = new PublishedInstancesManager<ConfigurationSectionSetting, PublishedInstanceKey>();
		protected ConfigurationSectionSetting()
		{
		}
		protected ConfigurationSectionSetting(ConfigurationElement sourceElement)
			: base(sourceElement)
		{
		}
		public override void Publish()
		{
			PublishedInstanceKey key = new PublishedInstanceKey(this.ApplicationName, this.SectionName);
			publishedInstancesManager.Publish(this, key);
		}
		public override void Revoke()
		{
			PublishedInstanceKey key = new PublishedInstanceKey(this.ApplicationName, this.SectionName);
			publishedInstancesManager.Revoke(this, key);
		}
		public static void ClearPublishedInstances()
		{
			publishedInstancesManager.ClearPublishedInstances();
		}
		protected static IEnumerable<T> GetInstances<T>()
			where T : ConfigurationSectionSetting
		{
			return publishedInstancesManager.GetInstances<T>();
		}
		protected static T BindInstance<T>(string applicationName, string sectionName)
			where T : ConfigurationSectionSetting
		{
			PublishedInstanceKey key = new PublishedInstanceKey(applicationName, sectionName);
			return publishedInstancesManager.BindInstance<T>(key);
		}
		[ManagementKey]
		[ManagementQualifier("Override", Value = "ApplicationName")]
		public override string ApplicationName
		{
			get { return base.ApplicationName; }
			set { base.ApplicationName = value; }
		}
		[ManagementKey]
		[ManagementQualifier("Override", Value = "SectionName")]
		public override string SectionName
		{
			get { return base.SectionName; }
			set { base.SectionName = value; }
		}
		private struct PublishedInstanceKey
		{
			private readonly string applicationName;
			private readonly string sectionName;
			public PublishedInstanceKey(string applicationName, string sectionName)
			{
				this.applicationName = applicationName;
				this.sectionName = sectionName;
			}
			public override int GetHashCode()
			{
				return this.sectionName != null ? this.sectionName.GetHashCode() : 0;
			}
			public override bool Equals(object obj)
			{
				if (obj is PublishedInstanceKey)
				{
					PublishedInstanceKey otherKey = (PublishedInstanceKey) obj;
					return this.applicationName == otherKey.applicationName
					       && this.sectionName == otherKey.sectionName;
				}
				return false;
			}
		}
	}
}
