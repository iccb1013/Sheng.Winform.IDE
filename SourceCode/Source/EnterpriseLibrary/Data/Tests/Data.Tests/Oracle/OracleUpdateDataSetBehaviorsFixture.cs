/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Data;
using System.Data.OracleClient;
using Microsoft.Practices.EnterpriseLibrary.Data.Tests;
using Microsoft.Practices.EnterpriseLibrary.Data.Tests.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Tests
{
    [TestClass]
    public class OracleUpdateDataSetBehaviorsFixture : UpdateDataSetBehaviorsFixture
    {
        [TestInitialize]
        public void Initialize()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(TestConfigurationSource.CreateConfigurationSource());
            db = factory.Create("OracleTest");
            try
            {
                DeleteStoredProcedures();
            }
            catch {}
            CreateStoredProcedures();
            base.SetUp();
        }
        [TestCleanup]
        public void TestCleanup()
        {
            DeleteStoredProcedures();
            base.TearDown();
        }
        [TestMethod]
        public void UpdateWithTransactionalBehaviorAndBadData()
        {
            DataRow errRow = null;
            try
            {
                errRow = AddRowsWithErrorsToDataTable(startingData.Tables[0]);
                db.UpdateDataSet(startingData, "Table", insertCommand, updateCommand, deleteCommand, UpdateBehavior.Transactional);
            }
            catch (OracleException)
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
            catch (OracleException)
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
            Assert.AreEqual(502, Convert.ToInt32(resultTable.Rows[6]["RegionID"]));
            Assert.AreEqual("Washington", resultTable.Rows[6]["RegionDescription"].ToString().Trim());
        }
        [TestMethod]
        public void UpdateWithContinueBehaviorAndBadData()
        {
            DataRow errRow = AddRowsWithErrorsToDataTable(startingData.Tables[0]);
            db.UpdateDataSet(startingData, "Table", insertCommand, updateCommand, deleteCommand, UpdateBehavior.Continue);
            DataSet resultDataSet = GetDataSetFromTable();
            DataTable resultTable = resultDataSet.Tables[0];
            Assert.IsTrue(errRow.HasErrors);
            Assert.AreEqual(Resources.ExceptionMessageUpdateDataSetRowFailure, errRow.RowError);
            Assert.AreEqual(10, resultTable.Rows.Count);
            Assert.AreEqual(500, Convert.ToInt32(resultTable.Rows[4]["RegionID"]));
            Assert.AreEqual(502, Convert.ToInt32(resultTable.Rows[6]["RegionID"]));
        }
        [TestMethod]
        public void OracleUpdateWithTransactionalBehavior()
        {
            base.UpdateWithTransactionalBehavior();
        }
        [TestMethod]
        public void OracleUpdateWithStandardBehavior()
        {
            base.UpdateWithStandardBehavior();
        }
        protected override void CreateDataAdapterCommands()
        {
            OracleDataSetHelper.CreateDataAdapterCommands(db, ref insertCommand, ref updateCommand, ref deleteCommand);
        }
        protected override void CreateStoredProcedures()
        {
            OracleDataSetHelper.CreateStoredProcedures(db);
        }
        protected override void DeleteStoredProcedures()
        {
            OracleDataSetHelper.DeleteStoredProcedures(db);
        }
    }
}
