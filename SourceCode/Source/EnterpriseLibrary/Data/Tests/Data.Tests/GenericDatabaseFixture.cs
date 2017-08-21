/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests
{
    [TestClass]
    public class GenericDatabaseFixture
    {
        const string connectionString = "some connection string;";
        [TestMethod]
        public void CanCreateGenericDatabaseFromSysConfiguration()
        {
            Database database = (new DatabaseProviderFactory(new SystemConfigurationSource())).Create("OdbcDatabase");
            Assert.IsNotNull(database);
            Assert.AreEqual(database.GetType(), typeof(GenericDatabase));
            Assert.AreEqual(database.DbProviderFactory.GetType(), typeof(OdbcFactory));
            Assert.AreEqual(connectionString, database.ConnectionStringWithoutCredentials);
        }
        [TestMethod]
        public void CanDoExecuteDataReaderForGenericDatabaseBug1836()
        {
            Database db = new GenericDatabase(@"Driver={Microsoft Access Driver (*.mdb)};Dbq=northwind.mdb;Uid=sa;Pwd=sa;", OdbcFactory.Instance);
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    using (IDataReader dataReader = db.ExecuteReader(transaction, CommandType.Text, "select * from [Order Details]")) {}
                    transaction.Commit();
                }
            }
        }
        [TestMethod]
        public void CanDoExecuteDataReaderForGenericDatabaseForSqlProviderBug1836()
        {
            Database db = new GenericDatabase(@"server=(local)\SQLEXPRESS;database=Northwind;Integrated Security=true", SqlClientFactory.Instance);
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    using (IDataReader dataReader = db.ExecuteReader(transaction, CommandType.Text, "select * from [Order Details]")) {}
                    transaction.Commit();
                }
            }
        }
        [TestMethod]
        public void DSDBWrapperTransSqlNoCommitTestBug1857()
        {
            Database db = new GenericDatabase(@"server=(local)\SQLEXPRESS;database=Northwind;Integrated Security=true", SqlClientFactory.Instance);
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    string sqlCommand = "select * from [Order Details]";
                    DbCommand dbCommand = db.GetSqlStringCommand(sqlCommand);
                    DataSet dsActualResult = db.ExecuteDataSet(dbCommand, transaction);
                }
            }
        }
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GenericDatabaseThrowsWhenAskedToDeriveParameters()
        {
            Database db = new GenericDatabase(@"server=(local)\SQLEXPRESS;database=Northwind;Integrated Security=true", SqlClientFactory.Instance);
            DbCommand storedProcedure = db.GetStoredProcCommand("CustOrdersOrders");
            db.DiscoverParameters(storedProcedure);
        }
    }
}
