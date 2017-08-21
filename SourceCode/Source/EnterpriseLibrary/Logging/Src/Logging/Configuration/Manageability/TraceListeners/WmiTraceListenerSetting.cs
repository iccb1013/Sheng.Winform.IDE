/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Management.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.TraceListeners
{
	[ManagementEntity]
	public partial class WmiTraceListenerSetting : TraceListenerSetting
	{
		public WmiTraceListenerSetting(WmiTraceListenerData sourceElement,
			string name,
			string traceOutputOptions,
			string filter)
			: base(sourceElement, name, traceOutputOptions, filter)
		{ }
		[ManagementEnumerator]
		public static IEnumerable<WmiTraceListenerSetting> GetInstances()
		{
			return NamedConfigurationSetting.GetInstances<WmiTraceListenerSetting>();
		}
		[ManagementBind]
		public static WmiTraceListenerSetting BindInstance(string ApplicationName, string SectionName, string Name)
		{
			return NamedConfigurationSetting.BindInstance<WmiTraceListenerSetting>(ApplicationName, SectionName, Name);
		}
		protected override bool SaveChanges(System.Configuration.ConfigurationElement sourceElement)
		{
			return false;	
		}
	}
}
