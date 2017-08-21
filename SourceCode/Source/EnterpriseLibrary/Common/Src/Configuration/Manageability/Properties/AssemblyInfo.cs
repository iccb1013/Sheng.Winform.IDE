/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
[assembly : ConfigurationSectionManageabilityProvider(InstrumentationConfigurationSection.SectionName, typeof(InstrumentationConfigurationSectionManageabilityProvider))]
