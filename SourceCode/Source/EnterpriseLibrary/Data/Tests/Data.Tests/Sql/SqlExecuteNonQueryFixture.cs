/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Sql.Tests
{
    [TestClass]
    public class SqlExecuteNonQueryFixture
    {
        ExecuteNonQueryFixture baseFixture;
        Database db;
        const string insertString = "insert into Region values (77, 'Elbonia')";
        const string countQuery = "select count(*) from Region";
        [TestInitialize]
        public void SetUp()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(TestConfigurationSource.CreateConfigurationSource());
            db = factory.CreateDefault();
            DbCommand insertionCommand = db.GetSqlStringCommand(insertString);
            DbCommand countCommand = db.GetSqlStringCommand(countQuery);
            baseFixture = new ExecuteNonQueryFixture(db, insertString, countQuery, insertionCommand, countCommand);
        }
        [TestMethod]
        public void CanExecuteNonQueryWithCommandTextWithDefinedTypeAndTransaction()
        {
            baseFixture.CanExecuteNonQueryWithCommandTextWithDefinedTypeAndTransaction();
        }
        [TestMethod]
        public void CanExecuteNonQueryWithStoredProc()
        {
            db.ExecuteNonQuery("Ten Most Expensive Products");
        }
        [TestMethod]
        public void CanExecuteNonQueryWithStoredProcInTransaction()
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (DbTransaction trans = connection.BeginTransaction())
                {
                    db.ExecuteNonQuery(trans, "Ten Most Expensive Products");
                    trans.Commit();
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
        [ExpectedException(typeof(ArgumentException))]
        public void ExecuteNonQueryWithNullDbCommandAndTransaction()
        {
            baseFixture.ExecuteNonQueryWithNullDbCommandAndTransaction();
        }
    }
}
