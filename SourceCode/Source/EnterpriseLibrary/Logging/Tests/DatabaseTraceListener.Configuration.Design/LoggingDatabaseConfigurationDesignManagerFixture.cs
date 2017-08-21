/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration.Design.Tests
{
    [TestClass]
    public class LoggingDatabaseConfigurationDesignManagerFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void OpenAndSaveConfiguration()
        {
            ApplicationNode.Hierarchy.Load();
            Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);
            ApplicationNode.Hierarchy.Open();
            Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);
            TraceListenerCollectionNode traceListenersNode = (TraceListenerCollectionNode)Hierarchy.FindNodeByType(ApplicationNode, typeof(TraceListenerCollectionNode));
            AddLoggingDatabaseCommand cmd = new AddLoggingDatabaseCommand(ServiceProvider);
            cmd.Execute(traceListenersNode);
            LoggingDatabaseNode dataNode = (LoggingDatabaseNode)Hierarchy.FindNodeByType(ApplicationNode, typeof(LoggingDatabaseNode));
            ConnectionStringSettingsNode connectNode = (ConnectionStringSettingsNode)Hierarchy.FindNodeByType(ApplicationNode, typeof(ConnectionStringSettingsNode));
            dataNode.DatabaseInstance = connectNode;
            Assert.AreEqual(1, Hierarchy.FindNodesByType(ApplicationNode, typeof(DatabaseSectionNode)).Count);
            Hierarchy.Save();
            Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);
            Hierarchy.Load();
            Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);
            Hierarchy.Open();
            Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);
            Assert.AreEqual(1, Hierarchy.FindNodesByType(ApplicationNode, typeof(LoggingDatabaseNode)).Count);
            dataNode = (LoggingDatabaseNode)Hierarchy.FindNodeByType(ApplicationNode, typeof(LoggingDatabaseNode));
            Assert.AreEqual(dataNode.DatabaseInstance.Name, connectNode.Name);
            DatabaseSectionNode databaseSectionNode = (DatabaseSectionNode)Hierarchy.FindNodeByType(ApplicationNode, typeof(DatabaseSectionNode));
            databaseSectionNode.Remove();
            dataNode.Remove();
            Hierarchy.Save();
            Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);
        }
        [TestMethod]
        public void EnsureDatabaseSettingsAreAddedOnNewNode()
        {
            AddLoggingDatabaseCommand cmd = new AddLoggingDatabaseCommand(ServiceProvider);
            cmd.Execute(Hierarchy.RootNode);
            Assert.IsNotNull(Hierarchy.FindNodeByType(ApplicationNode, typeof(DatabaseSectionNode)));
        }
    }
}
