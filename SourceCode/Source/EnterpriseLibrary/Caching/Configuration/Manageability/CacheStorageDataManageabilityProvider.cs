/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Manageability.Properties;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Manageability
{
	public class CacheStorageDataManageabilityProvider
		: ConfigurationElementManageabilityProviderBase<CacheStorageData>
	{
		public CacheStorageDataManageabilityProvider()
		{
			CacheStorageDataWmiMapper.RegisterWmiTypes();
		}
        protected override void AddAdministrativeTemplateDirectives(AdmContentBuilder contentBuilder,
			CacheStorageData configurationObject,
			IConfigurationSource configurationSource,
			String elementPolicyKeyName)
		{
			AddElementAdministrativeTemplateParts(contentBuilder,
				configurationObject,
				configurationSource,
				elementPolicyKeyName);
		}
        protected override void AddElementAdministrativeTemplateParts(AdmContentBuilder contentBuilder,
			CacheStorageData configurationObject,
			IConfigurationSource configurationSource,
			String elementPolicyKeyName)
		{
			contentBuilder.AddTextPart(Resources.NullBackingStoreNoSettingsPartName);
		}
        protected override string ElementPolicyNameTemplate
		{
			get
			{
				return null;	
			}
		}
        protected override void OverrideWithGroupPolicies(CacheStorageData configurationObject, IRegistryKey policyKey)
		{
		}
        protected override void GenerateWmiObjects(CacheStorageData configurationObject,
			ICollection<ConfigurationSetting> wmiSettings)
		{
			CacheStorageDataWmiMapper.GenerateWmiObjects(configurationObject, wmiSettings);
		}
	}
}
