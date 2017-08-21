/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design
{
	sealed class TraceListenerCollectionNodeBuilder : NodeBuilder
	{
		private TraceListenerDataCollection listeners;
		private TraceListenerCollectionNode node;
		private FormatterCollectionNode formatters;
		public TraceListenerCollectionNodeBuilder(IServiceProvider serviceProvider, TraceListenerDataCollection listeners, FormatterCollectionNode formatters)
			: base(serviceProvider)
		{
			this.listeners = listeners;
			this.formatters = formatters;
		}
		public TraceListenerCollectionNode Build()
		{
			node = new TraceListenerCollectionNode();
			listeners.ForEach(new Action<TraceListenerData>(CreateTraceListenerNode));
			return node;
		}
		private void CreateTraceListenerNode(TraceListenerData traceListenerData)
		{
			TraceListenerNode traceListenerNode = NodeCreationService.CreateNodeByDataType(traceListenerData.GetType(), new object[] { traceListenerData }) as TraceListenerNode;
			traceListenerNode.Filter = traceListenerData.Filter;
			traceListenerNode.TraceOutputOptions = traceListenerData.TraceOutputOptions;
			if (null == traceListenerNode)
			{
				LogNodeMapError(node, traceListenerData.GetType());
				return;
			}
			traceListenerNode.SetFormatter(formatters);
			node.AddNode(traceListenerNode);
		}
	}
}
