/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Sources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.Design.Tests
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
            ExceptionTypeNode node = (ExceptionTypeNode)Hierarchy.FindNodeByType(ApplicationNode, typeof(ExceptionTypeNode));
            AddLoggingExceptionHandlerCommand cmd = new AddLoggingExceptionHandlerCommand(ServiceProvider);
            cmd.Execute(node);
            Assert.IsNotNull(Hierarchy.FindNodeByType(typeof(LoggingSettingsNode)));
            LoggingExceptionHandlerNode logehNode = (LoggingExceptionHandlerNode)Hierarchy.FindNodeByType(typeof(LoggingExceptionHandlerNode));
            logehNode.FormatterType = typeof(XmlExceptionFormatter).AssemblyQualifiedName;
            logehNode.LogCategory = (CategoryTraceSourceNode)Hierarchy.FindNodeByType(typeof(CategoryTraceSourceNode));
            Hierarchy.Save();
            Hierarchy.Load();
            Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);
            Hierarchy.Open();
            Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);
            logehNode = (LoggingExceptionHandlerNode)Hierarchy.FindNodeByType(typeof(LoggingExceptionHandlerNode));
            logehNode.Remove();
            Hierarchy.Save();
        }
    }
}
