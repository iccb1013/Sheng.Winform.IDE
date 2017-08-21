/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners.Tests;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Tests.Configuration.Unity
{
	[TestClass]
	public class LoggingExceptionHandlerPolicyCreatorFixture
	{
		private IUnityContainer container;
		[TestInitialize]
		public void SetUp()
		{
			container = new UnityContainer();
			container.AddExtension(new EnterpriseLibraryCoreExtension());
		}
		[TestCleanup]
		public void TearDown()
		{
			container.Dispose();
		}
		[TestMethod]
		public void ExceptionHandledThroughLoggingBlock()
		{
			MockTraceListener.Reset();
			container.AddExtension(new ExceptionHandlingBlockExtension());
			container.AddExtension(new LoggingBlockExtension());
			ExceptionPolicyImpl policy = container.Resolve<ExceptionPolicyImpl>("Logging Policy");
			Assert.IsFalse(policy.HandleException(new Exception("TEST EXCEPTION")));
			Assert.AreEqual(1, MockTraceListener.LastEntry.Categories.Count);
			Assert.IsTrue(MockTraceListener.LastEntry.Categories.Contains("TestCat"));
			Assert.AreEqual(5, MockTraceListener.LastEntry.EventId);
			Assert.AreEqual(TraceEventType.Error, MockTraceListener.LastEntry.Severity);
			Assert.AreEqual("TestTitle", MockTraceListener.LastEntry.Title);
		}
		[TestMethod]
		public void MultipleRequestsUseSameLogWriterInstance()
		{
			MockTraceListener.Reset();
			container.AddExtension(new ExceptionHandlingBlockExtension());
			container.AddExtension(new LoggingBlockExtension());
			{
				ExceptionPolicyImpl policy = container.Resolve<ExceptionPolicyImpl>("Logging Policy");
				Assert.IsFalse(policy.HandleException(new Exception("TEST EXCEPTION")));
			}
			{
				ExceptionPolicyImpl policy = container.Resolve<ExceptionPolicyImpl>("Logging Policy");
				Assert.IsFalse(policy.HandleException(new Exception("TEST EXCEPTION")));
			}
			{
				ExceptionPolicyImpl policy = container.Resolve<ExceptionPolicyImpl>("Logging Policy");
				Assert.IsFalse(policy.HandleException(new Exception("TEST EXCEPTION")));
			}
			Assert.AreEqual(3, MockTraceListener.Entries.Count);
			Assert.AreEqual(3, MockTraceListener.Instances.Count);
			Assert.AreSame(MockTraceListener.Instances[0], MockTraceListener.Instances[1]);
			Assert.AreSame(MockTraceListener.Instances[0], MockTraceListener.Instances[2]);
		}
	}
}
