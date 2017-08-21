/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Data.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests
{
    [TestClass]
    public class DatabaseFixture
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructDatabaseWithNullConnectionStringThrows()
        {
            DbProviderFactory factory = DbProviderFactories.GetFactory("System.Data.SqlClient");
            new TestDatabase(null, factory);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructDatabaseWithNullDbProviderFactoryThrows()
        {
            new TestDatabase("foo", null);
        }
        class TestDatabase : Database
        {
            public TestDatabase(string connectionString,
                                DbProviderFactory dbProviderFactory)
                : base(connectionString, dbProviderFactory) {}
            protected override void DeriveParameters(DbCommand discoveryCommand) {}
        }
    }
}
