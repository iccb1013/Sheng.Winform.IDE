/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration
{
	public class DatabaseSettings : SerializableConfigurationSection
	{
		private const string defaultDatabaseProperty = "defaultDatabase";
		private const string dbProviderMappingsProperty = "providerMappings";
		public const string SectionName = "dataConfiguration";
		public DatabaseSettings()
			: base()
		{
		}
		public static DatabaseSettings GetDatabaseSettings(IConfigurationSource configurationSource)
		{
			return (DatabaseSettings)configurationSource.GetSection(SectionName);
		}
		[ConfigurationProperty(defaultDatabaseProperty, IsRequired = false)]
		public string DefaultDatabase
		{
			get
			{
				return (string)this[defaultDatabaseProperty];
			}
			set
			{
				this[defaultDatabaseProperty] = value;
			}
		}
		[ConfigurationProperty(dbProviderMappingsProperty, IsRequired = false)]
		public NamedElementCollection<DbProviderMapping> ProviderMappings
		{
			get
			{
				return (NamedElementCollection<DbProviderMapping>)base[dbProviderMappingsProperty];
			}
		}
	}
}
