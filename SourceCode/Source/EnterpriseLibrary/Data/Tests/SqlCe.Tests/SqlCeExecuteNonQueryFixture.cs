/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.SqlCe;
using Microsoft.Practices.EnterpriseLibrary.Data.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Data.SqlCe.Tests.VSTS
{
    [TestClass]
    public class SqlCeExecuteNonQueryFixture
    {
        TestConnectionString testConnection;
        ExecuteNonQueryFixture baseFixture;
        Database db;
        const string insertString = "insert into Region values (77, 'Elbonia')";
        const string countQuery = "select count(*) from Region";
        [TestInitialize]
        public void SetUp()
        {
            testConnection = new TestConnectionString();
            testConnection.CopyFile();
            db = new SqlCeDatabase(testConnection.ConnectionString);
            DbCommand insertionCommand = db.GetSqlStringCommand(insertString);
            DbCommand countCommand = db.GetSqlStringCommand(countQuery);
            baseFixture = new ExecuteNonQueryFixture(db, insertString, countQuery, insertionCommand, countCommand);
        }
        [TestCleanup]
        public void TearDown()
        {
            SqlCeConnectionPool.CloseSharedConnections();
            testConnection = new TestConnectionString();
            testConnection.DeleteFile();
        }
        [TestMethod]
        public void CanExecuteNonQueryWithCommandTextWithDefinedTypeAndTransaction()
        {
            baseFixture.CanExecuteNonQueryWithCommandTextWithDefinedTypeAndTransaction();
        }
        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void CannotExecuteNonQueryWithStoredProc()
        {
            db.ExecuteNonQuery("Ten Most Expensive Products");
        }
        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void CanExecuteNonQueryWithStoredProcInTransaction()
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (DbTransaction trans = connection.BeginTransaction())
                {
                    db.ExecuteNonQuery(trans, "Ten Most Expensive Products");
                    trans.Rollback();
                }
            }
        }
        [TestMethod]
        public void CanExecuteNonQueryWithDbCommand()
        {
            baseFixture.CanExecuteNonQueryWithDbCommand();
        }
        [TestMethod]
        public void CanExecuteNonQueryThroughTransaction()
        {
            baseFixture.CanExecuteNonQueryThroughTransaction();
        }
        [TestMethod]
        public void TransactionActuallyRollsBack()
        {
            baseFixture.TransactionActuallyRollsBack();
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExecuteNonQueryWithNullDbTransaction()
        {
            baseFixture.ExecuteNonQueryWithNullDbTransaction();
        }
        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void ExecuteNonQueryWithNullDbCommandAndTransaction()
        {
            baseFixture.ExecuteNonQueryWithNullDbCommandAndTransaction();
        }
    }
}
