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
using System.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{
	sealed class DatabaseSectionNodeBuilder : NodeBuilder
	{
		private DatabaseSettings databaseSettings;
		public DatabaseSectionNodeBuilder(IServiceProvider serviceProvider, DatabaseSettings databaseSettings)
			: base(serviceProvider)
		{
			this.databaseSettings = databaseSettings;
		}
		public DatabaseSectionNode Build()
		{
			DatabaseSectionNode node = new DatabaseSectionNode();
			ProviderMappingsNode mappingsNode = new ProviderMappingsNode();
			foreach (DbProviderMapping mapping in databaseSettings.ProviderMappings)
			{
				mappingsNode.AddNode(new ProviderMappingNode(mapping));
			}
			node.AddNode(mappingsNode);
			node.RequirePermission = databaseSettings.SectionInformation.RequirePermission;
			return node;
		}
	}
}
