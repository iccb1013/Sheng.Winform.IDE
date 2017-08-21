/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Instrumentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests
{
    [TestClass]
    public class ExceptionHandlerInstrumentationListenerFixture
    {
        const string policyName = "policy";
        const string exceptionMessage = "exception message";
        const string counterCategoryName = "Enterprise Library Exception Handling Counters";
        const string TotalExceptionHandlersExecuted = "Total Exception Handlers Executed";
        const string TotalExceptionsHandled = "Total Exceptions Handled";
        const string instanceName = "Foo";
        string formattedInstanceName;
        ExceptionHandlingInstrumentationListener listener;
        IPerformanceCounterNameFormatter nameFormatter;
        EnterpriseLibraryPerformanceCounter totalExceptionHandlersExecuted;
        EnterpriseLibraryPerformanceCounter totalExceptionsHandled;
        ExceptionPolicyImpl exceptionPolicy;
        [TestInitialize]
        public void SetUp()
        {
            IExceptionHandler[] handlers = new IExceptionHandler[] { new MockThrowingExceptionHandler() };
            ExceptionPolicyEntry policyEntry = new ExceptionPolicyEntry(PostHandlingAction.None, handlers);
            Dictionary<Type, ExceptionPolicyEntry> policyEntries = new Dictionary<Type, ExceptionPolicyEntry>();
            policyEntries.Add(typeof(ArgumentException), policyEntry);
            exceptionPolicy = new ExceptionPolicyImpl(policyName, policyEntries);
            ReflectionInstrumentationAttacher attacher
                = new ReflectionInstrumentationAttacher(
                    exceptionPolicy.GetInstrumentationEventProvider(),
                    typeof(ExceptionHandlingInstrumentationListener),
                    new object[] { policyName, true, true, true, "ApplicationInstanceName" });
            attacher.BindInstrumentation();
            nameFormatter = new FixedPrefixNameFormatter("Prefix - ");
            listener = new ExceptionHandlingInstrumentationListener(instanceName, true, true, true, nameFormatter);
            formattedInstanceName = nameFormatter.CreateName(instanceName);
            totalExceptionHandlersExecuted = new EnterpriseLibraryPerformanceCounter(counterCategoryName, TotalExceptionHandlersExecuted, formattedInstanceName);
            totalExceptionsHandled = new EnterpriseLibraryPerformanceCounter(counterCategoryName, TotalExceptionsHandled, formattedInstanceName);
        }
        [TestMethod]
        public void TotalExceptionHandlersExecutedCounterIncremented()
        {
            listener.ExceptionHandlerExecuted(this, EventArgs.Empty);
            long expected = 1;
            Assert.AreEqual(expected, totalExceptionHandlersExecuted.Value);
        }
        [TestMethod]
        public void TotalExceptionsHandledCounterIncremented()
        {
            listener.ExceptionHandled(this, EventArgs.Empty);
            long expected = 1;
            Assert.AreEqual(expected, totalExceptionsHandled.Value);
        }
        [TestMethod]
        public void FailureHandlingExceptionWritesToEventLog()
        {
            ArgumentException exception = new ArgumentException(exceptionMessage);
            using (EventLog eventLog = GetEventLog())
            {
                int eventCount = eventLog.Entries.Count;
                try
                {
                    exceptionPolicy.HandleException(exception);
                }
                catch (ExceptionHandlingException) {}
                Assert.AreEqual(eventCount + 1, eventLog.Entries.Count);
                Assert.IsTrue(eventLog.Entries[eventCount].Message.IndexOf(exceptionMessage) > -1);
            }
        }
        static EventLog GetEventLog()
        {
            return new EventLog("Application", ".", "Enterprise Library ExceptionHandling");
        }
        public class FixedPrefixNameFormatter : IPerformanceCounterNameFormatter
        {
            string prefix;
            public FixedPrefixNameFormatter(string prefix)
            {
                this.prefix = prefix;
            }
            public string CreateName(string nameSuffix)
            {
                return prefix + nameSuffix;
            }
        }
    }
}
