/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Configuration;
using System.Data.Common;
using System.Data.OracleClient;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Tests
{
    [TestClass]
    public class OracleDatabaseFixture
    {
        IConfigurationSource configurationSource;
        DatabaseConfigurationView view;
        IDatabaseAssembler assembler;
        [TestInitialize]
        public void SetUp()
        {
            configurationSource = new SystemConfigurationSource();
            view = new DatabaseConfigurationView(configurationSource);
            assembler = new DatabaseCustomFactory().GetAssembler(typeof(OracleDatabase), "", new ConfigurationReflectionCache());
        }
        [TestMethod]
        public void CanConnectToOracleAndExecuteAReader()
        {
            ConnectionStringSettings data = view.GetConnectionStringSettings("OracleTest");
            OracleDatabase oracleDatabase = (OracleDatabase)assembler.Assemble(data.Name, data, configurationSource);
            DbConnection connection = oracleDatabase.CreateConnection();
            Assert.IsNotNull(connection);
            Assert.IsTrue(connection is OracleConnection);
            connection.Open();
            DbCommand cmd = oracleDatabase.GetSqlStringCommand("Select * from Region");
            cmd.CommandTimeout = 0;
        }
        [TestMethod]
        public void CanExecuteCommandWithEmptyPackages()
        {
            ConnectionStringSettings data = view.GetConnectionStringSettings("OracleTest");
            OracleDatabase oracleDatabase = new OracleDatabase(data.ConnectionString);
            DbConnection connection = oracleDatabase.CreateConnection();
            Assert.IsNotNull(connection);
            Assert.IsTrue(connection is OracleConnection);
            connection.Open();
            DbCommand cmd = oracleDatabase.GetSqlStringCommand("Select * from Region");
            cmd.CommandTimeout = 0;
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructingAnOracleDatabaseWithNullPackageListThrows()
        {
            ConnectionStringSettings data = view.GetConnectionStringSettings("OracleTest");
            new OracleDatabase(data.ConnectionString, null);
        }
    }
}
