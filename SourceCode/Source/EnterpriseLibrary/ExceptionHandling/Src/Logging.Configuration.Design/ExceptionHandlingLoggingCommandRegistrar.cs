/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.Design.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.Design
{
    sealed class ExceptionHandlingLoggingCommandRegistrar : CommandRegistrar
    {
        public ExceptionHandlingLoggingCommandRegistrar(IServiceProvider serviceProvider)
            : base(serviceProvider) {}
        void AddLoggingExceptionHandlerCommand()
        {
            ConfigurationUICommand cmd = ConfigurationUICommand.CreateMultipleUICommand(ServiceProvider,
                                                                                        Resources.LoggingHandlerName,
                                                                                        string.Format(Resources.Culture, Resources.GenericCreateStatusText, Resources.LoggingHandlerName),
                                                                                        new AddLoggingExceptionHandlerCommand(ServiceProvider),
                                                                                        typeof(ExceptionHandlerNode));
            AddUICommand(cmd, typeof(ExceptionTypeNode));
        }
        public override void Register()
        {
            AddLoggingExceptionHandlerCommand();
            AddDefaultCommands(typeof(LoggingExceptionHandlerNode));
            AddMoveUpDownCommands(typeof(LoggingExceptionHandlerNode));
        }
    }
}
