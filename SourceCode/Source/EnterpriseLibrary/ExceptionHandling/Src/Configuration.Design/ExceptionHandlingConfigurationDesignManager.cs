/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Configuration;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design
{
    public sealed class ExceptionHandlingConfigurationDesignManager : ConfigurationDesignManager
    {
        public ExceptionHandlingConfigurationDesignManager()
        {
        }
        public override void Register(IServiceProvider serviceProvider)
        {
			ExceptionHandlingCommandRegistrar commandRegistrar = new ExceptionHandlingCommandRegistrar(serviceProvider);
			commandRegistrar.Register();
			ExceptionHandlingNodeMapRegistrar nodeMapRegistrar = new ExceptionHandlingNodeMapRegistrar(serviceProvider);
			nodeMapRegistrar.Register();
        }
		protected override void OpenCore(IServiceProvider serviceProvider, ConfigurationApplicationNode rootNode, ConfigurationSection section)
		{
			if (null != section)
			{
				ExceptionHandlingSettingsNodeBuilder builder = new ExceptionHandlingSettingsNodeBuilder(serviceProvider, (ExceptionHandlingSettings)section);
                ExceptionHandlingSettingsNode node = builder.Build();
                SetProtectionProvider(section, node);
				rootNode.AddNode(node);
			}
		}
		protected override ConfigurationSectionInfo GetConfigurationSectionInfo(IServiceProvider serviceProvider)
		{
			ConfigurationNode rootNode = ServiceHelper.GetCurrentRootNode(serviceProvider);
			ExceptionHandlingSettingsNode node = null; 
			if (null != rootNode) node = rootNode.Hierarchy.FindNodeByType(rootNode, typeof(ExceptionHandlingSettingsNode)) as ExceptionHandlingSettingsNode;
			ExceptionHandlingSettings exceptionHandlingConfiguration = null;
			if (node == null)
			{
				exceptionHandlingConfiguration = null ;
			}
			else
			{
				ExceptionHandlingSettingsBuilder builder = new ExceptionHandlingSettingsBuilder(serviceProvider, node);
				exceptionHandlingConfiguration = builder.Build();
			}
            string protectionProviderName = GetProtectionProviderName(node);
            return new ConfigurationSectionInfo(node, exceptionHandlingConfiguration, ExceptionHandlingSettings.SectionName, protectionProviderName);
		}
    }
}
