/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.ComponentModel;
using System.Configuration.Install;
using System.Management.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : DefaultManagementProjectInstaller 
    {
        public ProjectInstaller()
        {
			Installers.Add(new ReflectionInstaller<EventLogInstallerBuilder>());
			Installers.Add(new ReflectionInstaller<PerformanceCounterInstallerBuilder>());
			Installers.Add(new DefaultManagementInstaller());
		}
    }
}
