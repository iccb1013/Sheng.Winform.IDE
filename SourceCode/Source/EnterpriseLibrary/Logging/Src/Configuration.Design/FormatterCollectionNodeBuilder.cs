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
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design
{
	sealed class FormatterCollectionNodeBuilder : NodeBuilder
	{
		private NameTypeConfigurationElementCollection<FormatterData, CustomFormatterData> formatters;
		private FormatterCollectionNode node;
        public FormatterCollectionNodeBuilder(IServiceProvider serviceProvider, NameTypeConfigurationElementCollection<FormatterData, CustomFormatterData> formatters)
			: base(serviceProvider)
		{
			this.formatters = formatters;
		}
		public FormatterCollectionNode Build()
		{
			node = new FormatterCollectionNode();
			formatters.ForEach(new Action<FormatterData>(CreateFormatterNode));
			return node;
		}
		private void CreateFormatterNode(FormatterData formatterData)
		{
			FormatterNode formatterNode = NodeCreationService.CreateNodeByDataType(formatterData.GetType(), new object[] { formatterData }) as FormatterNode;
			if (null == formatterNode)
			{
				LogNodeMapError(node, formatterData.GetType());
				return;
			}
			node.AddNode(formatterNode);
		}
	}
}
