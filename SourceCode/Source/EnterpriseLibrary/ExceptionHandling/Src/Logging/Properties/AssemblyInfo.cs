/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Reflection;
using System.Security.Permissions;
using System.Runtime.ConstrainedExecution;
using System.Security;
[assembly: ReliabilityContract(Consistency.WillNotCorruptState, Cer.None)]
[assembly : AssemblyTitle("Enterprise Library Exception Handling Logging Provider")]
[assembly : AssemblyDescription("Enterprise Library Exception Handling Logging Provider")]
[assembly: AssemblyVersion("4.1.0.0")]
[assembly: AllowPartiallyTrustedCallers]
[assembly: SecurityTransparent]
