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
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests;
using Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.Configuration
{
    [TestClass]
    public class FormattedEventLogTraceListenerConfigurationFixture
    {
        IBuilderContext context;
        ConfigurationReflectionCache reflectionCache;

        [TestInitialize]
        public void SetUp()
        {
            AppDomain.CurrentDomain.SetData("APPBASE", Environment.CurrentDirectory);
			context = new BuilderContext(new StrategyChain(), null, null, new PolicyList(), null, null);
            reflectionCache = new ConfigurationReflectionCache();
        }

        [TestMethod]
        public void ListenerDataIsCreatedCorrectlyUsingDefaults()
        {
            FormattedEventLogTraceListenerData listenerData = new FormattedEventLogTraceListenerData("listener", "unknown source", "formatter");

            Assert.AreSame(typeof(FormattedEventLogTraceListener), listenerData.Type);
            Assert.AreSame(typeof(FormattedEventLogTraceListenerData), listenerData.ListenerDataType);
            Assert.AreEqual("listener", listenerData.Name);
            Assert.AreEqual("unknown source", listenerData.Source);
            Assert.AreEqual("formatter", listenerData.Formatter);
            Assert.AreEqual(FormattedEventLogTraceListener.DefaultLogName, listenerData.Log);
            Assert.AreEqual(FormattedEventLogTraceListener.DefaultMachineName, listenerData.MachineName);
        }

        [TestMethod]
        public void ListenerDataIsCreatedCorrectly()
        {
            FormattedEventLogTraceListenerData listenerData = new FormattedEventLogTraceListenerData("listener", "unknown source", "custom", "machine", "formatter");

            Assert.AreSame(typeof(FormattedEventLogTraceListener), listenerData.Type);
            Assert.AreSame(typeof(FormattedEventLogTraceListenerData), listenerData.ListenerDataType);
            Assert.AreEqual("listener", listenerData.Name);
            Assert.AreEqual("unknown source", listenerData.Source);
            Assert.AreEqual("formatter", listenerData.Formatter);
            Assert.AreEqual("custom", listenerData.Log);
            Assert.AreEqual("machine", listenerData.MachineName);
        }

        [TestMethod]
        public void CanDeserializeSerializedConfiguration()
        {
            string name = "name";
            string source = "source";
            string log = "log";
            string machine = "machine";
            string formatter = "formatter";

            TraceListenerData data = new FormattedEventLogTraceListenerData(name, source, log, machine,
                                                                            formatter, TraceOptions.Callstack);

            LoggingSettings settings = new LoggingSettings();
            settings.TraceListeners.Add(data);

            IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
            sections[LoggingSettings.SectionName] = settings;
            IConfigurationSource configurationSource
                = ConfigurationTestHelper.SaveSectionsInFileAndReturnConfigurationSource(sections);

            LoggingSettings roSettigs = (LoggingSettings)configurationSource.GetSection(LoggingSettings.SectionName);

            Assert.AreEqual(1, roSettigs.TraceListeners.Count);
            Assert.IsNotNull(roSettigs.TraceListeners.Get(name));
            Assert.AreEqual(TraceOptions.Callstack, roSettigs.TraceListeners.Get(name).TraceOutputOptions);
            Assert.AreSame(typeof(FormattedEventLogTraceListenerData), roSettigs.TraceListeners.Get(name).GetType());
            Assert.AreSame(typeof(FormattedEventLogTraceListenerData), roSettigs.TraceListeners.Get(name).ListenerDataType);
            Assert.AreSame(typeof(FormattedEventLogTraceListener), roSettigs.TraceListeners.Get(name).Type);
            Assert.AreEqual(formatter, ((FormattedEventLogTraceListenerData)roSettigs.TraceListeners.Get(name)).Formatter);
            Assert.AreEqual(log, ((FormattedEventLogTraceListenerData)roSettigs.TraceListeners.Get(name)).Log);
            Assert.AreEqual(machine, ((FormattedEventLogTraceListenerData)roSettigs.TraceListeners.Get(name)).MachineName);
            Assert.AreEqual(source, ((FormattedEventLogTraceListenerData)roSettigs.TraceListeners.Get(name)).Source);
        }

        [TestMethod]
        public void CanDeserializeSerializedConfigurationWithDefaults()
        {
            LoggingSettings rwLoggingSettings = new LoggingSettings();
            rwLoggingSettings.TraceListeners.Add(new FormattedEventLogTraceListenerData("listener1", "unknown source", "formatter"));
            rwLoggingSettings.TraceListeners.Add(new FormattedEventLogTraceListenerData("listener2", "unknown source", "log", "machine", "formatter"));

            IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
            sections[LoggingSettings.SectionName] = rwLoggingSettings;
            IConfigurationSource configurationSource
                = ConfigurationTestHelper.SaveSectionsInFileAndReturnConfigurationSource(sections);

            LoggingSettings roLoggingSettings = (LoggingSettings)configurationSource.GetSection(LoggingSettings.SectionName);

            Assert.AreEqual(2, roLoggingSettings.TraceListeners.Count);

            Assert.IsNotNull(roLoggingSettings.TraceListeners.Get("listener1"));
            Assert.AreEqual(typeof(FormattedEventLogTraceListenerData), roLoggingSettings.TraceListeners.Get("listener1").GetType());
            FormattedEventLogTraceListenerData listener1Data =
                (FormattedEventLogTraceListenerData)roLoggingSettings.TraceListeners.Get("listener1");
            Assert.AreEqual("listener1", listener1Data.Name);
            Assert.AreEqual("unknown source", listener1Data.Source);
            Assert.AreEqual("formatter", listener1Data.Formatter);
            Assert.AreEqual(typeof(FormattedEventLogTraceListener), listener1Data.Type);
            Assert.AreEqual(typeof(FormattedEventLogTraceListenerData), listener1Data.ListenerDataType);
            Assert.AreEqual(FormattedEventLogTraceListener.DefaultLogName, listener1Data.Log);
            Assert.AreEqual(FormattedEventLogTraceListener.DefaultMachineName, listener1Data.MachineName);

            Assert.IsNotNull(roLoggingSettings.TraceListeners.Get("listener2"));
            Assert.AreEqual(typeof(FormattedEventLogTraceListenerData), roLoggingSettings.TraceListeners.Get("listener2").GetType());
            FormattedEventLogTraceListenerData listener2Data =
                (FormattedEventLogTraceListenerData)roLoggingSettings.TraceListeners.Get("listener2");
            Assert.AreEqual("listener2", listener2Data.Name);
            Assert.AreEqual("unknown source", listener2Data.Source);
            Assert.AreEqual("formatter", listener2Data.Formatter);
            Assert.AreEqual(typeof(FormattedEventLogTraceListener), listener2Data.Type);
            Assert.AreEqual(typeof(FormattedEventLogTraceListenerData), listener2Data.ListenerDataType);
            Assert.AreEqual("log", listener2Data.Log);
            Assert.AreEqual("machine", listener2Data.MachineName);
        }

        [TestMethod]
        public void CanCreateInstanceFromGivenName()
        {
            FormattedEventLogTraceListenerData listenerData = new FormattedEventLogTraceListenerData("listener", "unknown source", "formatter");

            MockLogObjectsHelper helper = new MockLogObjectsHelper();
            helper.loggingSettings.Formatters.Add(new TextFormatterData("formatter", "foobar template"));
            helper.loggingSettings.TraceListeners.Add(listenerData);

            TraceListener listener = TraceListenerCustomFactory.Instance.Create(context, "listener", helper.configurationSource, reflectionCache);

            Assert.IsNotNull(listener);
            Assert.AreEqual("listener", listener.Name);
            Assert.AreEqual(listener.GetType(), typeof(FormattedEventLogTraceListener));
            FormattedEventLogTraceListener castedListener = (FormattedEventLogTraceListener)listener;
            Assert.IsNotNull(castedListener.Formatter);
            Assert.AreEqual("unknown source", ((EventLogTraceListener)castedListener.SlaveListener).EventLog.Source);
            Assert.AreEqual(FormattedEventLogTraceListener.DefaultLogName, ((EventLogTraceListener)castedListener.SlaveListener).EventLog.Log);
            Assert.AreEqual(FormattedEventLogTraceListener.DefaultMachineName, ((EventLogTraceListener)castedListener.SlaveListener).EventLog.MachineName);
            Assert.AreEqual(castedListener.Formatter.GetType(), typeof(TextFormatter));
            Assert.AreEqual("foobar template", ((TextFormatter)castedListener.Formatter).Template);
        }

        [TestMethod]
        public void CanCreateInstanceFromGivenConfiguration()
        {
            FormattedEventLogTraceListenerData listenerData = new FormattedEventLogTraceListenerData("listener", "unknown source", "formatter");

            MockLogObjectsHelper helper = new MockLogObjectsHelper();
            helper.loggingSettings.Formatters.Add(new TextFormatterData("formatter", "foobar template"));

            TraceListener listener = TraceListenerCustomFactory.Instance.Create(context, listenerData, helper.configurationSource, reflectionCache);

            Assert.IsNotNull(listener);
            Assert.AreEqual("listener", listener.Name);
            Assert.AreEqual(listener.GetType(), typeof(FormattedEventLogTraceListener));
            FormattedEventLogTraceListener castedListener = (FormattedEventLogTraceListener)listener;
            Assert.IsNotNull(castedListener.Formatter);
            Assert.AreEqual("unknown source", ((EventLogTraceListener)castedListener.SlaveListener).EventLog.Source);
            Assert.AreEqual(FormattedEventLogTraceListener.DefaultLogName, ((EventLogTraceListener)castedListener.SlaveListener).EventLog.Log);
            Assert.AreEqual(FormattedEventLogTraceListener.DefaultMachineName, ((EventLogTraceListener)castedListener.SlaveListener).EventLog.MachineName);
            Assert.AreEqual(castedListener.Formatter.GetType(), typeof(TextFormatter));
            Assert.AreEqual("foobar template", ((TextFormatter)castedListener.Formatter).Template);
        }

        [TestMethod]
        public void CanCreateInstanceFromConfigurationFile()
        {
            LoggingSettings loggingSettings = new LoggingSettings();
            loggingSettings.Formatters.Add(new TextFormatterData("formatter", "foobar template"));
            loggingSettings.TraceListeners.Add(new FormattedEventLogTraceListenerData("listener", "unknown source", "log", "machine", "formatter"));

            TraceListener listener = TraceListenerCustomFactory.Instance.Create(context, "listener", CommonUtil.SaveSectionsAndGetConfigurationSource(loggingSettings), reflectionCache);

            Assert.IsNotNull(listener);
            Assert.AreEqual(listener.GetType(), typeof(FormattedEventLogTraceListener));
            FormattedEventLogTraceListener castedListener = (FormattedEventLogTraceListener)listener;
            Assert.IsNotNull(castedListener.Formatter);
            Assert.AreEqual("unknown source", ((EventLogTraceListener)castedListener.SlaveListener).EventLog.Source);
            Assert.AreEqual("log", ((EventLogTraceListener)castedListener.SlaveListener).EventLog.Log);
            Assert.AreEqual("machine", ((EventLogTraceListener)castedListener.SlaveListener).EventLog.MachineName);
            Assert.AreEqual(castedListener.Formatter.GetType(), typeof(TextFormatter));
            Assert.AreEqual("foobar template", ((TextFormatter)castedListener.Formatter).Template);
        }

        [TestMethod]
        public void CanCreateInstanceFromConfigurationFileWithMinimalConfiguration()
        {
            LoggingSettings loggingSettings = new LoggingSettings();
            loggingSettings.Formatters.Add(new TextFormatterData("formatter", "foobar template"));
            FormattedEventLogTraceListenerData listenerData = new FormattedEventLogTraceListenerData();
            listenerData.Name = "listener";
            listenerData.Type = typeof(FormattedEventLogTraceListener);
            listenerData.ListenerDataType = typeof(FormattedEventLogTraceListenerData);
            listenerData.Source = "unknown source";
            listenerData.Formatter = "formatter";
            loggingSettings.TraceListeners.Add(listenerData);

            TraceListener listener = TraceListenerCustomFactory.Instance.Create(context, "listener", CommonUtil.SaveSectionsAndGetConfigurationSource(loggingSettings), reflectionCache);

            Assert.IsNotNull(listener);
            Assert.AreEqual(listener.GetType(), typeof(FormattedEventLogTraceListener));
            FormattedEventLogTraceListener castedListener = (FormattedEventLogTraceListener)listener;
            Assert.IsNotNull(castedListener.Formatter);
            Assert.AreEqual("unknown source", ((EventLogTraceListener)castedListener.SlaveListener).EventLog.Source);
            Assert.AreEqual(FormattedEventLogTraceListener.DefaultLogName, ((EventLogTraceListener)castedListener.SlaveListener).EventLog.Log);
            Assert.AreEqual(FormattedEventLogTraceListener.DefaultMachineName, ((EventLogTraceListener)castedListener.SlaveListener).EventLog.MachineName);
            Assert.AreEqual(castedListener.Formatter.GetType(), typeof(TextFormatter));
            Assert.AreEqual("foobar template", ((TextFormatter)castedListener.Formatter).Template);
        }
    }
}
