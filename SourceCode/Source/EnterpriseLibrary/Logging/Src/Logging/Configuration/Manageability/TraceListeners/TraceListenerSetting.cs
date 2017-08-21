/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Diagnostics;
using System.Management.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.TraceListeners
{
	[ManagementEntity]
	public abstract class TraceListenerSetting : NamedConfigurationSetting
	{
		private string traceOutputOptions;
		private string filter;
		protected TraceListenerSetting(TraceListenerData sourceElement, string name, string traceOutputOptions, string filter)
			: base(sourceElement, name)
		{
			this.traceOutputOptions = traceOutputOptions;
			this.filter = filter;
		}
		[ManagementConfiguration]
		public string TraceOutputOptions
		{
			get { return traceOutputOptions; }
			set { traceOutputOptions = value; }
		}
		[ManagementConfiguration]
		public string Filter
		{
			get { return filter; }
			set { filter = value; }
		}
	}
}
