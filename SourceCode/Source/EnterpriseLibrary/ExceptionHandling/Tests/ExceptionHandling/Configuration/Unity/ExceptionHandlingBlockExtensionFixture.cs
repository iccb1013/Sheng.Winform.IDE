//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Tests;
using Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests.Properties;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests.Configuration.Unity
{
	[TestClass]
	public class ExceptionHandlingBlockExtensionFixture
	{
		private IUnityContainer container;
		private ExceptionHandlingSettings settings;
		private DictionaryConfigurationSource configurationSource;

		[TestInitialize]
		public void SetUp()
		{
			container = new UnityContainer();

			settings = new ExceptionHandlingSettings();
			configurationSource = new DictionaryConfigurationSource();
			configurationSource.Add(ExceptionHandlingSettings.SectionName, settings);
			configurationSource.Add(
				InstrumentationConfigurationSection.SectionName,
				new InstrumentationConfigurationSection(false, false, true));

			container.AddExtension(new EnterpriseLibraryCoreExtension(configurationSource));
		}

		[TestCleanup]
		public void TearDown()
		{
			container.Dispose();
		}

		[TestMethod]
		public void CanCreateEmptyPolicy()
		{
			ExceptionPolicyData exceptionPolicyData = new ExceptionPolicyData("policy");
			settings.ExceptionPolicies.Add(exceptionPolicyData);

			container.AddExtension(new ExceptionHandlingBlockExtension());

			ExceptionPolicyImpl policy = container.Resolve<ExceptionPolicyImpl>("policy");

			Assert.IsNotNull(policy);
		}

		[TestMethod]
		public void CanCreatePolicyWithEmptyTypes()
		{
			ExceptionPolicyData exceptionPolicyData = new ExceptionPolicyData("policy");
			settings.ExceptionPolicies.Add(exceptionPolicyData);

			ExceptionTypeData exceptionTypeData1
				= new ExceptionTypeData("type1", typeof(Exception), PostHandlingAction.None);
			exceptionPolicyData.ExceptionTypes.Add(exceptionTypeData1);
			ExceptionTypeData exceptionTypeData2
				= new ExceptionTypeData("type2", typeof(ArgumentException), PostHandlingAction.NotifyRethrow);
			exceptionPolicyData.ExceptionTypes.Add(exceptionTypeData2);

			container.AddExtension(new ExceptionHandlingBlockExtension());

			ExceptionPolicyImpl policy = container.Resolve<ExceptionPolicyImpl>("policy");

			Assert.IsNotNull(policy);
			Assert.IsNotNull(policy.GetPolicyEntry(typeof(Exception)));
			Assert.IsNotNull(policy.GetPolicyEntry(typeof(ArgumentException)));
			Assert.IsNull(policy.GetPolicyEntry(typeof(InvalidOperationException)));

			// little detail is exposed for policy entries - need to probe its behavior for a proper assert
			Assert.IsFalse(policy.GetPolicyEntry(typeof(Exception)).Handle(new Exception()));
			Assert.IsTrue(policy.GetPolicyEntry(typeof(ArgumentException)).Handle(new ArgumentException()));
		}

		[TestMethod]
		public void PoliciesForExceptionPoliciesCanHandleRepeatedTypes()
		{
			ExceptionPolicyData exceptionPolicy1Data = new ExceptionPolicyData("policy1");
			settings.ExceptionPolicies.Add(exceptionPolicy1Data);

			ExceptionTypeData exceptionTypeData11
				= new ExceptionTypeData("type1", typeof(Exception), PostHandlingAction.None);
			exceptionPolicy1Data.ExceptionTypes.Add(exceptionTypeData11);
			ExceptionTypeData exceptionTypeData12
				= new ExceptionTypeData("type2", typeof(ArgumentException), PostHandlingAction.NotifyRethrow);
			exceptionPolicy1Data.ExceptionTypes.Add(exceptionTypeData12);

			ExceptionPolicyData exceptionPolicy2Data = new ExceptionPolicyData("policy2");
			settings.ExceptionPolicies.Add(exceptionPolicy2Data);

			ExceptionTypeData exceptionTypeData21
				= new ExceptionTypeData("type1", typeof(InvalidOperationException), PostHandlingAction.NotifyRethrow);
			exceptionPolicy2Data.ExceptionTypes.Add(exceptionTypeData21);
			ExceptionTypeData exceptionTypeData22
				= new ExceptionTypeData("type2", typeof(ArgumentNullException), PostHandlingAction.NotifyRethrow);
			exceptionPolicy2Data.ExceptionTypes.Add(exceptionTypeData22);

			container.AddExtension(new ExceptionHandlingBlockExtension());

			ExceptionPolicyImpl policy = container.Resolve<ExceptionPolicyImpl>("policy1");

			Assert.IsNotNull(policy);
			Assert.IsNotNull(policy.GetPolicyEntry(typeof(Exception)));
			Assert.IsNotNull(policy.GetPolicyEntry(typeof(ArgumentException)));
			Assert.IsNull(policy.GetPolicyEntry(typeof(InvalidOperationException)));

			// this breaks if the type1 for policy2 overrides the policy for policy1's type
			Assert.IsFalse(policy.GetPolicyEntry(typeof(Exception)).Handle(new Exception()));
			Assert.IsTrue(policy.GetPolicyEntry(typeof(ArgumentException)).Handle(new ArgumentException()));
		}

		[TestMethod]
		public void CanCreatePolicyWithSimpleExceptionHandler()
		{
			ExceptionPolicyData exceptionPolicyData = new ExceptionPolicyData("policy");
			settings.ExceptionPolicies.Add(exceptionPolicyData);

			ExceptionTypeData exceptionTypeData = new ExceptionTypeData("type1", typeof(Exception), PostHandlingAction.ThrowNewException);
			exceptionPolicyData.ExceptionTypes.Add(exceptionTypeData);

			ExceptionHandlerData exceptionHandlerData = new WrapHandlerData("handler1", "message",
				typeof(ArgumentException).AssemblyQualifiedName);
			exceptionTypeData.ExceptionHandlers.Add(exceptionHandlerData);

			container.AddExtension(new ExceptionHandlingBlockExtension());

			ExceptionPolicyImpl policy = container.Resolve<ExceptionPolicyImpl>("policy");

			try
			{
				policy.HandleException(new Exception("to be wrapped"));
				Assert.Fail("a new exception should have been thrown");
			}
			catch (ArgumentException e)
			{
				Assert.AreEqual("message", e.Message);
			}
		}

		[TestMethod]
		public void WrapperHandlerCanGetExceptionMessageFromResource()
		{
			const string resourceName = "ExceptionMessage";

			ExceptionPolicyData exceptionPolicyData = new ExceptionPolicyData("policy");
			settings.ExceptionPolicies.Add(exceptionPolicyData);

			ExceptionTypeData exceptionTypeData = new ExceptionTypeData("type1", typeof(Exception), PostHandlingAction.ThrowNewException);
			exceptionPolicyData.ExceptionTypes.Add(exceptionTypeData);

			WrapHandlerData exceptionHandlerData = new WrapHandlerData("handler1", "message",
				typeof(ArgumentException).AssemblyQualifiedName);

			exceptionHandlerData.ExceptionMessageResourceName = resourceName;
			exceptionHandlerData.ExceptionMessageResourceType = typeof(Resources).AssemblyQualifiedName;

			string resourceValue = Resources.ExceptionMessage;

			exceptionTypeData.ExceptionHandlers.Add(exceptionHandlerData);

			container.AddExtension(new ExceptionHandlingBlockExtension());

			ExceptionPolicyImpl policy = container.Resolve<ExceptionPolicyImpl>("policy");

			try
			{
				policy.HandleException(new Exception("to be wrapped"));
				Assert.Fail("a new exception should have been thrown");
			}
			catch (ArgumentException e)
			{
				Assert.AreEqual(resourceValue, e.Message);
			}
		}

		[TestMethod]
		public void PoliciesForExceptionPoliciesCanHandleRepeatedTypesWithRepeatedHandlers()
		{
			ExceptionPolicyData exceptionPolicy1Data = new ExceptionPolicyData("policy1");
			settings.ExceptionPolicies.Add(exceptionPolicy1Data);

			ExceptionTypeData exceptionTypeData11
				= new ExceptionTypeData("type1", typeof(Exception), PostHandlingAction.ThrowNewException);
			exceptionPolicy1Data.ExceptionTypes.Add(exceptionTypeData11);
			ExceptionHandlerData exceptionHandlerData11 = new WrapHandlerData("handler1", "message1",
				typeof(ArgumentException).AssemblyQualifiedName);
			exceptionTypeData11.ExceptionHandlers.Add(exceptionHandlerData11);

			ExceptionPolicyData exceptionPolicy2Data = new ExceptionPolicyData("policy2");
			settings.ExceptionPolicies.Add(exceptionPolicy2Data);

			ExceptionTypeData exceptionTypeData21
				= new ExceptionTypeData("type1", typeof(Exception), PostHandlingAction.ThrowNewException);
			exceptionPolicy2Data.ExceptionTypes.Add(exceptionTypeData21);
			ExceptionHandlerData exceptionHandlerData21 = new WrapHandlerData("handler1", "message2",
				typeof(ArgumentException).AssemblyQualifiedName);
			exceptionTypeData21.ExceptionHandlers.Add(exceptionHandlerData21);

			container.AddExtension(new ExceptionHandlingBlockExtension());

			ExceptionPolicyImpl policy = container.Resolve<ExceptionPolicyImpl>("policy1");

			try
			{
				policy.HandleException(new Exception("to be wrapped"));
				Assert.Fail("a new exception should have been thrown");
			}
			catch (ArgumentException e)
			{
				Assert.AreEqual("message1", e.Message, "Policy 1 is using the handler definitions from policy 2");
			}
		}

		[TestMethod]
		public void PoliciesForExceptionManagerAreCreated()
		{
			ExceptionPolicyData exceptionPolicy1Data = new ExceptionPolicyData("policy1");
			settings.ExceptionPolicies.Add(exceptionPolicy1Data);

			ExceptionTypeData exceptionTypeData11
				= new ExceptionTypeData("type1", typeof(Exception), PostHandlingAction.ThrowNewException);
			exceptionPolicy1Data.ExceptionTypes.Add(exceptionTypeData11);
			CustomHandlerData exceptionHandlerData11 = new CustomHandlerData("handler1", typeof(TestCustomExceptionHandler));
			exceptionHandlerData11.Attributes.Add(TestCustomExceptionHandler.AttributeKey, "handler1");
			exceptionTypeData11.ExceptionHandlers.Add(exceptionHandlerData11);

			container.AddExtension(new ExceptionHandlingBlockExtension());

			ExceptionManager manager = container.Resolve<ExceptionManager>();
			Assert.IsNotNull(manager);

			Exception exceptionToThrow = new Exception("some message");

			try
			{
				manager.Process(() => { throw exceptionToThrow; }, "policy1");
				Assert.Fail("a new exception should have been thrown");
			}
			catch (Exception e)
			{
				Assert.AreSame(exceptionToThrow, e.InnerException);
				Assert.AreEqual("handler1", e.Message);
			}
		}

		[TestMethod]
		public void ExceptionManagerGetsInstrumented()
		{
			container.AddExtension(new ExceptionHandlingBlockExtension());

			ExceptionManager manager = container.Resolve<ExceptionManager>();
			Assert.IsNotNull(manager);

			Exception exceptionToThrow = new Exception("some message");

			using (WmiEventWatcher eventListener = new WmiEventWatcher(1))
			{
				try
				{
					manager.HandleException(new Exception(), "non-existing policy");
				}
				catch (ExceptionHandlingException)
				{
					eventListener.WaitForEvents();
					Assert.AreEqual(1, eventListener.EventsReceived.Count);
					Assert.AreEqual(typeof(ExceptionHandlingFailureEvent).Name,
						eventListener.EventsReceived[0].ClassPath.ClassName);
					Assert.AreEqual("non-existing policy", eventListener.EventsReceived[0].GetPropertyValue("InstanceName"));
				}
			}
		}

		public class TestCustomExceptionHandler : MockCustomProviderBase, IExceptionHandler
		{
			public TestCustomExceptionHandler(NameValueCollection attributes)
				: base(attributes)
			{ }

			public Exception HandleException(Exception exception, Guid handlingInstanceId)
			{
				return new Exception(this.customValue, exception);
			}
		}
	}
}
