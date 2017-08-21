/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration.Design.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration.Design
{
    sealed class CommandRegistrar : EnterpriseLibrary.Configuration.Design.CommandRegistrar
    {
        public CommandRegistrar(IServiceProvider serviceProvider)
            : base(serviceProvider) {}
        public override void Register()
        {
            AddMultipleChildNodeCommand(
                Resources.FaultContractExceptionHandlerNodeUICommandText,
                Resources.FaultContractExceptionHandlerNodeUICommandLongText,
                typeof(FaultContractExceptionHandlerNode),
                typeof(ExceptionTypeNode));
            AddDefaultCommands(typeof(FaultContractExceptionHandlerNode));
            AddMoveUpDownCommands(typeof(FaultContractExceptionHandlerNode));
        }
    }
}
