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
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Tests;
using System.Management.Instrumentation;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Tests
{
    [RunInstaller(true)]
    public partial class PerfCounterInstaller : DefaultManagementProjectInstaller
    {
        public PerfCounterInstaller()
        {
            InitializeComponent();
            InstallPerformanceCounters();
        }
        private void InstallPerformanceCounters()
        {
            PerformanceCounterInstaller installer = new PerformanceCounterInstaller();
            installer.CategoryName = EnterpriseLibraryPerformanceCounterFixture.counterCategoryName;
            installer.CategoryHelp = "J Random Text";
            installer.CategoryType = PerformanceCounterCategoryType.MultiInstance;
			CounterCreationData firstCounterData = new CounterCreationData(EnterpriseLibraryPerformanceCounterFixture.counterName, "Test Counter", PerformanceCounterType.NumberOfItems32);
			CounterCreationData secondCounterData = new CounterCreationData("SecondTestCounter", "Second Test Counter", PerformanceCounterType.NumberOfItems32);
            installer.Counters.Add(firstCounterData);
			installer.Counters.Add(secondCounterData);
            Installers.Add(installer);
        }
    }
}
