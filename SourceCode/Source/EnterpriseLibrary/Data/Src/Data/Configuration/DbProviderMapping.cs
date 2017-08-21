/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration
{
	public class DbProviderMapping : NamedConfigurationElement
	{
        private static AssemblyQualifiedTypeNameConverter typeConverter = new AssemblyQualifiedTypeNameConverter();
		public const string DefaultSqlProviderName = "System.Data.SqlClient";
		public const string DefaultOracleProviderName = "System.Data.OracleClient";
		internal const string DefaultGenericProviderName = "generic";
		private const string databaseTypeProperty = "databaseType";
		public DbProviderMapping()
		{
		}
		public DbProviderMapping(string dbProviderName, Type databaseType)
            : this(dbProviderName, (string) typeConverter.ConvertTo(databaseType, typeof(string)))
		{
		}
        public DbProviderMapping(string dbProviderName, string databaseTypeName)
            : base(dbProviderName)
        {
            this.DatabaseTypeName = databaseTypeName;
        }
        public Type DatabaseType
        {
            get { return (Type)typeConverter.ConvertFrom(DatabaseTypeName); }
            set { DatabaseTypeName = typeConverter.ConvertToString(value); }
        }
        [ConfigurationProperty(databaseTypeProperty)]
        public string DatabaseTypeName
        {
            get { return (string)this[databaseTypeProperty]; }
            set { this[databaseTypeProperty] = value; }
        }
		public string DbProviderName
		{
			get { return Name; }
		}
	}
}
