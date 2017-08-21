/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration.Design.Tests
{
    [TestClass]
    public class ExceptionHandlingLoggingConfigurationDesignManagerFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void OpenAndSaveTest()
        {
            Hierarchy.Load();
            Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);
            Hierarchy.Open();
            Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);
            Assert.AreEqual(1, Hierarchy.FindNodesByType(ApplicationNode, typeof(ExceptionHandlingSettingsNode)).Count);
            Assert.AreEqual(1, Hierarchy.FindNodesByType(ApplicationNode, typeof(ExceptionPolicyNode)).Count);
            Assert.AreEqual(2, Hierarchy.FindNodesByType(ApplicationNode, typeof(ExceptionTypeNode)).Count);
            Assert.AreEqual(3, Hierarchy.FindNodesByType(ApplicationNode, typeof(ExceptionHandlerNode)).Count);
            Hierarchy.Load();
            Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);
            Hierarchy.Open();
            Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);
            Assert.AreEqual(1, Hierarchy.FindNodesByType(ApplicationNode, typeof(ExceptionHandlingSettingsNode)).Count);
            Assert.AreEqual(1, Hierarchy.FindNodesByType(ApplicationNode, typeof(ExceptionPolicyNode)).Count);
            Assert.AreEqual(2, Hierarchy.FindNodesByType(ApplicationNode, typeof(ExceptionTypeNode)).Count);
            Assert.AreEqual(3, Hierarchy.FindNodesByType(ApplicationNode, typeof(ExceptionHandlerNode)).Count);
            FaultContractExceptionHandlerNode faultNode = new FaultContractExceptionHandlerNode();
            ExceptionTypeNode node = (ExceptionTypeNode)Hierarchy.FindNodeByType(ApplicationNode, typeof(ExceptionTypeNode));
            node.AddNode(faultNode);
            faultNode = (FaultContractExceptionHandlerNode)Hierarchy.FindNodeByType(typeof(FaultContractExceptionHandlerNode));
            faultNode.FaultContractType = typeof(object).AssemblyQualifiedName;
            faultNode.ExceptionMessage = "my exception message";
            Hierarchy.Save();
            Hierarchy.Load();
            Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);
            Hierarchy.Open();
            Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);
            faultNode = (FaultContractExceptionHandlerNode)Hierarchy.FindNodeByType(typeof(FaultContractExceptionHandlerNode));
            faultNode.Remove();
            Hierarchy.Save();
        }
    }
}
