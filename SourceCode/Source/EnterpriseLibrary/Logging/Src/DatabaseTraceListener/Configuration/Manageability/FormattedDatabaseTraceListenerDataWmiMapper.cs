/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration.Manageability
{
	public static class FormattedDatabaseTraceListenerDataWmiMapper 
	{
		public static void GenerateWmiObjects(FormattedDatabaseTraceListenerData configurationObject,
			ICollection<ConfigurationSetting> wmiSettings)
		{
			wmiSettings.Add(
				new FormattedDatabaseTraceListenerSetting(configurationObject,
					configurationObject.Name,
					configurationObject.DatabaseInstanceName,
					configurationObject.WriteLogStoredProcName,
					configurationObject.AddCategoryStoredProcName,
					configurationObject.Formatter,
					configurationObject.TraceOutputOptions.ToString(),
					configurationObject.Filter.ToString()));
		}
		internal static void RegisterWmiTypes()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(FormattedDatabaseTraceListenerSetting));
		}
        internal static bool SaveChanges(FormattedDatabaseTraceListenerSetting setting, ConfigurationElement sourceElement)
        {
            var element = (FormattedDatabaseTraceListenerData) sourceElement;
            element.AddCategoryStoredProcName = setting.AddCategoryStoredProcName;
            element.DatabaseInstanceName = setting.DatabaseInstanceName;
            element.Formatter = setting.Formatter;
            element.WriteLogStoredProcName = setting.WriteLogStoredProcName;
            SourceLevels filter;
            TraceOptions traceOptions;
            if(ParseHelper.TryParseEnum(setting.Filter, out filter))
            {
                element.Filter = filter;
            }
            if(ParseHelper.TryParseEnum(setting.TraceOutputOptions, out traceOptions))
            {
                element.TraceOutputOptions = traceOptions;
            }
            return true;
        }
	}
}
