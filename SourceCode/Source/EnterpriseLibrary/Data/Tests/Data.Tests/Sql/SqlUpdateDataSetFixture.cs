/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.Data.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Sql.Tests
{
    [TestClass]
    public class SqlUpdateDataSetFixture : UpdateDataSetFixture
    {
        [TestInitialize]
        public void TestInitialize()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(TestConfigurationSource.CreateConfigurationSource());
            db = factory.CreateDefault();
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
            base.TearDown();
            DeleteStoredProcedures();
        }
        [TestMethod]
        public void SqlModifyRowWithStoredProcedure()
        {
            base.ModifyRowWithStoredProcedure();
        }
        [TestMethod]
        public void SqlModifyRowsWithStoredProcedureAndBatchUpdate()
        {
            base.ModifyRowsWithStoredProcedureAndBatchUpdate();
        }
        [TestMethod]
        public void SqlDeleteRowWithStoredProcedure()
        {
            base.DeleteRowWithStoredProcedure();
        }
        [TestMethod]
        public void SqlInsertRowWithStoredProcedure()
        {
            base.InsertRowWithStoredProcedure();
        }
        [TestMethod]
        public void SqlDeleteRowWithMissingInsertAndUpdateCommands()
        {
            base.DeleteRowWithMissingInsertAndUpdateCommands();
        }
        [TestMethod]
        public void SqlUpdateRowWithMissingInsertAndDeleteCommands()
        {
            base.UpdateRowWithMissingInsertAndDeleteCommands();
        }
        [TestMethod]
        public void SqlInsertRowWithMissingUpdateAndDeleteCommands()
        {
            base.InsertRowWithMissingUpdateAndDeleteCommands();
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SqlUpdateDataSetWithAllCommandsMissing()
        {
            base.UpdateDataSetWithAllCommandsMissing();
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SqlUpdateDataSetWithNullTable()
        {
            base.UpdateDataSetWithNullTable();
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SqlUpdateSetWithNullDataSet()
        {
            base.UpdateSetWithNullDataSet();
        }
        protected override void CreateDataAdapterCommands()
        {
            SqlDataSetHelper.CreateDataAdapterCommands(db, ref insertCommand, ref updateCommand, ref deleteCommand);
        }
        protected override void CreateStoredProcedures()
        {
            SqlDataSetHelper.CreateStoredProcedures(db);
        }
        protected override void DeleteStoredProcedures()
        {
            SqlDataSetHelper.DeleteStoredProcedures(db);
        }
        protected override void AddTestData()
        {
            SqlDataSetHelper.AddTestData(db);
        }
    }
}
