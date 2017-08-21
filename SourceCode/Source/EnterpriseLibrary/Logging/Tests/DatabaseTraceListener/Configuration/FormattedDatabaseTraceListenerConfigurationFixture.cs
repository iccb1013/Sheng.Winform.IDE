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
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Database.Tests.Configuration
{
    [TestClass]
    public class FormattedDatabaseTraceListenerConfigurationFixture
    {
        private IBuilderContext context;
        private ConfigurationReflectionCache reflectionCache;

        [TestInitialize]
        public void SetUp()
        {
            AppDomain.CurrentDomain.SetData("APPBASE", Environment.CurrentDirectory);
            context
                = new BuilderContext(new StrategyChain(new object[] { new ConfiguredObjectStrategy() }),
                                     null,
                                     null,
                                     new PolicyList(),
                                     null,
                                     null);
            reflectionCache = new ConfigurationReflectionCache();
        }

        [TestMethod]
        public void ListenerDataIsCreatedCorrectly()
        {
            FormattedDatabaseTraceListenerData listenerData =
                new FormattedDatabaseTraceListenerData("listener", "WriteLog", "AddCategory", "LoggingDb", "formatter");

            Assert.AreSame(typeof(FormattedDatabaseTraceListener), listenerData.Type);
            Assert.AreSame(typeof(FormattedDatabaseTraceListenerData), listenerData.ListenerDataType);
            Assert.AreEqual("listener", listenerData.Name);
            Assert.AreEqual("WriteLog", listenerData.WriteLogStoredProcName);
            Assert.AreEqual("AddCategory", listenerData.AddCategoryStoredProcName);
            Assert.AreEqual("LoggingDb", listenerData.DatabaseInstanceName);
            Assert.AreEqual("formatter", listenerData.Formatter);
        }

        [TestMethod]
        public void CanDeserializeSerializedConfiguration()
        {
            string name = "name";
            string write = "write";
            string add = "add";
            string database = "database";
            string formatter = "formatter";

            TraceListenerData data =
                new FormattedDatabaseTraceListenerData(name,
                                                       write,
                                                       add,
                                                       database,
                                                       formatter,
                                                       TraceOptions.Callstack,
                                                       SourceLevels.Critical);

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
            Assert.AreSame(typeof(FormattedDatabaseTraceListenerData), roSettigs.TraceListeners.Get(name).GetType());
            Assert.AreSame(typeof(FormattedDatabaseTraceListenerData), roSettigs.TraceListeners.Get(name).ListenerDataType);
            Assert.AreSame(typeof(FormattedDatabaseTraceListener), roSettigs.TraceListeners.Get(name).Type);
            Assert.AreEqual(add,
                            ((FormattedDatabaseTraceListenerData)roSettigs.TraceListeners.Get(name)).AddCategoryStoredProcName);
            Assert.AreEqual(database,
                            ((FormattedDatabaseTraceListenerData)roSettigs.TraceListeners.Get(name)).DatabaseInstanceName);
            Assert.AreEqual(formatter, ((FormattedDatabaseTraceListenerData)roSettigs.TraceListeners.Get(name)).Formatter);
            Assert.AreEqual(write,
                            ((FormattedDatabaseTraceListenerData)roSettigs.TraceListeners.Get(name)).WriteLogStoredProcName);
            Assert.AreEqual(SourceLevels.Critical, roSettigs.TraceListeners.Get(name).Filter);
        }

        [TestMethod]
        public void CanDeserializeSerializedConfigurationWithDefaults()
        {
            LoggingSettings rwLoggingSettings = new LoggingSettings();
            rwLoggingSettings.TraceListeners.Add(
                new FormattedDatabaseTraceListenerData("listener1", "WriteLog", "AddCategory", "LoggingDb", "formatter"));
            rwLoggingSettings.TraceListeners.Add(
                new FormattedDatabaseTraceListenerData("listener2", "WriteLog", "AddCategory", "LoggingDb", "formatter"));

            IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
            sections[LoggingSettings.SectionName] = rwLoggingSettings;
            IConfigurationSource configurationSource
                = ConfigurationTestHelper.SaveSectionsInFileAndReturnConfigurationSource(sections);

            LoggingSettings roLoggingSettings = (LoggingSettings)configurationSource.GetSection(LoggingSettings.SectionName);

            Assert.AreEqual(2, roLoggingSettings.TraceListeners.Count);
            Assert.IsNotNull(roLoggingSettings.TraceListeners.Get("listener1"));
            Assert.IsNotNull(roLoggingSettings.TraceListeners.Get("listener2"));
        }

        [TestMethod]
        public void CanCreateInstanceFromGivenName()
        {
            FormattedDatabaseTraceListenerData listenerData =
                new FormattedDatabaseTraceListenerData("listener", "WriteLog", "AddCategory", "LoggingDb", "formatter");
            MockLogObjectsHelper helper = new MockLogObjectsHelper();
            helper.loggingSettings.Formatters.Add(new TextFormatterData("formatter", "foobar template"));
            helper.loggingSettings.TraceListeners.Add(listenerData);

            TraceListener listener =
                TraceListenerCustomFactory.Instance.Create(context, "listener", helper.configurationSource, reflectionCache);

            Assert.IsNotNull(listener);
            Assert.AreEqual(listener.GetType(), typeof(FormattedDatabaseTraceListener));
            Assert.IsNotNull(((FormattedDatabaseTraceListener)listener).Formatter);
            Assert.AreEqual(((FormattedDatabaseTraceListener)listener).Formatter.GetType(), typeof(TextFormatter));
            Assert.AreEqual("foobar template", ((TextFormatter)((FormattedDatabaseTraceListener)listener).Formatter).Template);
        }

        [TestMethod]
        public void CanCreateInstanceFromConfigurationFile()
        {
            LoggingSettings loggingSettings = new LoggingSettings();
            loggingSettings.Formatters.Add(new TextFormatterData("formatter", "foobar template"));
            loggingSettings.TraceListeners.Add(
                new FormattedDatabaseTraceListenerData("listener", "WriteLog", "AddCategory", "LoggingDb", "formatter"));

            TraceListener listener =
                TraceListenerCustomFactory.Instance.Create(context,
                                                           "listener",
                                                           CommonUtil.SaveSectionsAndGetConfigurationSource(loggingSettings),
                                                           reflectionCache);

            Assert.IsNotNull(listener);
            Assert.AreEqual(listener.GetType(), typeof(FormattedDatabaseTraceListener));
            Assert.IsNotNull(((FormattedDatabaseTraceListener)listener).Formatter);
            Assert.AreEqual(((FormattedDatabaseTraceListener)listener).Formatter.GetType(), typeof(TextFormatter));
            Assert.AreEqual("foobar template", ((TextFormatter)((FormattedDatabaseTraceListener)listener).Formatter).Template);
        }

        [TestMethod]
        public void CanCreateInstanceWithNoFormatter()
        {
            LoggingSettings loggingSettings = new LoggingSettings();
            loggingSettings.TraceListeners.Add(
                new FormattedDatabaseTraceListenerData("listener", "WriteLog", "AddCategory", "LoggingDb", null));

            TraceListener listener =
                TraceListenerCustomFactory.Instance.Create(context,
                                                           "listener",
                                                           CommonUtil.SaveSectionsAndGetConfigurationSource(loggingSettings),
                                                           reflectionCache);

            Assert.IsNotNull(listener);
            Assert.AreEqual(listener.GetType(), typeof(FormattedDatabaseTraceListener));
            Assert.IsNull(((FormattedDatabaseTraceListener)listener).Formatter);
        }
    }
}
