/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests
{
    [TestClass]
    public class DatabaseConfigurationViewFixture
    {
        const string connectionName = "Service_Dflt";
        const string unknownConnectionName = "not in config";
        const string connectionNameWithoutProvider = "no provider";
        const string connectionString = @"server=(local)\SQLEXPRESS;database=Northwind;Integrated Security=true";
        const string providerName = "System.Data.SqlClient";
        const string instanceName = "Database Instance";
        ConnectionStringsSection GetConnectionStringsSection()
        {
            ConnectionStringsSection section = new ConnectionStringsSection();
            ConnectionStringSettings connectionStringSettings = new ConnectionStringSettings(instanceName, connectionString, providerName);
            section.ConnectionStrings.Add(connectionStringSettings);
            return section;
        }
        [TestMethod]
        public void CanGetConnectionStringsIfConnectionStringExists()
        {
            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
            DatabaseConfigurationView view = new DatabaseConfigurationView(configurationSource);
            ConnectionStringSettings data = view.GetConnectionStringSettings(connectionName);
            Assert.IsNotNull(data);
            Assert.AreEqual(connectionString, data.ConnectionString);
        }
        [ExpectedException(typeof(ConfigurationErrorsException))]
        [TestMethod]
        public void FailsGetConnectionStringIfConnectionStringDoesNotExist()
        {
            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
            DatabaseConfigurationView view = new DatabaseConfigurationView(configurationSource);
            view.GetConnectionStringSettings(unknownConnectionName);
        }
        [TestMethod]
        public void CreateNamedDatabaseInstanceWithDictSource()
        {
            DictionaryConfigurationSource source = new DictionaryConfigurationSource();
            DatabaseSettings settings = new DatabaseSettings();
            ConnectionStringsSection connSection = GetConnectionStringsSection();
            source.Add("dataConfiguration", settings);
            source.Add("connectionStrings", connSection);
            DatabaseProviderFactory factory = new DatabaseProviderFactory(source);
            Database dbIns = factory.Create(instanceName);
            Assert.IsNotNull(dbIns);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RequestForNullNameThrows()
        {
            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
            DatabaseConfigurationView view = new DatabaseConfigurationView(configurationSource);
            view.GetConnectionStringSettings(null);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RequestForEmptyNameThrows()
        {
            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
            DatabaseConfigurationView view = new DatabaseConfigurationView(configurationSource);
            view.GetConnectionStringSettings("");
        }
    }
}
