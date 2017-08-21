/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.Configuration
{
    [TestClass]
    public class FormatterEmailTraceListenerConfigurationFixture
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
            LoggingSettings rwLoggingSettings = new LoggingSettings();
            rwLoggingSettings.TraceListeners.Add(
                new EmailTraceListenerData("listener", "obviously.bad.email.address@127.0.0.1", "logging@entlib.com", "EntLib-Logging:",
                                           "has occurred", "smtphost", 25, "formatter"));
            rwLoggingSettings.TraceListeners.Add(
                new EmailTraceListenerData("listener2", "obviously.bad.email.address@127.0.0.1", "logging@entlib.com", "EntLib-Logging:",
                                           "has occurred", "smtphost", 25, "formatter"));
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = "test.exe.config";
            System.Configuration.Configuration rwConfiguration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            rwConfiguration.Sections.Remove(LoggingSettings.SectionName);
            rwConfiguration.Sections.Add(LoggingSettings.SectionName, rwLoggingSettings);
            File.SetAttributes(fileMap.ExeConfigFilename, FileAttributes.Normal);
            rwConfiguration.Save();
            System.Configuration.Configuration roConfiguration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            LoggingSettings roLoggingSettings = roConfiguration.GetSection(LoggingSettings.SectionName) as LoggingSettings;
            Assert.AreEqual(2, roLoggingSettings.TraceListeners.Count);
            Assert.IsNotNull(roLoggingSettings.TraceListeners.Get("listener"));
            Assert.IsNotNull(roLoggingSettings.TraceListeners.Get("listener2"));
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
        public void CanCreateInstanceFromGivenConfigurationWithoutFormatter()
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
    }
}
