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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using System.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{
	sealed class ConnectionStringsSectionBuilder 
	{
		private ConnectionStringsSectionNode connectionStringsSectionNode;		
		private ConnectionStringsSection connectionStringsSection;
		IConfigurationUIHierarchy hierarchy;
		public ConnectionStringsSectionBuilder(IServiceProvider serviceProvider, ConnectionStringsSectionNode connectionStringsSectionNode) 
		{
			this.connectionStringsSectionNode = connectionStringsSectionNode;
			hierarchy = ServiceHelper.GetCurrentHierarchy(serviceProvider);
		}
		public ConnectionStringsSection Build()
		{
			connectionStringsSection = new ConnectionStringsSection();
			foreach (ConnectionStringSettingsNode node in connectionStringsSectionNode.Nodes)
			{
				BuildConnectionString(node);
			}
			return connectionStringsSection;
		}
		private void BuildConnectionString(ConnectionStringSettingsNode connectionStringNode)
		{
			connectionStringsSection.ConnectionStrings.Add(new ConnectionStringSettings(connectionStringNode.Name,
				connectionStringNode.ConnectionString, connectionStringNode.ProviderName));
		}
	}
}
