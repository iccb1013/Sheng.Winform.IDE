/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests
{
    [TestClass]
    public class InstrumentationInjectionFixture
    {
        public static bool wasCalled;
        DictionaryConfigurationSource configSource;
        [TestInitialize]
        public void SetUp()
        {
            wasCalled = false;
            configSource = new DictionaryConfigurationSource();
            configSource.Add(InstrumentationConfigurationSection.SectionName,
                             new InstrumentationConfigurationSection(true, true, true, "fooApplicationInstanceName"));
        }
        [TestMethod]
        public void InstrumentationIsWiredUpCorrectlyWhenConfigurationSectionIsPresent()
        {
            EventSource source
                = EnterpriseLibraryFactory.BuildUp<EventSource>("ignore", configSource);
            Assert.IsTrue(source.IsWired);
        }
        [TestMethod]
        public void InstrumentationAvoidsTryingToWireUpToObjectsWithNoListenerDefined()
        {
            NoListenerEventSource source
                = EnterpriseLibraryFactory.BuildUp<NoListenerEventSource>("ignore", configSource);
            Assert.IsFalse(source.IsWired);
        }
        [TestMethod]
        public void InstrumentationIsAttachedWhenInstrumentedAttributeIsInBaseClass()
        {
            DerivedEventSource source = EnterpriseLibraryFactory.BuildUp<DerivedEventSource>("ignore", configSource);
            Assert.IsTrue(source.IsWired);
        }
        [TestMethod]
        public void InstrumentationNotWiredWhenConfigurationSectionNotPresent()
        {
            EventSource source
                = EnterpriseLibraryFactory.BuildUp<EventSource>("ignore", new DictionaryConfigurationSource());
            Assert.IsFalse(source.IsWired);
        }
        [TestMethod]
        public void InstrumentationNotWiredWhenConfigurationValuesAllFalse()
        {
            DictionaryConfigurationSource section = new DictionaryConfigurationSource();
            section.Add(InstrumentationConfigurationSection.SectionName,
                        new InstrumentationConfigurationSection(false, false, false, "fooApplicationInstanceName"));
            EventSource source
                = EnterpriseLibraryFactory.BuildUp<EventSource>("ignore", new DictionaryConfigurationSource());
            Assert.IsFalse(source.IsWired);
        }
        [CustomFactory(typeof(MockCustomFactory<DerivedEventSource>))]
        public class DerivedEventSource : EventSource
        {
            public DerivedEventSource()
                : base() {}
        }
        [CustomFactory(typeof(MockCustomFactory<NoListenerEventSource>))]
        public class NoListenerEventSource
        {
            public NoListenerEventSource() {}
            public bool IsWired
            {
                get { return TestEvent != null; }
            }
            [InstrumentationProvider("MySubject")]
            public event EventHandler<EventArgs> TestEvent;
        }
        [CustomFactory(typeof(MockCustomFactory<EventSource>))]
        [InstrumentationListener(typeof(EventListener))]
        public class EventSource : IInstrumentationEventProvider
        {
            public EventSource() {}
            public bool IsWired
            {
                get { return TestEvent != null; }
            }
            [InstrumentationProvider("MySubject")]
            public event EventHandler<EventArgs> TestEvent;
			object IInstrumentationEventProvider.GetInstrumentationEventProvider()
			{
				return this;
			}
		}
        public class EventListener
        {
            public EventListener(string instanceName,
                                 bool a,
                                 bool b,
                                 bool c,
                                 string applicationInstanceName) {}
            [InstrumentationConsumer("MySubject")]
            public void CallMe(object sender,
                               EventArgs e)
            {
                wasCalled = true;
            }
        }
        public class MockCustomFactory<T> : ICustomFactory
            where T : new()
        {
            public object CreateObject(IBuilderContext context,
                                       string name,
                                       IConfigurationSource configurationSource,
                                       ConfigurationReflectionCache reflectionCache)
            {
                return new T();
            }
        }
    }
}
