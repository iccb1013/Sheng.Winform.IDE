/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Data;
using System.Data.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests
{
    public class ParameterDiscoveryFixture
    {
        DbCommand storedProcedure;
        public ParameterDiscoveryFixture(DbCommand storedProcedure)
        {
            this.storedProcedure = storedProcedure;
        }
        public void CanCreateStoredProcedureCommand()
        {
            Assert.AreEqual(storedProcedure.CommandType, CommandType.StoredProcedure);
        }
        public class TestCache : ParameterCache
        {
            public bool CacheUsed = false;
            protected override void AddParametersFromCache(DbCommand command,
                                                           Database database)
            {
                CacheUsed = true;
                base.AddParametersFromCache(command, database);
            }
        }
    }
}
