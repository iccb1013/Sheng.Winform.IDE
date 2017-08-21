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
using System.Data.SqlClient;
using System.Security.Permissions;
using System.Transactions;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Data.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Sql
{
	/// <summary>
	/// <para>Represents a SQL Server database.</para>
	/// </summary>
	/// <remarks> 
	/// <para>
	/// Internally uses SQL Server .NET Managed Provider from Microsoft (System.Data.SqlClient) to connect to the database.
	/// </para>  
	/// </remarks>
	[SqlClientPermission(SecurityAction.Demand)]
	[DatabaseAssembler(typeof(SqlDatabaseAssembler))]
	[ContainerPolicyCreator(typeof(SqlDatabasePolicyCreator))]
	public class SqlDatabase : Database
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SqlDatabase"/> class with a connection string.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		public SqlDatabase(string connectionString)
			: base(connectionString, SqlClientFactory.Instance)
		{
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

		/// <summary>
		/// <para>Executes the <see cref="SqlCommand"/> and returns a new <see cref="XmlReader"/>.</para>
		/// </summary>
		/// <remarks>
		///		Unlike other Execute... methods that take a <see cref="DbCommand"/> instance, this method
		///		does not set the command behavior to close the connection when you close the reader.
		///		That means you'll need to close the connection yourself, by calling the
		///		command.Connection.Close() method.
		///		<para>
		///			There is one exception to the rule above. If you're using <see cref="TransactionScope"/> to provide
		///			implicit transactions, you should NOT close the connection on this reader when you're
		///			done. Only close the connection if <see cref="Transaction"/>.Current is null.
		///		</para>
		/// </remarks>
		/// <param name="command">
		/// <para>The <see cref="SqlCommand"/> to execute.</para>
		/// </param>
		/// <returns>
		/// <para>An <see cref="XmlReader"/> object.</para>
		/// </returns>
		public XmlReader ExecuteXmlReader(DbCommand command)
		{
			SqlCommand sqlCommand = CheckIfSqlCommand(command);

			ConnectionWrapper wrapper = GetOpenConnection(false);
			PrepareCommand(command, wrapper.Connection);
			return DoExecuteXmlReader(sqlCommand);
		}

		/// <summary>
		/// <para>Executes the <see cref="SqlCommand"/> in a transaction and returns a new <see cref="XmlReader"/>.</para>
		/// </summary>
		/// <remarks>
		///		Unlike other Execute... methods that take a <see cref="DbCommand"/> instance, this method
		///		does not set the command behavior to close the connection when you close the reader.
		///		That means you'll need to close the connection yourself, by calling the
		///		command.Connection.Close() method.
		/// </remarks>
		/// <param name="command">
		/// <para>The <see cref="SqlCommand"/> to execute.</para>
		/// </param>
		/// <param name="transaction">
		/// <para>The <see cref="IDbTransaction"/> to execute the command within.</para>
		/// </param>
		/// <returns>
		/// <para>An <see cref="XmlReader"/> object.</para>
		/// </returns>
		public XmlReader ExecuteXmlReader(DbCommand command, DbTransaction transaction)
		{
			SqlCommand sqlCommand = CheckIfSqlCommand(command);

			PrepareCommand(sqlCommand, transaction);
			return DoExecuteXmlReader(sqlCommand);
		}

		/// <devdoc>
		/// Execute the actual XML Reader call.
		/// </devdoc>        
		private XmlReader DoExecuteXmlReader(SqlCommand sqlCommand)
		{
			try
			{
				DateTime startTime = DateTime.Now;
				XmlReader reader = sqlCommand.ExecuteXmlReader();
				instrumentationProvider.FireCommandExecutedEvent(startTime);
				return reader;
			}
			catch (Exception e)
			{
				instrumentationProvider.FireCommandFailedEvent(sqlCommand.CommandText, ConnectionStringNoCredentials, e);
				throw;
			}
		}

		private static SqlCommand CheckIfSqlCommand(DbCommand command)
		{
			SqlCommand sqlCommand = command as SqlCommand;
			if (sqlCommand == null) throw new ArgumentException(Resources.ExceptionCommandNotSqlCommand, "command");
			return sqlCommand;
		}

		/// <devdoc>
		/// Listens for the RowUpdate event on a dataadapter to support UpdateBehavior.Continue
		/// </devdoc>
		private void OnSqlRowUpdated(object sender, SqlRowUpdatedEventArgs rowThatCouldNotBeWritten)
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

		/// <summary>
		/// Retrieves parameter information from the stored procedure specified in the <see cref="DbCommand"/> and populates the Parameters collection of the specified <see cref="DbCommand"/> object. 
		/// </summary>
		/// <param name="discoveryCommand">The <see cref="DbCommand"/> to do the discovery.</param>
		/// <remarks>The <see cref="DbCommand"/> must be a <see cref="SqlCommand"/> instance.</remarks>
		protected override void DeriveParameters(DbCommand discoveryCommand)
		{
			SqlCommandBuilder.DeriveParameters((SqlCommand)discoveryCommand);
		}

		/// <summary>
		/// Returns the starting index for parameters in a command.
		/// </summary>
		/// <returns>The starting index for parameters in a command.</returns>
		protected override int UserParametersStartIndex()
		{
			return 1;
		}

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
		/// Sets the RowUpdated event for the data adapter.
		/// </summary>
		/// <param name="adapter">The <see cref="DbDataAdapter"/> to set the event.</param>
		protected override void SetUpRowUpdatedEvent(DbDataAdapter adapter)
		{
			((SqlDataAdapter)adapter).RowUpdated += OnSqlRowUpdated;
		}

		/// <summary>
		/// Determines if the number of parameters in the command matches the array of parameter values.
		/// </summary>
		/// <param name="command">The <see cref="DbCommand"/> containing the parameters.</param>
		/// <param name="values">The array of parameter values.</param>
		/// <returns><see langword="true"/> if the number of parameters and values match; otherwise, <see langword="false"/>.</returns>
		protected override bool SameNumberOfParametersAndValues(DbCommand command, object[] values)
		{
			int returnParameterCount = 1;
			int numberOfParametersToStoredProcedure = command.Parameters.Count - returnParameterCount;
			int numberOfValuesProvidedForStoredProcedure = values.Length;
			return numberOfParametersToStoredProcedure == numberOfValuesProvidedForStoredProcedure;
		}

		/// <summary>
		/// <para>Adds a new instance of a <see cref="DbParameter"/> object to the command.</para>
		/// </summary>
		/// <param name="command">The command to add the parameter.</param>
		/// <param name="name"><para>The name of the parameter.</para></param>
		/// <param name="dbType"><para>One of the <see cref="DbType"/> values.</para></param>
		/// <param name="size"><para>The maximum size of the data within the column.</para></param>
		/// <param name="direction"><para>One of the <see cref="ParameterDirection"/> values.</para></param>
		/// <param name="nullable"><para>A value indicating whether the parameter accepts <see langword="null"/> (<b>Nothing</b> in Visual Basic) values.</para></param>
		/// <param name="precision"><para>The maximum number of digits used to represent the <paramref name="value"/>.</para></param>
		/// <param name="scale"><para>The number of decimal places to which <paramref name="value"/> is resolved.</para></param>
		/// <param name="sourceColumn"><para>The name of the source column mapped to the DataSet and used for loading or returning the <paramref name="value"/>.</para></param>
		/// <param name="sourceVersion"><para>One of the <see cref="DataRowVersion"/> values.</para></param>
		/// <param name="value"><para>The value of the parameter.</para></param>       
		public virtual void AddParameter(DbCommand command, string name, SqlDbType dbType, int size, ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
		{
			DbParameter parameter = CreateParameter(name, dbType, size, direction, nullable, precision, scale, sourceColumn, sourceVersion, value);
			command.Parameters.Add(parameter);
		}

		/// <summary>
		/// <para>Adds a new instance of a <see cref="DbParameter"/> object to the command.</para>
		/// </summary>
		/// <param name="command">The command to add the parameter.</param>
		/// <param name="name"><para>The name of the parameter.</para></param>
		/// <param name="dbType"><para>One of the <see cref="SqlDbType"/> values.</para></param>        
		/// <param name="direction"><para>One of the <see cref="ParameterDirection"/> values.</para></param>                
		/// <param name="sourceColumn"><para>The name of the source column mapped to the DataSet and used for loading or returning the <paramref name="value"/>.</para></param>
		/// <param name="sourceVersion"><para>One of the <see cref="DataRowVersion"/> values.</para></param>
		/// <param name="value"><para>The value of the parameter.</para></param>    
		public void AddParameter(DbCommand command, string name, SqlDbType dbType, ParameterDirection direction, string sourceColumn, DataRowVersion sourceVersion, object value)
		{
			AddParameter(command, name, dbType, 0, direction, false, 0, 0, sourceColumn, sourceVersion, value);
		}

		/// <summary>
		/// Adds a new Out <see cref="DbParameter"/> object to the given <paramref name="command"/>.
		/// </summary>
		/// <param name="command">The command to add the out parameter.</param>
		/// <param name="name"><para>The name of the parameter.</para></param>
		/// <param name="dbType"><para>One of the <see cref="SqlDbType"/> values.</para></param>        
		/// <param name="size"><para>The maximum size of the data within the column.</para></param>        
		public void AddOutParameter(DbCommand command, string name, SqlDbType dbType, int size)
		{
			AddParameter(command, name, dbType, size, ParameterDirection.Output, true, 0, 0, String.Empty, DataRowVersion.Default, DBNull.Value);
		}

		/// <summary>
		/// Adds a new In <see cref="DbParameter"/> object to the given <paramref name="command"/>.
		/// </summary>
		/// <param name="command">The command to add the in parameter.</param>
		/// <param name="name"><para>The name of the parameter.</para></param>
		/// <param name="dbType"><para>One of the <see cref="SqlDbType"/> values.</para></param>                
		/// <remarks>
		/// <para>This version of the method is used when you can have the same parameter object multiple times with different values.</para>
		/// </remarks>        
		public void AddInParameter(DbCommand command, string name, SqlDbType dbType)
		{
			AddParameter(command, name, dbType, ParameterDirection.Input, String.Empty, DataRowVersion.Default, null);
		}

		/// <summary>
		/// Adds a new In <see cref="DbParameter"/> object to the given <paramref name="command"/>.
		/// </summary>
		/// <param name="command">The commmand to add the parameter.</param>
		/// <param name="name"><para>The name of the parameter.</para></param>
		/// <param name="dbType"><para>One of the <see cref="SqlDbType"/> values.</para></param>                
		/// <param name="value"><para>The value of the parameter.</para></param>      
		public void AddInParameter(DbCommand command, string name, SqlDbType dbType, object value)
		{
			AddParameter(command, name, dbType, ParameterDirection.Input, String.Empty, DataRowVersion.Default, value);
		}

		/// <summary>
		/// Adds a new In <see cref="DbParameter"/> object to the given <paramref name="command"/>.
		/// </summary>
		/// <param name="command">The command to add the parameter.</param>
		/// <param name="name"><para>The name of the parameter.</para></param>
		/// <param name="dbType"><para>One of the <see cref="SqlDbType"/> values.</para></param>                
		/// <param name="sourceColumn"><para>The name of the source column mapped to the DataSet and used for loading or returning the value.</para></param>
		/// <param name="sourceVersion"><para>One of the <see cref="DataRowVersion"/> values.</para></param>
		public void AddInParameter(DbCommand command, string name, SqlDbType dbType, string sourceColumn, DataRowVersion sourceVersion)
		{
			AddParameter(command, name, dbType, 0, ParameterDirection.Input, true, 0, 0, sourceColumn, sourceVersion, null);
		}

		/// <summary>
		/// <para>Adds a new instance of a <see cref="DbParameter"/> object.</para>
		/// </summary>
		/// <param name="name"><para>The name of the parameter.</para></param>
		/// <param name="dbType"><para>One of the <see cref="DbType"/> values.</para></param>
		/// <param name="size"><para>The maximum size of the data within the column.</para></param>
		/// <param name="direction"><para>One of the <see cref="ParameterDirection"/> values.</para></param>
		/// <param name="nullable"><para>A value indicating whether the parameter accepts <see langword="null"/> (<b>Nothing</b> in Visual Basic) values.</para></param>
		/// <param name="precision"><para>The maximum number of digits used to represent the <paramref name="value"/>.</para></param>
		/// <param name="scale"><para>The number of decimal places to which <paramref name="value"/> is resolved.</para></param>
		/// <param name="sourceColumn"><para>The name of the source column mapped to the DataSet and used for loading or returning the <paramref name="value"/>.</para></param>
		/// <param name="sourceVersion"><para>One of the <see cref="DataRowVersion"/> values.</para></param>
		/// <param name="value"><para>The value of the parameter.</para></param>  
		protected DbParameter CreateParameter(string name, SqlDbType dbType, int size, ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
		{
			SqlParameter param = CreateParameter(name) as SqlParameter;
			ConfigureParameter(param, name, dbType, size, direction, nullable, precision, scale, sourceColumn, sourceVersion, value);
			return param;
		}

		/// <summary>
		/// Configures a given <see cref="DbParameter"/>.
		/// </summary>
		/// <param name="param">The <see cref="DbParameter"/> to configure.</param>
		/// <param name="name"><para>The name of the parameter.</para></param>
		/// <param name="dbType"><para>One of the <see cref="SqlDbType"/> values.</para></param>
		/// <param name="size"><para>The maximum size of the data within the column.</para></param>
		/// <param name="direction"><para>One of the <see cref="ParameterDirection"/> values.</para></param>
		/// <param name="nullable"><para>A value indicating whether the parameter accepts <see langword="null"/> (<b>Nothing</b> in Visual Basic) values.</para></param>
		/// <param name="precision"><para>The maximum number of digits used to represent the <paramref name="value"/>.</para></param>
		/// <param name="scale"><para>The number of decimal places to which <paramref name="value"/> is resolved.</para></param>
		/// <param name="sourceColumn"><para>The name of the source column mapped to the DataSet and used for loading or returning the <paramref name="value"/>.</para></param>
		/// <param name="sourceVersion"><para>One of the <see cref="DataRowVersion"/> values.</para></param>
		/// <param name="value"><para>The value of the parameter.</para></param>  
		protected virtual void ConfigureParameter(SqlParameter param, string name, SqlDbType dbType, int size, ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
		{
			param.SqlDbType = dbType;
			param.Size = size;
			param.Value = (value == null) ? DBNull.Value : value;
			param.Direction = direction;
			param.IsNullable = nullable;
			param.SourceColumn = sourceColumn;
			param.SourceVersion = sourceVersion;
		}
	}
}
