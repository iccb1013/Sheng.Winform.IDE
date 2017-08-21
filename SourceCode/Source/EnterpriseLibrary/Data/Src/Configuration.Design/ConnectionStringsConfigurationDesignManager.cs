/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Configuration;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design.Properties;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{
    public sealed class ConnectionStringsConfigurationDesignManager : ConfigurationDesignManager
    {
		public ConnectionStringsConfigurationDesignManager()
        {
        }
        public override void Register(IServiceProvider serviceProvider)
        {
			ConnectionStringsCommandRegistrar registrar = new ConnectionStringsCommandRegistrar(serviceProvider);
			registrar.Register();
        }
		protected override void OpenCore(IServiceProvider serviceProvider, ConfigurationApplicationNode rootNode, ConfigurationSection section)
		{			
			if (null != section)				
			{
				string defaultDatabase = string.Empty;
				DatabaseSettings databaseSection = DatabaseSettings.GetDatabaseSettings(GetConfigurationSource(serviceProvider));
				if (null != databaseSection) defaultDatabase = databaseSection.DefaultDatabase;				
				DatabaseSectionNode node = rootNode.Hierarchy.FindNodeByType(typeof(DatabaseSectionNode)) as DatabaseSectionNode;
				if (null == node) 
				{
					AddDatabaseSectionNodeCommand dbCmd = new AddDatabaseSectionNodeCommand(serviceProvider, false);
					dbCmd.Execute(rootNode);
					node = dbCmd.ChildNode as DatabaseSectionNode;
					Debug.Assert(node != null);
				}
				ConnectionStringsSectionNodeBuilder builder = new ConnectionStringsSectionNodeBuilder(serviceProvider, (ConnectionStringsSection)section, defaultDatabase, node);
				node.AddNode(builder.Build());
			}
		}
		protected override ConfigurationSectionInfo GetConfigurationSectionInfo(IServiceProvider serviceProvider)
		{
			ConfigurationNode rootNode = ServiceHelper.GetCurrentRootNode(serviceProvider);
			ConnectionStringsSectionNode node = null;
            DatabaseSectionNode databaseSectionNode = null;
            if (null != rootNode)
            {
                node = rootNode.Hierarchy.FindNodeByType(rootNode, typeof(ConnectionStringsSectionNode)) as ConnectionStringsSectionNode;
                databaseSectionNode = rootNode.Hierarchy.FindNodeByType(rootNode, typeof(DatabaseSectionNode)) as DatabaseSectionNode;
            }
			ConnectionStringsSection connectionStrings = null;
			if (node == null)
			{
				connectionStrings = null;
			}
			else
			{
				ConnectionStringsSectionBuilder builder = new ConnectionStringsSectionBuilder(serviceProvider, node);
				connectionStrings = builder.Build();
			}
            string protectionProviderName = GetProtectionProviderName(databaseSectionNode);
            return new ConfigurationSectionInfo(node, connectionStrings, "connectionStrings", protectionProviderName);
		}
	}	
}
