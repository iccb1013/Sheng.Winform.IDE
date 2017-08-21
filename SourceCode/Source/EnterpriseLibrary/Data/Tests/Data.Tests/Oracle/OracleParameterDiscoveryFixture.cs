/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Tests
{
    [TestClass]
    public class OracleParameterDiscoveryFixture
    {
        ParameterCache cache;
        Database db;
        DbConnection connection;
        ParameterDiscoveryFixture baseFixture;
        DbCommand storedProcedure;
        [TestInitialize]
        public void TestInitialize()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(TestConfigurationSource.CreateConfigurationSource());
            db = factory.Create("OracleTest");
            storedProcedure = db.GetStoredProcCommand("CustOrdersOrders");
            connection = db.CreateConnection();
            connection.Open();
            storedProcedure.Connection = connection;
            cache = new ParameterCache();
            baseFixture = new ParameterDiscoveryFixture(storedProcedure);
        }
        [TestCleanup]
        public void TestCleanup()
        {
            if (connection != null)
            {
                connection.Dispose();
            }
        }
        [TestMethod]
        public void CanGetParametersForStoredProcedure()
        {
            cache.SetParameters(storedProcedure, db);
            Assert.AreEqual(2, storedProcedure.Parameters.Count);
            Assert.AreEqual("CUR_OUT", ((IDataParameter)storedProcedure.Parameters["CUR_OUT"]).ParameterName);
            Assert.AreEqual("VCUSTOMERID", ((IDataParameter)storedProcedure.Parameters["VCUSTOMERID"]).ParameterName);
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
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetParametersWithNullCommandThrows()
        {
            cache.SetParameters(null, db);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetParametersWithNullDatabaseThrows()
        {
            cache.SetParameters(storedProcedure, null);
        }
    }
}
