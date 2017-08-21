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
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.TraceListeners
{
	public static class RollingFlatFileTraceListenerDataWmiMapper
	{
		public static void GenerateWmiObjects(RollingFlatFileTraceListenerData configurationObject,
			ICollection<ConfigurationSetting> wmiSettings)
		{
			wmiSettings.Add(
				new RollingFlatFileTraceListenerSetting(configurationObject,
					configurationObject.Name,
					configurationObject.FileName,
					configurationObject.Header,
					configurationObject.Footer,
					configurationObject.Formatter,
					configurationObject.RollFileExistsBehavior.ToString(),
					configurationObject.RollInterval.ToString(),
					configurationObject.RollSizeKB,
					configurationObject.TimeStampPattern,
					configurationObject.TraceOutputOptions.ToString(),
					configurationObject.Filter.ToString()));
		}
		internal static bool SaveChanges(RollingFlatFileTraceListenerSetting setting,
			ConfigurationElement sourceElement)
		{
			RollingFlatFileTraceListenerData element = (RollingFlatFileTraceListenerData)sourceElement;
			element.FileName = setting.FileName;
			element.Footer = setting.Footer;
			element.Formatter = setting.Formatter;
			element.Header = setting.Header;
			element.RollFileExistsBehavior
				= ParseHelper.ParseEnum<RollFileExistsBehavior>(setting.RollFileExistsBehavior, false);
			element.RollInterval
				= ParseHelper.ParseEnum<RollInterval>(setting.RollInterval, false);
			element.RollSizeKB = setting.RollSizeKB;
			element.TimeStampPattern = setting.TimeStampPattern;
			element.TraceOutputOptions
				= ParseHelper.ParseEnum<TraceOptions>(setting.TraceOutputOptions, false);
		    SourceLevels filter;
            if(ParseHelper.TryParseEnum(setting.Filter, out filter))
            {
                element.Filter = filter;
            }
			return true;
		}
		internal static void RegisterWmiTypes()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(RollingFlatFileTraceListenerSetting));
		}
	}
}
