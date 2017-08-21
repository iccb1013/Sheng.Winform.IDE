/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.ComponentModel;
using System.Management.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
namespace Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation
{
	[RunInstaller(true)]
	public partial class CachingInstrumentationInstaller : DefaultManagementProjectInstaller
	{
		public CachingInstrumentationInstaller()
		{
			InitializeComponent();
			Installers.Add(new ReflectionInstaller<PerformanceCounterInstallerBuilder>());
			Installers.Add(new ReflectionInstaller<EventLogInstallerBuilder>());
			Installers.Add(new DefaultManagementInstaller());
		}
	}
}
