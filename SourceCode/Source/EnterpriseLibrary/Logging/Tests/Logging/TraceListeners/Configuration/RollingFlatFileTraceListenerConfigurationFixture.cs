/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests;
using Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.Configuration
{
    [TestClass]
    public class RollingFlatFileTraceListenerConfigurationFixture
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
            RollingFlatFileTraceListenerData listenerData = new RollingFlatFileTraceListenerData("listener", "log.txt", "header", "footer", 10, "yyyy-MM-dd", RollFileExistsBehavior.Increment,
                                                                                                 RollInterval.Minute, TraceOptions.DateTime, "SimpleTextFormat");
            Assert.AreSame(typeof(RollingFlatFileTraceListener), listenerData.Type);
            Assert.AreSame(typeof(RollingFlatFileTraceListenerData), listenerData.ListenerDataType);
            Assert.AreEqual("listener", listenerData.Name);
            Assert.AreEqual("log.txt", listenerData.FileName);
            Assert.AreEqual(10, listenerData.RollSizeKB);
            Assert.AreEqual("yyyy-MM-dd", listenerData.TimeStampPattern);
            Assert.AreEqual(RollFileExistsBehavior.Increment, listenerData.RollFileExistsBehavior);
            Assert.AreEqual(RollInterval.Minute, listenerData.RollInterval);
            Assert.AreEqual(TraceOptions.DateTime, listenerData.TraceOutputOptions);
            Assert.AreEqual("SimpleTextFormat", listenerData.Formatter);
            Assert.AreEqual("header", listenerData.Header);
            Assert.AreEqual("footer", listenerData.Footer);
        }
        [TestMethod]
        public void CanDeserializeSerializedConfiguration()
        {
            string name = "some name";
            string fileName = "some filename";
            string timesTampPattern = "yyyy-MM-dd";
            int rollSizeKB = 10;
            RollFileExistsBehavior rollFileExistsBehavior = RollFileExistsBehavior.Increment;
            RollInterval rollInterval = RollInterval.Hour;
            TraceOptions traceOptions = TraceOptions.LogicalOperationStack;
            string SimpleTextFormat = "SimpleTextFormat";
            string header = "header";
            string footer = "footer";
            RollingFlatFileTraceListenerData data = new RollingFlatFileTraceListenerData(name, fileName, header, footer, rollSizeKB, timesTampPattern,
                                                                                         rollFileExistsBehavior, rollInterval, traceOptions, SimpleTextFormat, SourceLevels.Critical);
            data.TraceOutputOptions = traceOptions;
            LoggingSettings settings = new LoggingSettings();
            settings.TraceListeners.Add(data);
            IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
            sections[LoggingSettings.SectionName] = settings;
            IConfigurationSource configurationSource
                = ConfigurationTestHelper.SaveSectionsInFileAndReturnConfigurationSource(sections);
            LoggingSettings roSettigs = (LoggingSettings)configurationSource.GetSection(LoggingSettings.SectionName);
            Assert.AreEqual(1, roSettigs.TraceListeners.Count);
            Assert.IsNotNull(roSettigs.TraceListeners.Get(name));
            Assert.AreEqual(TraceOptions.LogicalOperationStack, roSettigs.TraceListeners.Get(name).TraceOutputOptions);
            Assert.AreEqual(SourceLevels.Critical, roSettigs.TraceListeners.Get(name).Filter);
            Assert.AreSame(typeof(RollingFlatFileTraceListenerData), roSettigs.TraceListeners.Get(name).GetType());
            Assert.AreSame(typeof(RollingFlatFileTraceListenerData), roSettigs.TraceListeners.Get(name).ListenerDataType);
            Assert.AreSame(typeof(RollingFlatFileTraceListener), roSettigs.TraceListeners.Get(name).Type);
            Assert.AreEqual(fileName, ((RollingFlatFileTraceListenerData)roSettigs.TraceListeners.Get(name)).FileName);
            Assert.AreEqual(timesTampPattern, ((RollingFlatFileTraceListenerData)roSettigs.TraceListeners.Get(name)).TimeStampPattern);
            Assert.AreEqual(rollSizeKB, ((RollingFlatFileTraceListenerData)roSettigs.TraceListeners.Get(name)).RollSizeKB);
            Assert.AreEqual(rollFileExistsBehavior, ((RollingFlatFileTraceListenerData)roSettigs.TraceListeners.Get(name)).RollFileExistsBehavior);
            Assert.AreEqual(rollInterval, ((RollingFlatFileTraceListenerData)roSettigs.TraceListeners.Get(name)).RollInterval);
            Assert.AreEqual("SimpleTextFormat", ((RollingFlatFileTraceListenerData)roSettigs.TraceListeners.Get(name)).Formatter);
            Assert.AreEqual(header, ((RollingFlatFileTraceListenerData)roSettigs.TraceListeners.Get(name)).Header);
            Assert.AreEqual(footer, ((RollingFlatFileTraceListenerData)roSettigs.TraceListeners.Get(name)).Footer);
        }
        [TestMethod]
        public void CanDeserializeSerializedConfigurationWithDefaults()
        {
            LoggingSettings rwLoggingSettings = new LoggingSettings();
            rwLoggingSettings.TraceListeners.Add(new RollingFlatFileTraceListenerData("listener1", "log1.txt", "header", "footer", 10, "yyyy-MM-dd", RollFileExistsBehavior.Increment,
                                                                                      RollInterval.Minute, TraceOptions.DateTime, "SimpleTextFormat1"));
            rwLoggingSettings.TraceListeners.Add(new RollingFlatFileTraceListenerData("listener2", "log2.txt", "header", "footer", 10, "yyyy-MM-dd", RollFileExistsBehavior.Increment,
                                                                                      RollInterval.Minute, TraceOptions.DateTime, "SimpleTextFormat1"));
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
        public void CanCreateInstanceFromGivenConfiguration()
        {
            RollingFlatFileTraceListenerData listenerData = new RollingFlatFileTraceListenerData("listener", "log.txt", "header", "footer", 10, "yyyy-MM-dd", RollFileExistsBehavior.Increment,
                                                                                                 RollInterval.Minute, TraceOptions.DateTime, "SimpleTextFormat");
            MockLogObjectsHelper helper = new MockLogObjectsHelper();
            helper.loggingSettings.Formatters.Add(new TextFormatterData("SimpleTextFormat", "foobar template"));
            TraceListener listener = TraceListenerCustomFactory.Instance.Create(context, listenerData, helper.configurationSource, reflectionCache);
            Assert.IsNotNull(listener);
            Assert.AreEqual("listener", listener.Name);
            Assert.AreEqual(listener.GetType(), typeof(RollingFlatFileTraceListener));
        }
        [TestMethod]
        public void CanCreateInstanceFromConfigurationFile()
        {
            LoggingSettings loggingSettings = new LoggingSettings();
            loggingSettings.Formatters.Add(new TextFormatterData("SimpleTextFormat", "foobar template"));
            loggingSettings.TraceListeners.Add(new RollingFlatFileTraceListenerData("listener", "log.txt", "header", "footer", 10, "yyyy-MM-dd", RollFileExistsBehavior.Increment,
                                                                                    RollInterval.Minute, TraceOptions.DateTime, "SimpleTextFormat"));
            TraceListener listener = TraceListenerCustomFactory.Instance.Create(context, "listener", CommonUtil.SaveSectionsAndGetConfigurationSource(loggingSettings), reflectionCache);
            Assert.IsNotNull(listener);
            Assert.AreEqual(listener.GetType(), typeof(RollingFlatFileTraceListener));
            Assert.AreEqual(TraceOptions.DateTime, listener.TraceOutputOptions);
        }
    }
}
