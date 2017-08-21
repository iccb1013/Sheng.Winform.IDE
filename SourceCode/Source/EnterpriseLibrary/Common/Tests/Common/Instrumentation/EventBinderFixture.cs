/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Tests
{
    [TestClass]
    public class EventBinderFixture
    {
        [TestMethod]
        public void AttachesEventToSubject()
        {
            Publisher publisher = new Publisher();
            EventInfo eventInfo = GetMemberInfo<EventInfo>(publisher, "FooEvent");
            Subscriber subscriber = new Subscriber();
            MethodInfo methodInfo = GetMemberInfo<MethodInfo>(subscriber, "HookMeUp");
            EventBinder binder = new EventBinder(publisher, subscriber);
            binder.Bind(eventInfo, methodInfo);
            publisher.Raise();
            Assert.IsTrue(subscriber.EventRaised);
        }
        TMemberInfo GetMemberInfo<TMemberInfo>(object targetObject,
                                               string name) where TMemberInfo : MemberInfo
        {
            Type type = targetObject.GetType();
            MemberInfo memberInfo = type.GetMember(name)[0];
            return (TMemberInfo)memberInfo;
        }
        public class Publisher
        {
            public delegate void FooDelegate();
            public event FooDelegate FooEvent;
            public void Raise()
            {
                FooEvent();
            }
        }
        public class Subscriber
        {
            public bool EventRaised = false;
            public void HookMeUp()
            {
                EventRaised = true;
            }
        }
    }
}
