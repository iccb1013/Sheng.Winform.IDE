/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests
{
	public abstract class StoredProcedureCreationBase
	{
		protected Database db;
		protected StoredProcedureCreatingFixture baseFixture;
		protected abstract void CreateStoredProcedure();
		protected abstract void DeleteStoredProcedure();
		protected void CompleteSetup(Database db)
		{
			this.db = db;
			baseFixture = new StoredProcedureCreatingFixture(db);
			Database.ClearParameterCache();
			CreateStoredProcedure();
		}
		protected void Cleanup()
		{
			DeleteStoredProcedure();
			Database.ClearParameterCache();
		}
	}
}
