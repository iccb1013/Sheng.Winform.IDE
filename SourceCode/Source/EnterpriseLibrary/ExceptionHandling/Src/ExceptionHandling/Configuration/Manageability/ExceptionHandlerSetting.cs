/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Configuration;
using System.Management.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Manageability
{
	[ManagementEntity]
	public abstract class ExceptionHandlerSetting : ConfigurationSetting
	{
		private static readonly PublishedInstancesManager<ExceptionHandlerSetting, PublishedInstanceKey>
			publishedInstancesManager = new PublishedInstancesManager<ExceptionHandlerSetting, PublishedInstanceKey>();
		private string name;
		private string policy;
		private string exceptionType;
		private int order;
		protected ExceptionHandlerSetting(ConfigurationElement sourceElement, string name)
			: base(sourceElement)
		{
			this.name = name;
		}
		public override void Publish()
		{
			PublishedInstanceKey key =
				new PublishedInstanceKey(this.ApplicationName, this.SectionName, this.policy, this.exceptionType, this.name);
			publishedInstancesManager.Publish(this, key);
		}
		public override void Revoke()
		{
			PublishedInstanceKey key =
				new PublishedInstanceKey(this.ApplicationName, this.SectionName, this.policy, this.exceptionType, this.name);
			publishedInstancesManager.Revoke(this, key);
		}
		public static void ClearPublishedInstances()
		{
			publishedInstancesManager.ClearPublishedInstances();
		}
		protected static IEnumerable<T> GetInstances<T>()
			where T : ExceptionHandlerSetting
		{
			return publishedInstancesManager.GetInstances<T>();
		}
		public static T BindInstance<T>(string applicationName,
		                                string sectionName,
		                                string policy,
		                                string exceptionType,
		                                string name)
			where T : ExceptionHandlerSetting
		{
			PublishedInstanceKey key = new PublishedInstanceKey(applicationName, sectionName, policy, exceptionType, name);
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
		[ManagementKey]
		public string ExceptionType
		{
			get { return exceptionType; }
			set { exceptionType = value; }
		}
		[ManagementKey]
		public string Policy
		{
			get { return policy; }
			set { policy = value; }
		}
		[ManagementProbe]
		public int Order
		{
			get { return order; }
			set { order = value; }
		}
		private struct PublishedInstanceKey
		{
			private readonly string applicationName;
			private readonly string sectionName;
			private readonly string policy;
			private readonly string exceptionType;
			private readonly string name;
			public PublishedInstanceKey(string applicationName,
			                            string sectionName,
			                            string policy,
			                            string exceptionType,
			                            string name)
			{
				this.applicationName = applicationName;
				this.sectionName = sectionName;
				this.policy = policy;
				this.exceptionType = exceptionType;
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
					       && this.sectionName == otherKey.sectionName
					       && this.policy == otherKey.policy
					       && this.exceptionType == otherKey.exceptionType;
				}
				return false;
			}
		}
	}
}
