/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Data.Common;
using System.Data.SqlServerCe;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.SqlCe;
using Microsoft.Practices.EnterpriseLibrary.Data.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Data.SqlCe.Tests.VSTS
{
    [TestClass]
    public class SqlCeExecuteReaderFixture
    {
        TestConnectionString testConnection;
        const string insertString = "Insert into Region values (99, 'Midwest')";
        const string queryString = "Select * from Region";
        Database db;
        ExecuteReaderFixture baseFixture;
        [TestInitialize]
        public void TestInitialize()
        {
            testConnection = new TestConnectionString();
            testConnection.CopyFile();
            db = new SqlCeDatabase(testConnection.ConnectionString);
            DbCommand insertCommand = db.GetSqlStringCommand(insertString);
            DbCommand queryCommand = db.GetSqlStringCommand(queryString);
            baseFixture = new ExecuteReaderFixture(db, insertString, insertCommand, queryString, queryCommand);
        }
        [TestCleanup]
        public void TearDown()
        {
            SqlCeConnectionPool.CloseSharedConnections();
        }
        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void ExecuteReaderWithStoredProcInTransactionNotSupported()
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (DbTransaction trans = connection.BeginTransaction())
                {
                    using (db.ExecuteReader(trans, "Ten Most Expensive Products")) {}
                    trans.Commit();
                }
            }
        }
        [TestMethod]
        public void CanExecuteReaderWithCommandText()
        {
            baseFixture.CanExecuteReaderWithCommandText();
        }
        [TestMethod]
        public void CanExecuteReaderFromDbCommand()
        {
            baseFixture.CanExecuteReaderFromDbCommand();
        }
        [TestMethod]
        [ExpectedException(typeof(SqlCeException))]
        public void ExecuteReaderWithBadCommandThrows()
        {
            DbCommand badCommand = db.GetSqlStringCommand("select * from foobar");
            db.ExecuteReader(badCommand);
        }
        [TestMethod]
        public void WhatGetsReturnedWhenWeDoAnInsertThroughDbCommandExecute()
        {
            baseFixture.WhatGetsReturnedWhenWeDoAnInsertThroughDbCommandExecute();
        }
        [TestMethod]
        public void CanExecuteQueryThroughDataReaderUsingTransaction()
        {
            baseFixture.CanExecuteQueryThroughDataReaderUsingTransaction();
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CanExecuteQueryThroughDataReaderUsingNullCommand()
        {
            baseFixture.ExecuteQueryThroughDataReaderUsingNullCommandThrows();
        }
        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void ExecuteQueryThroughDataReaderUsingNullCommandAndNullTransactionThrows()
        {
            baseFixture.ExecuteQueryThroughDataReaderUsingNullCommandAndNullTransactionThrows();
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExecuteQueryThroughDataReaderUsingNullTransactionThrows()
        {
            baseFixture.ExecuteQueryThroughDataReaderUsingNullTransactionThrows();
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExecuteReaderWithNullCommand()
        {
            baseFixture.ExecuteReaderWithNullCommand();
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NullQueryStringTest()
        {
            baseFixture.NullQueryStringTest();
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EmptyQueryStringTest()
        {
            baseFixture.EmptyQueryStringTest();
        }
    }
}
