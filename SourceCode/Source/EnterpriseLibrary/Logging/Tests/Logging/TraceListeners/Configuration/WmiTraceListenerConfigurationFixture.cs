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
    public class WmiTraceListenerConfigurationFixture
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
            WmiTraceListenerData listenerData =
                new WmiTraceListenerData("listener");
            Assert.AreSame(typeof(WmiTraceListener), listenerData.Type);
            Assert.AreSame(typeof(WmiTraceListenerData), listenerData.ListenerDataType);
            Assert.AreEqual("listener", listenerData.Name);
        }
        [TestMethod]
        public void CanDeserializeSerializedConfiguration()
        {
            string name = "name";
            TraceListenerData data = new WmiTraceListenerData(name, TraceOptions.Callstack, SourceLevels.Critical);
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
            Assert.AreSame(typeof(WmiTraceListenerData), roSettigs.TraceListeners.Get(name).GetType());
            Assert.AreSame(typeof(WmiTraceListenerData), roSettigs.TraceListeners.Get(name).ListenerDataType);
            Assert.AreSame(typeof(WmiTraceListener), roSettigs.TraceListeners.Get(name).Type);
        }
        [TestMethod]
        public void CanDeserializeSerializedConfigurationWithDefaults()
        {
            LoggingSettings rwLoggingSettings = new LoggingSettings();
            rwLoggingSettings.TraceListeners.Add(
                new WmiTraceListenerData("listener"));
            rwLoggingSettings.TraceListeners.Add(
                new WmiTraceListenerData("listener2"));
            IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
            sections[LoggingSettings.SectionName] = rwLoggingSettings;
            IConfigurationSource configurationSource
                = ConfigurationTestHelper.SaveSectionsInFileAndReturnConfigurationSource(sections);
            LoggingSettings roLoggingSettings = (LoggingSettings)configurationSource.GetSection(LoggingSettings.SectionName);
            Assert.AreEqual(2, roLoggingSettings.TraceListeners.Count);
            Assert.IsNotNull(roLoggingSettings.TraceListeners.Get("listener"));
            Assert.IsNotNull(roLoggingSettings.TraceListeners.Get("listener2"));
        }
        [TestMethod]
        public void CanCreateInstanceFromGivenName()
        {
            WmiTraceListenerData listenerData =
                new WmiTraceListenerData("listener");
            MockLogObjectsHelper helper = new MockLogObjectsHelper();
            helper.loggingSettings.TraceListeners.Add(listenerData);
            TraceListener listener = TraceListenerCustomFactory.Instance.Create(context, "listener", helper.configurationSource, reflectionCache);
            Assert.IsNotNull(listener);
            Assert.AreEqual("listener", listener.Name);
            Assert.AreEqual(listener.GetType(), typeof(WmiTraceListener));
        }
        [TestMethod]
        public void CanCreateInstanceFromGivenConfiguration()
        {
            WmiTraceListenerData listenerData =
                new WmiTraceListenerData("listener");
            MockLogObjectsHelper helper = new MockLogObjectsHelper();
            TraceListener listener = TraceListenerCustomFactory.Instance.Create(context, listenerData, helper.configurationSource, reflectionCache);
            Assert.IsNotNull(listener);
            Assert.AreEqual("listener", listener.Name);
            Assert.AreEqual(listener.GetType(), typeof(WmiTraceListener));
        }
        [TestMethod]
        public void CanCreateInstanceFromConfigurationFile()
        {
            LoggingSettings loggingSettings = new LoggingSettings();
            loggingSettings.TraceListeners.Add(
                new WmiTraceListenerData("listener"));
            TraceListener listener = TraceListenerCustomFactory.Instance.Create(context, "listener", CommonUtil.SaveSectionsAndGetConfigurationSource(loggingSettings), reflectionCache);
            Assert.IsNotNull(listener);
            Assert.AreEqual(listener.GetType(), typeof(WmiTraceListener));
        }
    }
}
