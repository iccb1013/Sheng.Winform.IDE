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
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data.SqlServerCe;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.SqlCe.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Data.SqlCe
{
	/// <summary>
	///		Provides helper methods to make working with a Sql Server Compact Edition database
	///		easier.
	/// </summary>
	/// <remarks>
	///		<para>
	///			Because Sql Server CE has no connection pooling and the cost of opening the initial
	///			connection is high, this class implements a simple connection pool.
	///		</para>
	///		<para>
	///			Sql Server CE requires full trust to run, so it cannot be used in partial trust
	///			environments.
	///		</para>
	/// </remarks>
	[DatabaseAssembler(typeof(SqlCeDatabaseAssembler))]
	public class SqlCeDatabase : Database
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SqlCeDatabase"/> class with a connection string.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		public SqlCeDatabase(string connectionString)
			: base(connectionString, SqlCeProviderFactory.Instance)
		{
		}

		/// <summary>
		///		This will close the "keep alive" connection that is kept open once you first access
		///		the database through this class. Calling this method will close the "keep alive"
		///		connection for all instances. The next database access will open a new "keep alive"
		///		connection.
		/// </summary>
		public void CloseSharedConnection()
		{
			SqlCeConnectionPool.CloseSharedConnection(this);
		}

		/// <summary>
		///		<para>Creates a connection for this database.</para>
		/// </summary>
		/// <remarks>
		///		This method has been overridden to support keeping a single connection open until you
		///		explicitly close it with a call to <see cref="CloseSharedConnection"/>.
		/// </remarks>
		/// <returns>
		///		<para>The <see cref="DbConnection"/> for this database.</para>
		/// </returns>
		/// <seealso cref="DbConnection"/>        
		public override DbConnection CreateConnection()
		{
			DbConnection connection = SqlCeConnectionPool.CreateConnection(this);
			connection.ConnectionString = ConnectionString;
			return connection;
		}

		/// <summary>
		///		Don't need an implementation for Sql Server CE.
		/// </summary>
		/// <param name="discoveryCommand"></param>
		protected override void DeriveParameters(DbCommand discoveryCommand)
		{
		}

		/// <summary>
		/// Sets the RowUpdated event for the data adapter.
		/// </summary>
		/// <param name="adapter">The <see cref="DbDataAdapter"/> to set the event.</param>
		protected override void SetUpRowUpdatedEvent(DbDataAdapter adapter)
		{
			((SqlCeDataAdapter)adapter).RowUpdated += new SqlCeRowUpdatedEventHandler(OnSqlRowUpdated);
		}

		internal void SetConnectionString(DbConnection connection)
		{
			connection.ConnectionString = ConnectionString;
		}

		#region Not Implemented Overrides

		/// <summary>
		///		Stored procedures are not support on Sql Server CE.
		/// </summary>
		/// <param name="storedProcedureName"></param>
		/// <param name="parameterValues"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException">Always throws.</exception>
		public override DataSet ExecuteDataSet(string storedProcedureName, params object[] parameterValues)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		///		Stored procedures are not support on Sql Server CE.
		/// </summary>
		/// <param name="storedProcedureName"></param>
		/// <param name="parameterValues"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException">Always throws.</exception>
		public override int ExecuteNonQuery(string storedProcedureName, params object[] parameterValues)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		///		Stored procedures are not support on Sql Server CE.
		/// </summary>
		/// <param name="storedProcedureName"></param>
		/// <param name="parameterValues"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException">Always throws.</exception>
		public override object ExecuteScalar(string storedProcedureName, params object[] parameterValues)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		///		Stored procedures are not support on Sql Server CE.
		/// </summary>
		/// <param name="storedProcedureName"></param>
		/// <param name="dataSet"></param>
		/// <param name="tableNames"></param>
		/// <param name="parameterValues"></param>
		/// <exception cref="NotImplementedException">Always throws.</exception>
		public override void LoadDataSet(string storedProcedureName, DataSet dataSet, string[] tableNames, params object[] parameterValues)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		///		Since Sql Server CE doesn't support stored procedures, we've changed the meaning of queries
		///		that take a stored procedure name to take a SQL statement instead.
		/// </summary>
		/// <param name="storedProcedureName"></param>
		/// <param name="parameterValues"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException">Always throws.</exception>
		public override DbCommand GetStoredProcCommand(string storedProcedureName, params object[] parameterValues)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		///		Since Sql Server CE doesn't support stored procedures, we've changed the meaning of queries
		///		that take a stored procedure name to take a SQL statement instead.
		/// </summary>
		/// <param name="storedProcedureName">The SQL statement to execute.</param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException">Always throws.</exception>
		public override DbCommand GetStoredProcCommand(string storedProcedureName)
		{
			throw new NotImplementedException();
		}

		#endregion

		/// <summary>
		/// Builds a value parameter name for the current database.
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <returns>A correctly formated parameter name.</returns>
		public override string BuildParameterName(string name)
		{
			if (name[0] != this.ParameterToken)
			{
				return name.Insert(0, new string(this.ParameterToken, 1));
			}
			return name;
		}

		/// <summary>
		/// <para>Gets the parameter token used to delimit parameters for the SQL Server database.</para>
		/// </summary>
		/// <value>
		/// <para>The '@' symbol.</para>
		/// </value>
		protected char ParameterToken
		{
			get { return '@'; }
		}

		#region SqlCeExtensions

		/// <summary>
		///		Creates a new, empty database file using the file name provided in the Data Source attribute
		///		of the connection string.
		/// </summary>
		public void CreateFile()
		{
			SqlCeEngine engine = new SqlCeEngine(ConnectionString);
			engine.CreateDatabase();
		}

		/// <summary>
		///		Creates a new parameter and sets the name of the parameter.
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="type">The type of the parameter.</param>
		/// <param name="size">The size of this parameter.</param>
		/// <param name="value">
		///		The value you want assigned to this parameter. A null value will be converted to
		///		a <see cref="DBNull"/> value in the parameter.
		/// </param>
		/// <returns>
		///		A new <see cref="DbParameter"/> instance of the correct type for this database.</returns>
		/// <remarks>
		///		The database will automatically add the correct prefix, like "@" for SQL Server, to the
		///		parameter name. In other words, you can just supply the name without a prefix.
		/// </remarks>
		public virtual DbParameter CreateParameter(string name, DbType type, int size, object value)
		{
			DbParameter param = CreateParameter(name);
			param.DbType = type;
			param.Size = size;
			param.Value = (value == null) ? DBNull.Value : value;
			return param;
		}

		/// <summary>
		///		Executes a SQL SELECT statement and returns a dataset.
		/// </summary>
		/// <param name="sqlCommand"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public virtual DataSet ExecuteDataSetSql(string sqlCommand, params DbParameter[] parameters)
		{
			using (DbCommand command = GetSqlStringCommand(sqlCommand))
			{
				AddParameters(command, parameters);
				return ExecuteDataSet(command);
			}
		}

		/// <summary>
		///		This method is a short-hand for executing a SQL statement directly since Sql Everywhere
		///		doesn't support stored procedures.
		/// </summary>
		/// <param name="sqlCommand">The SQL statement to execute.</param>
		/// <param name="parameters">An optional set of parameters and values</param>
		/// <returns>Number of rows affected.</returns>
		public virtual int ExecuteNonQuerySql(string sqlCommand, params DbParameter[] parameters)
		{
			using (DbCommand command = GetSqlStringCommand(sqlCommand))
			{
				AddParameters(command, parameters);
				return ExecuteNonQuery(command);
			}
		}

		/// <summary>
		///		This overload allows you to execute an INSERT statement and receive the identity of
		///		the row that was inserted for identity tables.
		/// </summary>
		/// <param name="sqlCommand">The SQL statement to execute</param>
		/// <param name="lastAddedId">The identity value for the last row added, or DBNull</param>
		/// <param name="parameters">Zero or more parameters</param>
		/// <returns>The number of rows affected</returns>
		public virtual int ExecuteNonQuerySql(string sqlCommand, out int lastAddedId, params DbParameter[] parameters)
		{
			using (ConnectionWrapper wrapper = GetOpenConnection())
			{
				using (DbCommand command = GetSqlStringCommand(sqlCommand))
				{
					AddParameters(command, parameters);
					PrepareCommand(command, wrapper.Connection);
					int result = DoExecuteNonQuery(command);
					lastAddedId = GetLastId(wrapper.Connection);
					return result;
				}
			}
		}

		/// <summary>
		///		Execute a command and return a <see cref="DbDataReader"/> that contains the rows
		///		returned.
		/// </summary>
		/// <param name="sqlCommand">The SQL query to execute.</param>
		/// <param name="parameters">Zero or more parameters for the query.</param>
		/// <returns>A <see cref="DbDataReader"/> that contains the rows returned by the query.</returns>
		public virtual IDataReader ExecuteReaderSql(string sqlCommand, params DbParameter[] parameters)
		{
			using (DbCommand command = GetSqlStringCommand(sqlCommand))
			{
				AddParameters(command, parameters);
				return ExecuteReader(command);
			}
		}

		/// <summary>
		///		Sql Server CE provides a new type of data reader, the <see cref="SqlCeResultSet"/>, that provides
		///		new abilities and better performance over a standard reader. This method provides access to
		///		this reader.
		/// </summary>
		/// <remarks>
		///		Unlike other Execute... methods that take a <see cref="DbCommand"/> instance, this method
		///		does not set the command behavior to close the connection when you close the reader.
		///		That means you'll need to close the connection yourself, by calling the
		///		command.Connection.Close() method after you're finished using the reader.
		/// </remarks>
		/// <param name="command">
		///		The command that contains the SQL to execute. It should be a SELECT statement.
		/// </param>
		/// <param name="options">Controls how the <see cref="SqlCeResultSet"/> behaves</param>
		/// <param name="parameters">An option set of <see cref="DbParameter"/> parameters</param>
		/// <returns>The reader in the form of a <see cref="SqlCeResultSet"/></returns>
		public virtual SqlCeResultSet ExecuteResultSet(DbCommand command, ResultSetOptions options, params DbParameter[] parameters)
		{
			ConnectionWrapper wrapper = GetOpenConnection(false);

			try
			{
				AddParameters(command, parameters);
				PrepareCommand(command, wrapper.Connection);
				return DoExecuteResultSet((SqlCeCommand)command, options);
			}
			catch
			{
				wrapper.Connection.Close();			// Close the connection since we asked the wrapper not to, and we're done with it
				throw;
			}
		}

		/// <summary>
		///		Sql Server CE provides a new type of data reader, the <see cref="SqlCeResultSet"/>, that provides
		///		new abilities and better performance over a standard reader. This method provides access to
		///		this reader.
		/// </summary>
		/// <remarks>
		///		Unlike other Execute... methods that take a <see cref="DbCommand"/> instance, this method
		///		does not set the command behavior to close the connection when you close the reader.
		///		That means you'll need to close the connection yourself, by calling the
		///		command.Connection.Close() method after you're finished using the reader.
		/// </remarks>
		/// <param name="command">The command that contains the SQL to execute. It should be a SELECT statement.</param>
		/// <param name="parameters">An option set of <see cref="DbParameter"/> parameters</param>
		/// <returns>The reader in the form of a <see cref="SqlCeResultSet"/></returns>
		public virtual SqlCeResultSet ExecuteResultSet(DbCommand command, params DbParameter[] parameters)
		{
			return ExecuteResultSet(command, ResultSetOptions.None, parameters);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="command"></param>
		/// <param name="transaction"></param>
		/// <param name="options"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public virtual SqlCeResultSet ExecuteResultSet(DbCommand command, DbTransaction transaction, ResultSetOptions options, params DbParameter[] parameters)
		{
			AddParameters(command, parameters);
			PrepareCommand(command, transaction);
			return DoExecuteResultSet((SqlCeCommand)command, options);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="command"></param>
		/// <param name="transaction"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public virtual SqlCeResultSet ExecuteResultSet(DbCommand command, DbTransaction transaction, params DbParameter[] parameters)
		{
			return ExecuteResultSet(command, transaction, ResultSetOptions.None, parameters);
		}

		private SqlCeResultSet DoExecuteResultSet(SqlCeCommand command, ResultSetOptions options)
		{
			try
			{
				DateTime startTime = DateTime.Now;
				SqlCeResultSet reader = command.ExecuteResultSet(options);
				instrumentationProvider.FireCommandExecutedEvent(startTime);
				return reader;
			}
			catch (Exception e)
			{
				instrumentationProvider.FireCommandFailedEvent(command.CommandText, ConnectionStringNoCredentials, e);
				throw;
			}
		}

		/// <summary>
		///		Executes the <paramref name="command"/> and returns the first column of the first
		///		row in the result set returned by the query. Extra columns or rows are ignored.
		/// </summary>
		/// <param name="sqlCommand">The SQL statement to execute.</param>
		/// <param name="parameters">Zero or more parameters for the query.</param>
		/// <returns>
		/// <para>
		///		The first column of the first row in the result set.
		/// </para>
		/// </returns>
		/// <seealso cref="IDbCommand.ExecuteScalar"/>
		public virtual object ExecuteScalarSql(string sqlCommand, params DbParameter[] parameters)
		{
			using (DbCommand command = GetSqlStringCommand(sqlCommand))
			{
				AddParameters(command, parameters);
				return ExecuteScalar(command);
			}
		}

		/// <summary>
		///		Returns the ID of the most recently added row.
		/// </summary>
		/// <returns>
		///		The ID of the row added, or -1 if no row added, or the table doesn't have an identity column.
		///	</returns>
		private int GetLastId(DbConnection connection)
		{
			using (DbCommand command = GetSqlStringCommand("SELECT @@IDENTITY"))
			{
				command.Connection = connection;
				using (IDataReader reader = command.ExecuteReader())
				{
					if (!reader.Read())
						return -1;
					else if (reader[0] is DBNull)
						return -1;
					else return Convert.ToInt32(reader[0]);
				}
			}
		}

		/// <summary>
		///		Adds any parameters to the command
		/// </summary>
		/// <param name="command">The command object you want prepared.</param>
		/// <param name="parameters">Zero or more parameters to add to the command.</param>
		protected void AddParameters(DbCommand command, params DbParameter[] parameters)
		{
			if (parameters != null)
			{
				for (int i = 0; i < parameters.Length; i++)
					command.Parameters.Add(parameters[i]);
			}
		}

		/// <summary>
		///		Checks to see if a table exists in the open database.
		/// </summary>
		/// <param name="tableName">Name of the table.</param>
		/// <returns>true if the table exists, otherwise false.</returns>
		public virtual bool TableExists(string tableName)
		{
			if (string.IsNullOrEmpty(tableName))
				throw new ArgumentException(Properties.Resources.ExceptionNullOrEmptyString, "tableName");

			string sql = @"
					SELECT	COUNT(*) 
					FROM	[INFORMATION_SCHEMA].[TABLES] 
					WHERE	[TABLE_NAME] = " + BuildParameterName("tableName");

			int count = (int)ExecuteScalarSql(sql, CreateParameter("tableName", DbType.String, 0, tableName));
			return (count != 0);
		}

		#endregion

		/// <devdoc>
		/// Listens for the RowUpdate event on a dataadapter to support UpdateBehavior.Continue
		/// </devdoc>
		private void OnSqlRowUpdated(object sender, SqlCeRowUpdatedEventArgs rowThatCouldNotBeWritten)
		{
			if (rowThatCouldNotBeWritten.RecordsAffected == 0)
			{
				if (rowThatCouldNotBeWritten.Errors != null)
				{
					rowThatCouldNotBeWritten.Row.RowError = Resources.ExceptionMessageUpdateDataSetRowFailure;
					rowThatCouldNotBeWritten.Status = UpdateStatus.SkipCurrentRow;
				}
			}
		}
	}
}
