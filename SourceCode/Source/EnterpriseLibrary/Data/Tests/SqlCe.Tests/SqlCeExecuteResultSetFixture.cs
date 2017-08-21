//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlServerCe;
using Data.SqlCe.Tests.VSTS;
using Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Data.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.SqlCe.Tests
{
    [TestClass]
    public class SqlCeExecuteResultSetFixture
    {
        TestConnectionString testConnection;
        const string insertString = "Insert into Region values (99, 'Midwest')";
        const string queryString = "Select * from Region";
        Database db;

        [TestInitialize]
        public void TestInitialize()
        {
            testConnection = new TestConnectionString();
            testConnection.CopyFile();
            db = new SqlCeDatabase(testConnection.ConnectionString);
        }

        [TestCleanup]
        public void TearDown()
        {
            SqlCeConnectionPool.CloseSharedConnections();
        }

        [TestMethod]
        public void ExecuteResultSet_ShouldNotCloseConnection()
        {
            SqlCeDatabase db = (SqlCeDatabase)this.db;
            using (DbCommand command = db.GetSqlStringCommand(queryString))
            {
                SqlCeResultSet reader = db.ExecuteResultSet(command);
                reader.Close();

                Assert.AreEqual(ConnectionState.Open, command.Connection.State);
                command.Connection.Close();
            }
        }

        [TestMethod]
        public void CanExecuteResultSetWithCommand()
        {
            SqlCeDatabase db = (SqlCeDatabase)this.db;
            using (DbCommand command = db.GetSqlStringCommand(queryString))
            {
                SqlCeResultSet reader = db.ExecuteResultSet(command);
                string accumulator = "";
                while (reader.Read())
                {
                    accumulator += ((string)reader["RegionDescription"]).Trim();
                }
                reader.Close();
                command.Connection.Close();

                Assert.AreEqual("EasternWesternNorthernSouthern", accumulator);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(SqlCeException))]
        public void ExecuteResultSetWithBadCommandThrowsAndClosesConnection()
        {
            SqlCeDatabase db = (SqlCeDatabase)this.db;
            DbCommand badCommand = db.GetSqlStringCommand("select * from foobar");
            try
            {
                db.ExecuteResultSet(badCommand);
            }
            finally
            {
                Assert.IsFalse(badCommand.Connection != null && badCommand.Connection.State == ConnectionState.Open);
            }
        }

        [TestMethod]
        public void ShouldHaveCorrectRowsAffectedAfterInsertCommand()
        {
            int count = -1;
            SqlCeResultSet reader = null;
            DbCommand command = null;
            try
            {
                SqlCeDatabase db = (SqlCeDatabase)this.db;
                command = db.GetSqlStringCommand(insertString);
                reader = db.ExecuteResultSet(command);
                count = reader.RecordsAffected;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (command != null)
                {
                    command.Connection.Close();
                    command.Dispose();
                }

                string deleteString = "Delete from Region where RegionId = 99";
                DbCommand cleanupCommand = db.GetSqlStringCommand(deleteString);
                db.ExecuteNonQuery(cleanupCommand);
            }

            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void CanExecuteQueryThroughDataReaderUsingTransaction()
        {
            SqlCeDatabase db = (SqlCeDatabase)this.db;

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (DbCommand command = db.GetSqlStringCommand(insertString))
                {
                    using (RollbackTransactionWrapper transaction = new RollbackTransactionWrapper(connection.BeginTransaction()))
                    {
                        using (SqlCeResultSet reader = db.ExecuteResultSet(command, transaction.Transaction))
                        {
                            Assert.AreEqual(1, reader.RecordsAffected);
                            reader.Close();
                        }
                    }
                    Assert.AreEqual(ConnectionState.Open, connection.State);

                    command.Connection.Close();
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExecuteResultSetUsingNullCommandThrows()
        {
            SqlCeDatabase db = (SqlCeDatabase)this.db;

            using (SqlCeResultSet reader = db.ExecuteResultSet((DbCommand)null)) {}
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExecuteQueryThroughDataReaderUsingNullCommandAndNullTransactionThrows()
        {
            SqlCeDatabase db = (SqlCeDatabase)this.db;

            using (SqlCeResultSet reader = db.ExecuteResultSet(null, (DbTransaction)null)) {}
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExecuteQueryThroughDataReaderUsingNullTransactionThrows()
        {
            SqlCeDatabase db = (SqlCeDatabase)this.db;

            using (DbConnection connection = db.CreateConnection())
            {
                try
                {
                    connection.Open();

                    using (DbCommand command = db.GetSqlStringCommand(queryString))
                    {
                        using (IDataReader reader = this.db.ExecuteReader(command, null)) {}
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExecuteResultSetWithNullCommandThrows()
        {
            SqlCeDatabase db = (SqlCeDatabase)this.db;
            db.ExecuteResultSet(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NullQueryStringTest()
        {
            SqlCeDatabase db = (SqlCeDatabase)this.db;

            using (DbCommand myCommand = db.GetSqlStringCommand(null))
            {
                IDataReader reader = db.ExecuteResultSet(myCommand);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EmptyQueryStringTest()
        {
            SqlCeDatabase db = (SqlCeDatabase)this.db;

            using (DbCommand myCommand = db.GetSqlStringCommand(String.Empty))
            {
                IDataReader reader = db.ExecuteResultSet(myCommand);
            }
        }

        [TestMethod]
        public void ExecuteResultSetCallsInstrumentationFireCommandExecutedEvent()
        {
            SqlCeDatabase db = (SqlCeDatabase)this.db;
            int executeCount = 0;
            int failedCount = 0;

            DataInstrumentationProvider instrumentation = (DataInstrumentationProvider)db.GetInstrumentationEventProvider();
            instrumentation.commandExecuted += delegate(object sender,
                                                        CommandExecutedEventArgs e)
                                               {
                                                   executeCount++;
                                               };
            instrumentation.commandFailed += delegate(object sender,
                                                      CommandFailedEventArgs e)
                                             {
                                                 failedCount++;
                                             };

            using (DbCommand command = db.GetSqlStringCommand(queryString))
            {
                SqlCeResultSet reader = db.ExecuteResultSet(command);
                reader.Close();

                command.Connection.Close();
            }
            Assert.AreEqual(1, executeCount);
            Assert.AreEqual(0, failedCount);
        }

        [TestMethod]
        public void ExecuteResultSetWithBadCommandCallsInstrumentationFireCommandFailedEvent()
        {
            SqlCeDatabase db = (SqlCeDatabase)this.db;
            int executeCount = 0;
            int failedCount = 0;

            DataInstrumentationProvider instrumentation = (DataInstrumentationProvider)db.GetInstrumentationEventProvider();
            instrumentation.commandExecuted += delegate(object sender,
                                                        CommandExecutedEventArgs e)
                                               {
                                                   executeCount++;
                                               };
            instrumentation.commandFailed += delegate(object sender,
                                                      CommandFailedEventArgs e)
                                             {
                                                 failedCount++;
                                             };

            try
            {
                using (DbCommand command = db.GetSqlStringCommand("select * from junk"))
                {
                    SqlCeResultSet reader = db.ExecuteResultSet(command);
                    reader.Close();

                    command.Connection.Close();
                }
            }
            catch {}

            Assert.AreEqual(0, executeCount);
            Assert.AreEqual(1, failedCount);
        }
    }
}
