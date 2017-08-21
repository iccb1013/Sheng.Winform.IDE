/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Tests
{
    [TestClass]
    public class EnterpriseLibraryPerformanceCounterFactoryFixture
    {
        static readonly string categoryName = EnterpriseLibraryPerformanceCounterFixture.counterCategoryName;
        static readonly string counterName = EnterpriseLibraryPerformanceCounterFixture.counterName;
        static readonly string differentCounterName = "SecondTestCounter";
        EnterpriseLibraryPerformanceCounterFactory factory;
        [TestInitialize]
        public void SetUp()
        {
            factory = new EnterpriseLibraryPerformanceCounterFactory();
        }
        [TestMethod]
        public void WillCreateEnterpriseLibraryCounterWithSingleEmbeddedCounterWhenGivenSingleInstanceName()
        {
            EnterpriseLibraryPerformanceCounter counter = factory.CreateCounter(categoryName, counterName, new string[] { "foo" });
            PerformanceCounter[] counters = counter.Counters;
            Assert.AreEqual(1, counters.Length);
            Assert.AreEqual("foo", counters[0].InstanceName);
            Assert.AreEqual(counterName, counters[0].CounterName);
        }
        [TestMethod]
        public void WillCreateELCounterWithTwoEmbeddedCountersWhenGivenTwoInstanceNames()
        {
            EnterpriseLibraryPerformanceCounter counter = factory.CreateCounter(categoryName, counterName, new string[] { "foo", "bar" });
            PerformanceCounter[] counters = counter.Counters;
            Assert.AreEqual(2, counters.Length);
            Assert.AreEqual(counterName, counters[0].CounterName);
            Assert.AreEqual("foo", counters[0].InstanceName);
            Assert.AreEqual("bar", counters[1].InstanceName);
        }
        [TestMethod]
        public void WillEmbedSameNamedCounterInMultipleInstancesOfELCounter()
        {
            EnterpriseLibraryPerformanceCounter first = factory.CreateCounter(categoryName, counterName, new string[] { "foo" });
            EnterpriseLibraryPerformanceCounter second = factory.CreateCounter(categoryName, counterName, new string[] { "foo" });
            Assert.AreSame(first.Counters[0], second.Counters[0]);
        }
        [TestMethod]
        public void CounterCreatedThroughFactoryCanBeIncremented()
        {
            EnterpriseLibraryPerformanceCounter counter = factory.CreateCounter(categoryName, counterName, new string[] { "foo", "bar" });
            counter.Clear();
            counter.Increment();
            Assert.AreEqual(1L, counter.Counters[0].RawValue);
            Assert.AreEqual(1L, counter.Counters[1].RawValue);
        }
        [TestMethod]
        public void CreatingTwoDifferentCountersWithSameInstanceNameResultsInTwoSeparateCountersBeingCreated()
        {
            EnterpriseLibraryPerformanceCounter first = factory.CreateCounter(categoryName, counterName, new string[] { "foo" });
            EnterpriseLibraryPerformanceCounter second = factory.CreateCounter(categoryName, differentCounterName, new string[] { "foo" });
            Assert.IsFalse(ReferenceEquals(first.Counters[0], second.Counters[0]));
        }
    }
}
