/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Configuration;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
	[Assembler(typeof(FormattedEventLogTraceListenerAssembler))]
	public class FormattedEventLogTraceListenerData : TraceListenerData
	{
		private const string sourceProperty = "source";
		private const string formatterNameProperty = "formatter";
		private const string logNameProperty = "log";
		private const string machineNameProperty = "machineName";
		public FormattedEventLogTraceListenerData()
		{
		}
		public FormattedEventLogTraceListenerData(string source, string formatterName)
			: this("unnamed", source, formatterName)
		{
		}
		public FormattedEventLogTraceListenerData(string name, string source, string formatterName)
			: this(name, source, FormattedEventLogTraceListener.DefaultLogName, FormattedEventLogTraceListener.DefaultMachineName, formatterName)
		{
		}
		public FormattedEventLogTraceListenerData(string name, string source, string logName, string machineName, string formatterName)
			: this(name, source, logName, machineName, formatterName, TraceOptions.None)
		{
		}
		public FormattedEventLogTraceListenerData(string name, string source, string logName, 
					string machineName, string formatterName, TraceOptions traceOutputOptions)
			: base(name, typeof(FormattedEventLogTraceListener), traceOutputOptions)
		{
			this.Source = source;
			this.Log = logName;
			this.MachineName = machineName;
			this.Formatter = formatterName;
		}
		[ConfigurationProperty(sourceProperty, IsRequired = true)]
		public string Source
		{
			get { return (string)base[sourceProperty]; }
			set { base[sourceProperty] = value; }
		}
		[ConfigurationProperty(formatterNameProperty, IsRequired = false)]
		public string Formatter
		{
			get { return (string)base[formatterNameProperty]; }
			set { base[formatterNameProperty] = value; }
		}
		[ConfigurationProperty(logNameProperty, IsRequired = false, DefaultValue = FormattedEventLogTraceListener.DefaultLogName)]
		public string Log
		{
			get { return (string)base[logNameProperty]; }
			set { base[logNameProperty] = value; }
		}
		[ConfigurationProperty(machineNameProperty, IsRequired = false, DefaultValue = FormattedEventLogTraceListener.DefaultMachineName)]
		public string MachineName
		{
			get { return (string)base[machineNameProperty]; }
			set { base[machineNameProperty] = value; }
		}
	}
	public class FormattedEventLogTraceListenerAssembler : TraceListenerAsssembler
	{
		public override TraceListener Assemble(IBuilderContext context, TraceListenerData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			FormattedEventLogTraceListenerData castedObjectConfiguration
				= (FormattedEventLogTraceListenerData)objectConfiguration;
			ILogFormatter formatter 
				= GetFormatter(context, castedObjectConfiguration.Formatter, configurationSource, reflectionCache);
			TraceListener createdObject
				= new FormattedEventLogTraceListener(
					castedObjectConfiguration.Source,
					castedObjectConfiguration.Log,
					castedObjectConfiguration.MachineName,
					formatter);
			return createdObject;
		}
	}
}
