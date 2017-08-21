/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.Manageability
{
	public static class LoggingExceptionHandlerDataWmiMapper
	{
		public static void GenerateWmiObjects(LoggingExceptionHandlerData configurationObject,
			ICollection<ConfigurationSetting> wmiSettings)
		{
			wmiSettings.Add(
				new LoggingExceptionHandlerSetting(configurationObject,
					configurationObject.Name,
					configurationObject.EventId,
					configurationObject.FormatterType.AssemblyQualifiedName,
					configurationObject.LogCategory,
					configurationObject.Priority,
					configurationObject.Severity.ToString(),
					configurationObject.Title));
		}
		public static bool SaveChanges(LoggingExceptionHandlerSetting loggingExceptionHandlerSettingSetting, ConfigurationElement sourceElement)
		{
			LoggingExceptionHandlerData element = (LoggingExceptionHandlerData)sourceElement;
			element.EventId = loggingExceptionHandlerSettingSetting.EventId;
			element.Title = loggingExceptionHandlerSettingSetting.Title;
			element.Priority = loggingExceptionHandlerSettingSetting.Priority;
			element.LogCategory = loggingExceptionHandlerSettingSetting.LogCategory;
			element.FormatterTypeName = loggingExceptionHandlerSettingSetting.FormatterType;
			return true;
		}
		internal static void RegisterWmiTypes()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(LoggingExceptionHandlerSetting));
		}
	}
}
