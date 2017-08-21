/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
	public static class InstrumentationConfigurationSectionWmiMapper
	{
		public static void GenerateWmiObjects(InstrumentationConfigurationSection configurationObject,
			ICollection<ConfigurationSetting> wmiSettings)
		{
			wmiSettings.Add(new InstrumentationSetting(configurationObject,
					configurationObject.EventLoggingEnabled,
					configurationObject.PerformanceCountersEnabled,
					configurationObject.WmiEnabled));
		}
		public static bool SaveChanges(InstrumentationSetting instrumentationSetting, ConfigurationElement sourceElement)
		{
			InstrumentationConfigurationSection section = (InstrumentationConfigurationSection)sourceElement;
			section.EventLoggingEnabled = instrumentationSetting.EventLoggingEnabled;
			section.PerformanceCountersEnabled = instrumentationSetting.PerformanceCountersEnabled;
			section.WmiEnabled = instrumentationSetting.WmiEnabled;
			return true;
		}
		internal static void RegisterWmiTypes()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(InstrumentationSetting));
		}
	}
}
