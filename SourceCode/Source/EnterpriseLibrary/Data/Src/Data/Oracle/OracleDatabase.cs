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
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
using System.Globalization;
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Data.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Oracle
{
	/// <summary>
	/// <para>Represents an Oracle database.</para>
	/// </summary>
	/// <remarks> 
	/// <para>
	/// Internally uses Oracle .NET Managed Provider from Microsoft (<see cref="System.Data.OracleClient"/>) to connect to a Oracle 9i database.
	/// </para>  
	/// <para>
	/// When retrieving a result set, it will build the package name. The package name should be set based
	/// on the stored procedure prefix and this should be set via configuration. For 
	/// example, a package name should be set as prefix of "ENTLIB_" and package name of
	/// "pkgENTLIB_ARCHITECTURE". For your applications, this is required only if you are defining your stored procedures returning 
	/// ref cursors.
	/// </para>
	/// </remarks>
	[OraclePermission(SecurityAction.Demand)]
	[DatabaseAssembler(typeof(OracleDatabaseAssembler))]
	[ContainerPolicyCreator(typeof(OracleDatabasePolicyCreator))]
	public class OracleDatabase : Database
	{
		private const string RefCursorName = "cur_OUT";
		private readonly IList<IOraclePackage> packages;
		private static readonly IList<IOraclePackage> emptyPackages = new List<IOraclePackage>(0);
		private readonly IDictionary<string, ParameterTypeRegistry> registeredParameterTypes 
			= new Dictionary<string, ParameterTypeRegistry>();

		/// <summary>
		/// Initializes a new instance of the <see cref="OracleDatabase"/> class with a connection string and a list of Oracle packages.
		/// </summary>
		/// <param name="connectionString">The connection string for the database.</param>
		public OracleDatabase(string connectionString)
			: this(connectionString, emptyPackages)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OracleDatabase"/> class with a connection string and a list of Oracle packages.
		/// </summary>
		/// <param name="connectionString">The connection string for the database.</param>
		/// <param name="packages">A list of <see cref="IOraclePackage"/> objects.</param>
		public OracleDatabase(string connectionString, IList<IOraclePackage> packages)
			: base(connectionString, OracleClientFactory.Instance)
		{
			if (packages == null) throw new ArgumentNullException("packages");

			this.packages = packages;
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
		public override void AddParameter(DbCommand command, string name, DbType dbType, int size,
			ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn,
			DataRowVersion sourceVersion, object value)
		{
			if (DbType.Guid.Equals(dbType))
			{
				object convertedValue = ConvertGuidToByteArray(value);

				AddParameter((OracleCommand)command, name, OracleType.Raw, 16, direction, nullable, precision,
					scale, sourceColumn, sourceVersion, convertedValue);

				RegisterParameterType(command, name, dbType);
			}
			else
			{
				base.AddParameter(command, name, dbType, size, direction, nullable, precision, scale,
					sourceColumn, sourceVersion, value);
			}
		}

		/// <summary>
		/// <para>Adds a new instance of an <see cref="OracleParameter"/> object to the command.</para>
		/// </summary>
		/// <param name="command">The <see cref="OracleCommand"/> to add the parameter.</param>
		/// <param name="name"><para>The name of the parameter.</para></param>
		/// <param name="oracleType"><para>One of the <see cref="OracleType"/> values.</para></param>
		/// <param name="size"><para>The maximum size of the data within the column.</para></param>
		/// <param name="direction"><para>One of the <see cref="ParameterDirection"/> values.</para></param>
		/// <param name="nullable"><para>A value indicating whether the parameter accepts <see langword="null"/> (<b>Nothing</b> in Visual Basic) values.</para></param>
		/// <param name="precision"><para>The maximum number of digits used to represent the <paramref name="value"/>.</para></param>
		/// <param name="scale"><para>The number of decimal places to which <paramref name="value"/> is resolved.</para></param>
		/// <param name="sourceColumn"><para>The name of the source column mapped to the DataSet and used for loading or returning the <paramref name="value"/>.</para></param>
		/// <param name="sourceVersion"><para>One of the <see cref="DataRowVersion"/> values.</para></param>
		/// <param name="value"><para>The value of the parameter.</para></param>      
		public void AddParameter(OracleCommand command, string name, OracleType oracleType, int size,
			ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn,
			DataRowVersion sourceVersion, object value)
		{
			OracleParameter param = CreateParameter(name, DbType.AnsiString, size, direction, nullable, precision, scale, sourceColumn, sourceVersion, value) as OracleParameter;
			param.OracleType = oracleType;
			command.Parameters.Add(param);
		}

		/// <summary>
		/// Creates an <see cref="OracleDataReader"/> based on the <paramref name="command"/>.
		/// </summary>
		/// <param name="command">The command wrapper to execute.</param>        
		/// <returns>An <see cref="OracleDataReader"/> object.</returns>        
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="command"/> can not be <see langword="null"/> (Nothing in Visual Basic).</para>
		/// </exception>
		public override IDataReader ExecuteReader(DbCommand command)
		{
			PrepareCWRefCursor(command);
			return new OracleDataReaderWrapper((OracleDataReader)base.ExecuteReader(command));
		}

		/// <summary>
		/// <para>Creates an <see cref="OracleDataReader"/> based on the <paramref name="command"/>.</para>
		/// </summary>        
		/// <param name="command"><para>The command wrapper to execute.</para></param>        
		/// <param name="transaction"><para>The transaction to participate in when executing this reader.</para></param>        
		/// <returns><para>An <see cref="OracleDataReader"/> object.</para></returns>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="command"/> can not be <see langword="null"/> (Nothing in Visual Basic).</para>
		/// <para>- or -</para>
		/// <para><paramref name="transaction"/> can not be <see langword="null"/> (Nothing in Visual Basic).</para>
		/// </exception>
		public override IDataReader ExecuteReader(DbCommand command, DbTransaction transaction)
		{
			PrepareCWRefCursor(command);
			return new OracleDataReaderWrapper((OracleDataReader)base.ExecuteReader(command, transaction));
		}

		/// <summary>
		/// <para>Executes a command and returns the results in a new <see cref="DataSet"/>.</para>
		/// </summary>
		/// <param name="command"><para>The command to execute to fill the <see cref="DataSet"/></para></param>
		/// <returns><para>A <see cref="DataSet"/> filed with records and, if necessary, schema.</para></returns>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="command"/> can not be <see langword="null"/> (Nothing in Visual Basic).</para>
		/// </exception>
		public override DataSet ExecuteDataSet(DbCommand command)
		{
			PrepareCWRefCursor(command);
			return base.ExecuteDataSet(command);
		}

		/// <summary>
		/// <para>Executes a command and returns the result in a new <see cref="DataSet"/>.</para>
		/// </summary>
		/// <param name="command"><para>The command to execute to fill the <see cref="DataSet"/></para></param>
		/// <param name="transaction"><para>The transaction to participate in when executing this reader.</para></param>        
		/// <returns><para>A <see cref="DataSet"/> filed with records and, if necessary, schema.</para></returns>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="command"/> can not be <see langword="null"/> (<b>Nothing</b> in Visual Basic).</para>
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="command"/> can not be <see langword="null"/> (<b>Nothing</b> in Visual Basic).</para>
		/// <para>- or -</para>
		/// <para><paramref name="transaction"/> can not be <see langword="null"/> (<b>Nothing</b> in Visual Basic).</para>
		/// </exception>
		public override DataSet ExecuteDataSet(DbCommand command, DbTransaction transaction)
		{
			PrepareCWRefCursor(command);
			return base.ExecuteDataSet(command, transaction);
		}

		/// <summary>
		/// <para>Loads a <see cref="DataSet"/> from a <see cref="DbCommand"/>.</para>
		/// </summary>
		/// <param name="command">
		/// <para>The command to execute to fill the <see cref="DataSet"/>.</para>
		/// </param>
		/// <param name="dataSet">
		/// <para>The <see cref="DataSet"/> to fill.</para>
		/// </param>
		/// <param name="tableNames">
		/// <para>An array of table name mappings for the <see cref="DataSet"/>.</para>
		/// </param>
		public override void LoadDataSet(DbCommand command, DataSet dataSet, string[] tableNames)
		{
			PrepareCWRefCursor(command);
			base.LoadDataSet(command, dataSet, tableNames);
		}

		/// <summary>
		/// <para>Loads a <see cref="DataSet"/> from a <see cref="DbCommand"/> in a transaction.</para>
		/// </summary>
		/// <param name="command">
		/// <para>The command to execute to fill the <see cref="DataSet"/>.</para>
		/// </param>
		/// <param name="dataSet">
		/// <para>The <see cref="DataSet"/> to fill.</para>
		/// </param>
		/// <param name="tableNames">
		/// <para>An array of table name mappings for the <see cref="DataSet"/>.</para>
		/// </param>
		/// <param name="transaction">
		/// <para>The <see cref="IDbTransaction"/> to execute the command in.</para>
		/// </param>
		public override void LoadDataSet(DbCommand command, DataSet dataSet, string[] tableNames, DbTransaction transaction)
		{
			PrepareCWRefCursor(command);
			base.LoadDataSet(command, dataSet, tableNames, transaction);
		}

		/// <summary>
		/// Gets a parameter value.
		/// </summary>
		/// <param name="command">The command that contains the parameter.</param>
		/// <param name="parameterName">The name of the parameter.</param>
		/// <returns>The value of the parameter.</returns>
		public override object GetParameterValue(DbCommand command, string parameterName)
		{
			object convertedValue = base.GetParameterValue(command, parameterName);

			ParameterTypeRegistry registry = GetParameterTypeRegistry(command.CommandText);
			if (registry != null)
			{
				if (registry.HasRegisteredParameterType(parameterName))
				{
					DbType dbType = registry.GetRegisteredParameterType(parameterName);

					if (DbType.Guid == dbType)
					{
						convertedValue = ConvertByteArrayToGuid(convertedValue);
					}
					else if (DbType.Boolean == dbType)
					{
						convertedValue = Convert.ToBoolean(convertedValue, CultureInfo.InvariantCulture);
					}
				}
			}

			return convertedValue;
		}

		/// <summary>
		/// Sets a parameter value.
		/// </summary>
		/// <param name="command">The command with the parameter.</param>
		/// <param name="parameterName">The parameter name.</param>
		/// <param name="value">The parameter value.</param>
		public override void SetParameterValue(DbCommand command, string parameterName, object value)
		{
			object convertedValue = value;

			ParameterTypeRegistry registry = GetParameterTypeRegistry(command.CommandText);
			if (registry != null)
			{
				if (registry.HasRegisteredParameterType(parameterName))
				{
					DbType dbType = registry.GetRegisteredParameterType(parameterName);

					if (DbType.Guid == dbType)
					{
						convertedValue = ConvertGuidToByteArray(value);
					}
				}
			}

			base.SetParameterValue(command, parameterName, convertedValue);
		}

		/// <devdoc>
		/// This is a private method that will build the Oracle package name if your stored procedure
		/// has proper prefix and postfix. 
		/// This functionality is include for
		/// the portability of the architecture between SQL and Oracle datbase.
		/// This method also adds the reference cursor to the command writer if not already added. This
		/// is required for Oracle .NET managed data provider.
		/// </devdoc>        
		private void PrepareCWRefCursor(DbCommand command)
		{
			if (command == null) throw new ArgumentNullException("command");

			if (CommandType.StoredProcedure == command.CommandType)
			{
				// Check for ref. cursor in the command writer, if it does not exist, add a know reference cursor out
				// of "cur_OUT"
				if (QueryProcedureNeedsCursorParameter(command))
				{
					AddParameter(command as OracleCommand, RefCursorName, OracleType.Cursor, 0, ParameterDirection.Output, true, 0, 0, String.Empty, DataRowVersion.Default, Convert.DBNull);
				}
			}
		}

		private ParameterTypeRegistry GetParameterTypeRegistry(string commandText)
		{
			ParameterTypeRegistry registry;
			registeredParameterTypes.TryGetValue(commandText, out registry);
			return registry;
		}


		private void RegisterParameterType(DbCommand command, string parameterName, DbType dbType)
		{
			ParameterTypeRegistry registry = GetParameterTypeRegistry(command.CommandText);
			if (registry == null)
			{
				registry = new ParameterTypeRegistry(command.CommandText);
				registeredParameterTypes.Add(command.CommandText, registry);
			}

			registry.RegisterParameterType(parameterName, dbType);
		}

		private static object ConvertGuidToByteArray(object value)
		{
			return ((value is DBNull) || (value == null)) ? Convert.DBNull : ((Guid)value).ToByteArray();
		}

		private static object ConvertByteArrayToGuid(object value)
		{
			byte[] buffer = (byte[])value;
			if (buffer.Length == 0)
			{
				return DBNull.Value;
			}
			else
			{
				return new Guid(buffer);
			}
		}

		private static bool QueryProcedureNeedsCursorParameter(DbCommand command)
		{
			foreach (OracleParameter parameter in command.Parameters)
			{
				if (parameter.OracleType == OracleType.Cursor)
				{
					return false;
				}
			}
			return true;
		}

		/// <devdoc>
		/// Listens for the RowUpdate event on a data adapter to support UpdateBehavior.Continue
		/// </devdoc>
		private void OnOracleRowUpdated(object sender, OracleRowUpdatedEventArgs args)
		{
			if (args.RecordsAffected == 0)
			{
				if (args.Errors != null)
				{
					args.Row.RowError = Resources.ExceptionMessageUpdateDataSetRowFailure;
					args.Status = UpdateStatus.SkipCurrentRow;
				}
			}
		}

		/// <summary>
		/// Retrieves parameter information from the stored procedure specified in the <see cref="DbCommand"/> and populates the Parameters collection of the specified <see cref="DbCommand"/> object. 
		/// </summary>
		/// <param name="discoveryCommand">The <see cref="DbCommand"/> to do the discovery.</param>
		/// <remarks>
		/// The <see cref="DbCommand"/> must be an instance of a <see cref="OracleCommand"/> object.
		/// </remarks>
		protected override void DeriveParameters(DbCommand discoveryCommand)
		{
			OracleCommandBuilder.DeriveParameters((OracleCommand)discoveryCommand);
		}

		/// <summary>
		/// <para>Creates a <see cref="DbCommand"/> for a stored procedure.</para>
		/// </summary>
		/// <param name="storedProcedureName"><para>The name of the stored procedure.</para></param>
		/// <param name="parameterValues"><para>The list of parameters for the procedure.</para></param>
		/// <returns><para>The <see cref="DbCommand"/> for the stored procedure.</para></returns>
		/// <remarks>
		/// <para>The parameters for the stored procedure will be discovered and the values are assigned in positional order.</para>
		/// </remarks>        
		public override DbCommand GetStoredProcCommand(string storedProcedureName, params object[] parameterValues)
		{
			// need to do this before of eventual parameter discovery
			string updatedStoredProcedureName = TranslatePackageSchema(storedProcedureName);
			DbCommand command = base.GetStoredProcCommand(updatedStoredProcedureName, parameterValues);
			return command;
		}

		/// <summary>
		/// <para>Creates a <see cref="DbCommand"/> for a stored procedure.</para>
		/// </summary>
		/// <param name="storedProcedureName"><para>The name of the stored procedure.</para></param>		
		/// <returns><para>The <see cref="DbCommand"/> for the stored procedure.</para></returns>
		/// <remarks>
		/// <para>The parameters for the stored procedure will be discovered and the values are assigned in positional order.</para>
		/// </remarks>        
		public override DbCommand GetStoredProcCommand(string storedProcedureName)
		{
			// need to do this before of eventual parameter discovery
			string updatedStoredProcedureName = TranslatePackageSchema(storedProcedureName);
			DbCommand command = base.GetStoredProcCommand(updatedStoredProcedureName);
			return command;
		}

		/// <devdoc>
		/// Looks into configuration and gets the information on how the command wrapper should be updated if calling a package on this
		/// connection.
		/// </devdoc>        
		private string TranslatePackageSchema(string storedProcedureName)
		{
			const string allPrefix = "*";
			string packageName = String.Empty;
			string updatedStoredProcedureName = storedProcedureName;

			if (packages != null && !string.IsNullOrEmpty(storedProcedureName))
			{
				foreach (IOraclePackage oraPackage in packages)
				{
					if ((oraPackage.Prefix == allPrefix) || (storedProcedureName.StartsWith(oraPackage.Prefix)))
					{
						//use the package name for the matching prefix
						packageName = oraPackage.Name;
						//prefix = oraPackage.Prefix;
						break;
					}
				}
			}
			if (0 != packageName.Length)
			{
				updatedStoredProcedureName = String.Format(CultureInfo.InvariantCulture, "{0}.{1}", packageName, storedProcedureName);
			}

			return updatedStoredProcedureName;
		}

		/// <summary>
		/// Sets the RowUpdated event for the data adapter.
		/// </summary>
		/// <param name="adapter">The <see cref="DbDataAdapter"/> to set the event.</param>
		/// <remarks>The <see cref="DbDataAdapter"/> must be an <see cref="OracleDataAdapter"/>.</remarks>
		protected override void SetUpRowUpdatedEvent(DbDataAdapter adapter)
		{
			((OracleDataAdapter)adapter).RowUpdated += OnOracleRowUpdated;
		}
	}

	internal sealed class ParameterTypeRegistry
	{
		private string commandText;
		private IDictionary<string, DbType> parameterTypes;

		internal ParameterTypeRegistry(string commandText)
		{
			this.commandText = commandText;
			this.parameterTypes = new Dictionary<string, DbType>();
		}

		internal void RegisterParameterType(string parameterName, DbType parameterType)
		{
			this.parameterTypes[parameterName] = parameterType;
		}

		internal bool HasRegisteredParameterType(string parameterName)
		{
			return this.parameterTypes.ContainsKey(parameterName);
		}

		internal DbType GetRegisteredParameterType(string parameterName)
		{
			return this.parameterTypes[parameterName];
		}
	}
}
