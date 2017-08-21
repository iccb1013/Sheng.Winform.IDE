/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Manageability
{
	public static class ReplaceHandlerDataWmiMapper
	{
		public static void GenerateWmiObjects(ReplaceHandlerData configurationObject,
			ICollection<ConfigurationSetting> wmiSettings)
		{
			wmiSettings.Add(
				new ReplaceHandlerSetting(configurationObject,
					configurationObject.Name,
					configurationObject.ExceptionMessage,
					configurationObject.ReplaceExceptionType.AssemblyQualifiedName));
		}
		public static bool SaveChanges(ReplaceHandlerSetting replaceHandlerSettingSetting, ConfigurationElement sourceElement)
		{
			ReplaceHandlerData element = (ReplaceHandlerData)sourceElement;
			element.ReplaceExceptionTypeName = replaceHandlerSettingSetting.ReplaceExceptionType;
			element.ExceptionMessage = replaceHandlerSettingSetting.ExceptionMessage;
			return true;
		}
		internal static void RegisterWmiTypes()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(ReplaceHandlerSetting));
		}
	}
}
