/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Manageability
{
	public static class OracleConnectionSettingsWmiMapper
	{
		public static void GenerateWmiObjects(OracleConnectionSettings configurationObject,
			ICollection<ConfigurationSetting> wmiSettings)
		{
		}
		public static void GenerateOracleConnectionSettingWmiObjects(OracleConnectionData connectionData,
			ICollection<ConfigurationSetting> wmiSettings)
		{
			string[] packages = GeneratePackagesArray(connectionData.Packages);
			wmiSettings.Add(new OracleConnectionSetting(connectionData, connectionData.Name, packages));
		}
		private static string[] GeneratePackagesArray(NamedElementCollection<OraclePackageData> packages)
		{
			string[] packagesArray = new string[packages.Count];
			int i = 0;
			foreach (OraclePackageData package in packages)
			{
				packagesArray[i++]
					= KeyValuePairEncoder.EncodeKeyValuePair(package.Name, package.Prefix);
			}
			return packagesArray;
		}
		public static bool SaveChanges(OracleConnectionSetting setting, ConfigurationElement sourceElement)
		{
			OracleConnectionData element = (OracleConnectionData)sourceElement;
			Dictionary<String, String> packagesDictionary = new Dictionary<string, string>();
			foreach (string encodedKeyValuePair in setting.Packages)
			{
				KeyValuePairParser.ExtractKeyValueEntries(encodedKeyValuePair, packagesDictionary);
			}
			element.Packages.Clear();
			foreach (KeyValuePair<String, String> kvp in packagesDictionary)
			{
				element.Packages.Add(new OraclePackageData(kvp.Key, kvp.Value));
			}
			return true;
		}
		internal static void RegisterWmiTypes()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(OracleConnectionSetting));
		}
	}
}
