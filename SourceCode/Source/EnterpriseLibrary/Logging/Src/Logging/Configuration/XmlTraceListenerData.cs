/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Diagnostics;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
	[Assembler(typeof(XmlTraceListenerAssembler))]
	public class XmlTraceListenerData : TraceListenerData
	{
		private const string fileNameProperty = "fileName";
		public XmlTraceListenerData()
		{
		}
		public XmlTraceListenerData(string name, string fileName)
			: base(name, typeof(XmlTraceListener), TraceOptions.None)
		{
			this.FileName = fileName;
		}
		[ConfigurationProperty(fileNameProperty, IsRequired = true)]
		public string FileName
		{
			get { return (string)base[fileNameProperty]; }
			set { base[fileNameProperty] = value; }
		}
	}
	public class XmlTraceListenerAssembler : TraceListenerAsssembler
	{
		public override TraceListener Assemble(IBuilderContext context, TraceListenerData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			XmlTraceListenerData castedObjectConfiguration
				= (XmlTraceListenerData)objectConfiguration;
			XmlTraceListener createdObject
				= new XmlTraceListener(castedObjectConfiguration.FileName);
			return createdObject;
		}
	}
}
