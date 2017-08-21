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
	public class ExceptionPolicySetting : NamedConfigurationSetting
	{
		public ExceptionPolicySetting(ConfigurationElement sourceElement, string name)
			: base(sourceElement, name)
		{
		}
		[ManagementEnumerator]
		public static IEnumerable<ExceptionPolicySetting> GetInstances()
		{
			return GetInstances<ExceptionPolicySetting>();
		}
		[ManagementBind]
		public static ExceptionPolicySetting BindInstance(string ApplicationName, string SectionName, string Name)
		{
			return BindInstance<ExceptionPolicySetting>(ApplicationName, SectionName, Name);
		}
		protected override bool SaveChanges(ConfigurationElement sourceElement)
		{
			return false; 
		}
	}
}
