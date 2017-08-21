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
	public class SystemDiagnosticsTraceListenerDataManageabilityProvider
		: BasicCustomTraceListenerDataManageabilityProvider<SystemDiagnosticsTraceListenerData>
	{
		public SystemDiagnosticsTraceListenerDataManageabilityProvider()
		{
			SystemDiagnosticsTraceListenerDataWmiMapper.RegisterWmiTypes();
		}
        protected override void GenerateWmiObjects(SystemDiagnosticsTraceListenerData configurationObject, 
			ICollection<ConfigurationSetting> wmiSettings)
		{
			SystemDiagnosticsTraceListenerDataWmiMapper.GenerateWmiObjects(configurationObject, wmiSettings);
		}
	}
}
