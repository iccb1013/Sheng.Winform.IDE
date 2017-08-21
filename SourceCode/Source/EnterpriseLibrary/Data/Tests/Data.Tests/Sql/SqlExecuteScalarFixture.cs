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
    public class SqlExecuteScalarFixture
    {
        Database db;
        ExecuteScalarFixture baseFixture;
        [TestInitialize]
        public void SetUp()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(TestConfigurationSource.CreateConfigurationSource());
            db = factory.CreateDefault();
            DbCommand command = db.GetSqlStringCommand("Select count(*) from region");
            baseFixture = new ExecuteScalarFixture(db, command);
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
        public void CanExecuteReaderWithStoredProcInTransaction()
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (DbTransaction trans = connection.BeginTransaction())
                {
                    db.ExecuteScalar(trans, "Ten Most Expensive Products");
                    trans.Commit();
                }
            }
        }
        [TestMethod]
        [ExpectedException(typeof(SqlException))]
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
        [ExpectedException(typeof(ArgumentException))]
        public void ExecuteScalarWithNullIDbCommandAndNullTransaction()
        {
            baseFixture.ExecuteScalarWithNullIDbCommandAndNullTransaction();
        }
    }
}
