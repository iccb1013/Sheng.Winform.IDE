/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Configuration;
using System.Management.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Manageability
{
	[ManagementEntity]
	public partial class ConnectionStringSetting : NamedConfigurationSetting
	{
		private string connectionString;
		private string providerName;
		public ConnectionStringSetting(ConnectionStringSettings sourceElement,
			string name,
			string connectionString,
			string providerName
		)
			: base(sourceElement, name)
		{
			this.connectionString = connectionString;
			this.providerName = providerName;
		}
		[ManagementConfiguration]
		public string ConnectionString
		{
			get { return connectionString; }
			set { connectionString = value; }
		}
		[ManagementConfiguration]
		public string ProviderName
		{
			get { return providerName; }
			set { providerName = value; }
		}
		[ManagementEnumerator]
		public static IEnumerable<ConnectionStringSetting> GetInstances()
		{
			return NamedConfigurationSetting.GetInstances<ConnectionStringSetting>();
		}
		[ManagementBind]
		public static ConnectionStringSetting BindInstance(string ApplicationName, string SectionName, string Name)
		{
			return NamedConfigurationSetting.BindInstance<ConnectionStringSetting>(ApplicationName, SectionName, Name);
		}
		protected override bool SaveChanges(ConfigurationElement sourceElement)
		{
			return ConnectionStringsWmiMapper.SaveChanges(this, sourceElement);
		}
	}
}
