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
    public class FormattedEmailTraceListenerConfigurationFixture
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
        public void ListenerDataIsCreatedCorrectly()
        {
            EmailTraceListenerData listenerData =
                new EmailTraceListenerData("listener", "obviously.bad.email.address@127.0.0.1", "logging@entlib.com", "EntLib-Logging:",
                                           "has occurred", "smtphost", 25, "formatter");

            Assert.AreSame(typeof(EmailTraceListener), listenerData.Type);
            Assert.AreSame(typeof(EmailTraceListenerData), listenerData.ListenerDataType);
            Assert.AreEqual("listener", listenerData.Name);
            Assert.AreEqual("smtphost", listenerData.SmtpServer);
            Assert.AreEqual("formatter", listenerData.Formatter);
        }

        [TestMethod]
        public void CanDeserializeSerializedConfiguration()
        {
            string name = "name";
            string toAddress = "obviously.bad.email.address@127.0.0.1";
            string fromAddress = "logging@entlib.com";
            string subjectStarter = "EntLib-Logging:";
            string subjectEnder = "has occurred";
            string server = "smtphost";
            int port = 25;
            string formatter = "formatter";

            TraceListenerData data = new EmailTraceListenerData(name, toAddress, fromAddress, subjectStarter,
                                                                subjectEnder, server, port, formatter, TraceOptions.Callstack, SourceLevels.Critical);

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
            Assert.AreEqual(SourceLevels.Critical, roSettigs.TraceListeners.Get(name).Filter);
            Assert.AreSame(typeof(EmailTraceListenerData), roSettigs.TraceListeners.Get(name).GetType());
            Assert.AreSame(typeof(EmailTraceListenerData), roSettigs.TraceListeners.Get(name).ListenerDataType);
            Assert.AreSame(typeof(EmailTraceListener), roSettigs.TraceListeners.Get(name).Type);
            Assert.AreEqual(formatter, ((EmailTraceListenerData)roSettigs.TraceListeners.Get(name)).Formatter);
            Assert.AreEqual(fromAddress, ((EmailTraceListenerData)roSettigs.TraceListeners.Get(name)).FromAddress);
            Assert.AreEqual(port, ((EmailTraceListenerData)roSettigs.TraceListeners.Get(name)).SmtpPort);
            Assert.AreEqual(server, ((EmailTraceListenerData)roSettigs.TraceListeners.Get(name)).SmtpServer);
            Assert.AreEqual(subjectEnder, ((EmailTraceListenerData)roSettigs.TraceListeners.Get(name)).SubjectLineEnder);
            Assert.AreEqual(subjectStarter, ((EmailTraceListenerData)roSettigs.TraceListeners.Get(name)).SubjectLineStarter);
            Assert.AreEqual(toAddress, ((EmailTraceListenerData)roSettigs.TraceListeners.Get(name)).ToAddress);
        }

        [TestMethod]
        public void CanCreateInstanceFromGivenName()
        {
            EmailTraceListenerData listenerData =
                new EmailTraceListenerData("listener", "obviously.bad.email.address@127.0.0.1", "logging@entlib.com", "EntLib-Logging:",
                                           "has occurred", "smtphost", 25, "formatter");

            MockLogObjectsHelper helper = new MockLogObjectsHelper();
            helper.loggingSettings.TraceListeners.Add(listenerData);
            helper.loggingSettings.Formatters.Add(new TextFormatterData("formatter", "foobar template"));

            TraceListener listener = TraceListenerCustomFactory.Instance.Create(context, "listener", helper.configurationSource, reflectionCache);

            Assert.IsNotNull(listener);
            Assert.AreEqual("listener", listener.Name);
            Assert.AreEqual(listener.GetType(), typeof(EmailTraceListener));
            Assert.IsNotNull(((EmailTraceListener)listener).Formatter);
            Assert.AreEqual(((EmailTraceListener)listener).Formatter.GetType(), typeof(TextFormatter));
            Assert.AreEqual("foobar template", ((TextFormatter)((EmailTraceListener)listener).Formatter).Template);
        }

        [TestMethod]
        public void CanCreateInstanceFromGivenConfiguration()
        {
            EmailTraceListenerData listenerData =
                new EmailTraceListenerData("listener", "obviously.bad.email.address@127.0.0.1", "logging@entlib.com", "EntLib-Logging:",
                                           "has occurred", "smtphost", 25, "formatter");

            MockLogObjectsHelper helper = new MockLogObjectsHelper();
            helper.loggingSettings.Formatters.Add(new TextFormatterData("formatter", "foobar template"));

            TraceListener listener = TraceListenerCustomFactory.Instance.Create(context, listenerData, helper.configurationSource, reflectionCache);

            Assert.IsNotNull(listener);
            Assert.AreEqual("listener", listener.Name);
            Assert.AreEqual(listener.GetType(), typeof(EmailTraceListener));
            Assert.IsNotNull(((EmailTraceListener)listener).Formatter);
            Assert.AreEqual(((EmailTraceListener)listener).Formatter.GetType(), typeof(TextFormatter));
            Assert.AreEqual("foobar template", ((TextFormatter)((EmailTraceListener)listener).Formatter).Template);
        }

        [TestMethod]
        public void CanCreateInstanceFromGivenConfigurationWithNullFormatter()
        {
            EmailTraceListenerData listenerData =
                new EmailTraceListenerData("listener", "obviously.bad.email.address@127.0.0.1", "logging@entlib.com", "EntLib-Logging:",
                                           "has occurred", "smtphost", 25, null);

            MockLogObjectsHelper helper = new MockLogObjectsHelper();

            TraceListener listener = TraceListenerCustomFactory.Instance.Create(context, listenerData, helper.configurationSource, reflectionCache);

            Assert.IsNotNull(listener);
            Assert.AreEqual("listener", listener.Name);
            Assert.AreEqual(listener.GetType(), typeof(EmailTraceListener));
            Assert.IsNull(((EmailTraceListener)listener).Formatter);
        }

        [TestMethod]
        public void CanCreateInstanceFromConfigurationFile()
        {
            LoggingSettings loggingSettings = new LoggingSettings();
            loggingSettings.Formatters.Add(new TextFormatterData("formatter", "foobar template"));
            loggingSettings.TraceListeners.Add(
                new EmailTraceListenerData("listener", "obviously.bad.email.address@127.0.0.1", "logging@entlib.com", "EntLib-Logging:",
                                           "has occurred", "smtphost", 25, "formatter"));

            TraceListener listener = TraceListenerCustomFactory.Instance.Create(context, "listener", CommonUtil.SaveSectionsAndGetConfigurationSource(loggingSettings), reflectionCache);

            Assert.IsNotNull(listener);
            Assert.AreEqual(listener.GetType(), typeof(EmailTraceListener));
            Assert.IsNotNull(((EmailTraceListener)listener).Formatter);
            Assert.AreEqual(((EmailTraceListener)listener).Formatter.GetType(), typeof(TextFormatter));
            Assert.AreEqual("foobar template", ((TextFormatter)((EmailTraceListener)listener).Formatter).Template);
        }

        [TestMethod]
        public void CanCreateInstanceFromConfigurationFileWithoutFormatter()
        {
            LoggingSettings loggingSettings = new LoggingSettings();
            loggingSettings.TraceListeners.Add(
                new EmailTraceListenerData("listener", "obviously.bad.email.address@127.0.0.1", "logging@entlib.com", "EntLib-Logging:",
                                           "has occurred", "smtphost", 25, null));

            TraceListener listener = TraceListenerCustomFactory.Instance.Create(context, "listener", CommonUtil.SaveSectionsAndGetConfigurationSource(loggingSettings), reflectionCache);

            Assert.IsNotNull(listener);
            Assert.AreEqual(listener.GetType(), typeof(EmailTraceListener));
            Assert.IsNull(((EmailTraceListener)listener).Formatter);
        }
    }
}
