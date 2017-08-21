/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Data;
using System.Data.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests.Sql
{
    [TestClass]
    public class SqlExecuteDatasetFixture
    {
        const string queryString = "Select * from Region";
        const string storedProc = "Ten Most Expensive Products";
        Database db;
        [TestInitialize]
        public void TestInitialize()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(TestConfigurationSource.CreateConfigurationSource());
            db = factory.CreateDefault();
        }
        [TestMethod]
        public void CanRetriveDataSetFromSqlString()
        {
            DataSet dataSet = db.ExecuteDataSet(CommandType.Text, queryString);
            Assert.AreEqual(1, dataSet.Tables.Count);
        }
        [TestMethod]
        public void CanRetiveDataSetFromStoredProcedure()
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
        public void CanRetiveDataSetFromStoredProcedureWithTransaction()
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
