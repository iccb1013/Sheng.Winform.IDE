/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Sources;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design
{
    public sealed class LoggingConfigurationDesignManager : ConfigurationDesignManager
    {
        public LoggingConfigurationDesignManager()
        {
        }
        public override void Register(IServiceProvider serviceProvider)
        {
			LoggingCommandRegistrar cmdRegistrar = new LoggingCommandRegistrar(serviceProvider);
			cmdRegistrar.Register();
			LoggingNodeMapRegistrar registrar = new LoggingNodeMapRegistrar(serviceProvider);
			registrar.Register();
        }
        protected override void OpenCore(IServiceProvider serviceProvider, ConfigurationApplicationNode rootNode, ConfigurationSection section)
        {
            if (null != section)
            {
				LoggingSettingsNodeBuilder builder = new LoggingSettingsNodeBuilder(serviceProvider, (LoggingSettings)section);
                LoggingSettingsNode node = builder.Build();
                SetProtectionProvider(section, node);
                rootNode.AddNode(node);
            }
        }
        protected override ConfigurationSectionInfo GetConfigurationSectionInfo(IServiceProvider serviceProvider)
        {
            ConfigurationNode rootNode = ServiceHelper.GetCurrentRootNode(serviceProvider);
            LoggingSettingsNode node = null;
            if (rootNode != null) node = (LoggingSettingsNode)rootNode.Hierarchy.FindNodeByType(rootNode, typeof(LoggingSettingsNode));
            LoggingSettings logggingSection = null;
            if (node != null)
            {
				LoggingSettingsBuilder builder = new LoggingSettingsBuilder(serviceProvider, node);
                logggingSection = builder.Build();
            }
            string protectionProviderName = GetProtectionProviderName(node);
            return new ConfigurationSectionInfo(node, logggingSection, LoggingSettings.SectionName, protectionProviderName);
        }
    }
}
