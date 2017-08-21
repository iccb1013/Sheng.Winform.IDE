/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
	[Assembler(typeof(CustomTraceListenerAssembler))]
	[ContainerPolicyCreator(typeof(BaseCustomTraceListenerPolicyCreator))]
	public class CustomTraceListenerData
		: BasicCustomTraceListenerData
	{
		internal const string formatterNameProperty = "formatter";
		public CustomTraceListenerData()
			: base()
		{
		}
		public CustomTraceListenerData(string name, Type type, string initData)
			: base(name, type, initData)
		{
		}
		public CustomTraceListenerData(string name, Type type, string initData, TraceOptions traceOutputOptions)
			: base(name, type, initData, traceOutputOptions)
		{
		}
        public CustomTraceListenerData(string name, string typeName, string initData, TraceOptions traceOutputOptions)
            : base(name, typeName, initData, traceOutputOptions)
        {
        }
		public string Formatter
		{
			get { return (string)base[formatterNameProperty]; }
			set { base[formatterNameProperty] = value; }
		}
		protected override CustomProviderDataHelper<BasicCustomTraceListenerData> CreateHelper()
		{
			return new CustomTraceListenerDataHelper(this);
		}
	}
	public class CustomTraceListenerAssembler : SystemDiagnosticsTraceListenerAssembler
	{
		public override TraceListener Assemble(IBuilderContext context, TraceListenerData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			TraceListener createdObject = base.Assemble(context, objectConfiguration, configurationSource, reflectionCache);
			if (createdObject is CustomTraceListener)
			{
				CustomTraceListenerData castedObjectConfiguration
					= (CustomTraceListenerData)objectConfiguration;
				ILogFormatter formatter = GetFormatter(context, castedObjectConfiguration.Formatter, configurationSource, reflectionCache);
				((CustomTraceListener)createdObject).Formatter = formatter;
			}
			return createdObject;
		}
	}
}
