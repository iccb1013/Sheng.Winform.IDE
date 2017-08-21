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
    public class DataAccessTestsFixture
    {
        DbCommand command;
        DataSet dataSet;
        Database db;
        string sqlQuery = "SELECT * FROM Region";
        public DataAccessTestsFixture(Database db)
        {
            this.db = db;
            dataSet = new DataSet();
            command = db.GetSqlStringCommand(sqlQuery);
        }
        public void CanGetNonEmptyResultSet()
        {
            db.LoadDataSet(command, dataSet, "Foo");
            Assert.AreEqual(4, dataSet.Tables["Foo"].Rows.Count);
        }
        public void CanGetNonEmptyResultSetUsingTransaction()
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    db.LoadDataSet(command, dataSet, "Foo", transaction);
                    transaction.Commit();
                }
            }
            Assert.AreEqual(4, dataSet.Tables[0].Rows.Count);
        }
        public void CanGetNonEmptyResultSetUsingTransactionWithNullTableName()
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    db.LoadDataSet(command, dataSet, "Foo", transaction);
                    transaction.Commit();
                }
            }
            Assert.AreEqual(4, dataSet.Tables[0].Rows.Count);
        }
        public void CanGetTablePositionally()
        {
            db.LoadDataSet(command, dataSet, "Foo");
            Assert.AreEqual(4, dataSet.Tables[0].Rows.Count);
        }
        public void CannotLoadDataSetWithEmptyTableName()
        {
            db.LoadDataSet(command, dataSet, "");
            Assert.Fail("Cannot call LoadDataSet with empty SourceTable name");
        }
        public void ExecuteCommandNullCommand()
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    db.LoadDataSet(null, dataSet, "Foo", transaction);
                    transaction.Commit();
                }
            }
            Assert.AreEqual(4, dataSet.Tables[0].Rows.Count);
        }
        public void ExecuteCommandNullDataset()
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    db.LoadDataSet(command, null, "Foo", transaction);
                    transaction.Commit();
                }
            }
            Assert.AreEqual(4, dataSet.Tables[0].Rows.Count);
        }
        public void ExecuteCommandNullTableName()
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    db.LoadDataSet(command, dataSet, (string)null, transaction);
                    transaction.Commit();
                }
            }
            Assert.AreEqual(4, dataSet.Tables[0].Rows.Count);
        }
        public void ExecuteCommandNullTransaction()
        {
            db.LoadDataSet(command, dataSet, "Foo", null);
        }
        public void ExecuteDataSetWithCommand()
        {
            db.LoadDataSet(command, dataSet, "Foo");
            Assert.AreEqual(4, dataSet.Tables[0].Rows.Count);
        }
        public void ExecuteDataSetWithDbTransaction()
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    db.LoadDataSet(command, dataSet, "Foo", transaction);
                    transaction.Commit();
                }
            }
            Assert.AreEqual(4, dataSet.Tables[0].Rows.Count);
        }
        public void ExecuteNullCommand()
        {
            db.LoadDataSet(null, null, (string)null);
        }
    }
}
