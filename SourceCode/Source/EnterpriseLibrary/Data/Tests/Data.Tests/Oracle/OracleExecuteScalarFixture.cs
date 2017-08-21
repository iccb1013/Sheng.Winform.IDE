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
namespace Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Tests
{
    [TestClass]
    public class OracleExecuteScalarFixture
    {
        static Database db;
        static ExecuteScalarFixture baseFixture;
        [TestInitialize]
        public void SetUp()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(TestConfigurationSource.CreateConfigurationSource());
            db = factory.Create("OracleTest");
            DbCommand command = db.GetSqlStringCommand("Select count(*) from region");
            baseFixture = new ExecuteScalarFixture(db, command);
        }
        [TestMethod]
        public void ExecuteScalarWithIDbCommand()
        {
            baseFixture.ExecuteScalarWithIDbCommand();
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
        [TestMethod]
        public void ExecuteScalarWithCommandTextAndTypeInTransaction()
        {
            baseFixture.ExecuteScalarWithCommandTextAndTypeInTransaction();
        }
    }
}
