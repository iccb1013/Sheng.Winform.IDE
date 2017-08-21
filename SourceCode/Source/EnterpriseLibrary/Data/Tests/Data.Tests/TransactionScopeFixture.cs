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
using System.Threading;
using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IsolationLevel=System.Transactions.IsolationLevel;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests
{
    //
    // Here is a sumary of the "stories" we wanted to support when you're using TransactionScope:
    //
    //	*	Different modes: Require, RequireNew, None
    //	*	Use either connection string as key
    //	*	Scopes are thread local by default
    //	*	Can create transaction with behavior
    //
    public class TransactionScopeFixture
    {
        const string countString = "select count(*) from Region";
        const string insertString = "insert into Region values (77, 'Elbonia')";
        const string insertString2 = "insert into Region values (78, 'Australia')";
        const string queryString = "select * from Region";
        Database db;

        public TransactionScopeFixture(Database db)
        {
            this.db = db;
        }

        public void Clenaup()
        {
            db.ExecuteNonQuery(CommandType.Text, "delete from Region where RegionID >= 77");
        }

        public void Comit_ShouldKeepInnerChangesForNestedTransaction()
        {
            int totalRows = TotalRows();
            using (TransactionScope scope1 = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                using (TransactionScope scope2 = new TransactionScope(TransactionScopeOption.Required))
                {
                    db.ExecuteNonQuery(CommandType.Text, insertString);
                    scope2.Complete();
                }
                scope1.Complete();
            }
            Assert.AreEqual(totalRows + 1, TotalRows());
        }

        public void Commit_ShouldDisposeTransactionConnection()
        {
            int disposeCount = 0;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                DbConnection connection = TransactionScopeConnections.GetConnection(db);
                connection.Disposed += delegate(object sender,
                                                EventArgs e)
                                       {
                                           disposeCount++;
                                       };
                Assert.IsNotNull(connection);
                scope.Complete();
            }
            //for (int i = 0; i < 10 && disposeCount == 0; i++)
            //{
            //    Thread.Sleep(20);						// Oracle doesn't dispose right away because of DTC
            //}
            Assert.AreEqual(1, disposeCount);
        }

        public void Commit_ShouldKeepChanges()
        {
            int totalRows = TotalRows();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                db.ExecuteNonQuery(CommandType.Text, insertString);
                scope.Complete();
            }
            Assert.AreEqual(totalRows + 1, TotalRows());
        }

        public void Complete_ShouldDiscardInnerChangesWhenOuterNotCompleted()
        {
            int totalRows = TotalRows();
            using (TransactionScope scope1 = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                using (TransactionScope scope2 = new TransactionScope(TransactionScopeOption.Required))
                {
                    db.ExecuteNonQuery(CommandType.Text, insertString);
                    scope2.Complete();
                }
            }
            Assert.AreEqual(totalRows, TotalRows());
        }

        public void ExecuteDataSetWithCommand_ShouldRetriveDataSet()
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                db.ExecuteNonQuery(CommandType.Text, insertString);

                DataSet dataSet = db.ExecuteDataSet(db.GetSqlStringCommand(queryString));
                Assert.AreEqual(1, dataSet.Tables.Count);
                Assert.AreEqual(5, dataSet.Tables[0].Rows.Count);
            }
        }

        public void ExecuteDataSetWithCommandText_ShouldRetriveDataSet()
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                db.ExecuteNonQuery(CommandType.Text, insertString);

                DataSet dataSet = db.ExecuteDataSet(CommandType.Text, queryString);
                Assert.AreEqual(1, dataSet.Tables.Count);
                Assert.AreEqual(5, dataSet.Tables[0].Rows.Count);
            }
        }

        public void ExecuteDataSetWithStoredProcedure_ShouldRetriveDataSet()
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                db.ExecuteNonQuery(CommandType.Text, insertString);

                DataSet dataSet = db.ExecuteDataSet("RegionSelect");
                Assert.AreEqual(1, dataSet.Tables.Count);
                Assert.AreEqual(5, dataSet.Tables[0].Rows.Count);
            }
        }

        public void ExecuteNonQueryWithCommand_ShouldUseTransaction()
        {
            int totalRows = TotalRows();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                DbCommand command = db.GetSqlStringCommand(insertString);
                int rows = db.ExecuteNonQuery(command);
                Assert.AreEqual(1, rows);
            }
            Assert.AreEqual(totalRows, TotalRows());
        }

        public void ExecuteNonQueryWithStoredProcedure_ShouldUseTransaction()
        {
            int totalRows = TotalRows();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                int rows = db.ExecuteNonQuery("RegionInsert", 77, "Elbonia");
                Assert.AreEqual(rows, 1);
            }
            Assert.AreEqual(totalRows, TotalRows());
        }

        public void ExecuteNonQueryWithTextCommand_ShouldUseTransaction()
        {
            int totalRows = TotalRows();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                int rows = db.ExecuteNonQuery(CommandType.Text, insertString);
                Assert.AreEqual(1, rows);
            }
            Assert.AreEqual(totalRows, TotalRows());
        }

        public void ExecuteReaderWithCommand_ShouldRetrieveDataInTransaction()
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                db.ExecuteNonQuery(CommandType.Text, insertString);

                DbCommand command = db.GetSqlStringCommand("select * from region where regionid=77");
                IDataReader reader = db.ExecuteReader(command);
                Assert.IsNotNull(reader);
                Assert.IsTrue(reader.Read());
            }
        }

        public void ExecuteReaderWithCommandText_ShouldRetrieveDataInTransaction()
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                db.ExecuteNonQuery(CommandType.Text, insertString);

                IDataReader reader = db.ExecuteReader(CommandType.Text, "select * from region where regionid=77");
                Assert.IsNotNull(reader);
                Assert.IsTrue(reader.Read());
            }
        }

        public void ExecuteReaderWithStoredProcedure_ShouldRetrieveDataInTransaction()
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                db.ExecuteNonQuery(CommandType.Text, insertString);

                IDataReader reader = db.ExecuteReader("RegionSelect");
                Assert.IsNotNull(reader);
                int numRows = 0;
                while (reader.Read())
                    numRows++;

                Assert.AreEqual(5, numRows);
            }
        }

        public void ExecuteScalarWithCommand_ShouldUseTransaction()
        {
            int totalRows = TotalRows();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                db.ExecuteNonQuery(CommandType.Text, insertString);

                DbCommand command = db.GetSqlStringCommand(countString);
                int rows = Convert.ToInt32(db.ExecuteScalar(command));
                Assert.AreEqual(totalRows + 1, rows);
            }
            Assert.AreEqual(totalRows, TotalRows());
        }

        public void ExecuteScalarWithCommandText_ShouldUseTransaction()
        {
            int totalRows = TotalRows();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                db.ExecuteNonQuery(CommandType.Text, insertString);

                int rows = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, countString));
                Assert.AreEqual(totalRows + 1, rows);
            }
            Assert.AreEqual(totalRows, TotalRows());
        }

        public void ExecuteScalarWithStoredProcedure_ShouldUseTransaction()
        {
            int totalRows = TotalRows();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                db.ExecuteScalar("RegionInsert", 77, "Elbonia");
                Assert.AreEqual(totalRows + 1, TotalRows());
            }
            Assert.AreEqual(totalRows, TotalRows());
        }

        public void Insert_ShouldAddRowsWhenNoTransactionActive()
        {
            int totalRows = TotalRows();
            using (TransactionScope scope1 = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                using (TransactionScope scope2 = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    db.ExecuteNonQuery(CommandType.Text, insertString);
                }
            }
            Assert.AreEqual(totalRows + 1, TotalRows());
        }

        public void LoadDataSetWithCommand_LoadsDataInTransaction()
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                db.ExecuteNonQuery(CommandType.Text, insertString);

                DataSet ds = new DataSet();
                DbCommand command = db.GetSqlStringCommand(queryString);
                db.LoadDataSet(command, ds, "Junk");

                Assert.AreEqual(5, ds.Tables[0].Rows.Count);
            }
        }

        public void LoadDataSetWithCommandText_LoadsDataInTransaction()
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                db.ExecuteNonQuery(CommandType.Text, insertString);

                DataSet ds = new DataSet();
                db.LoadDataSet(CommandType.Text, queryString, ds, new string[] { "Junk" });

                Assert.AreEqual(5, ds.Tables[0].Rows.Count);
            }
        }

        public void Rollback_ShouldDisposeTransactionConnection()
        {
            int disposeCount = 0;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                DbConnection connection = TransactionScopeConnections.GetConnection(db);
                connection.Disposed += delegate(object sender,
                                                EventArgs e)
                                       {
                                           disposeCount++;
                                       };
                Assert.IsNotNull(connection);
            }
            for (int i = 0; i < 10 && disposeCount == 0; i++)
            {
                Thread.Sleep(20); // Oracle doesn't dispose right away because of DTC
            }
            Assert.AreEqual(1, disposeCount);
        }

        public void ShouldAllowCommandsAfterInnerScopeDisposed()
        {
            int totalRows = TotalRows();
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope scope1 = new TransactionScope(TransactionScopeOption.RequiresNew, options))
            {
                Assert.AreEqual(totalRows, TotalRows());
                using (TransactionScope scope2 = new TransactionScope(TransactionScopeOption.RequiresNew, options))
                {
                    db.ExecuteNonQuery(CommandType.Text, insertString);
                }
                Assert.AreEqual(totalRows, TotalRows());
                db.ExecuteNonQuery(CommandType.Text, insertString2);
                scope1.Complete();
            }
            Assert.AreEqual(totalRows + 1, TotalRows());
        }

        int TotalRows()
        {
            object result = db.ExecuteScalar(CommandType.Text, countString);
            return Convert.ToInt32(result);
        }

        public void TransactionScope_ShouldDiscardChangesOnDispose()
        {
            int totalRows = TotalRows();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                int rows = db.ExecuteNonQuery(CommandType.Text, insertString);
                Assert.AreEqual(1, rows);
            }
            Assert.AreEqual(totalRows, TotalRows());
        }

        public void TransactionScope_ShouldNotPromoteToDTC()
        {
            int totalRows = TotalRows();
            int dtcCount = 0;

            TransactionManager.DistributedTransactionStarted += delegate(object sender,
                                                                         TransactionEventArgs e)
                                                                {
                                                                    dtcCount++;
                                                                };
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                int rows = db.ExecuteNonQuery(CommandType.Text, insertString);
                Assert.AreEqual(1, rows);

                rows = db.ExecuteNonQuery(CommandType.Text, insertString2);
                Assert.AreEqual(1, rows);
            }
            Assert.AreEqual(totalRows, TotalRows());
            Assert.AreEqual(0, dtcCount);
        }

        public void UpdateDataSet_ShouldAddToTransaction()
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                DataSet ds = db.ExecuteDataSet(CommandType.Text, queryString);
                DbCommand insertCommand = db.GetStoredProcCommandWithSourceColumns("RegionInsert", "RegionID", "RegionDescription");

                DataRow row = ds.Tables[0].NewRow();
                row["RegionID"] = 77;
                row["RegionDescription"] = "Australia";
                ds.Tables[0].Rows.Add(row);

                int rows = db.UpdateDataSet(ds, ds.Tables[0].TableName, insertCommand, null, null, UpdateBehavior.Standard);
                Assert.AreEqual(1, rows);
                Assert.AreEqual(5, TotalRows());
            }
            Assert.AreEqual(4, TotalRows());
        }

        public void UpdateDataSetWithUpdateBlockSize_ShouldAddToTransaction()
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                DataSet ds = db.ExecuteDataSet(CommandType.Text, queryString);
                DbCommand insertCommand = db.GetStoredProcCommandWithSourceColumns("RegionInsert", "RegionID", "RegionDescription");

                DataRow row = ds.Tables[0].NewRow();
                row["RegionID"] = 77;
                row["RegionDescription"] = "Australia";
                ds.Tables[0].Rows.Add(row);

                int rows = db.UpdateDataSet(ds, ds.Tables[0].TableName, insertCommand, null, null, UpdateBehavior.Standard, 10);
                Assert.AreEqual(1, rows);
                Assert.AreEqual(5, TotalRows());
            }
            Assert.AreEqual(4, TotalRows());
        }
    }
}
