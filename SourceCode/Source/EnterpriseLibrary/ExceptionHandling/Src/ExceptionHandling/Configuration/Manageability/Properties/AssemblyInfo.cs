/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Manageability;
[assembly : ConfigurationSectionManageabilityProvider(ExceptionHandlingSettings.SectionName, typeof(ExceptionHandlingSettingsManageabilityProvider))]
[assembly : ConfigurationElementManageabilityProvider(typeof(CustomHandlerDataManageabilityProvider), typeof(CustomHandlerData), typeof(ExceptionHandlingSettingsManageabilityProvider))]
[assembly : ConfigurationElementManageabilityProvider(typeof(ReplaceHandlerDataManageabilityProvider), typeof(ReplaceHandlerData), typeof(ExceptionHandlingSettingsManageabilityProvider))]
[assembly : ConfigurationElementManageabilityProvider(typeof(WrapHandlerDataManageabilityProvider), typeof(WrapHandlerData), typeof(ExceptionHandlingSettingsManageabilityProvider))]
