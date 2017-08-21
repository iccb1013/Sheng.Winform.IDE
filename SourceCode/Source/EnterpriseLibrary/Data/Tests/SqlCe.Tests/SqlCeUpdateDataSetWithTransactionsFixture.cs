/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Data;
using System.Data.Common;
using Data.SqlCe.Tests.VSTS;
using Microsoft.Practices.EnterpriseLibrary.Data.SqlCe.Tests.VSTS;
using Microsoft.Practices.EnterpriseLibrary.Data.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Data.SqlCe.Tests
{
    [TestClass]
    public class SqlCeUpdateDataSetWithTransactionsFixture : UpdateDataSetWithTransactionsFixture
    {
        TestConnectionString testConnection;
        [TestInitialize]
        public void TestInitialize()
        {
            testConnection = new TestConnectionString();
            testConnection.CopyFile();
            db = new SqlCeDatabase(testConnection.ConnectionString);
            base.SetUp();
        }
        [TestCleanup]
        public void Dispose()
        {
            deleteCommand.Dispose();
            insertCommand.Dispose();
            updateCommand.Dispose();
            base.TearDown();
            transaction.Connection.Dispose();
            transaction.Dispose();
            SqlCeConnectionPool.CloseSharedConnections();
            testConnection.DeleteFile();
        }
        [TestMethod]
        public void SqlModifyRowWithStoredProcedure()
        {
            base.ModifyRowWithStoredProcedure();
        }
        [TestMethod]
        public void SqlAttemptToInsertBadRowInsideOfATransaction()
        {
            base.AttemptToInsertBadRowInsideOfATransaction();
        }
        protected override DataSet GetDataSetFromTable()
        {
            using (DbCommand selectCommand = db.GetSqlStringCommand("select * from region"))
            {
                return db.ExecuteDataSet(selectCommand, transaction);
            }
        }
        protected override DataSet GetDataSetFromTableWithoutTransaction()
        {
            SqlCeDatabase db = (SqlCeDatabase)base.db;
            return db.ExecuteDataSetSql("select * from region");
        }
        protected override void CreateDataAdapterCommands()
        {
            SqlCeDataSetHelper.CreateDataAdapterCommands(db, ref insertCommand, ref updateCommand, ref deleteCommand);
        }
        protected override void CreateStoredProcedures() {}
        protected override void DeleteStoredProcedures() {}
        protected override void AddTestData()
        {
            SqlCeDataSetHelper.AddTestData(db);
        }
    }
}
