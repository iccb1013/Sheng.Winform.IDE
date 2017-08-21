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
    [Ignore]
    public class OracleTransactionScopeFixture
    {
        TransactionScopeFixture baseFixture;
        Database db;
        [TestInitialize]
        public void SetUp()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(TestConfigurationSource.CreateConfigurationSource());
            db = factory.Create("OracleTest");
            try
            {
                DeleteStoredProcedures();
            }
            catch {}
            CreateStoredProcedures();
            baseFixture = new TransactionScopeFixture(db);
            baseFixture.Clenaup();
        }
        [TestCleanup]
        public void Clenaup()
        {
            try
            {
                baseFixture.Clenaup();
                DeleteStoredProcedures();
            }
            catch {}
        }
        [TestMethod]
        public void TransactionScope_ShouldDiscardChangesOnDispose()
        {
            baseFixture.TransactionScope_ShouldDiscardChangesOnDispose();
        }
        [TestMethod]
        public void Commit_ShouldKeepChanges()
        {
            baseFixture.Commit_ShouldKeepChanges();
        }
        [TestMethod]
        public void Comit_ShouldKeepInnerChangesForNestedTransaction()
        {
            baseFixture.Comit_ShouldKeepInnerChangesForNestedTransaction();
        }
        [TestMethod]
        public void Complete_ShouldDiscardInnerChangesWhenOuterNotCompleted()
        {
            baseFixture.Complete_ShouldDiscardInnerChangesWhenOuterNotCompleted();
        }
        [TestMethod]
        public void Insert_ShouldAddRowsWhenNoTransactionActive()
        {
            baseFixture.Insert_ShouldAddRowsWhenNoTransactionActive();
        }
        [TestMethod]
        public void ShouldAllowCommandsAfterInnerScopeDisposed()
        {
            baseFixture.ShouldAllowCommandsAfterInnerScopeDisposed();
        }
        [TestMethod]
        public void Rollback_ShouldDisposeTransactionConnection()
        {
            baseFixture.Rollback_ShouldDisposeTransactionConnection();
        }
        [TestMethod]
        public void ExecuteNonQueryWithTextCommand_ShouldUseTransaction()
        {
            baseFixture.ExecuteNonQueryWithTextCommand_ShouldUseTransaction();
        }
        [TestMethod]
        public void ExecuteNonQueryWithCommand_ShouldUseTransaction()
        {
            baseFixture.ExecuteNonQueryWithCommand_ShouldUseTransaction();
        }
        [TestMethod]
        public void ExecuteNonQueryWithStoredProcedure_ShouldUseTransaction()
        {
            baseFixture.ExecuteNonQueryWithStoredProcedure_ShouldUseTransaction();
        }
        [TestMethod]
        public void ExecuteScalarWithCommand_ShouldUseTransaction()
        {
            baseFixture.ExecuteScalarWithCommand_ShouldUseTransaction();
        }
        [TestMethod]
        public void ExecuteScalarWithCommandText_ShouldUseTransaction()
        {
            baseFixture.ExecuteScalarWithCommandText_ShouldUseTransaction();
        }
        [TestMethod]
        public void ExecuteScalarWithStoredProcedure_ShouldUseTransaction()
        {
            baseFixture.ExecuteScalarWithStoredProcedure_ShouldUseTransaction();
        }
        [TestMethod]
        public void ExecuteDataSetWithCommandText_ShouldRetriveDataSet()
        {
            baseFixture.ExecuteDataSetWithCommandText_ShouldRetriveDataSet();
        }
        [TestMethod]
        public void ExecuteDataSetWithCommand_ShouldRetriveDataSet()
        {
            baseFixture.ExecuteDataSetWithCommand_ShouldRetriveDataSet();
        }
        [TestMethod]
        public void ExecuteReaderWithCommandText_ShouldRetrieveDataInTransaction()
        {
            baseFixture.ExecuteReaderWithCommandText_ShouldRetrieveDataInTransaction();
        }
        [TestMethod]
        public void ExecuteReaderWithCommand_ShouldRetrieveDataInTransaction()
        {
            baseFixture.ExecuteReaderWithCommand_ShouldRetrieveDataInTransaction();
        }
        [TestMethod]
        public void LoadDataSetWithCommandText_LoadsDataInTransaction()
        {
            baseFixture.LoadDataSetWithCommandText_LoadsDataInTransaction();
        }
        [TestMethod]
        public void LoadDataSetWithCommand_LoadsDataInTransaction()
        {
            baseFixture.LoadDataSetWithCommand_LoadsDataInTransaction();
        }
        [TestMethod]
        public void UpdateDataSet_ShouldAddToTransaction()
        {
            baseFixture.UpdateDataSet_ShouldAddToTransaction();
        }
        [TestMethod]
        public void UpdateDataSetWithUpdateBlockSize_ShouldAddToTransaction()
        {
            baseFixture.UpdateDataSetWithUpdateBlockSize_ShouldAddToTransaction();
        }
        void CreateStoredProcedures()
        {
            OracleDataSetHelper.CreateStoredProcedures(db);
        }
        void DeleteStoredProcedures()
        {
            OracleDataSetHelper.DeleteStoredProcedures(db);
        }
    }
}
