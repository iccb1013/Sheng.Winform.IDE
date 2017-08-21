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
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Instrumentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests
{
	[TestClass]
	public class ExceptionManagerFixture
	{
		private DefaultExceptionHandlingErrorEventArgs firedEvent;

		[TestInitialize]
		public void SetUp()
		{
			this.firedEvent = null;
			TestExceptionHandler.HandlingNames.Clear();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CreationWithNullPolicyDictionaryThrows()
		{
			new ExceptionManagerImpl(null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void HandleWithNullNameThrows()
		{
			new ExceptionManagerImpl(new Dictionary<string, ExceptionPolicyImpl>()).HandleException(new Exception(), null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void HandleWithNullExceptionThrows()
		{
			new ExceptionManagerImpl(new Dictionary<string, ExceptionPolicyImpl>()).HandleException(null, "policy");
		}

		[TestMethod]
		[ExpectedException(typeof(ExceptionHandlingException))]
		public void HandleWithNonExistingPolicyThrows()
		{
			new ExceptionManagerImpl(new Dictionary<string, ExceptionPolicyImpl>()).HandleException(new Exception(), "policy");
		}

		[TestMethod]
		public void HandleWithNonExistingPolicyFiresInstrumentation()
		{
			ExceptionManager manager = new ExceptionManagerImpl(new Dictionary<string, ExceptionPolicyImpl>());
			((DefaultExceptionHandlingInstrumentationProvider)((IInstrumentationEventProvider)manager).GetInstrumentationEventProvider())
				.exceptionHandlingErrorOccurred += (sender, args) => { firedEvent = args; };
			try
			{
				manager.HandleException(new Exception(), "policy");
				Assert.Fail("should have thrown");
			}
			catch (ExceptionHandlingException)
			{
				Assert.IsNotNull(this.firedEvent);
				Assert.AreEqual("policy", this.firedEvent.PolicyName);
			}
		}

		[TestMethod]
		public void HandleForwardsHandlingToConfiguredExceptionEntry()
		{
			Dictionary<string, ExceptionPolicyImpl> policies = new Dictionary<string, ExceptionPolicyImpl>();
			Dictionary<Type, ExceptionPolicyEntry> policy1Entries = new Dictionary<Type, ExceptionPolicyEntry>();
			policy1Entries.Add(
				typeof(ArithmeticException),
				new ExceptionPolicyEntry(PostHandlingAction.NotifyRethrow, new IExceptionHandler[] { new TestExceptionHandler("handler11") }));
			policy1Entries.Add(
				typeof(ArgumentException),
				new ExceptionPolicyEntry(PostHandlingAction.ThrowNewException, new IExceptionHandler[] { new TestExceptionHandler("handler12") }));
			policy1Entries.Add(
				typeof(ArgumentOutOfRangeException),
				new ExceptionPolicyEntry(PostHandlingAction.None, new IExceptionHandler[] { new TestExceptionHandler("handler13") }));
			policies.Add("policy1", new ExceptionPolicyImpl("policy1", policy1Entries));

			ExceptionManager manager = new ExceptionManagerImpl(policies);

			Exception thrownException;

			// is the exception rethrown?
			thrownException = new ArithmeticException();
			Assert.IsTrue(manager.HandleException(thrownException, "policy1"));
			Assert.AreEqual(1, TestExceptionHandler.HandlingNames.Count);
			Assert.AreEqual("handler11", TestExceptionHandler.HandlingNames[0]);

			// is the new exception thrown?
			TestExceptionHandler.HandlingNames.Clear();
			thrownException = new ArgumentException();
			try
			{
				manager.HandleException(thrownException, "policy1");
				Assert.Fail("should have thrown");
			}
			catch (Exception e)
			{
				Assert.AreSame(thrownException, e.InnerException);
				Assert.AreEqual(1, TestExceptionHandler.HandlingNames.Count);
				Assert.AreEqual("handler12", TestExceptionHandler.HandlingNames[0]);
			}

			// is the exception swallowed? action == None
			TestExceptionHandler.HandlingNames.Clear();
			thrownException = new ArgumentOutOfRangeException();
			Assert.IsFalse(manager.HandleException(thrownException, "policy1"));
			Assert.AreEqual(1, TestExceptionHandler.HandlingNames.Count);
			Assert.AreEqual("handler13", TestExceptionHandler.HandlingNames[0]);

			// is the unknwon exception rethrown?
			TestExceptionHandler.HandlingNames.Clear();
			thrownException = new Exception();
			Assert.IsTrue(manager.HandleException(thrownException, "policy1"));
			Assert.AreEqual(0, TestExceptionHandler.HandlingNames.Count);
		}

		[TestMethod]
		public void HandleWithReturnForwardsHandlingToConfiguredExceptionEntry()
		{
			Dictionary<string, ExceptionPolicyImpl> policies = new Dictionary<string, ExceptionPolicyImpl>();
			Dictionary<Type, ExceptionPolicyEntry> policy1Entries = new Dictionary<Type, ExceptionPolicyEntry>();
			policy1Entries.Add(
				typeof(ArithmeticException),
				new ExceptionPolicyEntry(PostHandlingAction.NotifyRethrow, new IExceptionHandler[] { new TestExceptionHandler("handler11") }));
			policy1Entries.Add(
				typeof(ArgumentException),
				new ExceptionPolicyEntry(PostHandlingAction.ThrowNewException, new IExceptionHandler[] { new TestExceptionHandler("handler12") }));
			policy1Entries.Add(
				typeof(ArgumentOutOfRangeException),
				new ExceptionPolicyEntry(PostHandlingAction.None, new IExceptionHandler[] { new TestExceptionHandler("handler13") }));
			policies.Add("policy1", new ExceptionPolicyImpl("policy1", policy1Entries));

			ExceptionManager manager = new ExceptionManagerImpl(policies);

			Exception thrownException;
			Exception exceptionToThrow;

			// is the exception rethrown?
			thrownException = new ArithmeticException();
			Assert.IsTrue(manager.HandleException(thrownException, "policy1", out exceptionToThrow));
			Assert.IsNull(exceptionToThrow);
			Assert.AreEqual(1, TestExceptionHandler.HandlingNames.Count);
			Assert.AreEqual("handler11", TestExceptionHandler.HandlingNames[0]);
			
			// is the new exception thrown?
			TestExceptionHandler.HandlingNames.Clear();
			thrownException = new ArgumentException();
			Assert.IsTrue(manager.HandleException(thrownException, "policy1", out exceptionToThrow));
			Assert.AreSame(thrownException, exceptionToThrow.InnerException);
			Assert.AreEqual(1, TestExceptionHandler.HandlingNames.Count);
			Assert.AreEqual("handler12", TestExceptionHandler.HandlingNames[0]);

			// is the exception swallowed? action == None
			TestExceptionHandler.HandlingNames.Clear();
			thrownException = new ArgumentOutOfRangeException();
			Assert.IsFalse(manager.HandleException(thrownException, "policy1", out exceptionToThrow));
			Assert.IsNull(exceptionToThrow);
			Assert.AreEqual(1, TestExceptionHandler.HandlingNames.Count);
			Assert.AreEqual("handler13", TestExceptionHandler.HandlingNames[0]);

			// is the unknwon exception rethrown?
			TestExceptionHandler.HandlingNames.Clear();
			thrownException = new Exception();
			Assert.IsTrue(manager.HandleException(thrownException, "policy1", out exceptionToThrow));
			Assert.IsNull(exceptionToThrow);
			Assert.AreEqual(0, TestExceptionHandler.HandlingNames.Count);
		}

		[TestMethod]
		public void ProcessForwardsHandlingToConfiguredExceptionEntry()
		{
			Dictionary<string, ExceptionPolicyImpl> policies = new Dictionary<string, ExceptionPolicyImpl>();
			Dictionary<Type, ExceptionPolicyEntry> policy1Entries = new Dictionary<Type, ExceptionPolicyEntry>();
			policy1Entries.Add(
				typeof(ArithmeticException),
				new ExceptionPolicyEntry(PostHandlingAction.NotifyRethrow, new IExceptionHandler[] { new TestExceptionHandler("handler11") }));
			policy1Entries.Add(
				typeof(ArgumentException),
				new ExceptionPolicyEntry(PostHandlingAction.ThrowNewException, new IExceptionHandler[] { new TestExceptionHandler("handler12") }));
			policy1Entries.Add(
				typeof(ArgumentOutOfRangeException),
				new ExceptionPolicyEntry(PostHandlingAction.None, new IExceptionHandler[] { new TestExceptionHandler("handler13") }));
			policies.Add("policy1", new ExceptionPolicyImpl("policy1", policy1Entries));

			ExceptionManager manager = new ExceptionManagerImpl(policies);

			Exception thrownException;

			// is the exception rethrown?
			thrownException = new ArithmeticException();
			try
			{
				manager.Process(
					() => { throw (thrownException); },
					"policy1");
				Assert.Fail("should have thrown");
			}
			catch (Exception e)
			{
				Assert.AreSame(thrownException, e);
				Assert.AreEqual(1, TestExceptionHandler.HandlingNames.Count);
				Assert.AreEqual("handler11", TestExceptionHandler.HandlingNames[0]);
			}

			// is the new exception thrown?
			TestExceptionHandler.HandlingNames.Clear();
			thrownException = new ArgumentException();
			try
			{
				manager.Process(
					() => { throw (thrownException); },
					"policy1");
				Assert.Fail("should have thrown");
			}
			catch (Exception e)
			{
				Assert.AreSame(thrownException, e.InnerException);
				Assert.AreEqual(1, TestExceptionHandler.HandlingNames.Count);
				Assert.AreEqual("handler12", TestExceptionHandler.HandlingNames[0]);
			}

			// is the exception swallowed? action == None
			TestExceptionHandler.HandlingNames.Clear();
			thrownException = new ArgumentOutOfRangeException();
			manager.Process(
				() => { throw (thrownException); },
				"policy1");
			Assert.AreEqual(1, TestExceptionHandler.HandlingNames.Count);
			Assert.AreEqual("handler13", TestExceptionHandler.HandlingNames[0]);

			// is the unknwon exception rethrown?
			TestExceptionHandler.HandlingNames.Clear();
			thrownException = new Exception();
			try
			{
				manager.Process(
					() => { throw (thrownException); },
					"policy1");
				Assert.Fail("should have thrown");
			}
			catch (Exception e)
			{
				Assert.AreSame(thrownException, e);
				Assert.AreEqual(0, TestExceptionHandler.HandlingNames.Count);
			}
		}
	}

	internal class TestExceptionHandler : IExceptionHandler
	{
		static TestExceptionHandler()
		{
			HandlingNames = new List<string>();
		}

		/// <summary>
		/// Initializes a new instance of the TestExceptionHandler class.
		/// </summary>
		public TestExceptionHandler(string name)
		{
			this.Name = name;
		}

		public Exception HandleException(Exception exception, Guid handlingInstanceId)
		{
			HandlingNames.Add(this.Name);

			Exception newException = new Exception("foo", exception);

			return newException;
		}

		public string Name { get; private set; }

		public static IList<string> HandlingNames { get; private set; }
	}
}
