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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Tests
{
    [TestClass]
    public class TraceSourceDataFixture
    {
        [TestInitialize]
        public void TestInitialize()
        {
            AppDomain.CurrentDomain.SetData("APPBASE", Environment.CurrentDirectory);
        }
        [TestMethod]
        public void CanDeserializeSerializedDefaultConfiguration()
        {
            string name = "name";
            bool autoFlush = true;
            TraceSourceData data = new TraceSourceData(name, SourceLevels.Critical);
            data.TraceListeners.Add(new TraceListenerReferenceData("listener1"));
            data.TraceListeners.Add(new TraceListenerReferenceData("listener2"));
            LoggingSettings settings = new LoggingSettings();
            settings.TraceSources.Add(data);
            IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
            sections[LoggingSettings.SectionName] = settings;
            IConfigurationSource configurationSource
                = ConfigurationTestHelper.SaveSectionsInFileAndReturnConfigurationSource(sections);
            LoggingSettings roSettigs = (LoggingSettings)configurationSource.GetSection(LoggingSettings.SectionName);
            Assert.AreEqual(1, roSettigs.TraceSources.Count);
            Assert.IsNotNull(roSettigs.TraceSources.Get(name));
            Assert.AreEqual(SourceLevels.Critical, roSettigs.TraceSources.Get(name).DefaultLevel);
            Assert.AreEqual(autoFlush, roSettigs.TraceSources.Get(name).AutoFlush);
            Assert.AreEqual(2, roSettigs.TraceSources.Get(name).TraceListeners.Count);
            Assert.IsNotNull(roSettigs.TraceSources.Get(name).TraceListeners.Get("listener1"));
            Assert.IsNotNull(roSettigs.TraceSources.Get(name).TraceListeners.Get("listener2"));
        }
        [TestMethod]
        public void CanDeserializeSerializedWithAutoFlushConfiguration()
        {
            string name = "name";
            bool autoFlush = false;
            TraceSourceData data = new TraceSourceData(name, SourceLevels.Critical, false);
            data.TraceListeners.Add(new TraceListenerReferenceData("listener1"));
            data.TraceListeners.Add(new TraceListenerReferenceData("listener2"));
            LoggingSettings settings = new LoggingSettings();
            settings.TraceSources.Add(data);
            IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
            sections[LoggingSettings.SectionName] = settings;
            IConfigurationSource configurationSource
                = ConfigurationTestHelper.SaveSectionsInFileAndReturnConfigurationSource(sections);
            LoggingSettings roSettigs = (LoggingSettings)configurationSource.GetSection(LoggingSettings.SectionName);
            Assert.AreEqual(1, roSettigs.TraceSources.Count);
            Assert.IsNotNull(roSettigs.TraceSources.Get(name));
            Assert.AreEqual(SourceLevels.Critical, roSettigs.TraceSources.Get(name).DefaultLevel);
            Assert.AreEqual(autoFlush, roSettigs.TraceSources.Get(name).AutoFlush);
            Assert.AreEqual(2, roSettigs.TraceSources.Get(name).TraceListeners.Count);
            Assert.IsNotNull(roSettigs.TraceSources.Get(name).TraceListeners.Get("listener1"));
            Assert.IsNotNull(roSettigs.TraceSources.Get(name).TraceListeners.Get("listener2"));
        }
    }
}
