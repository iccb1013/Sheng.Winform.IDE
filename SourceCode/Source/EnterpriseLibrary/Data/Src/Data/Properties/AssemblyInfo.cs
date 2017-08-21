/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Management.Instrumentation;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Security;
using System.Security.Permissions;
[assembly : SecurityPermission(SecurityAction.RequestMinimum)]
[assembly : ReliabilityContract(Consistency.WillNotCorruptState, Cer.None)]
[assembly : AssemblyTitle("Enterprise Library Data Access Application Block")]
[assembly : AssemblyDescription("Enterprise Library Data Access Application Block")]
[assembly : AssemblyVersion("4.1.0.0")]
[assembly : Instrumented(@"root\EnterpriseLibrary")]
[assembly : WmiConfiguration(@"root\EnterpriseLibrary", HostingModel = ManagementHostingModel.Decoupled, IdentifyLevel = false)]
[assembly : AllowPartiallyTrustedCallers]
[assembly : SecurityTransparent]
