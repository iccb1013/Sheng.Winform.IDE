/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Manageability.Properties;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Manageability
{
	public class CustomCacheManagerDataManageabilityProvider
		: CustomProviderDataManageabilityProvider<CustomCacheManagerData>
	{
		public CustomCacheManagerDataManageabilityProvider()
			: base(Resources.CacheManagerPolicyNameTemplate)
		{
			CustomCacheManagerDataWmiMapper.RegisterWmiTypes();
		}
		internal new void AddAdministrativeTemplateDirectives(AdmContentBuilder contentBuilder,
			ConfigurationElement configurationObject,
			IConfigurationSource configurationSource,
			string parentKey)
		{
			base.AddAdministrativeTemplateDirectives(contentBuilder,
				configurationObject,
				configurationSource,
				parentKey);
		}
		internal new bool OverrideWithGroupPoliciesAndGenerateWmiObjects(ConfigurationElement configurationObject,
			bool readGroupPolicies, IRegistryKey machineKey, IRegistryKey userKey,
			bool generateWmiObjects, ICollection<ConfigurationSetting> wmiSettings)
		{
			return base.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, readGroupPolicies, machineKey, userKey, generateWmiObjects,
				wmiSettings);
		}
        protected override void GenerateWmiObjects(CustomCacheManagerData configurationObject, 
			ICollection<ConfigurationSetting> wmiSettings)
		{
			CustomCacheManagerDataWmiMapper.GenerateWmiObjects(configurationObject, wmiSettings);
		}
	}
}
