/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration.Design.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration.Design
{
    sealed class LoggingDatabaseCommandRegistrar : CommandRegistrar
    {
        public LoggingDatabaseCommandRegistrar(IServiceProvider serviceProvider)
            : base(serviceProvider) {}
        void AddLoggingDatabaseCommand()
        {
            ConfigurationUICommand cmd = ConfigurationUICommand.CreateMultipleUICommand(ServiceProvider,
                                                                                        Resources.DatabaseTraceListenerUICommandText,
                                                                                        Resources.DatabaseTraceListenerUICommandLongText,
                                                                                        new AddLoggingDatabaseCommand(ServiceProvider),
                                                                                        typeof(LoggingDatabaseNode));
            AddUICommand(cmd, typeof(TraceListenerCollectionNode));
        }
        public override void Register()
        {
            AddLoggingDatabaseCommand();
            AddDefaultCommands(typeof(LoggingDatabaseNode));
        }
    }
}
