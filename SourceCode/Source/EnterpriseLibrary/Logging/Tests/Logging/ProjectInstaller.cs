/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.ComponentModel;
using System.Configuration.Install;
using System.Management.Instrumentation;
[assembly: Instrumented(@"root\EnterpriseLibrary")]
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : DefaultManagementProjectInstaller 
    {
        public ProjectInstaller()
        {
            ManagementInstaller managementInstaller = new ManagementInstaller();
            Installers.Add(managementInstaller);
        }
    }
}
