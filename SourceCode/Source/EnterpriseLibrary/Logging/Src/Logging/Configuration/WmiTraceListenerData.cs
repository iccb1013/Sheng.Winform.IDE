/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
	[Assembler(typeof(WmiTraceListenerAssembler))]
	public class WmiTraceListenerData : TraceListenerData
	{
		public WmiTraceListenerData()
		{
		}
		public WmiTraceListenerData(string name)
			: this(name, TraceOptions.None)
		{
		}
		public WmiTraceListenerData(string name, TraceOptions traceOutputOptions)
			: base(name, typeof(WmiTraceListener), traceOutputOptions)
		{
		}
        public WmiTraceListenerData(string name, TraceOptions traceOutputOptions, SourceLevels filter)
            : base(name, typeof(WmiTraceListener), traceOutputOptions, filter)
        {
        }
	}
	public class WmiTraceListenerAssembler : TraceListenerAsssembler
	{
		public override TraceListener Assemble(IBuilderContext context, TraceListenerData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			return new WmiTraceListener();
		}
	}
}
