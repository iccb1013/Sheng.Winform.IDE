/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.ComponentModel;
using System.Management.Instrumentation;
[assembly: WmiConfiguration(@"root\EnterpriseLibrary", HostingModel = ManagementHostingModel.Decoupled, IdentifyLevel = false)]
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests.Wmi
{
	[RunInstaller(true)]
	public class Installer : DefaultManagementInstaller
	{
	}
}
