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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{
    public sealed class DataConfigurationDesignManager : ConfigurationDesignManager
    {
        public DataConfigurationDesignManager()
        {
        }
        public override void Register(IServiceProvider serviceProvider)
        {
			DataCommandRegistrar registrar = new DataCommandRegistrar(serviceProvider);
			registrar.Register();
        }
		protected override void OpenCore(IServiceProvider serviceProvider, ConfigurationApplicationNode rootNode, ConfigurationSection section)
		{	
			if (null != section)
			{
                DatabaseSectionNodeBuilder builder = new DatabaseSectionNodeBuilder(serviceProvider, (DatabaseSettings)section);
                DatabaseSectionNode node = builder.Build();
                SetProtectionProvider(section, node);
                rootNode.AddNode(node);
			}
		}
		protected override ConfigurationSectionInfo GetConfigurationSectionInfo(IServiceProvider serviceProvider)
		{
			ConfigurationNode rootNode = ServiceHelper.GetCurrentRootNode(serviceProvider);
			DatabaseSectionNode node = null; 
			if (null != rootNode) node = rootNode.Hierarchy.FindNodeByType(rootNode, typeof(DatabaseSectionNode)) as DatabaseSectionNode;
			DatabaseSettings databaseSection = null;
			if (node == null)
			{
				databaseSection = null;
			}
			else
			{
				DatabaseSectionBuilder builder = new DatabaseSectionBuilder(serviceProvider, node);
				databaseSection = builder.Build();
			}
            string protectionProviderName = GetProtectionProviderName(node);
            return new ConfigurationSectionInfo(node, databaseSection, DatabaseSettings.SectionName, protectionProviderName);
		}
	}
}
