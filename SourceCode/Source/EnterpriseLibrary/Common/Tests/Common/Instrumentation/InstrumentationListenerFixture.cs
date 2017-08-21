/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Instrumentation
{
    [TestClass]
    public class InstrumentationListenerFixture
    {
        [TestMethod]
        public void CreatesMultipleInstancesGivenAnInstanceNameAtCreation()
        {
            TestInstrumentationListener listener = new TestInstrumentationListener("Foo");
            Assert.AreEqual(2, listener.savedInstanceNames.Length);
        }
        [TestMethod]
        public void CreatesOnlySinglePerfCounterInstanceGivenListenerWithNoInstanceName()
        {
            NoNameInstrumentationListener listener = new NoNameInstrumentationListener();
            Assert.AreEqual(1, listener.savedInstanceNames.Length);
        }
        [TestMethod]
        public void CreatesOnlySinglePerfCounterInstanceGivenListenerWithNoDefaultInstanceName()
        {
            NoDefaultNameInstrumentationListener listener = new NoDefaultNameInstrumentationListener("Foo");
            Assert.AreEqual(1, listener.savedInstanceNames.Length);
            Assert.AreEqual("Foo", listener.savedInstanceNames[0]);
        }
        class TestInstrumentationListener : InstrumentationListener
        {
            public string[] savedInstanceNames;
            public TestInstrumentationListener(string instanceName)
                : base(instanceName, true, true, true, new NoPrefixNameFormatter()) {}
            protected override void CreatePerformanceCounters(string[] instanceNames)
            {
                savedInstanceNames = instanceNames;
            }
        }
        class NoNameInstrumentationListener : InstrumentationListener
        {
            public string[] savedInstanceNames;
            public NoNameInstrumentationListener()
                : base(true, true, true, new NoPrefixNameFormatter()) {}
            protected override void CreatePerformanceCounters(string[] instanceNames)
            {
                savedInstanceNames = instanceNames;
            }
        }
        class NoDefaultNameInstrumentationListener : InstrumentationListener
        {
            public string[] savedInstanceNames;
            public NoDefaultNameInstrumentationListener(string instanceName)
                : base(new string[] { instanceName }, true, true, true, new NoPrefixNameFormatter()) {}
            protected override void CreatePerformanceCounters(string[] instanceNames)
            {
                savedInstanceNames = instanceNames;
            }
        }
    }
}
