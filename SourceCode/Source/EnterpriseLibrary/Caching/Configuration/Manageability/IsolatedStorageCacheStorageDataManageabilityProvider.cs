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
	public class IsolatedStorageCacheStorageDataManageabilityProvider
		: ConfigurationElementManageabilityProviderBase<IsolatedStorageCacheStorageData>
	{
		public const String PartitionNamePropertyName = "partitionName";
		public IsolatedStorageCacheStorageDataManageabilityProvider()
		{
			IsolatedStorageCacheStorageDataWmiMapper.RegisterWmiTypes();
		}
        protected override void AddAdministrativeTemplateDirectives(AdmContentBuilder contentBuilder,
			IsolatedStorageCacheStorageData configurationObject,
			IConfigurationSource configurationSource,
			String elementPolicyKeyName)
		{
			AddElementAdministrativeTemplateParts(contentBuilder,
				configurationObject,
				configurationSource,
				elementPolicyKeyName);
		}
        protected override void AddElementAdministrativeTemplateParts(AdmContentBuilder contentBuilder,
			IsolatedStorageCacheStorageData configurationObject,
			IConfigurationSource configurationSource,
			String elementPolicyKeyName)
		{
			contentBuilder.AddEditTextPart(Resources.IsolatedStorageCacheStorageDataPartitionNamePartName,
				elementPolicyKeyName,
				PartitionNamePropertyName,
				configurationObject.PartitionName,
				255,
				true);
		}
        protected override string ElementPolicyNameTemplate
		{
			get
			{
				return null;	
			}
		}
        protected override void OverrideWithGroupPolicies(IsolatedStorageCacheStorageData configurationObject, IRegistryKey policyKey)
		{
			String partitionNameOverride = policyKey.GetStringValue(PartitionNamePropertyName);
			configurationObject.PartitionName = partitionNameOverride;
		}
        protected override void GenerateWmiObjects(IsolatedStorageCacheStorageData configurationObject,
			ICollection<ConfigurationSetting> wmiSettings)
		{
			IsolatedStorageCacheStorageDataWmiMapper.GenerateWmiObjects(configurationObject, wmiSettings);
		}
	}
}
