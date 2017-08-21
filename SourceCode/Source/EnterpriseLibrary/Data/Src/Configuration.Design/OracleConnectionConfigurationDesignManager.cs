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
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Configuration;
using System.Runtime.InteropServices;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{
    public sealed class OracleConnectionConfigurationDesignManager : ConfigurationDesignManager
    {
		public OracleConnectionConfigurationDesignManager()
        {
        }
        public override void Register(IServiceProvider serviceProvider)
        {
			OracleConnectionCommandRegistrar registrar = new OracleConnectionCommandRegistrar(serviceProvider);
			registrar.Register();
        }
		protected override void OpenCore(IServiceProvider serviceProvider, ConfigurationApplicationNode rootNode, ConfigurationSection section)
		{
			if (null != section)
			{
				OracleConnectionNodeBuilder builder = new OracleConnectionNodeBuilder(serviceProvider, (OracleConnectionSettings)section);
				builder.Build();
			}
		}
		protected override ConfigurationSectionInfo GetConfigurationSectionInfo(IServiceProvider serviceProvider)
        {
            ConfigurationNode rootNode = ServiceHelper.GetCurrentRootNode(serviceProvider);
            DatabaseSectionNode databaseSectionNode = null;
            if (rootNode != null)
            {
                databaseSectionNode = rootNode.Hierarchy.FindNodeByType(rootNode, typeof(DatabaseSectionNode)) as DatabaseSectionNode;
            }
            OracleConnectionSettings oracleConnectionSection = null;
            IList<ConfigurationNode> connections = rootNode.Hierarchy.FindNodesByType(typeof(OracleConnectionElementNode));
            if (connections.Count == 0)
            {
                oracleConnectionSection = null;
            }
            else
            {
                OracleConnectionSettingsBuilder builder = new OracleConnectionSettingsBuilder(serviceProvider);
                oracleConnectionSection = builder.Build();
            }
            string protectionProviderName = GetProtectionProviderName(databaseSectionNode);
            return new ConfigurationSectionInfo(rootNode, oracleConnectionSection, OracleConnectionSettings.SectionName, protectionProviderName);
        }		
	}
}
