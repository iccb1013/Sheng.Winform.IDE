/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Instrumentation
{
    [TestClass]
    public class ReflectionInstallerForInternalClassesFixture
    {
        internal class InternalListener
        {
            public bool callbackHappened;
            [InstrumentationConsumer("TestMessage")]
            public void CallMe(object sender,
                               EventArgs e)
            {
                callbackHappened = true;
            }
        }
        public class EventSource
        {
            public void Fire()
            {
                myEvent(this, new EventArgs());
            }
            [InstrumentationProvider("TestMessage")]
            public event EventHandler<EventArgs> myEvent;
        }
        [TestMethod]
        public void InstallerWillWireUpSubjectToPublicMethodInInternalListenerClass()
        {
            InternalListener listener = new InternalListener();
            EventSource source = new EventSource();
            ReflectionInstrumentationBinder binder = new ReflectionInstrumentationBinder();
            binder.Bind(source, listener);
            source.Fire();
            Assert.IsTrue(listener.callbackHappened);
        }
    }
}
