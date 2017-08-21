/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.Data.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Tests
{
    [TestClass]
    public class OracleUpdateDataSetFixture : UpdateDataSetFixture
    {
        [TestInitialize]
        public void TestInitialize()
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
        public void OneTimeTearDown()
        {
            base.TearDown();
            DeleteStoredProcedures();
        }
        [TestMethod]
        public void OracleModifyRowWithStoredProcedure()
        {
            base.ModifyRowWithStoredProcedure();
        }
        [TestMethod]
        public void OracleDeleteRowWithStoredProcedure()
        {
            base.DeleteRowWithStoredProcedure();
        }
        [TestMethod]
        public void OracleInsertRowWithStoredProcedure()
        {
            base.InsertRowWithStoredProcedure();
        }
        [TestMethod]
        public void OracleDeleteRowWithMissingInsertAndUpdateCommands()
        {
            base.DeleteRowWithMissingInsertAndUpdateCommands();
        }
        [TestMethod]
        public void OracleUpdateRowWithMissingInsertAndDeleteCommands()
        {
            base.UpdateRowWithMissingInsertAndDeleteCommands();
        }
        [TestMethod]
        public void OracleInsertRowWithMissingUpdateAndDeleteCommands()
        {
            base.InsertRowWithMissingUpdateAndDeleteCommands();
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void OracleUpdateDataSetWithAllCommandsMissing()
        {
            base.UpdateDataSetWithAllCommandsMissing();
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void OracleUpdateDataSetWithNullTable()
        {
            base.UpdateDataSetWithNullTable();
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
        protected override void AddTestData()
        {
            OracleDataSetHelper.AddTestData(db);
        }
    }
}
