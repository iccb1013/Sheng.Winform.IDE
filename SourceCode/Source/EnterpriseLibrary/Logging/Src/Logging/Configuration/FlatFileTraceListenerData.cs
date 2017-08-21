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
	[Assembler(typeof(FlatFileTraceListenerAssembler))]
	public class FlatFileTraceListenerData : TraceListenerData
	{
		private const string fileNameProperty = "fileName";
		private const string headerProperty = "header";
		private const string footerProperty = "footer";
		private const string formatterNameProperty = "formatter";
		public FlatFileTraceListenerData()
		{
		}
		public FlatFileTraceListenerData(string fileName, string formatterName)
			: this("unnamed", fileName, formatterName)
		{
		}
		public FlatFileTraceListenerData(string name, string fileName, string formatterName)
			: this(name, typeof(FlatFileTraceListener), fileName, formatterName)
		{
		}
		public FlatFileTraceListenerData(string name, string fileName, string header, string footer, string formatterName)
			: this(name, fileName, header, footer, formatterName, TraceOptions.None)
		{
		}
		public FlatFileTraceListenerData(string name, string fileName, string header, string footer, string formatterName, 
						TraceOptions traceOutputOptions)
			: this(name, typeof(FlatFileTraceListener), fileName, formatterName, traceOutputOptions)
		{
			this.Header = header;
			this.Footer = footer;
		}
		public FlatFileTraceListenerData(string name, Type listenerType, string fileName, string formatterName)
			: this(name, listenerType, fileName, formatterName, TraceOptions.None)
		{
		}
		public FlatFileTraceListenerData(string name, Type listenerType, string fileName, string formatterName, TraceOptions traceOutputOptions)
			: base(name, listenerType, traceOutputOptions)
		{
			this.FileName = fileName;
			this.Formatter = formatterName;
		}
		[ConfigurationProperty(fileNameProperty, IsRequired = true)]
		public string FileName
		{
			get { return (string)base[fileNameProperty]; }
			set { base[fileNameProperty] = value; }
		}
		[ConfigurationProperty(headerProperty, IsRequired = false)]
		public string Header
		{
			get { return (string)base[headerProperty]; }
			set { base[headerProperty] = value; }
		}
		[ConfigurationProperty(footerProperty, IsRequired = false)]
		public string Footer
		{
			get { return (string)base[footerProperty]; }
			set { base[footerProperty] = value; }
		}
		[ConfigurationProperty(formatterNameProperty, IsRequired = false)]
		public string Formatter
		{
			get { return (string)base[formatterNameProperty]; }
			set { base[formatterNameProperty] = value; }
		}
	}
	public class FlatFileTraceListenerAssembler : TraceListenerAsssembler
	{
		public override TraceListener Assemble(IBuilderContext context, TraceListenerData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			FlatFileTraceListenerData castedObjectConfiguration
				= (FlatFileTraceListenerData)objectConfiguration;
			ILogFormatter formatter = GetFormatter(context, castedObjectConfiguration.Formatter, configurationSource, reflectionCache);
			TraceListener createdObject
				= new FlatFileTraceListener(
					castedObjectConfiguration.FileName,
					castedObjectConfiguration.Header,
					castedObjectConfiguration.Footer,
					formatter);
			return createdObject;
		}
	}
}
