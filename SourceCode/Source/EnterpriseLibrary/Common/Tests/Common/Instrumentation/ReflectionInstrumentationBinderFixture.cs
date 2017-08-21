/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Tests
{
    [TestClass]
    public class ReflectionInstrumentationBinderFixture
    {
        [TestMethod]
        public void AttachesSingleEventToSingleListener()
        {
            SingleEventSource eventSource = new SingleEventSource();
            SingleEventListener eventListener = new SingleEventListener();
            ReflectionInstrumentationBinder binder = new ReflectionInstrumentationBinder();
            binder.Bind(eventSource, eventListener);
            eventSource.Raise();
            Assert.IsTrue(eventListener.eventWasRaised);
        }
        [TestMethod]
        public void AttachesOnlyInstrumentedEventsToListener()
        {
            SingleEventSourceWithOtherEvents eventSource = new SingleEventSourceWithOtherEvents();
            SingleEventListener eventListener = new SingleEventListener();
            ReflectionInstrumentationBinder binder = new ReflectionInstrumentationBinder();
            binder.Bind(eventSource, eventListener);
            eventSource.Raise();
            Assert.IsTrue(eventListener.eventWasRaised);
        }
        [TestMethod]
        public void EventIsAttachedToCorrectListener()
        {
            SingleEventSource eventSource = new SingleEventSource();
            TwoEventListener eventListener = new TwoEventListener();
            ReflectionInstrumentationBinder binder = new ReflectionInstrumentationBinder();
            binder.Bind(eventSource, eventListener);
            eventSource.Raise();
            Assert.AreEqual("CallThis", eventListener.methodCalled);
        }
        [TestMethod]
        public void MultipleEventsAttachedToCorrectListener()
        {
            TwoOfSameEventSource eventSource = new TwoOfSameEventSource();
            CountingEventListener eventListener = new CountingEventListener();
            ReflectionInstrumentationBinder binder = new ReflectionInstrumentationBinder();
            binder.Bind(eventSource, eventListener);
            eventSource.Raise();
            Assert.AreEqual(2, eventListener.count);
        }
        [TestMethod]
        public void TwoDifferentSubjectsCanBeAttactedToSameListener()
        {
            TwoEventSource eventSource = new TwoEventSource();
            DualAttributedListener eventListener = new DualAttributedListener();
            ReflectionInstrumentationBinder binder = new ReflectionInstrumentationBinder();
            binder.Bind(eventSource, eventListener);
            eventSource.Raise();
            Assert.AreEqual(2, eventListener.count);
        }
        [TestMethod]
        public void EventsWithNoListenersDoNotCauseException()
        {
            SingleEventSource eventSource = new SingleEventSource();
            EmptyEventListener eventListener = new EmptyEventListener();
            ReflectionInstrumentationBinder binder = new ReflectionInstrumentationBinder();
            binder.Bind(eventSource, eventListener);
        }
        [TestMethod]
        public void SourceWithNoEventsDoesNotCauseException()
        {
            EmptyEventListener actingAsSourceInThisExample = new EmptyEventListener();
            EmptyEventListener eventListener = new EmptyEventListener();
            ReflectionInstrumentationBinder binder = new ReflectionInstrumentationBinder();
            binder.Bind(actingAsSourceInThisExample, eventListener);
        }
        [TestMethod]
        public void EventsDefinedInBaseAreAttachedToListener()
        {
            DerivedEventSource eventSource = new DerivedEventSource();
            SingleEventListener eventListener = new SingleEventListener();
            ReflectionInstrumentationBinder binder = new ReflectionInstrumentationBinder();
            binder.Bind(eventSource, eventListener);
            eventSource.Raise();
            Assert.IsTrue(eventListener.eventWasRaised);
        }
        [TestMethod]
        public void EventCanAttachToHandlerInListenerBase()
        {
            SingleEventSource eventSource = new SingleEventSource();
            DerivedSingleEventListener eventListener = new DerivedSingleEventListener();
            ReflectionInstrumentationBinder binder = new ReflectionInstrumentationBinder();
            binder.Bind(eventSource, eventListener);
            eventSource.Raise();
            Assert.IsTrue(eventListener.eventWasRaised);
        }
        [TestMethod]
        public void DerivedMethodWithoutAttributeIsNotUsedAsListenerMethod()
        {
            SingleEventSource eventSource = new SingleEventSource();
            DerivedListenerWithOverriddenNoAttributeListenerMethod eventListener =
                new DerivedListenerWithOverriddenNoAttributeListenerMethod();
            ReflectionInstrumentationBinder binder = new ReflectionInstrumentationBinder();
            binder.Bind(eventSource, eventListener);
            eventSource.Raise();
            Assert.IsFalse(eventListener.eventWasRaised);
        }
        [TestMethod]
        public void DerivedMethodWithOverridingAttributeIsUsedAsListenerMethod()
        {
            SingleEventSource eventSource = new SingleEventSource();
            DerivedListenerWithOverridingAttribute eventListener = new DerivedListenerWithOverridingAttribute();
            ReflectionInstrumentationBinder binder = new ReflectionInstrumentationBinder();
            binder.Bind(eventSource, eventListener);
            eventSource.Raise();
        }
        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void NullSubjectInProviderThrowsException()
        {
            NullSubjectSource eventSource = new NullSubjectSource();
            SingleEventListener eventListener = new SingleEventListener();
            ReflectionInstrumentationBinder binder = new ReflectionInstrumentationBinder();
            binder.Bind(eventSource, eventListener);
        }
    }
}
