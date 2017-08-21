/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.TraceListeners
{
	internal static class WmiTraceListenerDataWmiMapper 
	{
		public static void GenerateWmiObjects(WmiTraceListenerData configurationObject,
			ICollection<ConfigurationSetting> wmiSettings)
		{
			wmiSettings.Add(
				new WmiTraceListenerSetting(configurationObject,
					configurationObject.Name,
					configurationObject.TraceOutputOptions.ToString(),
					configurationObject.Filter.ToString()));
		}
		internal static void RegisterWmiTypes()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(WmiTraceListenerSetting));
		}
	}
}
