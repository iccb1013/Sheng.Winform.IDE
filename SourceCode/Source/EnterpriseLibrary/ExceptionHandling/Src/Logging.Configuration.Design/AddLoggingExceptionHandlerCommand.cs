/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.Design
{
    public sealed class AddLoggingExceptionHandlerCommand : AddChildNodeCommand
    {
        public AddLoggingExceptionHandlerCommand(IServiceProvider serviceProvider)
            : base(serviceProvider, typeof(LoggingExceptionHandlerNode)) {}
        protected override void OnExecuted(EventArgs e)
        {
            base.OnExecuted(e);
            LoggingExceptionHandlerNode node = ChildNode as LoggingExceptionHandlerNode;
            if (null == node) return;
            if (null == CurrentHierarchy.FindNodeByType(typeof(LoggingSettingsNode)))
            {
                ConfigurationApplicationNode applicationNode = (ConfigurationApplicationNode)CurrentHierarchy.FindNodeByType(typeof(ConfigurationApplicationNode));
                new AddLoggingSettingsNodeCommand(ServiceProvider).Execute(applicationNode);
            }
        }
    }
}
