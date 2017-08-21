/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.OracleClient;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle;
using Microsoft.Practices.EnterpriseLibrary.Data.Properties;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
namespace Microsoft.Practices.EnterpriseLibrary.Data
{
	public class DatabaseConfigurationView
	{
		private static readonly DbProviderMapping defaultSqlMapping = new DbProviderMapping(DbProviderMapping.DefaultSqlProviderName, typeof(SqlDatabase));
		private static readonly DbProviderMapping defaultOracleMapping = new DbProviderMapping(DbProviderMapping.DefaultOracleProviderName, typeof(OracleDatabase));
		private static readonly DbProviderMapping defaultGenericMapping = new DbProviderMapping(DbProviderMapping.DefaultGenericProviderName, typeof(GenericDatabase));
		private readonly IConfigurationSource configurationSource;
		public DatabaseConfigurationView(IConfigurationSource configurationSource)
		{
			this.configurationSource = configurationSource;
		}
		public DatabaseSettings DatabaseSettings
		{
			get { return (DatabaseSettings)configurationSource.GetSection(DatabaseSettings.SectionName); }
		}
		public string DefaultName
		{
			get
			{
				DatabaseSettings settings = this.DatabaseSettings;
				string databaseName = settings != null ? settings.DefaultDatabase : null;
				return databaseName;
			}
		}
		public ConnectionStringSettings GetConnectionStringSettings(string name)
		{
			ValidateInstanceName(name);
			ConnectionStringSettings connectionStringSettings;
			ConfigurationSection configSection = configurationSource.GetSection("connectionStrings");
			if ((configSection != null) && (configSection is ConnectionStringsSection))
			{
				ConnectionStringsSection connectionStringsSection = configSection as ConnectionStringsSection;
				connectionStringSettings = connectionStringsSection.ConnectionStrings[name];
			}
			else
				connectionStringSettings = ConfigurationManager.ConnectionStrings[name];
			ValidateConnectionStringSettings(name, connectionStringSettings);
			return connectionStringSettings;
		}
		public IEnumerable<ConnectionStringSettings> GetConnectionStringSettingsCollection()
		{
			ConnectionStringSettingsCollection collection;
			ConfigurationSection configSection = configurationSource.GetSection("connectionStrings");
			if ((configSection != null) && (configSection is ConnectionStringsSection))
			{
				ConnectionStringsSection connectionStringsSection = configSection as ConnectionStringsSection;
				collection = connectionStringsSection.ConnectionStrings;
			}
			else
			{
				collection = ConfigurationManager.ConnectionStrings;
			}
			foreach (ConnectionStringSettings settings in collection)
			{
				yield return settings;
			}
		}
		private void ValidateInstanceName(string name)
		{
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString);
            }
		}
		private static void ValidateDbProviderFactory(string name, DbProviderFactory providerFactory)
		{
			if (providerFactory == null)
			{
				throw new ConfigurationErrorsException(
					string.Format(
						Resources.Culture,
						Resources.ExceptionNoProviderDefinedForConnectionString,
						name));
			}
		}
		private static void ValidateConnectionStringSettings(string name, ConnectionStringSettings connectionStringSettings)
		{
			if (connectionStringSettings == null)
			{
				throw new ConfigurationErrorsException(string.Format(Resources.Culture, Resources.ExceptionNoDatabaseDefined, name));
			}
			if (string.IsNullOrEmpty(connectionStringSettings.ProviderName))
			{
				throw new ConfigurationErrorsException(string.Format(Resources.Culture, Resources.ExceptionNoProviderDefinedForConnectionString, name));
			}
		}
		public DbProviderMapping GetProviderMapping(string name, string dbProviderName)
		{
			DatabaseSettings settings = this.DatabaseSettings;
			if (settings != null)
			{
				DbProviderMapping existingMapping = settings.ProviderMappings.Get(dbProviderName);
				if (existingMapping != null)
				{
					return existingMapping;
				}
			}
			DbProviderMapping defaultMapping = this.GetDefaultMapping(name, dbProviderName);
			if (defaultMapping != null)
			{
				return defaultMapping;
			}
			return this.GetGenericMapping();
		}
		private DbProviderMapping GetDefaultMapping(string name, string dbProviderName)
		{
			if (DbProviderMapping.DefaultSqlProviderName.Equals(dbProviderName))
				return defaultSqlMapping;
			if (DbProviderMapping.DefaultOracleProviderName.Equals(dbProviderName))
				return defaultOracleMapping;
			DbProviderFactory providerFactory = DbProviderFactories.GetFactory(dbProviderName);
			ValidateDbProviderFactory(name, providerFactory);
			if (SqlClientFactory.Instance == providerFactory)
				return defaultSqlMapping;
			if (OracleClientFactory.Instance == providerFactory)
				return defaultOracleMapping;
			return null;
		}
		private DbProviderMapping GetGenericMapping()
		{
			return defaultGenericMapping;
		}
	}
}
