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
	public static class XmlTraceListenerDataWmiMapper
	{
		public static void GenerateWmiObjects(XmlTraceListenerData configurationObject,
											  ICollection<ConfigurationSetting> wmiSettings)
		{
			wmiSettings.Add(
				new XmlTraceListenerSetting(configurationObject,
											configurationObject.Name,
											configurationObject.FileName,
											configurationObject.TraceOutputOptions.ToString(),
											configurationObject.Filter.ToString()));
		}
		internal static void RegisterWmiTypes()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(XmlTraceListenerSetting));
		}
		internal static bool SaveChanges(XmlTraceListenerSetting setting,
										 ConfigurationElement sourceElement)
		{
			XmlTraceListenerData element = (XmlTraceListenerData)sourceElement;
			element.FileName = setting.FileName;
			element.TraceOutputOptions = ParseHelper.ParseEnum<TraceOptions>(setting.TraceOutputOptions, false);
		    SourceLevels filter;
            if(ParseHelper.TryParseEnum(setting.Filter, out filter))
            {
                element.Filter = filter;
            }
			return true;
		}
	}
}
