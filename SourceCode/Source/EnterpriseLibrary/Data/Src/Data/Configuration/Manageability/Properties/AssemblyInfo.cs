/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Configuration;
[assembly : ConfigurationSectionManageabilityProvider("connectionStrings", typeof(ConnectionStringsManageabilityProvider))]
[assembly : ConfigurationSectionManageabilityProvider(DatabaseSettings.SectionName, typeof(DatabaseSettingsManageabilityProvider))]
[assembly : ConfigurationSectionManageabilityProvider(OracleConnectionSettings.SectionName, typeof(OracleConnectionSettingsManageabilityProvider))]
