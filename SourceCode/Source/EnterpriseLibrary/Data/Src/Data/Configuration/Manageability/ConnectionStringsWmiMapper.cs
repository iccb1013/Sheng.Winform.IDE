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
	public static class ConnectionStringsWmiMapper
	{
		public static void GenerateWmiObjects(ConnectionStringsSection configurationObject,
			ICollection<ConfigurationSetting> wmiSettings)
		{
		}
		public static void GenerateConnectionStringWmiObjects(ConnectionStringSettings connectionString,
			ICollection<ConfigurationSetting> wmiSettings)
		{
			wmiSettings.Add(
				new ConnectionStringSetting(connectionString,
					connectionString.Name,
					connectionString.ConnectionString,
					connectionString.ProviderName));
		}
		public static bool SaveChanges(ConnectionStringSetting setting, ConfigurationElement sourceElement)
		{
			ConnectionStringSettings element = (ConnectionStringSettings)sourceElement;
			element.ConnectionString = setting.ConnectionString;
			element.ProviderName = setting.ProviderName;
			return true;
		}
		internal static void RegisterWmiTypes()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(ConnectionStringSetting));
		}
	}
}
