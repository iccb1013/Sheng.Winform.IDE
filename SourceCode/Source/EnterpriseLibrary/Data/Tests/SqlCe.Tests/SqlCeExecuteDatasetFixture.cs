/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.SqlCe;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Data.SqlCe.Tests.VSTS
{
    [TestClass]
    public class SqlCeExecuteDatasetFixture
    {
        TestConnectionString testConnection;
        const string queryString = "Select * from Region";
        const string storedProc = "Ten Most Expensive Products";
        Database db;
        [TestInitialize]
        public void TestInitialize()
        {
            testConnection = new TestConnectionString();
            testConnection.CopyFile();
            db = new SqlCeDatabase(testConnection.ConnectionString);
        }
        [TestCleanup]
        public void TearDown()
        {
            SqlCeConnectionPool.CloseSharedConnections();
            testConnection = new TestConnectionString();
            testConnection.DeleteFile();
        }
        [TestMethod]
        public void CanRetriveDataSetFromSqlString()
        {
            DataSet dataSet = db.ExecuteDataSet(CommandType.Text, queryString);
            Assert.AreEqual(1, dataSet.Tables.Count);
        }
        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void CannotRetiveDataSetFromStoredProcedure()
        {
            DataSet dataSet = db.ExecuteDataSet(storedProc);
            Assert.AreEqual(1, dataSet.Tables.Count);
        }
        [TestMethod]
        public void CanRetriveDataSetFromSqlStringWithTransaction()
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (DbTransaction trans = connection.BeginTransaction())
                {
                    DataSet dataSet = db.ExecuteDataSet(trans, CommandType.Text, queryString);
                    Assert.AreEqual(1, dataSet.Tables.Count);
                }
            }
        }
        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void CannotRetiveDataSetFromStoredProcedureWithTransaction()
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (DbTransaction trans = connection.BeginTransaction())
                {
                    DataSet dataSet = db.ExecuteDataSet(trans, storedProc);
                    Assert.AreEqual(1, dataSet.Tables.Count);
                }
            }
        }
    }
}
