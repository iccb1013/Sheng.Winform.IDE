/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Sources;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design
{
	sealed class SpecialTraceSourcesNodeBuilder : NodeBuilder
	{
		private SpecialTraceSourcesNode specialTraceSourcesNode;
		private SpecialTraceSourcesData specialTraceSourcesData;
		private TraceListenerCollectionNode listeners;
		public SpecialTraceSourcesNodeBuilder(IServiceProvider serviceProvider, SpecialTraceSourcesData data, TraceListenerCollectionNode listeners)
			: base(serviceProvider)
		{
			this.specialTraceSourcesData = data;
			this.listeners = listeners;
		}
		public SpecialTraceSourcesNode Build()
		{
			specialTraceSourcesNode = new SpecialTraceSourcesNode();
			if (specialTraceSourcesData.AllEventsTraceSource != null)
			{
				AllTraceSourceNode allNode = new AllTraceSourceNode(specialTraceSourcesData.AllEventsTraceSource);
				AddTraceListeners(allNode, specialTraceSourcesData.AllEventsTraceSource);
				specialTraceSourcesNode.AddNode(allNode);
			}
			if (specialTraceSourcesData.ErrorsTraceSource != null)
			{
				ErrorsTraceSourceNode errorsNode = new ErrorsTraceSourceNode(specialTraceSourcesData.ErrorsTraceSource);
				AddTraceListeners(errorsNode, specialTraceSourcesData.ErrorsTraceSource);
				specialTraceSourcesNode.AddNode(errorsNode);
			}
			if (specialTraceSourcesData.NotProcessedTraceSource != null)
			{
				NotProcessedTraceSourceNode notProcessedNode = new NotProcessedTraceSourceNode(specialTraceSourcesData.NotProcessedTraceSource);
				AddTraceListeners(notProcessedNode, specialTraceSourcesData.NotProcessedTraceSource);
				specialTraceSourcesNode.AddNode(notProcessedNode);
			}
			return specialTraceSourcesNode;
		}
		private void AddTraceListeners(ConfigurationNode node, TraceSourceData data)
		{
			foreach (TraceListenerReferenceData refData in data.TraceListeners)
			{
				TraceListenerReferenceNode referenceNode = new TraceListenerReferenceNode(refData);
				foreach(TraceListenerNode listenerNode in listeners.Nodes)
				{
					if (listenerNode.Name == referenceNode.Name)
					{
						referenceNode.ReferencedTraceListener = listenerNode;
					}
				}
				node.AddNode(referenceNode);
			}			
		}
	}
}
