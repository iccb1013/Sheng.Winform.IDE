/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Tests
{
    [TestClass]
    public class WMIEventExplorationFixture
    {
        [TestMethod]
        public void FireSimpleEvent()
        {
            using (WmiEventWatcher watcher = new WmiEventWatcher(1))
            {
                BaseWmiEvent myEvent = new TestEvent("Hello, World");
                System.Management.Instrumentation.Instrumentation.Fire(myEvent);
                watcher.WaitForEvents();
            }
        }
        [TestMethod]
        public void Send100Events()
        {
            using (WmiEventWatcher watcher = new WmiEventWatcher(100))
            {
                for (int i = 0; i < 100; i++)
                {
                    BaseWmiEvent myEvent = new TestEvent("" + i);
                    System.Management.Instrumentation.Instrumentation.Fire(myEvent);
                }
                watcher.WaitForEvents();
                Assert.AreEqual(100, watcher.EventsReceived.Count);
                Assert.AreEqual("50", watcher.EventsReceived[50].Properties["Text"].Value);
            }
        }
    }
}
