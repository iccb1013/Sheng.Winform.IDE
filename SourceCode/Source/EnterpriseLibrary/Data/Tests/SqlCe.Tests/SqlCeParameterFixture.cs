/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Data;
using System.Data.Common;
using Data.SqlCe.Tests.VSTS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Data.SqlCe.Tests
{
    [TestClass]
    public class SqlCeParameterFixture
    {
        [TestMethod]
        public void CanInsertNullStringParameter()
        {
            Database db = null;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(TestConfigurationSource.CreateConfigurationSource());
            db = factory.CreateDefault();
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    string sqlString = "insert into Customers (CustomerID, CompanyName, ContactName) Values (@id, @name, @contact)";
                    DbCommand insert = db.GetSqlStringCommand(sqlString);
                    db.AddInParameter(insert, "@id", DbType.Int32, 1);
                    db.AddInParameter(insert, "@name", DbType.String, "fee");
                    db.AddInParameter(insert, "@contact", DbType.String, null);
                    db.ExecuteNonQuery(insert, transaction);
                    transaction.Rollback();
                }
            }
        }
    }
}
