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
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.TraceListeners
{
	public static class FormattedEventLogTraceListenerDataWmiMapper
	{
		public static void GenerateWmiObjects(FormattedEventLogTraceListenerData configurationObject,
			ICollection<ConfigurationSetting> wmiSettings)
		{
			wmiSettings.Add(
				new FormattedEventLogTraceListenerSetting(configurationObject,
					configurationObject.Name,
					configurationObject.Source,
					configurationObject.Log,
					configurationObject.MachineName,
					configurationObject.Formatter,
					configurationObject.TraceOutputOptions.ToString(),
					configurationObject.Filter.ToString()));
		}
		internal static void RegisterWmiTypes()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(FormattedEventLogTraceListenerSetting));
		}
        internal static bool SaveChanges(FormattedEventLogTraceListenerSetting setting, ConfigurationElement sourceElement)
        {
            var element = (FormattedEventLogTraceListenerData) sourceElement;
            element.Formatter = setting.Formatter;
            element.MachineName = setting.MachineName;
            element.Log = setting.Log;
            element.Source = setting.Source;
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
