/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.Design.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.Design
{
    sealed class ExceptionHandlingLoggingNodeMapRegistrar : NodeMapRegistrar
    {
        public ExceptionHandlingLoggingNodeMapRegistrar(IServiceProvider serviceProvider)
            : base(serviceProvider) {}
        public override void Register()
        {
            AddMultipleNodeMap(Resources.LoggingHandlerName,
                               typeof(LoggingExceptionHandlerNode),
                               typeof(LoggingExceptionHandlerData));
        }
    }
}
