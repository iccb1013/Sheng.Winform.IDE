/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests.Instrumentation
{
    [TestClass]
    public class InstrumentationNoListenerEventBroadcastFixture
    {
        [TestMethod]
        public void NoEventBroadcastIfNoEventRegistered()
        {
            string connectionString = @"server=(local)\sqlexpress;database=northwind;integrated security=true;";
            SqlDatabase db = new SqlDatabase(connectionString);
            db.ExecuteNonQuery(CommandType.Text, "Select count(*) from Region");
        }
        [TestMethod]
        public void NoConnectionFailedEventBroadcastWithNoListener()
        {
            string connectionString = @"null;";
            SqlDatabase db = new SqlDatabase(connectionString);
            try
            {
                db.ExecuteNonQuery(CommandType.Text, "Select count(*) from Region");
            }
            catch (ArgumentException) {}
        }
    }
}
