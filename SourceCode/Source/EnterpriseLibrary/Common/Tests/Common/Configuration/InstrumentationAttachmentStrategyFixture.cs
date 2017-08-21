/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration
{
    [TestClass]
    public class InstrumentationAttachmentStrategyFixture
    {
        DictionaryConfigurationSource instrumentationConfigurationSource;
        InstrumentationAttachmentStrategy strategy;
        ConfigurationReflectionCache reflectionCache;
        [TestInitialize]
        public void CreateConfigurationSource()
        {
            instrumentationConfigurationSource = new DictionaryConfigurationSource();
            instrumentationConfigurationSource.Add(InstrumentationConfigurationSection.SectionName,
                                                   new InstrumentationConfigurationSection(true, true, true, "fooApplicationInstanceName"));
            strategy = new InstrumentationAttachmentStrategy();
            reflectionCache = new ConfigurationReflectionCache();
        }
        [TestMethod]
        public void InstrumentationCanBeAttachedWhenNoInstanceNameIsPresent()
        {
            UnnamedApplicationClass applicationObject = new UnnamedApplicationClass();
            strategy.AttachInstrumentation(applicationObject, instrumentationConfigurationSource, reflectionCache);
            Assert.IsTrue(applicationObject.IsWired);
        }
        [TestMethod]
        public void InstrumentationCanBeAttachedWhenInstanceNameIsPresent()
        {
            NamedApplicationClass applicationObject = new NamedApplicationClass();
            strategy.AttachInstrumentation("InstanceName", applicationObject, instrumentationConfigurationSource, reflectionCache);
            Assert.IsTrue(applicationObject.IsWired);
        }
        [TestMethod]
        public void InstrumentationCanBeAttachedToInstrumentationProviderWhenInstanceNameIsPresent()
        {
            ApplicationClass applicationObject = new ApplicationClass();
            strategy.AttachInstrumentation("InstanceName", applicationObject, instrumentationConfigurationSource, reflectionCache);
            Assert.IsTrue(((InstrumentationProvider)(applicationObject.GetInstrumentationEventProvider())).IsWired);
        }
        public class UnnamedInstrumentationListener
        {
            public UnnamedInstrumentationListener(bool a,
                                                  bool b,
                                                  bool c,
                                                  string applicationInstanceName) {}
            [InstrumentationConsumer("TestSubject")]
            public void TestSubjectMethod(object sender,
                                          EventArgs e) {}
        }
        public class NamedInstrumentationListener
        {
            public NamedInstrumentationListener(string instanceName,
                                                bool a,
                                                bool b,
                                                bool c,
                                                string applicationInstanceName) {}
            [InstrumentationConsumer("TestSubject")]
            public void TestSubjectMethod(object sender,
                                          EventArgs e) {}
        }
        [InstrumentationListener(typeof(NamedInstrumentationListener))]
        public class NamedApplicationClass : IInstrumentationEventProvider
        {
            public bool IsWired
            {
                get { return testEvent != null; }
            }
            [InstrumentationProvider("TestSubject")]
            public event EventHandler<EventArgs> testEvent;
			object IInstrumentationEventProvider.GetInstrumentationEventProvider()
			{
				return this;
			}
		}
        [InstrumentationListener(typeof(UnnamedInstrumentationListener))]
        public class UnnamedApplicationClass : IInstrumentationEventProvider
        {
            public bool IsWired
            {
                get { return testEvent != null; }
            }
            [InstrumentationProvider("TestSubject")]
            public event EventHandler<EventArgs> testEvent;
			object IInstrumentationEventProvider.GetInstrumentationEventProvider()
			{
				return this;
			}
		}
        public class ApplicationClass : IInstrumentationEventProvider
        {
            InstrumentationProvider instrumentationProvider = new InstrumentationProvider();
            public object GetInstrumentationEventProvider()
            {
                return instrumentationProvider;
            }
        }
        [InstrumentationListener(typeof(NamedInstrumentationListener))]
        public class InstrumentationProvider
        {
            public bool IsWired
            {
                get { return testEvent != null; }
            }
            [InstrumentationProvider("TestSubject")]
            public event EventHandler<EventArgs> testEvent;
        }
    }
}
