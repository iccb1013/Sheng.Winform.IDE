/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Manageability.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Manageability
{
	public class ConnectionStringsManageabilityProvider
		: ConfigurationSectionManageabilityProviderBase<ConnectionStringsSection>
	{
		public const String ConnectionStringPropertyName = "connectionString";
		public const String ProviderNamePropertyName = "providerName";
		public ConnectionStringsManageabilityProvider(IDictionary<Type, ConfigurationElementManageabilityProvider> subProviders)
			: base(subProviders)
		{
			ConnectionStringsWmiMapper.RegisterWmiTypes();
		}
        protected override void AddAdministrativeTemplateDirectives(AdmContentBuilder contentBuilder,
			ConnectionStringsSection configurationSection,
			IConfigurationSource configurationSource,
			String sectionKey)
		{
			contentBuilder.StartCategory(Resources.ConnectionStringsCategoryName);
			{
				foreach (ConnectionStringSettings connectionString in configurationSection.ConnectionStrings)
				{
					contentBuilder.StartPolicy(String.Format(CultureInfo.InvariantCulture,
															Resources.ConnectionStringPolicyNameTemplate,
															connectionString.Name),
						sectionKey + @"\" + connectionString.Name);
					contentBuilder.AddEditTextPart(Resources.ConnectionStringConnectionStringPartName,
						ConnectionStringPropertyName,
						connectionString.ConnectionString,
						500,
						true);
					contentBuilder.AddComboBoxPart(Resources.ConnectionStringProviderNamePartName,
						ProviderNamePropertyName,
						connectionString.ProviderName,
						255,
						true,
						"System.Data.SqlClient",
						"System.Data.OracleClient");
					contentBuilder.EndPolicy();
				}
			}
			contentBuilder.EndCategory();
		}
		protected override string SectionCategoryName
		{
			get { return Resources.DatabaseCategoryName; }
		}
		protected override string SectionName
		{
			get { return "connectionStrings"; }
		}
		protected override void OverrideWithGroupPoliciesForConfigurationSection(ConnectionStringsSection configurationSection,
			IRegistryKey policyKey)
		{
		}
		protected override void GenerateWmiObjectsForConfigurationSection(ConnectionStringsSection configurationSection,
			ICollection<ConfigurationSetting> wmiSettings)
		{
			ConnectionStringsWmiMapper.GenerateWmiObjects(configurationSection, wmiSettings);
		}
		protected override void OverrideWithGroupPoliciesAndGenerateWmiObjectsForConfigurationElements(ConnectionStringsSection configurationSection,
			bool readGroupPolicies, IRegistryKey machineKey, IRegistryKey userKey,
			bool generateWmiObjects, ICollection<ConfigurationSetting> wmiSettings)
		{
			List<ConnectionStringSettings> elementsToRemove = new List<ConnectionStringSettings>();
			foreach (ConnectionStringSettings connectionString in configurationSection.ConnectionStrings)
			{
				IRegistryKey machineOverrideKey = null;
				IRegistryKey userOverrideKey = null;
				try
				{
					LoadRegistrySubKeys(connectionString.Name, machineKey, userKey, out machineOverrideKey, out userOverrideKey);
					if (!OverrideWithGroupPoliciesAndGenerateWmiObjectsForConnectionString(connectionString,
							readGroupPolicies, machineOverrideKey, userOverrideKey,
							generateWmiObjects, wmiSettings))
					{
						elementsToRemove.Add(connectionString);
					}
				}
				finally
				{
					ReleaseRegistryKeys(machineOverrideKey, userOverrideKey);
				}
			}
			foreach (ConnectionStringSettings connectionString in elementsToRemove)
			{
				configurationSection.ConnectionStrings.Remove(connectionString);
			}
		}
		private bool OverrideWithGroupPoliciesAndGenerateWmiObjectsForConnectionString(ConnectionStringSettings connectionString,
			bool readGroupPolicies, IRegistryKey machineKey, IRegistryKey userKey,
			bool generateWmiObjects, ICollection<ConfigurationSetting> wmiSettings)
		{
			if (readGroupPolicies)
			{
				IRegistryKey policyKey = machineKey ?? userKey;
				if (policyKey != null)
				{
					if (policyKey.IsPolicyKey && !policyKey.GetBoolValue(PolicyValueName).Value)
					{
						return false;
					}
					try
					{
						String connectionStringOverride = policyKey.GetStringValue(ConnectionStringPropertyName);
						String providerNameOverride = policyKey.GetStringValue(ProviderNamePropertyName);
						connectionString.ConnectionString = connectionStringOverride;
						connectionString.ProviderName = providerNameOverride;
					}
					catch (RegistryAccessException ex)
					{
						LogExceptionWhileOverriding(ex);
					}
				}
			}
			if (generateWmiObjects)
			{
				ConnectionStringsWmiMapper.GenerateConnectionStringWmiObjects(connectionString, wmiSettings);
			}
			return true;
		}
	}
}
