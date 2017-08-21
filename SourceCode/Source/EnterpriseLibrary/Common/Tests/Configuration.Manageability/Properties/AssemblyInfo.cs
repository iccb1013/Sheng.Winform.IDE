/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Reflection;
using System.Security.Permissions;
using System.Management.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests.Mocks;
[assembly: AssemblyTitle("Enterprise Library Shared Library Manageability Tests")]
[assembly: AssemblyDescription("Enterprise Library Shared Library Manageability Tests")]
[assembly: AssemblyVersion("4.1.0.0")]
[assembly: ConfigurationSectionManageabilityProvider("section1", typeof(MockConfigurationSectionManageabilityProvider))]
[assembly: ConfigurationElementManageabilityProvider(typeof(MockConfigurationSectionManageabilityProviderAlt), typeof(string), typeof(MockConfigurationSectionManageabilityProvider))]
