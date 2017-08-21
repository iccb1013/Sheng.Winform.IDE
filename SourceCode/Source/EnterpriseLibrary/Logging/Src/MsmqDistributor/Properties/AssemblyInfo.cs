/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Reflection;
using System.Security.Permissions;
using System.Management.Instrumentation;
[assembly : FileIOPermission(SecurityAction.RequestMinimum)]
[assembly : SecurityPermission(SecurityAction.RequestMinimum)]
[assembly : RegistryPermission(SecurityAction.RequestMinimum)]
[assembly : ReflectionPermission(SecurityAction.RequestMinimum, Flags=ReflectionPermissionFlag.MemberAccess)]
[assembly: AssemblyTitle("Enterprise Library Logging Application Block MSMQ Distributor")]
[assembly: AssemblyDescription("Enterprise Library Logging Application Block MSMQ Distributor")]
[assembly: AssemblyVersion("4.1.0.0")]
[assembly: Instrumented(@"root\EnterpriseLibrary")]
[assembly: WmiConfiguration(@"root\EnterpriseLibrary", HostingModel = ManagementHostingModel.Decoupled, IdentifyLevel = false)]
