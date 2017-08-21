/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Manageability
{
	public static class DatabaseSettingsWmiMapper
	{
		public static void GenerateWmiObjects(DatabaseSettings configurationObject,
			ICollection<ConfigurationSetting> wmiSettings)
		{
			wmiSettings.Add(new DatabaseBlockSetting(configurationObject, configurationObject.DefaultDatabase));
		}
		public static bool SaveChanges(DatabaseBlockSetting setting, ConfigurationElement sourceElement)
		{
			DatabaseSettings section = (DatabaseSettings)sourceElement;
			section.DefaultDatabase = setting.DefaultDatabase;
			return true;
		}
		public static void GenerateDbProviderMappingWmiObjects(DbProviderMapping providerMapping,
			ICollection<ConfigurationSetting> wmiSettings)
		{
			wmiSettings.Add(
				new ProviderMappingSetting(providerMapping,
					providerMapping.DbProviderName,
					providerMapping.DatabaseType.AssemblyQualifiedName));
		}
		public static bool SaveChanges(ProviderMappingSetting providerMappingSetting, ConfigurationElement sourceElement)
		{
			DbProviderMapping element = (DbProviderMapping)sourceElement;
			element.DatabaseTypeName = providerMappingSetting.DatabaseType;
			return true;
		}
		internal static void RegisterWmiTypes()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(DatabaseBlockSetting),
				typeof(ProviderMappingSetting));
		}
	}
}
