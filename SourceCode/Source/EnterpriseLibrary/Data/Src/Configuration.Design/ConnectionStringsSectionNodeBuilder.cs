/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using System.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{	
	sealed class ConnectionStringsSectionNodeBuilder : NodeBuilder
	{
		private ConnectionStringsSection connectionStringSections;
		private ConnectionStringsSectionNode node;		
		private DatabaseSectionNode databaseSectionNode;
		private string defaultDatabaseName;
		public ConnectionStringsSectionNodeBuilder(IServiceProvider serviceProvider, ConnectionStringsSection conectionStringsSection, string defaultDatabaseName, DatabaseSectionNode databaseSectionNode)
			: base(serviceProvider)
		{
			this.connectionStringSections = conectionStringsSection;
			this.databaseSectionNode = databaseSectionNode;
			this.defaultDatabaseName = defaultDatabaseName;
		}
		public ConnectionStringsSectionNode Build()
		{
			node = new ConnectionStringsSectionNode();			
			foreach (ConnectionStringSettings connectionString in connectionStringSections.ConnectionStrings)
			{
				BuildConnectionStringNode(connectionString);
			}						
			return node;
		}		
		private void BuildConnectionStringNode(ConnectionStringSettings connectionString)
		{
			ConnectionStringSettingsNode connectionStringNode = new ConnectionStringSettingsNode(connectionString);
			if (connectionStringNode.Name == defaultDatabaseName) databaseSectionNode.DefaultDatabase = connectionStringNode;
			node.AddNode(connectionStringNode);
		}		
	}
}
