/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Configuration;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SysConfig=System.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests
{
    [TestClass]
    public class DatabaseFactoryOldFixture
    {
        [TestMethod]
        public void CanCreateDefaultDatabase()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(TestConfigurationSource.CreateConfigurationSource());
            Database db = factory.CreateDefault();
            Assert.IsNotNull(db);
        }
        [TestMethod]
        public void CanGetDatabaseByName()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(TestConfigurationSource.CreateConfigurationSource());
            Database db = factory.Create("NewDatabase");
            Assert.IsNotNull(db);
        }
        [TestMethod]
        public void CallingTwiceReturnsDifferenceDatabaseInstances()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(TestConfigurationSource.CreateConfigurationSource());
            Database firstDb = factory.Create("NewDatabase");
            Database secondDb = factory.Create("NewDatabase");
            Assert.IsFalse(firstDb == secondDb);
        }
        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ExceptionThrownWhenAskingForDatabaseWithUnknownName()
        {
            Database db = DatabaseFactory.CreateDatabase("ThisIsAnUnknownKey");
            Assert.IsNotNull(db);
        }
        [TestMethod]
        public void CreatingDatabaseWithUnknownInstanceNameWritesToEventLog()
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThisIsAnUnknownKey");
            }
            catch (ConfigurationErrorsException)
            {
                using (EventLog applicationLog = new EventLog("Application"))
                {
                    EventLogEntry lastEntry = applicationLog.Entries[applicationLog.Entries.Count - 1];
                    Assert.AreEqual("Enterprise Library Data", lastEntry.Source);
                    Assert.IsTrue(lastEntry.Message.Contains("ThisIsAnUnknownKey"));
                }
                return;
            }
            Assert.Fail("ConfigurationErrorsException expected");
        }
        [TestMethod]
        public void CreateDatabaseDefaultDatabaseWithDatabaseFactory()
        {
            Database db = DatabaseFactory.CreateDatabase();
            Assert.IsNotNull(db);
        }
        [TestMethod]
        public void CreateNamedDatabaseWithDatabaseFactory()
        {
            Database db = DatabaseFactory.CreateDatabase("OracleTest");
            Assert.IsNotNull(db);
        }
    }
}
