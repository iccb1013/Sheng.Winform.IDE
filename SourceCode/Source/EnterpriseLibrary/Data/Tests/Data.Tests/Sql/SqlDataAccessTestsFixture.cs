/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Sql.Tests
{
    [TestClass]
    public class SqlDataAccessTestsFixture
    {
        DataAccessTestsFixture baseFixture;
        Database db;
        [TestInitialize]
        public void SetUp()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(TestConfigurationSource.CreateConfigurationSource());
            db = factory.CreateDefault();
            baseFixture = new DataAccessTestsFixture(db);
        }
        [TestMethod]
        public void CanGetResultSetBackWithParamaterizedQuery()
        {
            string sqlCommand = "SELECT RegionDescription FROM Region where regionId = @ID";
            DataSet ds = new DataSet();
            DbCommand cmd = db.GetSqlStringCommand(sqlCommand);
            db.AddInParameter(cmd, "@ID", DbType.Int32, 4);
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    db.LoadDataSet(cmd, ds, "Foo", transaction);
                    Assert.AreEqual(1, ds.Tables[0].Rows.Count);
                }
            }
        }
        [TestMethod]
        public void OneTransactionLocksOutAnother()
        {
            DbCommand firstCommand = db.GetSqlStringCommand("insert into region values (99, 'Midwest')");
            DbCommand secondCommand = db.GetSqlStringCommand("Select * from Region");
            DbConnection connection1 = db.CreateConnection();
            connection1.Open();
            DbTransaction transaction1 = connection1.BeginTransaction(IsolationLevel.Serializable);
            DbConnection connection2 = db.CreateConnection();
            connection2.Open();
            DbTransaction transaction2 = connection2.BeginTransaction(IsolationLevel.Serializable);
            DataSet dataSet2 = new DataSet();
            secondCommand.CommandTimeout = 1;
            try
            {
                db.ExecuteNonQuery(firstCommand, transaction1);
                db.LoadDataSet(secondCommand, dataSet2, "Foo", transaction2);
                Assert.Fail("should have thrown some funky exception");
            }
            catch (SqlException) {}
            finally
            {
                transaction1.Rollback();
                transaction1.Dispose();
                transaction2.Dispose();
                connection1.Dispose();
                connection2.Dispose();
            }
        }
        [TestMethod]
        public void ResultsFromSelectOverTwoTablesMustReturnDataInNamedTables()
        {
            string selectSql = "Select * from Region; Select * from Orders";
            string[] tableNames = new string[] { "RegionData", "OrderData" };
            DbCommand command = db.GetSqlStringCommand(selectSql);
            DataSet dataSet = new DataSet();
            db.LoadDataSet(command, dataSet, tableNames);
            Assert.IsNotNull(dataSet.Tables["RegionData"]);
            Assert.IsNotNull(dataSet.Tables["OrderData"]);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullTableNameArrayCausesException()
        {
            string selectSql = "Select * from Region; Select * from Orders";
            DbCommand command = db.GetSqlStringCommand(selectSql);
            DataSet dataSet = new DataSet();
            db.LoadDataSet(command, dataSet, (string[])null);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TableNameArrayWithNoEntriesCausesException()
        {
            string selectSql = "Select * from Region; Select * from Orders";
            string[] tableNames = new string[0];
            DbCommand command = db.GetSqlStringCommand(selectSql);
            DataSet dataSet = new DataSet();
            db.LoadDataSet(command, dataSet, tableNames);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NullTableNameInArrayCausesException()
        {
            string selectSql = "Select * from Region; Select * from Orders";
            string[] tableNames = new string[] { "foo", null, "bar" };
            DbCommand command = db.GetSqlStringCommand(selectSql);
            DataSet dataSet = new DataSet();
            db.LoadDataSet(command, dataSet, tableNames);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EmptyTableNameInArrayCausesException()
        {
            string selectSql = "Select * from Region; Select * from Orders";
            string[] tableNames = new string[] { "foo", "", "bar" };
            DbCommand command = db.GetSqlStringCommand(selectSql);
            DataSet dataSet = new DataSet();
            db.LoadDataSet(command, dataSet, tableNames);
        }
        [TestMethod]
        public void CanGetNonEmptyResultSet()
        {
            baseFixture.CanGetNonEmptyResultSet();
        }
        [TestMethod]
        public void CanGetTablePositionally()
        {
            baseFixture.CanGetTablePositionally();
        }
        [TestMethod]
        public void CanGetNonEmptyResultSetUsingTransaction()
        {
            baseFixture.CanGetNonEmptyResultSetUsingTransaction();
        }
        [TestMethod]
        public void CanGetNonEmptyResultSetUsingTransactionWithNullTableName()
        {
            baseFixture.CanGetNonEmptyResultSetUsingTransactionWithNullTableName();
        }
        [TestMethod]
        public void ExecuteDataSetWithCommand()
        {
            baseFixture.ExecuteDataSetWithCommand();
        }
        [TestMethod]
        public void ExecuteDataSetWithDbTransaction()
        {
            baseFixture.ExecuteDataSetWithDbTransaction();
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CannotLoadDataSetWithEmptyTableName()
        {
            baseFixture.CannotLoadDataSetWithEmptyTableName();
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExecuteNullCommand()
        {
            baseFixture.ExecuteNullCommand();
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExecuteCommandNullTransaction()
        {
            baseFixture.ExecuteCommandNullTransaction();
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExecuteCommandNullDataset()
        {
            baseFixture.ExecuteCommandNullDataset();
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExecuteCommandNullCommand()
        {
            baseFixture.ExecuteCommandNullCommand();
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExecuteCommandNullTableName()
        {
            baseFixture.ExecuteCommandNullTableName();
        }
    }
}
