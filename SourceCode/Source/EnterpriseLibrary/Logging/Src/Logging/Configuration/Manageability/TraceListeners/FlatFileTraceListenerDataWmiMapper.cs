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
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.TraceListeners
{
	public static class FlatFileTraceListenerDataWmiMapper
	{
		public static void GenerateWmiObjects(FlatFileTraceListenerData configurationObject,
			ICollection<ConfigurationSetting> wmiSettings)
		{
			wmiSettings.Add(
				new FlatFileTraceListenerSetting(configurationObject,
					configurationObject.Name,
					configurationObject.FileName,
					configurationObject.Header,
					configurationObject.Footer,
					configurationObject.Formatter,
					configurationObject.TraceOutputOptions.ToString(),
					configurationObject.Filter.ToString()));
		}
		internal static bool SaveChanges(FlatFileTraceListenerSetting setting, ConfigurationElement sourceElement)
		{
			FlatFileTraceListenerData element = (FlatFileTraceListenerData)sourceElement;
			element.FileName = setting.FileName;
			element.Footer = setting.Footer;
			element.Formatter = setting.Formatter;
			element.Header = setting.Header;
		    SourceLevels filter;
            if (ParseHelper.TryParseEnum(setting.Filter, out filter))
            {
                element.Filter = filter;
            }
		    element.TraceOutputOptions = ParseHelper.ParseEnum<TraceOptions>(setting.TraceOutputOptions, false);
			return true;
		}
		internal static void RegisterWmiTypes()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(FlatFileTraceListenerSetting));
		}
	}
}
