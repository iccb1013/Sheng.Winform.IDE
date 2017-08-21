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

using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners.Tests;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Configuration.Unity
{
	[TestClass]
	public class TraceListenersPolicyCreationFixture
	{
		private IUnityContainer container;
		private LoggingSettings loggingSettings;
		private DictionaryConfigurationSource configurationSource;

		[TestInitialize]
		public void SetUp()
		{
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
		public void CanCreatePoliciesForWmiTraceListener()
		{
			WmiTraceListenerData listenerData = new WmiTraceListenerData("listener");
			listenerData.TraceOutputOptions = TraceOptions.Callstack | TraceOptions.ProcessId;
			listenerData.Filter = SourceLevels.Error;
			loggingSettings.TraceListeners.Add(listenerData);

			container.AddExtension(new LoggingBlockExtension());

			WmiTraceListener createdObject = (WmiTraceListener)container.Resolve<TraceListener>("listener");

			Assert.IsNotNull(createdObject);
			Assert.AreEqual(listenerData.TraceOutputOptions, createdObject.TraceOutputOptions);
			Assert.IsNotNull(createdObject.Filter);
			Assert.IsInstanceOfType(createdObject.Filter, typeof(EventTypeFilter));
			Assert.AreEqual(listenerData.Filter, ((EventTypeFilter)createdObject.Filter).EventType);
		}

		[TestMethod]
		public void CanCreatePoliciesForXmlTraceListener()
		{
			XmlTraceListenerData listenerData = new XmlTraceListenerData("listener", "test.log");
			listenerData.TraceOutputOptions = TraceOptions.Callstack | TraceOptions.ProcessId;
			listenerData.Filter = SourceLevels.Error;
			loggingSettings.TraceListeners.Add(listenerData);

			container.AddExtension(new LoggingBlockExtension());

			XmlTraceListener createdObject = (XmlTraceListener)container.Resolve<TraceListener>("listener");

			Assert.IsNotNull(createdObject);
			Assert.AreEqual(listenerData.TraceOutputOptions, createdObject.TraceOutputOptions);
			Assert.IsNotNull(createdObject.Filter);
			Assert.IsInstanceOfType(createdObject.Filter, typeof(EventTypeFilter));
			Assert.AreEqual(listenerData.Filter, ((EventTypeFilter)createdObject.Filter).EventType);
			Assert.AreEqual(listenerData.FileName, Path.GetFileName(((FileStream)((StreamWriter)createdObject.Writer).BaseStream).Name));
		}

		[TestMethod]
		public void CanCreatePoliciesForEmailTraceListener()
		{
			EmailTraceListenerData listenerData = new EmailTraceListenerData("listener", "to address", "from address", "starter", "ender", "smtp", 25, "");
			listenerData.TraceOutputOptions = TraceOptions.Callstack | TraceOptions.ProcessId;
			listenerData.Filter = SourceLevels.Error;
			loggingSettings.TraceListeners.Add(listenerData);

			container.AddExtension(new LoggingBlockExtension());

			EmailTraceListener createdObject = (EmailTraceListener)container.Resolve<TraceListener>("listener");

			Assert.IsNotNull(createdObject);
			Assert.AreEqual(listenerData.TraceOutputOptions, createdObject.TraceOutputOptions);
			Assert.IsNotNull(createdObject.Filter);
			Assert.IsInstanceOfType(createdObject.Filter, typeof(EventTypeFilter));
			Assert.AreEqual(listenerData.Filter, ((EventTypeFilter)createdObject.Filter).EventType);
			Assert.IsNull(createdObject.Formatter);
			// TODO test the actual values used to create the listener
		}

		[TestMethod]
		public void CanCreatePoliciesForEmailTraceListenerWithFormatter()
		{
			EmailTraceListenerData listenerData
				= new EmailTraceListenerData("listener", "to address", "from address", "starter", "ender", "smtp", 25, "formatter");
			listenerData.TraceOutputOptions = TraceOptions.Callstack | TraceOptions.ProcessId;
			listenerData.Filter = SourceLevels.Error;
			loggingSettings.TraceListeners.Add(listenerData);

			FormatterData formatterData = new TextFormatterData("formatter", "template");
			loggingSettings.Formatters.Add(formatterData);

			container.AddExtension(new LoggingBlockExtension());

			EmailTraceListener createdObject = (EmailTraceListener)container.Resolve<TraceListener>("listener");

			Assert.IsNotNull(createdObject);
			Assert.AreEqual(listenerData.TraceOutputOptions, createdObject.TraceOutputOptions);
			Assert.IsNotNull(createdObject.Filter);
			Assert.IsInstanceOfType(createdObject.Filter, typeof(EventTypeFilter));
			Assert.AreEqual(listenerData.Filter, ((EventTypeFilter)createdObject.Filter).EventType);
			Assert.IsNotNull(createdObject.Formatter);
			Assert.AreSame(typeof(TextFormatter), createdObject.Formatter.GetType());
			Assert.AreEqual("template", ((TextFormatter)createdObject.Formatter).Template);
			// TODO test the actual values used to create the listener
		}

		[TestMethod]
		public void CanCreatePoliciesForFormattedEventLogTraceListener()
		{
			FormattedEventLogTraceListenerData listenerData
				= new FormattedEventLogTraceListenerData("listener", CommonUtil.EventLogSourceName, CommonUtil.EventLogName, "machine", "");
			listenerData.TraceOutputOptions = TraceOptions.Callstack | TraceOptions.ProcessId;
			listenerData.Filter = SourceLevels.Error;
			loggingSettings.TraceListeners.Add(listenerData);

			container.AddExtension(new LoggingBlockExtension());

			FormattedEventLogTraceListener createdObject = (FormattedEventLogTraceListener)container.Resolve<TraceListener>("listener");

			Assert.IsNotNull(createdObject);
			Assert.AreEqual(listenerData.TraceOutputOptions, createdObject.TraceOutputOptions);
			Assert.IsNotNull(createdObject.Filter);
			Assert.IsInstanceOfType(createdObject.Filter, typeof(EventTypeFilter));
			Assert.AreEqual(listenerData.Filter, ((EventTypeFilter)createdObject.Filter).EventType);
			Assert.IsNull(createdObject.Formatter);
			Assert.AreEqual(CommonUtil.EventLogSourceName, ((EventLogTraceListener)createdObject.SlaveListener).EventLog.Source);
			Assert.AreEqual(CommonUtil.EventLogName, ((EventLogTraceListener)createdObject.SlaveListener).EventLog.Log);
			Assert.AreEqual("machine", ((EventLogTraceListener)createdObject.SlaveListener).EventLog.MachineName);
		}

		[TestMethod]
		public void CanCreatePoliciesForFormattedEventLogTraceListenerWithFormatter()
		{
			FormattedEventLogTraceListenerData listenerData
				= new FormattedEventLogTraceListenerData("listener", CommonUtil.EventLogSourceName, CommonUtil.EventLogName, "machine", "formatter");
			listenerData.TraceOutputOptions = TraceOptions.Callstack | TraceOptions.ProcessId;
			listenerData.Filter = SourceLevels.Error;
			loggingSettings.TraceListeners.Add(listenerData);

			FormatterData formatterData = new TextFormatterData("formatter", "template");
			loggingSettings.Formatters.Add(formatterData);

			container.AddExtension(new LoggingBlockExtension());

			FormattedEventLogTraceListener createdObject = (FormattedEventLogTraceListener)container.Resolve<TraceListener>("listener");

			Assert.IsNotNull(createdObject);
			Assert.AreEqual(listenerData.TraceOutputOptions, createdObject.TraceOutputOptions);
			Assert.IsNotNull(createdObject.Filter);
			Assert.IsInstanceOfType(createdObject.Filter, typeof(EventTypeFilter));
			Assert.AreEqual(listenerData.Filter, ((EventTypeFilter)createdObject.Filter).EventType);
			Assert.IsNotNull(createdObject.Formatter);
			Assert.AreSame(typeof(TextFormatter), createdObject.Formatter.GetType());
			Assert.AreEqual("template", ((TextFormatter)createdObject.Formatter).Template);
			Assert.AreEqual(CommonUtil.EventLogSourceName, ((EventLogTraceListener)createdObject.SlaveListener).EventLog.Source);
			Assert.AreEqual(CommonUtil.EventLogName, ((EventLogTraceListener)createdObject.SlaveListener).EventLog.Log);
			Assert.AreEqual("machine", ((EventLogTraceListener)createdObject.SlaveListener).EventLog.MachineName);
		}

		[TestMethod]
		public void CanCreatePoliciesForMsmqTraceListenerWithFormatter()
		{
			MsmqTraceListenerData listenerData
				= new MsmqTraceListenerData("listener", CommonUtil.MessageQueuePath, "binary");
			listenerData.TraceOutputOptions = TraceOptions.Callstack | TraceOptions.ProcessId;
			listenerData.Filter = SourceLevels.Error;
			loggingSettings.TraceListeners.Add(listenerData);
			loggingSettings.Formatters.Add(new BinaryLogFormatterData("binary"));

			container.AddExtension(new LoggingBlockExtension());

			MsmqTraceListener createdObject = (MsmqTraceListener)container.Resolve<TraceListener>("listener");

			Assert.IsNotNull(createdObject);
			Assert.AreEqual(listenerData.TraceOutputOptions, createdObject.TraceOutputOptions);
			Assert.IsNotNull(createdObject.Filter);
			Assert.IsInstanceOfType(createdObject.Filter, typeof(EventTypeFilter));
			Assert.AreEqual(listenerData.Filter, ((EventTypeFilter)createdObject.Filter).EventType);
			Assert.IsNotNull(createdObject.Formatter);
			Assert.IsInstanceOfType(createdObject.Formatter, typeof(BinaryLogFormatter));
			Assert.AreEqual(CommonUtil.MessageQueuePath, createdObject.QueuePath);
			// there's currently no way to test for other properties
		}

		[TestMethod]
		public void CanCreatePoliciesForRollingFlatFileTraceListener()
		{
			RollingFlatFileTraceListenerData listenerData
				= new RollingFlatFileTraceListenerData(
					"listener",
					Path.Combine(Environment.CurrentDirectory, "test.log"),
					"header",
					"footer",
					100,
					"pattern",
					RollFileExistsBehavior.Overwrite,
					RollInterval.Midnight,
					TraceOptions.Callstack | TraceOptions.ProcessId,
					"");
			listenerData.Filter = SourceLevels.Error;
			loggingSettings.TraceListeners.Add(listenerData);

			container.AddExtension(new LoggingBlockExtension());

			RollingFlatFileTraceListener createdObject = (RollingFlatFileTraceListener)container.Resolve<TraceListener>("listener");

			Assert.IsNotNull(createdObject);
			Assert.AreEqual(listenerData.TraceOutputOptions, createdObject.TraceOutputOptions);
			Assert.IsNotNull(createdObject.Filter);
			Assert.IsInstanceOfType(createdObject.Filter, typeof(EventTypeFilter));
			Assert.AreEqual(listenerData.Filter, ((EventTypeFilter)createdObject.Filter).EventType);
			Assert.IsNull(createdObject.Formatter);
			Assert.AreEqual(listenerData.FileName, ((FileStream)((StreamWriter)createdObject.Writer).BaseStream).Name);
		}

		[TestMethod]
		public void CanCreatePoliciesForRollingFlatFileTraceListenerWithFormatter()
		{
			RollingFlatFileTraceListenerData listenerData
				= new RollingFlatFileTraceListenerData(
					"listener",
					Path.Combine(Environment.CurrentDirectory, "test.log"),
					"header",
					"footer",
					100,
					"pattern",
					RollFileExistsBehavior.Overwrite,
					RollInterval.Midnight,
					TraceOptions.Callstack | TraceOptions.ProcessId,
					"formatter");
			listenerData.Filter = SourceLevels.Error;
			loggingSettings.TraceListeners.Add(listenerData);

			FormatterData formatterData = new TextFormatterData("formatter", "template");
			loggingSettings.Formatters.Add(formatterData);

			container.AddExtension(new LoggingBlockExtension());

			RollingFlatFileTraceListener createdObject = (RollingFlatFileTraceListener)container.Resolve<TraceListener>("listener");

			Assert.IsNotNull(createdObject);
			Assert.AreEqual("listener", createdObject.Name);
			Assert.AreEqual(listenerData.TraceOutputOptions, createdObject.TraceOutputOptions);
			Assert.IsNotNull(createdObject.Filter);
			Assert.IsInstanceOfType(createdObject.Filter, typeof(EventTypeFilter));
			Assert.AreEqual(listenerData.Filter, ((EventTypeFilter)createdObject.Filter).EventType);
			Assert.IsNotNull(createdObject.Formatter);
			Assert.AreSame(typeof(TextFormatter), createdObject.Formatter.GetType());
			Assert.AreEqual("template", ((TextFormatter)createdObject.Formatter).Template);
			Assert.AreEqual(listenerData.FileName, ((FileStream)((StreamWriter)createdObject.Writer).BaseStream).Name);
		}

		private const string initializationData = "custom initialization data";
		private const string attributeValue = "value";

		[TestMethod]
		public void CanCreatePoliciesForSysDiagsTraceListener()
		{
			SystemDiagnosticsTraceListenerData listenerData
				= new SystemDiagnosticsTraceListenerData("listener", typeof(MockCustomTraceListener), initializationData, TraceOptions.Callstack);
			listenerData.SetAttributeValue(MockCustomTraceListener.AttributeKey, attributeValue);
			loggingSettings.TraceListeners.Add(listenerData);

			container.AddExtension(new LoggingBlockExtension());

			TraceListener createdObject = container.Resolve<TraceListener>("listener");

			Assert.IsNotNull(createdObject);
			Assert.AreEqual("listener", createdObject.Name);
			Assert.AreEqual(TraceOptions.Callstack, createdObject.TraceOutputOptions);
			Assert.AreEqual(typeof(MockCustomTraceListener), createdObject.GetType());
			Assert.AreEqual(initializationData, ((MockCustomTraceListener)createdObject).initData);
			Assert.AreEqual(attributeValue, ((MockCustomTraceListener)createdObject).Attribute);
		}

		[TestMethod]
		public void CanCreatePoliciesForSysDiagsTraceListenerWithoutInitData()
		{
			SystemDiagnosticsTraceListenerData listenerData
				= new SystemDiagnosticsTraceListenerData("listener", typeof(MockCustomTraceListener), "", TraceOptions.Callstack);
			listenerData.SetAttributeValue(MockCustomTraceListener.AttributeKey, attributeValue);
			loggingSettings.TraceListeners.Add(listenerData);

			container.AddExtension(new LoggingBlockExtension());

			TraceListener createdObject = container.Resolve<TraceListener>("listener");

			Assert.IsNotNull(createdObject);
			Assert.AreEqual("listener", createdObject.Name);
			Assert.AreEqual(TraceOptions.Callstack, createdObject.TraceOutputOptions);
			Assert.AreEqual(typeof(MockCustomTraceListener), createdObject.GetType());
			Assert.AreEqual(null, ((MockCustomTraceListener)createdObject).initData);	// configured with "", but set to null
			Assert.AreEqual(attributeValue, ((MockCustomTraceListener)createdObject).Attribute);
		}

		[TestMethod]
		public void CanCreatePoliciesForCustomTraceListener()
		{
			CustomTraceListenerData listenerData
				= new CustomTraceListenerData("listener", typeof(MockCustomTraceListener), "", TraceOptions.Callstack);
			listenerData.SetAttributeValue(MockCustomTraceListener.AttributeKey, attributeValue);
			loggingSettings.TraceListeners.Add(listenerData);

			container.AddExtension(new LoggingBlockExtension());

			TraceListener createdObject = container.Resolve<TraceListener>("listener");

			Assert.IsNotNull(createdObject);
			Assert.AreEqual("listener", createdObject.Name);
			Assert.AreEqual(TraceOptions.Callstack, createdObject.TraceOutputOptions);
			Assert.AreEqual(typeof(MockCustomTraceListener), createdObject.GetType());
			Assert.AreEqual(null, ((MockCustomTraceListener)createdObject).initData);	// configured with "", but set to null
			Assert.AreEqual(attributeValue, ((MockCustomTraceListener)createdObject).Attribute);
			Assert.IsNull(((MockCustomTraceListener)createdObject).Formatter);
		}

		[TestMethod]
		public void CanCreatePoliciesForCustomTraceListenerWithFormatter()
		{
			CustomTraceListenerData listenerData
				= new CustomTraceListenerData("listener", typeof(MockCustomTraceListener), "", TraceOptions.Callstack);
			listenerData.Formatter = "formatter";
			listenerData.SetAttributeValue(MockCustomTraceListener.AttributeKey, attributeValue);
			loggingSettings.TraceListeners.Add(listenerData);

			FormatterData formatterData = new TextFormatterData("formatter", "template");
			loggingSettings.Formatters.Add(formatterData);

			container.AddExtension(new LoggingBlockExtension());

			TraceListener createdObject = container.Resolve<TraceListener>("listener");

			Assert.IsNotNull(createdObject);
			Assert.AreEqual("listener", createdObject.Name);
			Assert.AreEqual(TraceOptions.Callstack, createdObject.TraceOutputOptions);
			Assert.AreEqual(typeof(MockCustomTraceListener), createdObject.GetType());
			Assert.AreEqual(null, ((MockCustomTraceListener)createdObject).initData);	// configured with "", but set to null
			Assert.AreEqual(attributeValue, ((MockCustomTraceListener)createdObject).Attribute);
			Assert.IsNotNull(((MockCustomTraceListener)createdObject).Formatter);
			Assert.AreSame(typeof(TextFormatter), ((MockCustomTraceListener)createdObject).Formatter.GetType());
			Assert.AreEqual("template", ((TextFormatter)((MockCustomTraceListener)createdObject).Formatter).Template);
		}

		[TestMethod]
		public void CreatedPoliciesIncludeContainerLifetime()
		{
			TraceListenerData listenerData = new MockTraceListenerData("listener");
			loggingSettings.TraceListeners.Add(listenerData);

			MockTraceListener listener = null;

			using (IUnityContainer newContainer = new UnityContainer())
			{
				newContainer.AddExtension(new EnterpriseLibraryCoreExtension(configurationSource));
				newContainer.AddExtension(new LoggingBlockExtension());

				listener = (MockTraceListener)newContainer.Resolve<TraceListener>("listener");

				Assert.IsNotNull(listener);
				Assert.AreSame(listener, newContainer.Resolve<TraceListener>("listener"));	// lifetime managed?

				Assert.IsFalse(listener.wasDisposed);
			}

			// TODO make work
			Assert.IsTrue(listener.wasDisposed);	// lifetime managed by the container?
		}
	}
}
