/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using Microsoft.Practices.EnterpriseLibrary.Data.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Sql.Tests
{
    [TestClass]
    public class SqlUpdateDataSetWithTransactionsAndParameterDiscovery : UpdateDataSetWithTransactionsAndParameterDiscovery
    {
        [TestInitialize]
        public void Initialize()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(TestConfigurationSource.CreateConfigurationSource());
            db = factory.CreateDefault();
            try
            {
                DeleteStoredProcedures();
            }
            catch {}
            CreateStoredProcedures();
            base.SetUp();
        }
        [TestCleanup]
        public void Dispose()
        {
            base.TearDown();
            DeleteStoredProcedures();
        }
        [TestMethod]
        public void SqlModifyRowWithStoredProcedure()
        {
            base.ModifyRowWithStoredProcedure();
        }
        protected override void CreateStoredProcedures()
        {
            SqlDataSetHelper.CreateStoredProcedures(db);
        }
        protected override void DeleteStoredProcedures()
        {
            SqlDataSetHelper.DeleteStoredProcedures(db);
        }
        protected override void CreateDataAdapterCommands()
        {
            SqlDataSetHelper.CreateDataAdapterCommandsDynamically(db, ref insertCommand, ref updateCommand, ref deleteCommand);
        }
        protected override void AddTestData()
        {
            SqlDataSetHelper.AddTestData(db);
        }
    }
}
