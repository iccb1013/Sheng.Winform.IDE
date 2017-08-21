/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using Microsoft.Practices.EnterpriseLibrary.Data.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Tests
{
    [TestClass]
    public class OracleUpdateDataSetWithTransactionsAndParameterDiscovery : UpdateDataSetWithTransactionsAndParameterDiscovery
    {
        [TestInitialize]
        public void Initialize()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(TestConfigurationSource.CreateConfigurationSource());
            db = factory.Create("OracleTest");
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
        public void OracleModifyRowWithStoredProcedure()
        {
            base.ModifyRowWithStoredProcedure();
        }
        protected override void CreateStoredProcedures()
        {
            OracleDataSetHelper.CreateStoredProcedures(db);
        }
        protected override void DeleteStoredProcedures()
        {
            OracleDataSetHelper.DeleteStoredProcedures(db);
        }
        protected override void CreateDataAdapterCommands()
        {
            OracleDataSetHelper.CreateDataAdapterCommandsDynamically(db, ref insertCommand, ref updateCommand, ref deleteCommand);
        }
        protected override void AddTestData()
        {
            OracleDataSetHelper.AddTestData(db);
        }
    }
}
