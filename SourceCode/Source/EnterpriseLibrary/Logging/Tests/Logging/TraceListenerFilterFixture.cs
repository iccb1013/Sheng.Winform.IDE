/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests
{
    [TestClass]
    public class TraceListenerFilterFixture
    {
        [TestMethod]
        public void TraceListenerFilterOnEmptyCollectionReturnsHasNoElements()
        {
            TraceListenerFilter filter = new TraceListenerFilter();
            IList traceListenersCollection = new TraceListener[0];
            int i = 0;
            Dictionary<TraceListener, int> listeners = new Dictionary<TraceListener, int>();
            foreach (TraceListener listener in filter.GetAvailableTraceListeners(traceListenersCollection))
            {
                i++;
                listeners[listener] = (listeners.ContainsKey(listener) ? listeners[listener] : 0) + 1;
            }
            Assert.AreEqual(0, i);
        }
        [TestMethod]
        public void TraceListenerFilterOnSingleElementCollectionReturnsHasSingleElement()
        {
            TraceListenerFilter filter = new TraceListenerFilter();
            TraceListener listener1 = new MockTraceListener();
            IList traceListenersCollection = new TraceListener[] { listener1 };
            int i = 0;
            Dictionary<TraceListener, int> listeners = new Dictionary<TraceListener, int>();
            foreach (TraceListener listener in filter.GetAvailableTraceListeners(traceListenersCollection))
            {
                i++;
                listeners[listener] = (listeners.ContainsKey(listener) ? listeners[listener] : 0) + 1;
            }
            Assert.AreEqual(1, i);
            Assert.AreEqual(1, listeners[listener1]);
        }
        [TestMethod]
        public void TraceListenerFilterOnMultipleElementsCollectionReturnsHasSameElements()
        {
            TraceListenerFilter filter = new TraceListenerFilter();
            TraceListener listener1 = new MockTraceListener();
            TraceListener listener2 = new MockTraceListener();
            IList traceListenersCollection = new TraceListener[] { listener1, listener2 };
            int i = 0;
            Dictionary<TraceListener, int> listeners = new Dictionary<TraceListener, int>();
            foreach (TraceListener listener in filter.GetAvailableTraceListeners(traceListenersCollection))
            {
                i++;
                listeners[listener] = (listeners.ContainsKey(listener) ? listeners[listener] : 0) + 1;
            }
            Assert.AreEqual(2, i);
            Assert.AreEqual(1, listeners[listener1]);
            Assert.AreEqual(1, listeners[listener2]);
        }
        [TestMethod]
        public void TraceListenerFilterOnMultipleCollectionsWithDisjointElementsDoesNotRepeatElements()
        {
            TraceListenerFilter filter = new TraceListenerFilter();
            TraceListener listener1 = new MockTraceListener();
            TraceListener listener2 = new MockTraceListener();
            IList traceListenersCollection1 = new TraceListener[] { listener1, listener2 };
            TraceListener listener3 = new MockTraceListener();
            TraceListener listener4 = new MockTraceListener();
            IList traceListenersCollection2 = new TraceListener[] { listener3, listener4 };
            int i = 0;
            Dictionary<TraceListener, int> listeners = new Dictionary<TraceListener, int>();
            foreach (TraceListener listener in filter.GetAvailableTraceListeners(traceListenersCollection1))
            {
                i++;
                listeners[listener] = (listeners.ContainsKey(listener) ? listeners[listener] : 0) + 1;
            }
            foreach (TraceListener listener in filter.GetAvailableTraceListeners(traceListenersCollection2))
            {
                i++;
                listeners[listener] = (listeners.ContainsKey(listener) ? listeners[listener] : 0) + 1;
            }
            Assert.AreEqual(4, i);
            Assert.AreEqual(1, listeners[listener1]);
            Assert.AreEqual(1, listeners[listener2]);
            Assert.AreEqual(1, listeners[listener3]);
            Assert.AreEqual(1, listeners[listener4]);
        }
        [TestMethod]
        public void TraceListenerFilterOnMultipleCollectionsWithCommonElementsDoesNotRepeatElements()
        {
            TraceListenerFilter filter = new TraceListenerFilter();
            TraceListener listener1 = new MockTraceListener();
            TraceListener listener2 = new MockTraceListener();
            IList traceListenersCollection1 = new TraceListener[] { listener1, listener2 };
            TraceListener listener3 = new MockTraceListener();
            TraceListener listener4 = new MockTraceListener();
            IList traceListenersCollection2 = new TraceListener[] { listener2, listener3, listener4 };
            int i = 0;
            Dictionary<TraceListener, int> listeners = new Dictionary<TraceListener, int>();
            foreach (TraceListener listener in filter.GetAvailableTraceListeners(traceListenersCollection1))
            {
                i++;
                listeners[listener] = (listeners.ContainsKey(listener) ? listeners[listener] : 0) + 1;
            }
            foreach (TraceListener listener in filter.GetAvailableTraceListeners(traceListenersCollection2))
            {
                i++;
                listeners[listener] = (listeners.ContainsKey(listener) ? listeners[listener] : 0) + 1;
            }
            Assert.AreEqual(4, i);
            Assert.AreEqual(1, listeners[listener1]);
            Assert.AreEqual(1, listeners[listener2]);
            Assert.AreEqual(1, listeners[listener3]);
            Assert.AreEqual(1, listeners[listener4]);
        }
    }
}
