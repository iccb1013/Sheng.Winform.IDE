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
using System.Diagnostics;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{
	public class AddDatabaseSectionNodeCommand : AddChildNodeCommand
	{
		private bool addDefaultConnectionString;
		public AddDatabaseSectionNodeCommand(IServiceProvider serviceProvider)
			: this(serviceProvider, true)
        {
        }
		public AddDatabaseSectionNodeCommand(IServiceProvider serviceProvider, bool addDefaultConnectionString)
			: base(serviceProvider, typeof(DatabaseSectionNode))
		{
			this.addDefaultConnectionString = addDefaultConnectionString;
		}
		protected override void OnExecuted(EventArgs e)
		{
			DatabaseSectionNode node = ChildNode as DatabaseSectionNode;
			Debug.Assert(null != node, "Expected DatabaseSectionNode");
			if (addDefaultConnectionString)
			{
				new AddConnectionStringsSectionNodeCommand(ServiceProvider).Execute(node);				
				ConnectionStringSettingsNode defaultDatabaseNode = (ConnectionStringSettingsNode)CurrentHierarchy.FindNodeByType(node, typeof(ConnectionStringSettingsNode));
				node.DefaultDatabase = defaultDatabaseNode;
			}
			node.AddNode(new ProviderMappingsNode());
		}		
	}
}
