/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlServerCe;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.SqlCe;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Data.SqlCe.Tests.VSTS
{
    [TestClass]
    public class SqlCeDatabaseFixture
    {
        [TestInitialize]
        public void Initialize()
        {
            TestConnectionString test = new TestConnectionString();
            test.CopyFile();
        }
        [TestCleanup]
        public void TearDown()
        {
            SqlCeConnectionPool.CloseSharedConnections();
            TestConnectionString test = new TestConnectionString();
            test.DeleteFile();
        }
        const string instanceName = "Database Instance";
        const string connectionString = "Data Source='junk.sdf'";
        const string providerName = "System.Data.SqlServerCe.3.5";
        static ConnectionStringsSection GetConnectionStringsSection()
        {
            ConnectionStringsSection section = new ConnectionStringsSection();
            ConnectionStringSettings connectionStringSettings = new ConnectionStringSettings(instanceName, connectionString, providerName);
            section.ConnectionStrings.Add(connectionStringSettings);
            return section;
        }
        static DbProviderMapping GetProviderMapping()
        {
            DbProviderMapping providerMapping = new DbProviderMapping(providerName, typeof(SqlCeDatabase));
            return providerMapping;
        }
        [TestMethod]
        public void DatabaseCreatedByProviderFactoryIsASqlCeDatabase()
        {
            DictionaryConfigurationSource source = new DictionaryConfigurationSource();
            DatabaseSettings settings = new DatabaseSettings();
            ConnectionStringsSection connSection = GetConnectionStringsSection();
            DbProviderMapping providerMapping = GetProviderMapping();
            settings.ProviderMappings.Add(providerMapping);
            source.Add("dataConfiguration", settings);
            source.Add("connectionStrings", connSection);
            DatabaseProviderFactory factory = new DatabaseProviderFactory(source);
            Database db = factory.Create(instanceName);
            Assert.IsNotNull(db);
            Assert.AreSame(typeof(SqlCeDatabase), db.GetType());
        }
        [TestMethod]
        public void DatabaseCreatedByProviderFactoryIsGenericIfNoProviderMapping()
        {
            DictionaryConfigurationSource source = new DictionaryConfigurationSource();
            DatabaseSettings settings = new DatabaseSettings();
            ConnectionStringsSection connSection = GetConnectionStringsSection();
            source.Add("dataConfiguration", settings);
            source.Add("connectionStrings", connSection);
            DatabaseProviderFactory factory = new DatabaseProviderFactory(source);
            Database db = factory.Create(instanceName);
            Assert.IsNotNull(db);
            Assert.AreSame(typeof(GenericDatabase), db.GetType());
        }
        [TestMethod]
        public void CanOpenSqlCeDatabase()
        {
            TestConnectionString testConnection = new TestConnectionString();
            SqlCeDatabase database = new SqlCeDatabase(testConnection.ConnectionString);
            using (DbConnection connection = database.CreateConnection())
            {
                connection.Open();
                Assert.AreEqual(ConnectionState.Open, connection.State);
            }
        }
        [TestMethod]
        public void CanCreateNewDatabaseFile()
        {
            string filename = Path.Combine(Environment.CurrentDirectory, "newfile.sdf");
            string connectionString = "Data Source='{0}'";
            connectionString = String.Format(connectionString, filename);
            if (File.Exists(filename))
                File.Delete(filename);
            Assert.IsFalse(File.Exists(filename));
            SqlCeDatabase database = new SqlCeDatabase(connectionString);
            database.CreateFile();
            Assert.IsTrue(File.Exists(filename));
            File.Delete(filename);
        }
        [TestMethod]
        [ExpectedException(typeof(SqlCeException))]
        public void CreateNewDatabaseThrowsWhenFileAlreadyExists()
        {
            TestConnectionString file = new TestConnectionString();
            Assert.IsTrue(File.Exists(file.Filename));
            SqlCeDatabase database = new SqlCeDatabase(file.ConnectionString);
            database.CreateFile();
        }
    }
}
