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
    public class ExecuteReaderFixture
    {
        Database db;
        DbCommand insertCommand;
        string insertString;
        DbCommand queryCommand;
        string queryString;
        public ExecuteReaderFixture(Database db,
                                    string insertString,
                                    DbCommand insertCommand,
                                    string queryString,
                                    DbCommand queryCommand)
        {
            this.db = db;
            this.insertString = insertString;
            this.queryString = queryString;
            this.insertCommand = insertCommand;
            this.queryCommand = queryCommand;
        }
        public void CanExecuteQueryThroughDataReaderUsingTransaction()
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (RollbackTransactionWrapper transaction = new RollbackTransactionWrapper(connection.BeginTransaction()))
                {
                    using (IDataReader reader = db.ExecuteReader(insertCommand, transaction.Transaction))
                    {
                        Assert.AreEqual(1, reader.RecordsAffected);
                        reader.Close();
                    }
                }
                Assert.AreEqual(ConnectionState.Open, connection.State);
            }
        }
        public void CanExecuteReaderFromDbCommand()
        {
            IDataReader reader = db.ExecuteReader(queryCommand);
            string accumulator = "";
            while (reader.Read())
            {
                accumulator += ((string)reader["RegionDescription"]).Trim();
            }
            reader.Close();
            Assert.AreEqual("EasternWesternNorthernSouthern", accumulator);
            Assert.AreEqual(ConnectionState.Closed, queryCommand.Connection.State);
        }
        public void CanExecuteReaderWithCommandText()
        {
            IDataReader reader = db.ExecuteReader(CommandType.Text, queryString);
            string accumulator = "";
            while (reader.Read())
            {
                accumulator += ((string)reader["RegionDescription"]).Trim();
            }
            reader.Close();
            Assert.AreEqual("EasternWesternNorthernSouthern", accumulator);
        }
        public void EmptyQueryStringTest()
        {
            using (DbCommand myCommand = db.GetSqlStringCommand(string.Empty))
            {
                IDataReader reader = db.ExecuteReader(myCommand);
            }
        }
        public void ExecuteQueryThroughDataReaderUsingNullCommandAndNullTransactionThrows()
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (IDataReader reader = db.ExecuteReader(null, (string)null)) {}
            }
        }
        public void ExecuteQueryThroughDataReaderUsingNullCommandThrows()
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                insertCommand = null;
                using (IDataReader reader = db.ExecuteReader(insertCommand, null)) {}
            }
        }
        public void ExecuteQueryThroughDataReaderUsingNullTransactionThrows()
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (IDataReader reader = db.ExecuteReader(insertCommand, null)) {}
            }
        }
        public void ExecuteReaderWithNullCommand()
        {
            using (IDataReader reader = db.ExecuteReader((DbCommand)null)) {}
            Assert.AreEqual(null, insertCommand);
        }
        public void NullQueryStringTest()
        {
            using (DbCommand myCommand = db.GetSqlStringCommand(null))
            {
                IDataReader reader = db.ExecuteReader(myCommand);
            }
        }
        public void WhatGetsReturnedWhenWeDoAnInsertThroughDbCommandExecute()
        {
            int count = -1;
            IDataReader reader = null;
            try
            {
                reader = db.ExecuteReader(insertCommand);
                count = reader.RecordsAffected;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                string deleteString = "Delete from Region where RegionId = 99";
                DbCommand cleanupCommand = db.GetSqlStringCommand(deleteString);
                db.ExecuteNonQuery(cleanupCommand);
            }
            Assert.AreEqual(1, count);
        }
    }
}
