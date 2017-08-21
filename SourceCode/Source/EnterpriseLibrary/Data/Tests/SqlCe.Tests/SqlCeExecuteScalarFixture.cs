/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Data.Common;
using System.Data.SqlServerCe;
using Microsoft.Practices.EnterpriseLibrary.Data.SqlCe;
using Microsoft.Practices.EnterpriseLibrary.Data.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Data.SqlCe.Tests.VSTS
{
    [TestClass]
    public class SqlCeExecuteScalarFixture
    {
        TestConnectionString testConnection;
        SqlCeDatabase db;
        ExecuteScalarFixture baseFixture;
        [TestInitialize]
        public void SetUp()
        {
            testConnection = new TestConnectionString();
            testConnection.CopyFile();
            db = new SqlCeDatabase(testConnection.ConnectionString);
            DbCommand command = db.GetSqlStringCommand("Select count(*) from region");
            baseFixture = new ExecuteScalarFixture(db, command);
        }
        [TestCleanup]
        public void DeleteDb()
        {
            SqlCeConnectionPool.CloseSharedConnections();
            testConnection.DeleteFile();
        }
        [TestMethod]
        public void CanOpenDatabase()
        {
            Assert.IsNotNull(db);
        }
        [TestMethod]
        public void ExecuteScalarWithIDbCommand()
        {
            baseFixture.ExecuteScalarWithIDbCommand();
        }
        [TestMethod]
        public void ExecuteScalarWithCommandTextAndTypeInTransaction()
        {
            baseFixture.ExecuteScalarWithCommandTextAndTypeInTransaction();
        }
        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void CannotExecuteReaderWithStoredProcInTransaction()
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (DbTransaction trans = connection.BeginTransaction())
                {
                    db.ExecuteScalar(trans, "Ten Most Expensive Products");
                    trans.Rollback();
                }
            }
        }
        [TestMethod]
        [ExpectedException(typeof(SqlCeException))]
        public void ExecuteSqlWithBadCommandThrows()
        {
            DbCommand badCommand = db.GetSqlStringCommand("select * from foobar");
            db.ExecuteScalar(badCommand);
        }
        [TestMethod]
        public void ExecuteScalarWithIDbTransaction()
        {
            baseFixture.ExecuteScalarWithIDbTransaction();
        }
        [TestMethod]
        public void CanExecuteScalarDoAnInsertion()
        {
            baseFixture.CanExecuteScalarDoAnInsertion();
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExecuteScalarWithNullIDbCommand()
        {
            baseFixture.ExecuteScalarWithNullIDbCommand();
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExecuteScalarWithNullIDbTransaction()
        {
            baseFixture.ExecuteScalarWithNullIDbTransaction();
        }
        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void ExecuteScalarWithNullIDbCommandAndNullTransaction()
        {
            baseFixture.ExecuteScalarWithNullIDbCommandAndNullTransaction();
        }
    }
}
