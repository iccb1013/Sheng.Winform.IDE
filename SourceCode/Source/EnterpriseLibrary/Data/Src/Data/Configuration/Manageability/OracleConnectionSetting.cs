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
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Manageability
{
    [ManagementEntity]
    public partial class OracleConnectionSetting : NamedConfigurationSetting
    {
        string[] packages;
        public OracleConnectionSetting(OracleConnectionData sourceElement,
                                       string name,
                                       string[] packages)
            : base(sourceElement, name)
        {
            this.packages = packages;
        }
        [ManagementConfiguration]
        public string[] Packages
        {
            get { return packages; }
            set { packages = value; }
        }
        [ManagementBind]
        public static OracleConnectionSetting BindInstance(string ApplicationName,
                                                           string SectionName,
                                                           string Name)
        {
            return BindInstance<OracleConnectionSetting>(ApplicationName, SectionName, Name);
        }
        [ManagementEnumerator]
        public static IEnumerable<OracleConnectionSetting> GetInstances()
        {
            return GetInstances<OracleConnectionSetting>();
        }
        protected override bool SaveChanges(ConfigurationElement sourceElement)
        {
            return OracleConnectionSettingsWmiMapper.SaveChanges(this, sourceElement);
        }
    }
}
