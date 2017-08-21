/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Tests
{
    public class NullSubjectSource
    {
        [InstrumentationProvider(null)]
        public event EventHandler<EventArgs> Foo;
        public void NeverCalled()
        {
            Foo(this, new EventArgs());
        }
    }
    public class BaseWithAttributeToOverride
    {
        [InstrumentationConsumer("NeverMatchThis")]
        public virtual void Handle(object sender, EventArgs e)
        {
        }
    }
    public class DerivedListenerWithOverridingAttribute : BaseWithAttributeToOverride
    {
        public bool eventWasRaised;
        [InstrumentationConsumer("Single")]
        public override void Handle(object sender, EventArgs e)
        {
            eventWasRaised = true;
        }
    }
    public class DerivedListenerWithOverriddenNoAttributeListenerMethod : SingleEventListener
    {
        public override void TestHandler(object sender, EventArgs e)
        {
        }
    }
    public class DerivedSingleEventListener : SingleEventListener
    {
    }
    public class SingleEventSource
    {
        [InstrumentationProvider("Single")]
        public event EventHandler<EventArgs> TestEvent;
        public void Raise() { if (TestEvent != null) TestEvent(this, new EventArgs()); }
    }
    public class SingleEventSourceWithOtherEvents
    {
        public event EventHandler<EventArgs> NeverUsedEvent;
        [InstrumentationProvider("Single")]
        public event EventHandler<EventArgs> TestEvent;
        public void Raise() { TestEvent(this, new EventArgs()); }
        public void NeverCalled()
        {
            NeverUsedEvent(this, new EventArgs());
        }
    }
    public class TwoOfSameEventSource
    {
        [InstrumentationProvider("foo")]
        public event EventHandler<EventArgs> FooEvent;
        [InstrumentationProvider("foo")]
        public event EventHandler<EventArgs> BarEvent;
        public void Raise()
        {
            FooEvent(this, new EventArgs());
            BarEvent(this, new EventArgs());
        }
    }
    public class TwoEventSource
    {
        [InstrumentationProvider("Subject1")]
        public event EventHandler<EventArgs> Subject1Event;
        [InstrumentationProvider("Subject2")]
        public event EventHandler<EventArgs> Subject2Event;
        public void Raise()
        {
            Subject1Event(this, new EventArgs());
            Subject2Event(this, new EventArgs());
        }
    }
    public class BaseEventSource
    {
        [InstrumentationProvider("Single")]
        public event EventHandler<EventArgs> BaseEvent;
        public void Raise()
        {
            BaseEvent(this, new EventArgs());
        }
    }
    public class DerivedEventSource : BaseEventSource
    {
    }
    public class SingleEventListener
    {
        public bool eventWasRaised = false;
        [InstrumentationConsumer("Single")]
        public virtual void TestHandler(object sender, EventArgs e)
        {
            eventWasRaised = true;
        }
    }
    public class TwoEventListener
    {
        public string methodCalled = "";
        [InstrumentationConsumer("UnknownListener")]
        public void DoNotCallThis(object sender, EventArgs e)
        {
            methodCalled = "DoNotCallThis";
        }
        [InstrumentationConsumer("Single")]
        public void CallThis(object sender, EventArgs e)
        {
            methodCalled = "CallThis";
        }
    }
    public class CountingEventListener
    {
        public int count = 0;
        [InstrumentationConsumer("foo")]
        public void FooHandler(object sender, EventArgs e)
        {
            count++;
        }
    }
    public class DualAttributedListener
    {
        public int count = 0; 
        [InstrumentationConsumer("Subject1")]
        [InstrumentationConsumer("Subject2")]
        public void Handler(object sender, EventArgs e)
        {
            count++;
        }   
    }
    public class EmptyEventListener
    {
    }
}
