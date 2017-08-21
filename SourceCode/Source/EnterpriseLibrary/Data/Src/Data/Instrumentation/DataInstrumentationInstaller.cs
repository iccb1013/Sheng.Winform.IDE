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
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using System.Collections;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation
{
	[RunInstaller(true)]
    public partial class DataInstrumentationInstaller : DefaultManagementProjectInstaller
    {
		public DataInstrumentationInstaller()
        {
            Installers.Add(new ReflectionInstaller<PerformanceCounterInstallerBuilder>());
            Installers.Add(new ReflectionInstaller<EventLogInstallerBuilder>());
			Installers.Add(new DefaultManagementInstaller());
        }
    }
}
