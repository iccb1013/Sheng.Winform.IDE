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
	public partial class ProviderMappingSetting : NamedConfigurationSetting
	{
		private string databaseTypeName;
		public ProviderMappingSetting(DbProviderMapping sourceElement, string name, string databaseTypeName)
			: base(sourceElement, name)
		{
			this.databaseTypeName = databaseTypeName;
		}
		[ManagementConfiguration]
		public string DatabaseType
		{
			get { return databaseTypeName; }
			set { databaseTypeName = value; }
		}
        [ManagementEnumerator]
        public static IEnumerable<ProviderMappingSetting> GetInstances()
        {
            return NamedConfigurationSetting.GetInstances<ProviderMappingSetting>();
        }
        [ManagementBind]
        public static ProviderMappingSetting BindInstance(string ApplicationName, string SectionName, string Name)
        {
            return NamedConfigurationSetting.BindInstance<ProviderMappingSetting>(ApplicationName, SectionName, Name);
        }
		protected override bool SaveChanges(ConfigurationElement sourceElement)
		{
			return DatabaseSettingsWmiMapper.SaveChanges(this, sourceElement);
		}
	}
}
