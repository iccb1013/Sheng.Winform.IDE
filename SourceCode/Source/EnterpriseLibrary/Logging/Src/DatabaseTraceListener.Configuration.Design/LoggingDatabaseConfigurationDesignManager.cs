/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration.Design
{
    public sealed class LoggingDatabaseConfigurationDesignManager : ConfigurationDesignManager
    {
		public LoggingDatabaseConfigurationDesignManager()
        {
        }
		public override void Register(IServiceProvider serviceProvider)
        {
			LoggingDatabaseCommandRegistrar cmdRegistrar = new LoggingDatabaseCommandRegistrar(serviceProvider);
			cmdRegistrar.Register();
			LoggingDatabaseNodeMapRegistrar nodeRegistrar = new LoggingDatabaseNodeMapRegistrar(serviceProvider);
			nodeRegistrar.Register();
        }
		protected override void OpenCore(IServiceProvider serviceProvider, ConfigurationApplicationNode rootNode, ConfigurationSection section)
        {
			IConfigurationUIHierarchy hierarchy = ServiceHelper.GetCurrentHierarchy(serviceProvider);
			foreach (LoggingDatabaseNode loggingDatabaseNode in hierarchy.FindNodesByType(typeof(LoggingDatabaseNode)))
			{
				foreach (ConnectionStringSettingsNode connectionStringNode in hierarchy.FindNodesByType(typeof(ConnectionStringSettingsNode)))
				{
					if (connectionStringNode.Name == ((FormattedDatabaseTraceListenerData)loggingDatabaseNode.TraceListenerData).DatabaseInstanceName)
					{
						loggingDatabaseNode.DatabaseInstance = connectionStringNode;
						break;
					}
				}
			}			
        }		
    }
}
