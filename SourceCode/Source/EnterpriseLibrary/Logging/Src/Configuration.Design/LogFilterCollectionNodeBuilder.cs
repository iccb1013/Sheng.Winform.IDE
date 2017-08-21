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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Filters;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design
{
	sealed class LogFilterCollectionNodeBuilder : NodeBuilder
	{
		private NameTypeConfigurationElementCollection<LogFilterData, CustomLogFilterData> logFilters;
		private LogFilterCollectionNode node;
		public LogFilterCollectionNodeBuilder(IServiceProvider serviceProvider, NameTypeConfigurationElementCollection<LogFilterData, CustomLogFilterData> logFilters)
			: base(serviceProvider)
		{
			this.logFilters = logFilters;
		}
		public LogFilterCollectionNode Build()
		{
			node = new LogFilterCollectionNode();
			logFilters.ForEach(new Action<LogFilterData>(CreateLogFilterNode));
			return node;
		}
		private void CreateLogFilterNode(LogFilterData logFilterData)
		{
			LogFilterNode logFilterNode = NodeCreationService.CreateNodeByDataType(logFilterData.GetType(), new object[] { logFilterData }) as LogFilterNode;
			if (null == logFilterNode)
			{
				LogNodeMapError(node, logFilterData.GetType());
				return;
			}
			node.AddNode(logFilterNode);
		}
	}
}
