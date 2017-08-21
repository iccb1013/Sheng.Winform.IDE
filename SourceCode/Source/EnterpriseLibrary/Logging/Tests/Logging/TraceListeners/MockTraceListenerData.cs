/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners.Tests;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners
{
	[Assembler(typeof(MockTraceListenerAssembler))]
	public class MockTraceListenerData : TraceListenerData
	{
		public MockTraceListenerData()
		{
		}
		public MockTraceListenerData(string name)
			: base(name, typeof(MockTraceListener), TraceOptions.None, SourceLevels.All)
		{
		}
	}
	public class MockTraceListenerAssembler : IAssembler<TraceListener, TraceListenerData>
	{
		public TraceListener Assemble(IBuilderContext context,
									  TraceListenerData objectConfiguration,
									  IConfigurationSource configurationSource,
									  ConfigurationReflectionCache reflectionCache)
		{
			return new MockTraceListener();
		}
	}
}
