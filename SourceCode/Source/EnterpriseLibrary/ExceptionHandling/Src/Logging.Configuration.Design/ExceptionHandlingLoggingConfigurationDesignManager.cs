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
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Sources;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.Design
{
    public sealed class ExceptionHandlingLoggingConfigurationDesignManager: ConfigurationDesignManager
    {
        public override void Register(IServiceProvider serviceProvider)
        {
            CommandRegistrar commandRegistrar = new ExceptionHandlingLoggingCommandRegistrar(serviceProvider);
            commandRegistrar.Register();
            NodeMapRegistrar nodeMapRegistrar = new ExceptionHandlingLoggingNodeMapRegistrar(serviceProvider);
            nodeMapRegistrar.Register();
        }
		protected override void OpenCore(IServiceProvider serviceProvider, ConfigurationApplicationNode rootNode, ConfigurationSection section)
		{			
			IConfigurationUIHierarchy hierarchy = ServiceHelper.GetCurrentHierarchy(serviceProvider);
			foreach (LoggingExceptionHandlerNode handlerNode in hierarchy.FindNodesByType(typeof(LoggingExceptionHandlerNode)))
			{
				foreach (CategoryTraceSourceNode categoryNode in hierarchy.FindNodesByType(typeof(CategoryTraceSourceNode)))
				{
					if (categoryNode.Name == ((LoggingExceptionHandlerData)handlerNode.ExceptionHandlerData).LogCategory)
					{
						handlerNode.LogCategory = categoryNode;						
						break;
					}
				}				
			}			
		}
    }
}
