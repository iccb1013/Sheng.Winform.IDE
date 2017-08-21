/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Management.Instrumentation;
using System.Threading;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation
{
	[RunInstaller(true)]
	public partial class CommonWmiInstaller : DefaultManagementProjectInstaller
	{
		public CommonWmiInstaller()
		{
			InitializeComponent();
			Installers.Add(new ReflectionInstaller<EventLogInstallerBuilder>());
			Installers.Add(new DefaultManagementInstaller());
		}
	}
}
