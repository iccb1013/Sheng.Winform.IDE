/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Data.Common;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests
{
    public class RollbackTransactionWrapper : IDisposable
    {
        private DbTransaction transaction;
        public RollbackTransactionWrapper(DbTransaction transaction)
        {
            this.transaction = transaction;
        }
        public void Dispose()
        {
            transaction.Rollback();
        }
        public DbTransaction Transaction
        {
            get { return transaction; }
        }
    }
}
