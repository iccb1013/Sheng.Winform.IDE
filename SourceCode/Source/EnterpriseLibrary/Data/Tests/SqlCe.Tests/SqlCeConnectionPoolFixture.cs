/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Data;
using System.Data.Common;
using System.Data.SqlServerCe;
using Microsoft.Practices.EnterpriseLibrary.Data.SqlCe;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Data.SqlCe.Tests.VSTS
{
    [TestClass]
    public class SqlCeConnectionPoolFixture
    {
        [TestInitialize]
        public void CopyDatabase()
        {
            TestConnectionString testConnection = new TestConnectionString();
            testConnection.CopyFile();
        }
        [TestCleanup]
        public void CloseConnections()
        {
            SqlCeConnectionPool.CloseSharedConnections();
            TestConnectionString testConnection = new TestConnectionString();
            testConnection.DeleteFile();
        }
        [TestMethod]
        public void PoolIsInitiallyEmpty()
        {
            Assert.AreEqual(0, TestableSqlCeConnectionPool.PoolSize);
        }
        [TestMethod]
        public void GetConnectionAddsConnectionToPool()
        {
            TestConnectionString testConnection = new TestConnectionString();
            SqlCeDatabase db = new SqlCeDatabase(testConnection.ConnectionString);
            using (DbConnection connection = SqlCeConnectionPool.CreateConnection(db))
            {
                Assert.IsNotNull(connection);
                Assert.AreEqual(1, TestableSqlCeConnectionPool.PoolSize);
            }
        }
        [TestMethod]
        public void GetConnectionOpensConnectionInPool()
        {
            TestConnectionString testConnection = new TestConnectionString();
            SqlCeDatabase db = new SqlCeDatabase(testConnection.ConnectionString);
            using (DbConnection connection = SqlCeConnectionPool.CreateConnection(db))
            {
                DbConnection keepAlive = TestableSqlCeConnectionPool.GetConnection(0);
                Assert.AreEqual(ConnectionState.Open, keepAlive.State);
            }
        }
        [TestMethod]
        public void CloseSharedConnectionsShouldClearPool()
        {
            TestConnectionString testConnection = new TestConnectionString();
            SqlCeDatabase db = new SqlCeDatabase(testConnection.ConnectionString);
            using (DbConnection connection = SqlCeConnectionPool.CreateConnection(db)) {}
            Assert.AreEqual(1, TestableSqlCeConnectionPool.PoolSize);
            SqlCeConnectionPool.CloseSharedConnections();
            Assert.AreEqual(0, TestableSqlCeConnectionPool.PoolSize);
        }
        [TestMethod]
        public void ConnectionShouldRemainInPoolAfterReturnedConnectionDisposed()
        {
            TestConnectionString testConnection = new TestConnectionString();
            SqlCeDatabase db = new SqlCeDatabase(testConnection.ConnectionString);
            using (DbConnection connection = SqlCeConnectionPool.CreateConnection(db)) {}
            DbConnection keepAlive = TestableSqlCeConnectionPool.GetConnection(0);
            Assert.AreEqual(ConnectionState.Open, keepAlive.State);
        }
        [TestMethod]
        public void GetConnectionReturnsDifferentConnectionThanOneInPool()
        {
            TestConnectionString testConnection = new TestConnectionString();
            SqlCeDatabase db = new SqlCeDatabase(testConnection.ConnectionString);
            using (DbConnection connection = SqlCeConnectionPool.CreateConnection(db))
            {
                DbConnection keepAlive = TestableSqlCeConnectionPool.GetConnection(0);
                Assert.AreNotSame(connection, keepAlive);
            }
        }
        [TestMethod]
        public void GetConnectionReturnsNewConnectionButHasOnlyOneInPool()
        {
            TestConnectionString testConnection = new TestConnectionString();
            SqlCeDatabase db = new SqlCeDatabase(testConnection.ConnectionString);
            using (DbConnection connection1 = SqlCeConnectionPool.CreateConnection(db))
            {
                using (DbConnection connection2 = SqlCeConnectionPool.CreateConnection(db))
                {
                    Assert.AreNotSame(connection1, connection2);
                    Assert.AreEqual(1, TestableSqlCeConnectionPool.PoolSize);
                }
            }
        }
        [TestMethod]
        public void GetConnectionForTwoFilesAddsTwoConnectionsToPool()
        {
            TestConnectionString file1 = new TestConnectionString();
            TestConnectionString file2 = new TestConnectionString("test2.sdf");
            file2.CopyFile();
            SqlCeDatabase db1 = new SqlCeDatabase(file1.ConnectionString);
            SqlCeDatabase db2 = new SqlCeDatabase(file2.ConnectionString);
            using (DbConnection connection1 = SqlCeConnectionPool.CreateConnection(db1))
            {
                using (DbConnection connection2 = SqlCeConnectionPool.CreateConnection(db2)) {}
            }
            Assert.AreEqual(2, TestableSqlCeConnectionPool.PoolSize);
            SqlCeConnectionPool.CloseSharedConnections();
            file2.DeleteFile();
        }
        [TestMethod]
        public void ExecuteSqlCreatesConnectionInPool()
        {
            TestConnectionString testConnection = new TestConnectionString();
            SqlCeDatabase db = new SqlCeDatabase(testConnection.ConnectionString);
            DbCommand command = db.GetSqlStringCommand("select count(*) from region");
            Assert.AreEqual(0, TestableSqlCeConnectionPool.PoolSize);
            int result = (int)db.ExecuteScalar(command);
            Assert.AreEqual(4, result);
            Assert.AreEqual(1, TestableSqlCeConnectionPool.PoolSize);
        }
        [TestMethod]
        public void ShouldNotAddConnectionToPoolIfOpenFails()
        {
            SqlCeDatabase db = new SqlCeDatabase("Data Source='junk.sdf'");
            try
            {
                DbConnection connection = SqlCeConnectionPool.CreateConnection(db);
            }
            catch (SqlCeException) {}
            Assert.AreEqual(0, TestableSqlCeConnectionPool.PoolSize);
        }
        [TestMethod]
        public void PoolShouldCloseSharedConnection()
        {
            TestConnectionString testConnection = new TestConnectionString();
            SqlCeDatabase db = new SqlCeDatabase(testConnection.ConnectionString);
            using (DbConnection connection = SqlCeConnectionPool.CreateConnection(db)) {}
            Assert.AreEqual(1, TestableSqlCeConnectionPool.PoolSize);
            SqlCeConnectionPool.CloseSharedConnection(db);
            Assert.AreEqual(0, TestableSqlCeConnectionPool.PoolSize);
        }
        [TestMethod]
        public void DatabaseShouldCloseSharedConnection()
        {
            TestConnectionString file = new TestConnectionString();
            SqlCeDatabase database = new SqlCeDatabase(file.ConnectionString);
            using (DbConnection connection = SqlCeConnectionPool.CreateConnection(database)) {}
            Assert.AreEqual(1, TestableSqlCeConnectionPool.PoolSize);
            database.CloseSharedConnection();
            Assert.AreEqual(0, TestableSqlCeConnectionPool.PoolSize);
        }
        class TestableSqlCeConnectionPool : SqlCeConnectionPool
        {
            public static int PoolSize
            {
                get { return connections.Count; }
            }
            public static DbConnection GetConnection(int index)
            {
                int i = 0;
                foreach (DbConnection connection in connections.Values)
                {
                    if (i == index)
                        return connection;
                    i++;
                }
                return null;
            }
        }
    }
}
