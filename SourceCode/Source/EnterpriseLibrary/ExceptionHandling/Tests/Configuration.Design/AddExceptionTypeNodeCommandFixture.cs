/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design.Tests
{
    [TestClass]
    public class AddExceptionTypeNodeCommandFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void AddExceptionTypeNodeChangesNodeName()
        {
            AddExceptionTypeNodeCommandTest addExceptionTypeNodeCommand = new AddExceptionTypeNodeCommandTest(ServiceProvider);
            addExceptionTypeNodeCommand.Execute(ApplicationNode);
            ExceptionTypeNode exceptionTypeNode = (ExceptionTypeNode)Hierarchy.FindNodeByType(ApplicationNode, typeof(ExceptionTypeNode));
            Assert.IsNotNull(exceptionTypeNode);
            Assert.AreEqual("Exception", exceptionTypeNode.Name);
        }
        class AddExceptionTypeNodeCommandTest : AddExceptionTypeNodeCommand
        {
            public AddExceptionTypeNodeCommandTest(IServiceProvider serviceProvider)
                : base(serviceProvider, typeof(ExceptionTypeNode)) {}
            protected override Type SelectedType
            {
                get { return typeof(Exception); }
            }
        }
    }
}
