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
    public class SqlParameterDiscoveryFixture
    {
        ParameterCache cache;
        Database db;
        DbConnection connection;
        ParameterDiscoveryFixture baseFixture;
        DbCommand storedProcedure;
        [TestInitialize]
        public void SetUp()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(TestConfigurationSource.CreateConfigurationSource());
            db = factory.CreateDefault();
            storedProcedure = db.GetStoredProcCommand("CustOrdersOrders");
            connection = db.CreateConnection();
            connection.Open();
            storedProcedure.Connection = connection;
            cache = new ParameterCache();
            baseFixture = new ParameterDiscoveryFixture(storedProcedure);
        }
        [TestMethod]
        public void CanGetParametersForStoredProcedure()
        {
            cache.SetParameters(storedProcedure, db);
            Assert.AreEqual(2, storedProcedure.Parameters.Count);
            Assert.AreEqual("@RETURN_VALUE", ((IDataParameter)storedProcedure.Parameters["@RETURN_VALUE"]).ParameterName);
            Assert.AreEqual("@CustomerID", ((IDataParameter)storedProcedure.Parameters["@CustomerId"]).ParameterName);
        }
        [TestMethod]
        public void IsCacheUsed()
        {
            ParameterDiscoveryFixture.TestCache testCache = new ParameterDiscoveryFixture.TestCache();
            testCache.SetParameters(storedProcedure, db);
            DbCommand storedProcDuplicate = db.GetStoredProcCommand("CustOrdersOrders");
            storedProcDuplicate.Connection = connection;
            testCache.SetParameters(storedProcDuplicate, db);
            Assert.IsTrue(testCache.CacheUsed, "Cache is not used");
        }
        [TestMethod]
        public void CanDiscoverFeaturesWhileInsideTransaction()
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                DbCommand storedProcedure = db.GetStoredProcCommand("CustOrdersOrders");
                storedProcedure.Connection = transaction.Connection;
                storedProcedure.Transaction = transaction;
                db.DiscoverParameters(storedProcedure);
                Assert.AreEqual(2, storedProcedure.Parameters.Count);
            }
        }
        [TestMethod]
        public void CanCreateStoredProcedureCommand()
        {
            baseFixture.CanCreateStoredProcedureCommand();
        }
        [TestMethod]
        public void CanDiscoverUnicodeParametersUsingSql()
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                DbCommand storedProcedure = db.GetStoredProcCommand("CustOrderHist");
                storedProcedure.Connection = transaction.Connection;
                storedProcedure.Transaction = transaction;
                db.DiscoverParameters(storedProcedure);
                Assert.AreEqual(2, storedProcedure.Parameters.Count);
                SqlParameter sqlParameter = storedProcedure.Parameters[1] as SqlParameter;
                Assert.IsNotNull(sqlParameter);
                Assert.AreEqual(SqlDbType.NChar, sqlParameter.SqlDbType);
            }
        }
    }
}
