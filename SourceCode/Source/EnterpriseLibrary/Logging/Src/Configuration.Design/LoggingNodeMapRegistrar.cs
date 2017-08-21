/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Sources;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Filters;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design
{
	sealed class LoggingNodeMapRegistrar : NodeMapRegistrar
	{	
		public LoggingNodeMapRegistrar(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
		public override void Register()
		{
			AddMultipleNodeMap(Resources.WmiTraceListenerNode, 
				typeof(WmiTraceListenerNode),
				typeof(WmiTraceListenerData));
			AddMultipleNodeMap(Resources.SystemDiagnosticsTraceListenerNode, 
				typeof(SystemDiagnosticsTraceListenerNode),
				typeof(SystemDiagnosticsTraceListenerData));
			AddMultipleNodeMap(Resources.MsmqTraceListenerNode, 
				typeof(MsmqTraceListenerNode),
				typeof(MsmqTraceListenerData));
			AddMultipleNodeMap(Resources.FlatFileTraceListenerNode, 
				typeof(FlatFileTraceListenerNode),
				typeof(FlatFileTraceListenerData));
			AddMultipleNodeMap(Resources.EmailTraceListenerNode, 
				typeof(EmailTraceListenerNode),
				typeof(EmailTraceListenerData));
			AddMultipleNodeMap(Resources.CustomTraceListenerNode, 
				typeof(CustomTraceListenerNode),
				typeof(BasicCustomTraceListenerData));
			AddMultipleNodeMap(Resources.CategoryFilterNode, 
				typeof(CategoryFilterNode),
				typeof(CategoryFilterData));
			AddMultipleNodeMap(Resources.LogEnabledFilterNode, 
				typeof(LogEnabledFilterNode),
				typeof(LogEnabledFilterData));
			AddMultipleNodeMap(Resources.PriorityFilterNode, 
				typeof(PriorityFilterNode),
				typeof(PriorityFilterData));
			AddMultipleNodeMap(Resources.CustomFilterNode, 
				typeof(CustomLogFilterNode),
				typeof(CustomLogFilterData));
			AddMultipleNodeMap(Resources.TextFormatterNode, 
				typeof(TextFormatterNode),
				typeof(TextFormatterData));
			AddMultipleNodeMap(Resources.BinaryFormatterNode, 
				typeof(BinaryFormatterNode),
				typeof(BinaryLogFormatterData));			
			AddMultipleNodeMap(Resources.CustomFormatter, 
				typeof(CustomFormatterNode),
				typeof(CustomFormatterData));			
			AddMultipleNodeMap(Resources.FormattedEventLogTraceListenerNode,
				typeof(FormattedEventLogTraceListenerNode),
				typeof(FormattedEventLogTraceListenerData));
			AddMultipleNodeMap(Resources.TraceListenerReferenceNode,
				typeof(TraceListenerReferenceNode),
				typeof(TraceListenerReferenceData));
			AddMultipleNodeMap(Resources.CustomTraceListenerNode,
				typeof(CustomTraceListenerNode),
				typeof(CustomTraceListenerData));
			AddMultipleNodeMap(Resources.XmlTraceListenerNodeUICommandText,
			   typeof(XmlTraceListenerNode),
			   typeof(XmlTraceListenerData));
			AddMultipleNodeMap(Resources.RollingTraceListenerNodeUICommandText,
			   typeof(RollingTraceListenerNode),
			   typeof(Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.RollingFlatFileTraceListenerData));             
		}        
	}
}
