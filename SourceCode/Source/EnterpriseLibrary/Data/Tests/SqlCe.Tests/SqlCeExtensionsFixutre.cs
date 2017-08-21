/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data.SqlCe;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Data.SqlCe.Tests.VSTS
{
    [TestClass]
    public class SqlCeExtensionsFixutre
    {
        TestConnectionString testConnection;
        SqlCeDatabase db;
        [TestInitialize]
        public void SetUp()
        {
            testConnection = new TestConnectionString();
            testConnection.CopyFile();
            db = new SqlCeDatabase(testConnection.ConnectionString);
        }
        [TestCleanup]
        public void DeleteDb()
        {
            SqlCeConnectionPool.CloseSharedConnections();
            testConnection.DeleteFile();
        }
        [TestMethod]
        public void TypicalExecuteScalarWithParameters()
        {
            string sql = "select count(*) from region where regionId=" + db.BuildParameterName("regionId");
            using (DbCommand command = db.GetSqlStringCommand(sql))
            {
                db.AddInParameter(command, "regionId", DbType.Int32, 2);
                int result = (int)db.ExecuteScalar(command);
                Assert.AreEqual(1, result);
            }
        }
        [TestMethod]
        public void SimplifiedExecuteScalarWithParameters()
        {
            string sql = "select count(*) from region where regionId=" + db.BuildParameterName("regionId");
            int result = (int)db.ExecuteScalarSql(sql, db.CreateParameter("regionId", DbType.Int32, 0, 2));
            Assert.AreEqual(1, result);
        }
        [TestMethod]
        public void TypicalSqlStatementWithParameters()
        {
            string sql = "insert into region values ({0}, {1})";
            sql = String.Format(sql, db.BuildParameterName("regionId"), db.BuildParameterName("description"));
            using (DbCommand command = db.GetSqlStringCommand(sql))
            {
                db.AddInParameter(command, "regionId", DbType.Int32, 99);
                db.AddInParameter(command, "description", DbType.String, "test value");
                db.ExecuteNonQuery(command);
            }
        }
        [TestMethod]
        public void AlternateSqlStatementWithParameters()
        {
            string sql = "insert into region values ({0}, {1})";
            sql = String.Format(sql, db.BuildParameterName("regionId"), db.BuildParameterName("description"));
            DbParameter[] parameters = new DbParameter[]
                {
                    db.CreateParameter("regionId", DbType.Int32, 0, 99),
                    db.CreateParameter("description", DbType.String, 20, "test value")
                };
            db.ExecuteNonQuerySql(sql, parameters);
        }
        [TestMethod]
        public void CreateParameter_ShouldSetTypeAndValue()
        {
            DbParameter parameter = db.CreateParameter("testParam", DbType.String, 20, "test");
            Assert.IsNotNull(parameter);
            Assert.AreEqual(DbType.String, parameter.DbType);
            Assert.AreEqual(20, parameter.Size);
            Assert.AreEqual("test", (string)parameter.Value);
        }
        [TestMethod]
        public void CanGetLastId()
        {
            string sql = "create table testIdentity (testid int identity, dataValue int)";
            db.ExecuteNonQuerySql(sql);
            sql = "insert into testIdentity (dataValue) values (23)";
            int lastId;
            int result = db.ExecuteNonQuerySql(sql, out lastId);
            Assert.AreEqual(1, result);
            Assert.AreEqual(1, lastId);
            db.ExecuteNonQuerySql(sql, out lastId);
            Assert.AreEqual(2, lastId);
        }
        [TestMethod]
        public void GettingIdOfInsertedRowReturnsNegativeOneWhenNoIdentityColumn()
        {
            string sql = "insert into region values (99, 23)";
            int lastId;
            int result = db.ExecuteNonQuerySql(sql, out lastId);
            Assert.AreEqual(1, result);
            Assert.AreEqual(-1, lastId);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExecuteNonQuerySql_ShouldThrowForNullString()
        {
            db.ExecuteNonQuerySql(null);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExecuteNonQuerySql_ShouldThrowForEmptyString()
        {
            db.ExecuteNonQuerySql(String.Empty);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExecuteNonQuerySqlWithId_ShouldThrowForNullString()
        {
            int id;
            db.ExecuteNonQuerySql(null, out id);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExecuteNonQuerySqlWithId_ShouldThrowForEmptyString()
        {
            int id;
            db.ExecuteNonQuerySql(String.Empty, out id);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExecuteReaderSql_ShouldThrowForNullString()
        {
            db.ExecuteReaderSql(null);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExecuteReaderSql_ShouldThrowForEmptyString()
        {
            db.ExecuteReaderSql(String.Empty);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExecuteScalarSql_ShouldThrowForNullString()
        {
            db.ExecuteScalarSql(null);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExecuteScalarSql_ShouldThrowForEmptyString()
        {
            db.ExecuteScalarSql(String.Empty);
        }
        [TestMethod]
        public void TableExists_ShouldFindTable()
        {
            Assert.IsTrue(db.TableExists("region"));
            Assert.IsFalse(db.TableExists("junk"));
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TableExists_ShouldThrowForNullName()
        {
            db.TableExists(null);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TableExists_ShouldThrowForEmptyName()
        {
            db.TableExists(String.Empty);
        }
        [TestMethod]
        public void ExecuteDataSetSql_ShouldReturnOneTable()
        {
            DataSet ds = db.ExecuteDataSetSql("select * from region");
            Assert.IsNotNull(ds);
            Assert.AreEqual(1, ds.Tables.Count);
            DataTable table = ds.Tables[0];
            Assert.AreEqual(4, table.Rows.Count);
        }
        [TestMethod]
        public void ExecuteDataSetSqlWithParameters_ShouldReturnOneRow()
        {
            string sql = "select * from region where regionid=" + db.BuildParameterName("regionId");
            DataSet ds = db.ExecuteDataSetSql(sql, db.CreateParameter("regionId", DbType.Int32, 0, 1));
            Assert.IsNotNull(ds);
            Assert.AreEqual(1, ds.Tables.Count);
            DataTable table = ds.Tables[0];
            Assert.AreEqual(1, table.Rows.Count);
        }
    }
}
