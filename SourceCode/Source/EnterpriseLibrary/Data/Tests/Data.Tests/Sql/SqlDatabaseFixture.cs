/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Sql.Tests
{
    [TestClass]
    public class SqlDatabaseFixture
    {
        [TestMethod]
        public void ConnectionTest()
        {
            DatabaseConfigurationView view = new DatabaseConfigurationView(TestConfigurationSource.CreateConfigurationSource());
            ConnectionStringSettings data = view.GetConnectionStringSettings("NewDatabase");
            SqlDatabase sqlDatabase = new SqlDatabase(data.ConnectionString);
            DbConnection connection = sqlDatabase.CreateConnection();
            Assert.IsNotNull(connection);
            Assert.IsTrue(connection is SqlConnection);
            connection.Open();
            DbCommand cmd = sqlDatabase.GetSqlStringCommand("Select * from Region");
            cmd.CommandTimeout = 60;
            Assert.AreEqual(cmd.CommandTimeout, 60);
        }
        [TestMethod]
        public void CanGetConnectionWithoutCredentials()
        {
            DatabaseConfigurationView view = new DatabaseConfigurationView(TestConfigurationSource.CreateConfigurationSource());
            ConnectionStringSettings data = view.GetConnectionStringSettings("DbWithSqlServerAuthn");
            SqlDatabase sqlDatabase = new SqlDatabase(data.ConnectionString);
            Assert.AreEqual(@"server=(local)\sqlexpress;database=northwind;", sqlDatabase.ConnectionStringWithoutCredentials);
        }
        [TestMethod]
        public void CanGetConnectionForStringWithNoCredentials()
        {
            DatabaseConfigurationView view = new DatabaseConfigurationView(TestConfigurationSource.CreateConfigurationSource());
            ConnectionStringSettings data = view.GetConnectionStringSettings("NewDatabase");
            SqlDatabase sqlDatabase = new SqlDatabase(data.ConnectionString);
            Assert.AreEqual(@"server=(local)\sqlexpress;database=northwind;integrated security=true;", sqlDatabase.ConnectionStringWithoutCredentials);
        }
        [TestMethod]
        public void CheckNoPasswordInConnectionStringWithPersistInfoEqualsFalse()
        {
            try
            {
                CreateUser();
                DatabaseConfigurationView view = new DatabaseConfigurationView(TestConfigurationSource.CreateConfigurationSource());
                ConnectionStringSettings data = view.GetConnectionStringSettings("NorthwindPersistFalse");
                SqlDatabase sqlDatabase = new SqlDatabase(data.ConnectionString);
                DbConnection dbConnection = sqlDatabase.CreateConnection();
                dbConnection.Open();
                dbConnection.Close();
                string connectionString = dbConnection.ConnectionString;
                if (connectionString.ToLower().Contains("pwd") || connectionString.ToLower().Contains("password"))
                {
                    Assert.Fail();
                }
            }
            finally
            {
                DeleteUser();
            }
        }
        void CreateUser()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(TestConfigurationSource.CreateConfigurationSource());
            Database adminDb = factory.CreateDefault();
            using (DbConnection connection = adminDb.CreateConnection())
            {
                connection.Open();
                string query;
                DbCommand addUser;
                try
                {
                    query = string.Format("exec sp_addlogin '{0}', '{1}', 'Northwind'", TestConfigurationSource.NorthwindDummyUser, TestConfigurationSource.NorthwindDummyPassword);
                    addUser = adminDb.GetSqlStringCommand(query);
                    adminDb.ExecuteNonQuery(addUser);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                try
                {
                    query = string.Format("exec sp_grantdbaccess '{0}', '{0}'", TestConfigurationSource.NorthwindDummyUser);
                    addUser = adminDb.GetSqlStringCommand(query);
                    adminDb.ExecuteNonQuery(addUser);
                }
                catch {}
                try
                {
                    query = string.Format("exec sp_addrolemember N'db_owner', N'{0}'", TestConfigurationSource.NorthwindDummyUser);
                    addUser = adminDb.GetSqlStringCommand(query);
                    adminDb.ExecuteNonQuery(addUser);
                }
                catch {}
            }
        }
        void DeleteUser()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(TestConfigurationSource.CreateConfigurationSource());
            Database adminDb = factory.CreateDefault();
            using (DbConnection connection = adminDb.CreateConnection())
            {
                connection.Open();
                string query;
                DbCommand dropUser;
                try
                {
                    query = string.Format("exec sp_revokedbaccess '{0}'", TestConfigurationSource.NorthwindDummyUser);
                    dropUser = adminDb.GetSqlStringCommand(query);
                    adminDb.ExecuteNonQuery(dropUser);
                }
                catch {}
                try
                {
                    query = string.Format("exec sp_droplogin '{0}'", TestConfigurationSource.NorthwindDummyUser);
                    dropUser = adminDb.GetSqlStringCommand(query);
                    adminDb.ExecuteNonQuery(dropUser);
                }
                catch {}
            }
        }
        [TestMethod]
        public void CheckNoPasswordWithPersistInfoEqualsFalseForDynamicConnectionString()
        {
            try
            {
                CreateUser();
                ConnectionString testString =
                    new ConnectionString(@"server=(local)\SQLEXPRESS;database=Northwind;uid=entlib;pwd=hdf7&834k(*KA;Persist Security Info=false", "UserId,UId", "Password,Pwd");
                SqlDatabase sqlDatabase = new SqlDatabase(testString.ToString());
                DbConnection dbConnection = sqlDatabase.CreateConnection();
                dbConnection.Open();
                dbConnection.Close();
                string connectionString = dbConnection.ConnectionString;
                if (connectionString.ToLower().Contains("pwd") || connectionString.ToLower().Contains("password"))
                {
                    Assert.Fail();
                }
            }
            finally
            {
                DeleteUser();
            }
        }
    }
}
