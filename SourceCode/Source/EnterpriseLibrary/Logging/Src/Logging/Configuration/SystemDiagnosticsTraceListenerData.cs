/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Specialized;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
	[Assembler(typeof(SystemDiagnosticsTraceListenerAssembler))]
	[ContainerPolicyCreator(typeof(BaseCustomTraceListenerPolicyCreator))]
	public class SystemDiagnosticsTraceListenerData
		: BasicCustomTraceListenerData
	{
		public SystemDiagnosticsTraceListenerData()
			: base()
		{
		}
		public SystemDiagnosticsTraceListenerData(string name, Type type, string initData)
			: base(name, type, initData)
		{
		}
		public SystemDiagnosticsTraceListenerData(string name, string typeName, string initData)
			: base(name, typeName, initData)
		{
		}
		public SystemDiagnosticsTraceListenerData(string name, Type type, string initData, TraceOptions traceOutputOptions)
			: base(name, type, initData, traceOutputOptions)
		{
		}
	}
	public class SystemDiagnosticsTraceListenerAssembler : TraceListenerAsssembler
	{
		public override TraceListener Assemble(IBuilderContext context, TraceListenerData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			BasicCustomTraceListenerData castedObjectConfiguration
				= (BasicCustomTraceListenerData)objectConfiguration;
			Type type = castedObjectConfiguration.Type;
			string name = castedObjectConfiguration.Name;
			TraceOptions traceOutputOptions = castedObjectConfiguration.TraceOutputOptions;
			string initData = castedObjectConfiguration.InitData;
			NameValueCollection attributes = castedObjectConfiguration.Attributes;
			return SystemDiagnosticsTraceListenerCreationHelper.CreateSystemDiagnosticsTraceListener(name, type, initData, attributes);
		}
	}
}
