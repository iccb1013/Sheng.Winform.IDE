/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Configuration.Install;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Instrumentation
{
    [TestClass]
    public class EventLogInstallerBuilderFixture
    {
        [TestMethod]
        public void TypeWithNoEventLogsReturnsUnchangedInstaller()
        {
            Installer parentInstaller = new Installer();
            EventLogInstallerBuilder builder = new EventLogInstallerBuilder(new Type[] { typeof(NoLogsType) });
            builder.Fill(parentInstaller);
            Assert.AreEqual(0, parentInstaller.Installers.Count);
        }
        [TestMethod]
        public void LogIsAddedToInstaller()
        {
            Installer parentInstaller = new Installer();
            EventLogInstallerBuilder builder = new EventLogInstallerBuilder(new Type[] { typeof(SimpleLogType) });
            builder.Fill(parentInstaller);
            Assert.AreEqual(1, parentInstaller.Installers.Count);
            Assert.AreSame(typeof(EventLogInstaller), parentInstaller.Installers[0].GetType());
            EventLogInstaller installer = (EventLogInstaller)parentInstaller.Installers[0];
            Assert.AreEqual("FooLog", installer.Log);
            Assert.AreEqual("FooSource", installer.Source);
            Assert.AreEqual(0, installer.CategoryCount);
            Assert.IsNull(installer.CategoryResourceFile);
            Assert.IsNull(installer.MessageResourceFile);
            Assert.IsNull(installer.ParameterResourceFile);
        }
        [TestMethod]
        public void OptionalAttributesCanBeSet()
        {
            Installer parentInstaller = new Installer();
            EventLogInstallerBuilder builder = new EventLogInstallerBuilder(new Type[] { typeof(ComplexLogType) });
            builder.Fill(parentInstaller);
            Assert.AreEqual(1, parentInstaller.Installers.Count);
            Assert.AreSame(typeof(EventLogInstaller), parentInstaller.Installers[0].GetType());
            EventLogInstaller installer = (EventLogInstaller)parentInstaller.Installers[0];
            Assert.AreEqual("BarLog", installer.Log);
            Assert.AreEqual("BarSource", installer.Source);
            Assert.AreEqual(7, installer.CategoryCount);
            Assert.AreEqual("Bar.resources", installer.CategoryResourceFile);
            Assert.AreEqual("BarMessages.resources", installer.MessageResourceFile);
            Assert.AreEqual("BarResources.resources", installer.ParameterResourceFile);
        }
        [TestMethod]
        public void WillFindSingleEventLogTypeInListOfTypes()
        {
            Installer parentInstaller = new Installer();
            EventLogInstallerBuilder builder = new EventLogInstallerBuilder(new Type[] { typeof(NoLogsType), typeof(SimpleLogType) });
            builder.Fill(parentInstaller);
            Assert.AreEqual(1, parentInstaller.Installers.Count);
            Assert.AreSame(typeof(EventLogInstaller), parentInstaller.Installers[0].GetType());
            EventLogInstaller installer = (EventLogInstaller)parentInstaller.Installers[0];
            Assert.AreEqual("FooLog", installer.Log);
        }
        [TestMethod]
        public void NoExceptionThrownIfNoInstrumentedType()
        {
            Installer installer = new Installer();
            EventLogInstallerBuilder builder = new EventLogInstallerBuilder(new Type[] { typeof(NoLogsType) });
            builder.Fill(installer);
        }
        [TestMethod]
        public void SameSourceManyTimesCreatesMultipleInstallers()
        {
            Installer parentInstaller = new Installer();
            EventLogInstallerBuilder builder = new EventLogInstallerBuilder(
                new Type[]
                    {
                        typeof(SimpleLogType),
                        typeof(SimpleLogTypeSameSource)
                    });
            builder.Fill(parentInstaller);
            Assert.AreEqual(2, parentInstaller.Installers.Count);
            Assert.AreSame(typeof(EventLogInstaller), parentInstaller.Installers[0].GetType());
            EventLogInstaller installer1 = (EventLogInstaller)parentInstaller.Installers[0];
            EventLogInstaller installer2 = (EventLogInstaller)parentInstaller.Installers[1];
            Assert.AreEqual("FooLog", installer1.Log);
            Assert.AreEqual("FooSource", installer1.Source);
            Assert.AreEqual("FooLog2", installer2.Log);
            Assert.AreEqual("FooSource", installer2.Source);
        }
        [HasInstallableResources]
        public class NoLogsType {}
        [HasInstallableResourcesAttribute]
        [EventLogDefinition("FooLog", "FooSource")]
        public class SimpleLogType {}
        [HasInstallableResourcesAttribute]
        [EventLogDefinition("FooLog2", "FooSource")]
        public class SimpleLogTypeSameSource {}
        [HasInstallableResourcesAttribute]
        [EventLogDefinition("BarLog", "BarSource",
            CategoryCount = 7,
            CategoryResourceFile = "Bar.resources",
            MessageResourceFile = "BarMessages.resources",
            ParameterResourceFile = "BarResources.resources")]
        public class ComplexLogType {}
    }
}
