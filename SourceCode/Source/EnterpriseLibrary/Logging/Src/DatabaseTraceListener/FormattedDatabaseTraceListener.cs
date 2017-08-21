//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
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
using System.Diagnostics;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Database
{
	/// <summary>
	/// A <see cref="System.Diagnostics.TraceListener"/> that writes to a database, formatting the output with an <see cref="ILogFormatter"/>.
	/// </summary>
	[ConfigurationElementType(typeof(FormattedDatabaseTraceListenerData))]
	public class FormattedDatabaseTraceListener : FormattedTraceListenerBase
	{
		string writeLogStoredProcName = String.Empty;
		string addCategoryStoredProcName = String.Empty;
		Data.Database database;

		/// <summary>
		/// Initializes a new instance of <see cref="FormattedDatabaseTraceListener"/>.
		/// </summary>
		/// <param name="database">The database for writing the log.</param>
		/// <param name="writeLogStoredProcName">The stored procedure name for writing the log.</param>
		/// <param name="addCategoryStoredProcName">The stored procedure name for adding a category for this log.</param>
		/// <param name="formatter">The formatter.</param>        
		public FormattedDatabaseTraceListener(Data.Database database, string writeLogStoredProcName, string addCategoryStoredProcName, ILogFormatter formatter
			)
			: base(formatter)
		{
			this.writeLogStoredProcName = writeLogStoredProcName;
			this.addCategoryStoredProcName = addCategoryStoredProcName;
			this.database = database;
		}


		/// <summary>
		/// The Write method 
		/// </summary>
		/// <param name="message">The message to log</param>
		public override void Write(string message)
		{
			ExecuteWriteLogStoredProcedure(0, 5, TraceEventType.Information, string.Empty, DateTime.Now, string.Empty,
											string.Empty, string.Empty, string.Empty, null, null, message, database);
		}

		/// <summary>
		/// The WriteLine method.
		/// </summary>
		/// <param name="message">The message to log</param>
		public override void WriteLine(string message)
		{
			Write(message);
		}


		/// <summary>
		/// Delivers the trace data to the underlying database.
		/// </summary>
		/// <param name="eventCache">The context information provided by <see cref="System.Diagnostics"/>.</param>
		/// <param name="source">The name of the trace source that delivered the trace data.</param>
		/// <param name="eventType">The type of event.</param>
		/// <param name="id">The id of the event.</param>
		/// <param name="data">The data to trace.</param>
		public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
		{
            if ((this.Filter == null) || this.Filter.ShouldTrace(eventCache, source, eventType, id, null, null, data, null))
            {
                if (data is LogEntry)
                {
                    LogEntry logEntry = data as LogEntry;
                    if (ValidateParameters(logEntry))
                        ExecuteStoredProcedure(logEntry);
                    InstrumentationProvider.FireTraceListenerEntryWrittenEvent();
                }
                else if (data is string)
                {
                    Write(data as string);
                }
                else
                {
                    base.TraceData(eventCache, source, eventType, id, data);
                }
            }
		}

		/// <summary>
		/// Declare the supported attributes for <see cref="FormattedDatabaseTraceListener"/>
		/// </summary>
		protected override string[] GetSupportedAttributes()
		{
			return new string[4] { "formatter", "writeLogStoredProcName", "addCategoryStoredProcName", "databaseInstanceName" };
		}

		/// <summary>
		/// Validates that enough information exists to attempt executing the stored procedures
		/// </summary>
		/// <param name="logEntry">The LogEntry to validate.</param>
		/// <returns>A boolean indicating whether the parameters for the LogEntry configuration are valid.</returns>
		private bool ValidateParameters(LogEntry logEntry)
		{
			bool valid = true;

			if (writeLogStoredProcName == null ||
				writeLogStoredProcName.Length == 0)
			{
				return false;
			}

			if (addCategoryStoredProcName == null ||
				addCategoryStoredProcName.Length == 0)
			{
				return false;
			}

			return valid;
		}

		/// <summary>
		/// Executes the stored procedures
		/// </summary>
		/// <param name="logEntry">The LogEntry to store in the database</param>
		private void ExecuteStoredProcedure(LogEntry logEntry)
		{
			using (DbConnection connection = database.CreateConnection())
			{
				connection.Open();
				try
				{
					using (DbTransaction transaction = connection.BeginTransaction())
					{
						try
						{
							int logID = Convert.ToInt32(ExecuteWriteLogStoredProcedure(logEntry, database, transaction));
							ExecuteAddCategoryStoredProcedure(logEntry, logID, database, transaction);
							transaction.Commit();
						}
						catch
						{
							transaction.Rollback();
							throw;
						}

					}
				}
				finally
				{
					connection.Close();
				}
			}
		}

		/// <summary>
		/// Executes the WriteLog stored procedure
		/// </summary>
		/// <param name="eventId">The event id for this LogEntry.</param>
		/// <param name="priority">The priority for this LogEntry.</param>
		/// <param name="severity">The severity for this LogEntry.</param>
		/// <param name="title">The title for this LogEntry.</param>
		/// <param name="timeStamp">The timestamp for this LogEntry.</param>
		/// <param name="machineName">The machine name for this LogEntry.</param>
		/// <param name="appDomainName">The appDomainName for this LogEntry.</param>
		/// <param name="processId">The process id for this LogEntry.</param>
		/// <param name="processName">The processName for this LogEntry.</param>
		/// <param name="managedThreadName">The managedthreadName for this LogEntry.</param>
		/// <param name="win32ThreadId">The win32threadID for this LogEntry.</param>
		/// <param name="message">The message for this LogEntry.</param>
		/// <param name="db">An instance of the database class to use for storing the LogEntry</param>
		/// <returns>An integer for the LogEntry Id</returns>
		private int ExecuteWriteLogStoredProcedure(int eventId, int priority, TraceEventType severity, string title, DateTime timeStamp,
													string machineName, string appDomainName, string processId, string processName,
													string managedThreadName, string win32ThreadId, string message, Data.Database db)
		{
			DbCommand cmd = db.GetStoredProcCommand(writeLogStoredProcName);

			db.AddInParameter(cmd, "eventID", DbType.Int32, eventId);
			db.AddInParameter(cmd, "priority", DbType.Int32, priority);
			db.AddParameter(cmd, "severity", DbType.String, 32, ParameterDirection.Input, false, 0, 0, null, DataRowVersion.Default, severity.ToString());
            db.AddParameter(cmd, "title", DbType.String, 256, ParameterDirection.Input, false, 0, 0, null, DataRowVersion.Default, title);
			db.AddInParameter(cmd, "timestamp", DbType.DateTime, timeStamp);
            db.AddParameter(cmd, "machineName", DbType.String, 32, ParameterDirection.Input, false, 0, 0, null, DataRowVersion.Default, machineName);
            db.AddParameter(cmd, "AppDomainName", DbType.String, 512, ParameterDirection.Input, false, 0, 0, null, DataRowVersion.Default, appDomainName);
            db.AddParameter(cmd, "ProcessID", DbType.String, 256, ParameterDirection.Input, false, 0, 0, null, DataRowVersion.Default, processId);
            db.AddParameter(cmd, "ProcessName", DbType.String, 512, ParameterDirection.Input, false, 0, 0, null, DataRowVersion.Default, processName);
            db.AddParameter(cmd, "ThreadName", DbType.String, 512, ParameterDirection.Input, false, 0, 0, null, DataRowVersion.Default, managedThreadName);
            db.AddParameter(cmd, "Win32ThreadId", DbType.String, 128, ParameterDirection.Input, false, 0, 0, null, DataRowVersion.Default, win32ThreadId);
            db.AddParameter(cmd, "message", DbType.String, 1500, ParameterDirection.Input, false, 0, 0, null, DataRowVersion.Default, message);
			db.AddInParameter(cmd, "formattedmessage", DbType.String, message);

			db.AddOutParameter(cmd, "LogId", DbType.Int32, 4);

			db.ExecuteNonQuery(cmd);
			int logId = Convert.ToInt32(cmd.Parameters[cmd.Parameters.Count - 1].Value, CultureInfo.InvariantCulture);
			return logId;
		}

		/// <summary>
		/// Executes the WriteLog stored procedure
		/// </summary>
		/// <param name="logEntry">The LogEntry to store in the database.</param>
		/// <param name="db">An instance of the database class to use for storing the LogEntry</param>
		/// <param name="transaction">The transaction that wraps around the execution calls for storing the LogEntry</param>
		/// <returns>An integer for the LogEntry Id</returns>
		private int ExecuteWriteLogStoredProcedure(LogEntry logEntry, Data.Database db, DbTransaction transaction)
		{
			DbCommand cmd = db.GetStoredProcCommand(writeLogStoredProcName);


			db.AddInParameter(cmd, "eventID", DbType.Int32, logEntry.EventId);
			db.AddInParameter(cmd, "priority", DbType.Int32, logEntry.Priority);
            db.AddParameter(cmd, "severity", DbType.String, 32, ParameterDirection.Input, false, 0, 0, null, DataRowVersion.Default, logEntry.Severity.ToString());
            db.AddParameter(cmd, "title", DbType.String, 256, ParameterDirection.Input, false, 0, 0, null, DataRowVersion.Default, logEntry.Title);
			db.AddInParameter(cmd, "timestamp", DbType.DateTime, logEntry.TimeStamp);
            db.AddParameter(cmd, "machineName", DbType.String, 32, ParameterDirection.Input, false, 0, 0, null, DataRowVersion.Default, logEntry.MachineName);
            db.AddParameter(cmd, "AppDomainName", DbType.String, 512, ParameterDirection.Input, false, 0, 0, null, DataRowVersion.Default, logEntry.AppDomainName);
            db.AddParameter(cmd, "ProcessID", DbType.String, 256, ParameterDirection.Input, false, 0, 0, null, DataRowVersion.Default, logEntry.ProcessId);
            db.AddParameter(cmd, "ProcessName", DbType.String, 512, ParameterDirection.Input, false, 0, 0, null, DataRowVersion.Default, logEntry.ProcessName);
            db.AddParameter(cmd, "ThreadName", DbType.String, 512, ParameterDirection.Input, false, 0, 0, null, DataRowVersion.Default, logEntry.ManagedThreadName);
            db.AddParameter(cmd, "Win32ThreadId", DbType.String, 128, ParameterDirection.Input, false, 0, 0, null, DataRowVersion.Default, logEntry.Win32ThreadId);
            db.AddParameter(cmd, "message", DbType.String, 1500, ParameterDirection.Input, false, 0, 0, null, DataRowVersion.Default, logEntry.Message);

			if (Formatter != null)
				db.AddInParameter(cmd, "formattedmessage", DbType.String, Formatter.Format(logEntry));
			else
				db.AddInParameter(cmd, "formattedmessage", DbType.String, logEntry.Message);

			db.AddOutParameter(cmd, "LogId", DbType.Int32, 4);

			db.ExecuteNonQuery(cmd, transaction);
			int logId = Convert.ToInt32(cmd.Parameters[cmd.Parameters.Count - 1].Value, CultureInfo.InvariantCulture);
			return logId;
		}

		/// <summary>
		/// Executes the AddCategory stored procedure
		/// </summary>
		/// <param name="logEntry">The LogEntry to store in the database.</param>
		/// <param name="logID">The unique identifer for the LogEntry as obtained from the WriteLog Stored procedure.</param>
		/// <param name="db">An instance of the database class to use for storing the LogEntry</param>
		/// <param name="transaction">The transaction that wraps around the execution calls for storing the LogEntry</param>
		private void ExecuteAddCategoryStoredProcedure(LogEntry logEntry, int logID, Data.Database db, DbTransaction transaction)
		{
			foreach (string category in logEntry.Categories)
			{
				DbCommand cmd = db.GetStoredProcCommand(addCategoryStoredProcName);
				db.AddInParameter(cmd, "categoryName", DbType.String, category);
				db.AddInParameter(cmd, "logID", DbType.Int32, logID);
				db.ExecuteNonQuery(cmd, transaction);
			}
		}
	}
}
