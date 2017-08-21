/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design;
using ConfigurationDesignManager=Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration.Design.ConfigurationDesignManager;
[assembly : ConfigurationDesignManager(typeof(ConfigurationDesignManager), typeof(ExceptionHandlingConfigurationDesignManager))]
[assembly : ReflectionPermission(SecurityAction.RequestMinimum, MemberAccess = true)]
[assembly : SecurityPermission(SecurityAction.RequestMinimum)]
[assembly : ComVisible(false)]
[assembly : AssemblyTitle("Enterprise Library Exception Handling WCF Provider Design")]
[assembly : AssemblyDescription("Enterprise Library Exception Handling WCF Provider Design")]
[assembly : AssemblyVersion("4.1.0.0")]
[assembly : AllowPartiallyTrustedCallers]
[assembly : SecurityTransparent]
