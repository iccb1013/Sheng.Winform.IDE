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
	internal static class CustomCacheManagerDataWmiMapper
	{
		public static void GenerateWmiObjects(CustomCacheManagerData data,
			ICollection<ConfigurationSetting> wmiSettings)
		{
			wmiSettings.Add(
				new CustomCacheManagerSetting(data.Name,
					data.Type.AssemblyQualifiedName,
					CustomDataWmiMapperHelper.GenerateAttributesArray(data.Attributes)));
		}
		internal static void RegisterWmiTypes()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(CustomCacheManagerSetting));
		}
	}
}
