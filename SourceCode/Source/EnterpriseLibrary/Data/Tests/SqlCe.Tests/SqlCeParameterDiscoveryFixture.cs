/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.Data.SqlCe;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Data.SqlCe.Tests.VSTS
{
    [TestClass]
    public class SqlCeParameterDiscoveryFixture
    {
        [TestCleanup]
        public void TearDown()
        {
            SqlCeConnectionPool.CloseSharedConnections();
        }
        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void CannotGetCommandForStoredProcedure()
        {
            TestConnectionString testConnection = new TestConnectionString();
            testConnection.CopyFile();
            SqlCeDatabase db = new SqlCeDatabase(testConnection.ConnectionString);
            db.GetStoredProcCommand("CustOrdersOrders");
        }
    }
}
