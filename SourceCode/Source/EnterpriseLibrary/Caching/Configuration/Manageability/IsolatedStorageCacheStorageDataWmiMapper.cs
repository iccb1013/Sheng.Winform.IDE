/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Manageability
{
	internal static class IsolatedStorageCacheStorageDataWmiMapper 
	{
		public static void GenerateWmiObjects(IsolatedStorageCacheStorageData configurationObject,
			ICollection<ConfigurationSetting> wmiSettings)
		{
			wmiSettings.Add(
				new IsolatedStorageCacheStorageSetting(configurationObject.Name,
					configurationObject.PartitionName,
					configurationObject.StorageEncryption));
		}
		internal static void RegisterWmiTypes()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(IsolatedStorageCacheStorageSetting));
		}
	}
}
