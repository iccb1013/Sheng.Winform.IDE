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
	public abstract class NamedConfigurationSetting : ConfigurationSetting
	{
		private static readonly PublishedInstancesManager<NamedConfigurationSetting, PublishedInstanceKey>
			publishedInstancesManager = new PublishedInstancesManager<NamedConfigurationSetting, PublishedInstanceKey>();
		private string name;
		protected NamedConfigurationSetting(string name)
		{
			this.Name = name;
		}
		protected NamedConfigurationSetting(ConfigurationElement sourceElement, string name)
			: base(sourceElement)
		{
			this.Name = name;
		}
		public override void Publish()
		{
			PublishedInstanceKey key = new PublishedInstanceKey(this.ApplicationName, this.SectionName, this.Name);
			publishedInstancesManager.Publish(this, key);
		}
		public override void Revoke()
		{
			PublishedInstanceKey key = new PublishedInstanceKey(this.ApplicationName, this.SectionName, this.Name);
			publishedInstancesManager.Revoke(this, key);
		}
		public static void ClearPublishedInstances()
		{
			publishedInstancesManager.ClearPublishedInstances();
		}
		protected static IEnumerable<T> GetInstances<T>()
			where T : NamedConfigurationSetting
		{
			return publishedInstancesManager.GetInstances<T>();
		}
		public static T BindInstance<T>(string applicationName, string sectionName, string name)
			where T : NamedConfigurationSetting
		{
			PublishedInstanceKey key = new PublishedInstanceKey(applicationName, sectionName, name);
			return publishedInstancesManager.BindInstance<T>(key);
		}
		[ManagementKey]
		public string Name
		{
			get { return name; }
			set { name = value; }
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
			private readonly string name;
			public PublishedInstanceKey(string applicationName, string sectionName, string name)
			{
				this.applicationName = applicationName;
				this.sectionName = sectionName;
				this.name = name;
			}
			public override int GetHashCode()
			{
				return this.name != null ? this.name.GetHashCode() : 0;
			}
			public override bool Equals(object obj)
			{
				if (obj is PublishedInstanceKey)
				{
					PublishedInstanceKey otherKey = (PublishedInstanceKey) obj;
					return this.name == otherKey.name
					       && this.applicationName == otherKey.applicationName
					       && this.sectionName == otherKey.sectionName;
				}
				return false;
			}
		}
	}
}
