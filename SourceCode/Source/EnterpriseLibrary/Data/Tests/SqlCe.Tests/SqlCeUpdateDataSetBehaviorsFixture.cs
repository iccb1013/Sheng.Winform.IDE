/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Data;
using System.Data.SqlServerCe;
using Data.SqlCe.Tests.VSTS;
using Microsoft.Practices.EnterpriseLibrary.Data.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Data.SqlCe.Tests.VSTS
{
    [TestClass]
    public class SqlCeUpdateDataSetBehaviorsFixture : UpdateDataSetBehaviorsFixture
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
        public void TestCleanup()
        {
            deleteCommand.Dispose();
            insertCommand.Dispose();
            updateCommand.Dispose();
            base.TearDown();
            SqlCeConnectionPool.CloseSharedConnections();
            testConnection.DeleteFile();
        }
        [TestMethod]
        public void UpdateWithTransactionalBehaviorAndBadData()
        {
            DataRow errRow = null;
            try
            {
                errRow = AddRowsWithErrorsToDataTable(startingData.Tables[0]);
                db.UpdateDataSet(startingData, "Table", insertCommand, updateCommand,
                                 deleteCommand, UpdateBehavior.Transactional);
            }
            catch (SqlCeException)
            {
                DataSet resultDataSet = GetDataSetFromTable();
                DataTable resultTable = resultDataSet.Tables[0];
                Assert.IsTrue(errRow.HasErrors);
                Assert.AreEqual(4, resultTable.Rows.Count);
                return;
            }
            Assert.Fail();
        }
        [TestMethod]
        public void UpdateWithStandardBehaviorAndBadData()
        {
            DataRow errRow = null;
            try
            {
                errRow = AddRowsWithErrorsToDataTable(startingData.Tables[0]);
                db.UpdateDataSet(startingData, "Table", insertCommand, updateCommand, deleteCommand, UpdateBehavior.Standard);
            }
            catch (SqlCeException)
            {
                DataSet resultDataSet = GetDataSetFromTable();
                DataTable resultTable = resultDataSet.Tables[0];
                Assert.IsTrue(errRow.HasErrors);
                Assert.AreEqual(8, resultTable.Rows.Count);
                return;
            }
            Assert.Fail();
        }
        [TestMethod]
        public void UpdateWithContinueBehavior()
        {
            AddRowsToDataTable(startingData.Tables[0]);
            db.UpdateDataSet(startingData, "Table", insertCommand, updateCommand, deleteCommand, UpdateBehavior.Continue);
            DataSet resultDataSet = GetDataSetFromTable();
            DataTable resultTable = resultDataSet.Tables[0];
            Assert.AreEqual(8, resultTable.Rows.Count);
            Assert.AreEqual(502, (int)resultTable.Rows[6]["RegionID"]);
            Assert.AreEqual("Washington", resultTable.Rows[6]["RegionDescription"].ToString().Trim());
        }
        [TestMethod]
        public void UpdateWithContinueBehaviorAndBadData()
        {
            DataRow errRow = AddRowsWithErrorsToDataTable(startingData.Tables[0]);
            db.UpdateDataSet(startingData, "Table", insertCommand, updateCommand,
                             deleteCommand, UpdateBehavior.Continue);
            DataSet resultDataSet = GetDataSetFromTable();
            DataTable resultTable = resultDataSet.Tables[0];
            Assert.IsTrue(errRow.HasErrors);
            Assert.AreEqual(10, resultTable.Rows.Count);
            Assert.AreEqual(500, (int)resultTable.Rows[4]["RegionID"]);
            Assert.AreEqual(502, (int)resultTable.Rows[6]["RegionID"]);
        }
        [TestMethod]
        public void SqlUpdateWithTransactionalBehavior()
        {
            base.UpdateWithTransactionalBehavior();
        }
        [TestMethod]
        public void SqlUpdateWithStandardBehavior()
        {
            base.UpdateWithStandardBehavior();
        }
        protected override DataSet GetDataSetFromTable()
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
    }
}
