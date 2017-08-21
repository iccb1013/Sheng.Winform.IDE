/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Data;
using Data.SqlCe.Tests.VSTS;
using Microsoft.Practices.EnterpriseLibrary.Data.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Data.SqlCe.Tests.VSTS
{
    [TestClass]
    public class SqlCeUpdateDataSetFixture : UpdateDataSetFixture
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
        public void SqlModifyRowWithCommand()
        {
            base.ModifyRowWithStoredProcedure();
        }
        [TestMethod]
        public void SqlDeleteRowWithCommand()
        {
            base.DeleteRowWithStoredProcedure();
        }
        [TestMethod]
        public void SqlInsertRowWithCommand()
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
        protected override void AddTestData()
        {
            SqlCeDataSetHelper.AddTestData(db);
        }
    }
}
