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
	internal static class SystemDiagnosticsTraceListenerDataWmiMapper
	{
		public static void GenerateWmiObjects(SystemDiagnosticsTraceListenerData data,
			ICollection<ConfigurationSetting> wmiSettings)
		{
			wmiSettings.Add(
				new CustomTraceListenerSetting(data,
					data.Name,
					data.Type.AssemblyQualifiedName,
					data.InitData,
					CustomDataWmiMapperHelper.GenerateAttributesArray(data.Attributes),
					data.TraceOutputOptions.ToString(),
					data.Filter.ToString(),
					null));
		}
		internal static void RegisterWmiTypes()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(CustomTraceListenerSetting));
		}
	}
}
