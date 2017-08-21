/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration.Design.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration.Design
{
    sealed class NodeMapRegistrar : EnterpriseLibrary.Configuration.Design.NodeMapRegistrar
    {
        public NodeMapRegistrar(IServiceProvider serviceProvider)
            : base(serviceProvider) {}
        public override void Register()
        {
            AddMultipleNodeMap(Resources.FaultContractExceptionHandlerNodeUICommandText,
                               typeof(FaultContractExceptionHandlerNode),
                               typeof(FaultContractExceptionHandlerData));
        }
    }
}
