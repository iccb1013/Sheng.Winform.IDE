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
	public partial class DatabaseBlockSetting : ConfigurationSectionSetting
	{
		private string defaultDatabase;
		public DatabaseBlockSetting(DatabaseSettings sourceElement, string defaultDatabase)
			: base(sourceElement)
		{
			this.defaultDatabase = defaultDatabase;
		}
		[ManagementConfiguration]
		public string DefaultDatabase
		{
			get { return defaultDatabase; }
			set { defaultDatabase = value; }
		}
        [ManagementEnumerator]
        public static IEnumerable<DatabaseBlockSetting> GetInstances()
        {
            return ConfigurationSectionSetting.GetInstances<DatabaseBlockSetting>();
        }
        [ManagementBind]
        public static DatabaseBlockSetting BindInstance(string applicationName, string sectionName)
        {
            return ConfigurationSectionSetting.BindInstance<DatabaseBlockSetting>(applicationName, sectionName);
        }
		protected override bool SaveChanges(ConfigurationElement sourceElement)
		{
			return DatabaseSettingsWmiMapper.SaveChanges(this, sourceElement);
		}
	}
}
