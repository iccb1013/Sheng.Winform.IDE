//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners.Tests;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Configuration.Unity
{
	[TestClass]
	public class LoggingBlockExtensionFixture
	{
		private LoggingSettings loggingSettings;
		private DictionaryConfigurationSource configurationSource;
		private IUnityContainer container;
		private InstrumentationConfigurationSection instrumentationSettings;

		[TestInitialize]
		public void SetUp()
		{
			instrumentationSettings = new InstrumentationConfigurationSection(true, true, false);
			loggingSettings = new LoggingSettings();
			configurationSource = new DictionaryConfigurationSource();
			configurationSource.Add(LoggingSettings.SectionName, loggingSettings);

			container = new UnityContainer();

			container.AddExtension(new EnterpriseLibraryCoreExtension(configurationSource));
		}

		[TestCleanup]
		public void TearDown()
		{
			container.Dispose();
		}

		[TestMethod]
		public void CanCreateTraceManagerWithConfiguration()
		{
			configurationSource.Add(InstrumentationConfigurationSection.SectionName, instrumentationSettings);

			container.AddExtension(new LoggingBlockExtension());

			TraceManager createdObject = (TraceManager)container.Resolve<TraceManager>();

			Assert.IsNotNull(createdObject);

			Assert.IsTrue(createdObject.InstrumentationListener.PerformanceCountersEnabled);
		}

		[TestMethod]
		public void CanCreateTraceManagerWithNoConfiguration()
		{
			container.AddExtension(new LoggingBlockExtension());

			TraceManager createdObject = (TraceManager)container.Resolve<TraceManager>();

			Assert.IsNotNull(createdObject);

			Assert.IsFalse(createdObject.InstrumentationListener.PerformanceCountersEnabled);
		}

		[TestMethod]
		public void CanCreateLogFormatter()
		{
			FormatterData data = new TextFormatterData("name", "template");
			loggingSettings.Formatters.Add(data);

			container.AddExtension(new LoggingBlockExtension());

			TextFormatter createdObject = (TextFormatter)container.Resolve<ILogFormatter>("name");

			Assert.IsNotNull(createdObject);
			Assert.AreEqual("template", createdObject.Template);
		}

		[TestMethod]
		public void CanCreateLogFilter()
		{
			CategoryFilterData data = new CategoryFilterData();
			data.Type = typeof(CategoryFilter);
			data.Name = "name";
			data.CategoryFilterMode = CategoryFilterMode.DenyAllExceptAllowed;
			data.CategoryFilters.Add(new CategoryFilterEntry("foo"));
			data.CategoryFilters.Add(new CategoryFilterEntry("bar"));
			loggingSettings.LogFilters.Add(data);

			container.AddExtension(new LoggingBlockExtension());

			CategoryFilter createdObject = (CategoryFilter)container.Resolve<ILogFilter>("name");

			Assert.IsNotNull(createdObject);
			Assert.AreEqual(CategoryFilterMode.DenyAllExceptAllowed, createdObject.CategoryFilterMode);
			Assert.AreEqual(2, createdObject.CategoryFilters.Count);
			Assert.IsTrue(createdObject.CategoryFilters.Contains("foo"));
			Assert.IsTrue(createdObject.CategoryFilters.Contains("bar"));
		}

		[TestMethod]
		public void CanCreateTraceListenerWithReferenceToFormatter()
		{
			FormatterData data = new TextFormatterData("formattername", "template");
			loggingSettings.Formatters.Add(data);
			TraceListenerData traceListenerData = new FlatFileTraceListenerData("name", "filename.log", "formattername");
			traceListenerData.Filter = SourceLevels.Critical;
			traceListenerData.TraceOutputOptions = TraceOptions.ProcessId;
			loggingSettings.TraceListeners.Add(traceListenerData);

			container.AddExtension(new LoggingBlockExtension());

			FlatFileTraceListener createdObject = (FlatFileTraceListener)container.Resolve<TraceListener>("name");

			Assert.IsNotNull(createdObject);
			Assert.AreEqual("name", createdObject.Name);
			Assert.AreEqual(SourceLevels.Critical, ((EventTypeFilter)createdObject.Filter).EventType);
			Assert.AreEqual(TraceOptions.ProcessId, createdObject.TraceOutputOptions);
			Assert.IsNotNull(createdObject.Formatter);
			Assert.AreEqual("template", ((TextFormatter)createdObject.Formatter).Template);
		}

		[TestMethod]
		public void TraceListenerIsSingletonInContainer()
		{
			FormatterData data = new TextFormatterData("formattername", "template");
			loggingSettings.Formatters.Add(data);
			TraceListenerData traceListenerData = new FlatFileTraceListenerData("name", "filename.log", "formattername");
			loggingSettings.TraceListeners.Add(traceListenerData);

			container.AddExtension(new LoggingBlockExtension());

			FlatFileTraceListener createdObject1 = (FlatFileTraceListener)container.Resolve<TraceListener>("name");
			FlatFileTraceListener createdObject2 = (FlatFileTraceListener)container.Resolve<TraceListener>("name");

			Assert.AreSame(createdObject1, createdObject2);
		}

		[TestMethod]
		public void CanCreateTraceSource()
		{
			loggingSettings.TraceListeners.Add(new MockTraceListenerData("mock1"));
			loggingSettings.TraceListeners.Add(new MockTraceListenerData("mock2"));

			TraceSourceData sourceData = new TraceSourceData("name", SourceLevels.All);
			sourceData.TraceListeners.Add(new TraceListenerReferenceData("mock1"));
			sourceData.TraceListeners.Add(new TraceListenerReferenceData("mock2"));
			loggingSettings.TraceSources.Add(sourceData);

			container.AddExtension(new LoggingBlockExtension());

			LogSource createdObject = container.Resolve<LogSource>("name");

			Assert.IsNotNull(createdObject);
			Assert.AreEqual("name", createdObject.Name);
			Assert.AreEqual(SourceLevels.All, createdObject.Level);
			Assert.AreEqual(2, createdObject.Listeners.Count);
			Assert.AreSame(typeof(MockTraceListener), createdObject.Listeners[0].GetType());
			Assert.AreEqual("mock1", createdObject.Listeners[0].Name);
			Assert.AreSame(typeof(MockTraceListener), createdObject.Listeners[1].GetType());
			Assert.AreEqual("mock2", createdObject.Listeners[1].Name);
		}

		[TestMethod]
		public void CanCreateLogWriter()
		{
			CategoryFilterData data = new CategoryFilterData();
			data.Type = typeof(CategoryFilter);
			data.Name = "name";
			data.CategoryFilterMode = CategoryFilterMode.DenyAllExceptAllowed;
			data.CategoryFilters.Add(new CategoryFilterEntry("foo"));
			data.CategoryFilters.Add(new CategoryFilterEntry("bar"));
			loggingSettings.LogFilters.Add(data);

			loggingSettings.TraceListeners.Add(new MockTraceListenerData("mock1"));
			loggingSettings.TraceListeners.Add(new MockTraceListenerData("mock2"));

			TraceSourceData sourceData = new TraceSourceData("name", SourceLevels.All);
			sourceData.TraceListeners.Add(new TraceListenerReferenceData("mock1"));
			sourceData.TraceListeners.Add(new TraceListenerReferenceData("mock2"));
			loggingSettings.TraceSources.Add(sourceData);

			loggingSettings.SpecialTraceSources = new SpecialTraceSourcesData();
			loggingSettings.SpecialTraceSources.AllEventsTraceSource = new TraceSourceData("all", SourceLevels.All);
			loggingSettings.SpecialTraceSources.NotProcessedTraceSource = new TraceSourceData("not processed", SourceLevels.Warning);
			loggingSettings.SpecialTraceSources.ErrorsTraceSource = new TraceSourceData("errors", SourceLevels.Error);
			loggingSettings.SpecialTraceSources.ErrorsTraceSource.TraceListeners.Add(new TraceListenerReferenceData("mock1"));

			loggingSettings.DefaultCategory = "name";
			loggingSettings.LogWarningWhenNoCategoriesMatch = true;
			loggingSettings.TracingEnabled = false;

			container.AddExtension(new LoggingBlockExtension());

			LogWriter createdObject = container.Resolve<LogWriter>();

			Assert.IsNotNull(createdObject);

			CategoryFilter filter = createdObject.GetFilter<CategoryFilter>();
			Assert.IsNotNull(filter);
			Assert.AreEqual(2, filter.CategoryFilters.Count);
			Assert.IsTrue(filter.CategoryFilters.Contains("foo"));
			Assert.IsTrue(filter.CategoryFilters.Contains("bar"));

			Assert.AreEqual(1, createdObject.TraceSources.Count);
		}

		[TestMethod]
		public void TraceManagerPolicyCreationDoesNotTryToCreateALogWriter_Bug17444()
		{
			loggingSettings.TraceListeners.Add(new FakeTraceListenerData("fake"));

			TraceSourceData sourceData = new TraceSourceData("name", SourceLevels.All);
			sourceData.TraceListeners.Add(new TraceListenerReferenceData("fake"));
			loggingSettings.TraceSources.Add(sourceData);

			loggingSettings.SpecialTraceSources = new SpecialTraceSourcesData();
			loggingSettings.SpecialTraceSources.AllEventsTraceSource = new TraceSourceData("all", SourceLevels.All);
			loggingSettings.SpecialTraceSources.NotProcessedTraceSource = new TraceSourceData("not processed", SourceLevels.Warning);
			loggingSettings.SpecialTraceSources.ErrorsTraceSource = new TraceSourceData("errors", SourceLevels.Error);

			loggingSettings.DefaultCategory = "name";
			loggingSettings.LogWarningWhenNoCategoriesMatch = true;
			loggingSettings.TracingEnabled = false;


			// the order in which the extensions are added should not matter because they should only record policies
			// but the bug caused an attempt to actually try to build a log writer, and this set up would cause
			// an error because a required extension was not added yet.
			container.AddNewExtension<LoggingBlockExtension>();
			container.AddNewExtension<FakeBlockExtension>();
		}


		#region test fakes

		public class FakeBlockExtension : EnterpriseLibraryBlockExtension
		{
			protected override void Initialize()
			{
				new PolicyBuilder<FakeBlockObject, object>(NamedTypeBuildKey.Make<FakeBlockObject>(), null, c => new FakeBlockObject("some string"))
					.AddPoliciesToPolicyList(Context.Policies);
			}
		}

		public class FakeBlockObject
		{
			public FakeBlockObject(string ignored)
			{ }
		}

		public class FakeTraceListener : TraceListener
		{
			public FakeTraceListener(FakeBlockObject ignored)
			{ }

			public override void Write(string message)
			{
				throw new System.NotImplementedException();
			}

			public override void WriteLine(string message)
			{
				throw new System.NotImplementedException();
			}
		}

		[ContainerPolicyCreator(typeof(FakeTraceListenerPolicyCreator))]
		public class FakeTraceListenerData : TraceListenerData
		{
			public FakeTraceListenerData(string name)
				: base(name, typeof(FakeTraceListener), TraceOptions.None)
			{ }

		}

		public class FakeTraceListenerPolicyCreator : IContainerPolicyCreator
		{
			public void CreatePolicies(IPolicyList policyList, 
				string instanceName, 
				System.Configuration.ConfigurationElement configurationObject, 
				IConfigurationSource configurationSource)
			{
				new PolicyBuilder<FakeTraceListener, FakeTraceListenerData>(
					instanceName,
					(FakeTraceListenerData)configurationObject,
					c => new FakeTraceListener(
						Resolve.Reference<FakeBlockObject>(null)))
					.AddPoliciesToPolicyList(policyList);
			}
		}

		#endregion
	}
}
