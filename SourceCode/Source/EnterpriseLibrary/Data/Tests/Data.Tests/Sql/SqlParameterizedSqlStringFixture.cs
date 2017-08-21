/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Sql.Tests
{
    [TestClass]
    public class SqlParameterizedSqlStringFixture
    {
        Database db;
        [TestInitialize]
        public void SetUp()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(TestConfigurationSource.CreateConfigurationSource());
            db = factory.CreateDefault();
        }
        [TestMethod]
        public void ExecuteSqlStringCommandWithParameters()
        {
            string sql = "select * from Region where (RegionID=@Param1) and RegionDescription=@Param2";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "@Param1", DbType.Int32, 1);
            db.AddInParameter(cmd, "@Param2", DbType.String, "Eastern");
            DataSet ds = db.ExecuteDataSet(cmd);
            Assert.AreEqual(1, ds.Tables[0].Rows.Count);
        }
        [TestMethod]
        [ExpectedException(typeof(SqlException))]
        public void ExecuteSqlStringCommandWithNotEnoughParameterValues()
        {
            string sql = "select * from Region where RegionID=@Param1 and RegionDescription=@Param2";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "@Param1", DbType.Int32, 1);
            db.ExecuteDataSet(cmd);
        }
        [TestMethod]
        [ExpectedException(typeof(SqlException))]
        public void ExecuteSqlStringCommandWithTooManyParameterValues()
        {
            string sql = "select * from Region where RegionID=@Param1 and RegionDescription=@Param2";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "@Param1", DbType.Int32, 1);
            db.AddInParameter(cmd, "@Param2", DbType.String, "Eastern");
            db.AddInParameter(cmd, "@Param2", DbType.String, "Western");
            db.ExecuteDataSet(cmd);
        }
        [TestMethod]
        public void ExecuteSqlStringCommandWithDoubleAtSymbol()
        {
            string sql = "select @@SERVERNAME, * from Region where RegionID=@ID and @@MAX_CONNECTIONS>1 and RegionDescription=@Desc";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "@ID", DbType.Int32, 1);
            db.AddInParameter(cmd, "@Desc", DbType.String, "Eastern");
            DataSet ds = db.ExecuteDataSet(cmd);
            Assert.AreEqual(1, ds.Tables[0].Rows.Count);
        }
        [TestMethod]
        public void ExecuteSqlStringWithoutParametersButWithValues()
        {
            string sql = "select * from Region";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "@Param1", DbType.Int32, 1);
            db.AddInParameter(cmd, "@Param2", DbType.String, "Eastern");
            DataSet ds = db.ExecuteDataSet(cmd);
            Assert.AreEqual(4, ds.Tables[0].Rows.Count);
        }
    }
}
