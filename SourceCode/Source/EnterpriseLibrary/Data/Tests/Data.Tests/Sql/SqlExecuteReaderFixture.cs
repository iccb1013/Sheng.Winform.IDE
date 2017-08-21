/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Sql.Tests
{
    [TestClass]
    public class SqlExecuteReaderFixture
    {
        const string insertString = "Insert into Region values (99, 'Midwest')";
        const string queryString = "Select * from Region";
        Database db;
        ExecuteReaderFixture baseFixture;
        [TestInitialize]
        public void TestInitialize()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(TestConfigurationSource.CreateConfigurationSource());
            db = factory.CreateDefault();
            DbCommand insertCommand = db.GetSqlStringCommand(insertString);
            DbCommand queryCommand = db.GetSqlStringCommand(queryString);
            baseFixture = new ExecuteReaderFixture(db, insertString, insertCommand, queryString, queryCommand);
        }
        [TestMethod]
        public void CanExecuteReaderWithStoredProcInTransaction()
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
        [ExpectedException(typeof(SqlException))]
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
        [ExpectedException(typeof(ArgumentException))]
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
