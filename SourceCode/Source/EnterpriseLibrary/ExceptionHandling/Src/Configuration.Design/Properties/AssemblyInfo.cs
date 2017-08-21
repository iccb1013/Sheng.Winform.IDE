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
[assembly : ConfigurationDesignManager(typeof(ExceptionHandlingConfigurationDesignManager))]
[assembly : ReflectionPermission(SecurityAction.RequestMinimum, MemberAccess = true)]
[assembly : SecurityPermission(SecurityAction.RequestMinimum)]
[assembly : ComVisible(false)]
[assembly : AssemblyTitle("Enterprise Library Exception Handling Application Block Design")]
[assembly : AssemblyDescription("Enterprise Library Exception Handling Application Block Design")]
[assembly : AssemblyVersion("4.1.0.0")]
[assembly : AllowPartiallyTrustedCallers]
[assembly : SecurityTransparent]
