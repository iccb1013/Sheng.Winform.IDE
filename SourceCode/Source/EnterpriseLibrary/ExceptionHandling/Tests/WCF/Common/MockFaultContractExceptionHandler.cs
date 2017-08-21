/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Tests
{
    [ConfigurationElementType(typeof(MockFaultContractExceptionHandlerData))]
    public class MockFaultContractExceptionHandler : IExceptionHandler
    {
        public Exception HandledException;
        public Exception HandleException(Exception exception, Guid handlingInstanceId)
        {
            this.HandledException = exception;
            return new FaultContractWrapperException(new MockFaultContract(exception.Message), handlingInstanceId);
        }
    }
    [Assembler(typeof(MockFaultContractHandlerAssembler))]
    public class MockFaultContractExceptionHandlerData : ExceptionHandlerData
    {
        public MockFaultContractExceptionHandlerData()
        {
        }
        public MockFaultContractExceptionHandlerData(string name)
            : base(name, typeof(FaultContractExceptionHandler))
        {
        }
    }
    public class MockFaultContractHandlerAssembler : IAssembler<IExceptionHandler, ExceptionHandlerData>
    {
        public IExceptionHandler Assemble(IBuilderContext context,
            ExceptionHandlerData objectConfiguration,
            IConfigurationSource configurationSource,
            ConfigurationReflectionCache reflectionCache)
        {
            MockFaultContractExceptionHandler createdObject
                = new MockFaultContractExceptionHandler();
            return createdObject;
        }
    }
}
