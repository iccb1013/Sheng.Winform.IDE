/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests;
using Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Formatters
{
    [TestClass]
    public class BinaryLogFormatterFixture
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
        public void CanDeserializeSerializedConfiguration()
        {
            LoggingSettings rwLoggingSettings = new LoggingSettings();
            rwLoggingSettings.Formatters.Add(new BinaryLogFormatterData("formatter1"));
            rwLoggingSettings.Formatters.Add(new BinaryLogFormatterData("formatter2"));
            IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
            sections[LoggingSettings.SectionName] = rwLoggingSettings;
            IConfigurationSource configurationSource
                = ConfigurationTestHelper.SaveSectionsInFileAndReturnConfigurationSource(sections);
            LoggingSettings roLoggingSettings = (LoggingSettings)configurationSource.GetSection(LoggingSettings.SectionName);
            Assert.AreEqual(2, roLoggingSettings.Formatters.Count);
            Assert.IsNotNull(roLoggingSettings.Formatters.Get("formatter1"));
            Assert.AreSame(typeof(BinaryLogFormatterData), roLoggingSettings.Formatters.Get("formatter1").GetType());
            Assert.AreSame(typeof(BinaryLogFormatter), roLoggingSettings.Formatters.Get("formatter1").Type);
            Assert.IsNotNull(roLoggingSettings.Formatters.Get("formatter2"));
            Assert.AreSame(typeof(BinaryLogFormatterData), roLoggingSettings.Formatters.Get("formatter2").GetType());
            Assert.AreSame(typeof(BinaryLogFormatter), roLoggingSettings.Formatters.Get("formatter2").Type);
        }
        [TestMethod]
        public void CanCreateFormatterFromFactoryFromGivenName()
        {
            FormatterData data = new BinaryLogFormatterData("ignore");
            LoggingSettings settings = new LoggingSettings();
            settings.Formatters.Add(data);
            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
            configurationSource.Add(LoggingSettings.SectionName, settings);
            ILogFormatter formatter = LogFormatterCustomFactory.Instance.Create(context, "ignore", configurationSource, reflectionCache);
            Assert.IsNotNull(formatter);
            Assert.AreEqual(formatter.GetType(), typeof(BinaryLogFormatter));
        }
        [TestMethod]
        public void CanCreateFormatterFromFactoryFromGivenConfiguration()
        {
            FormatterData data = new BinaryLogFormatterData("ignore");
            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
            ILogFormatter formatter = LogFormatterCustomFactory.Instance.Create(context, data, configurationSource, reflectionCache);
            Assert.IsNotNull(formatter);
            Assert.AreEqual(formatter.GetType(), typeof(BinaryLogFormatter));
        }
        [TestMethod]
        public void CanDeserializeFormattedEntry()
        {
            LogEntry entry = CommonUtil.GetDefaultLogEntry();
            entry.Message = "message";
            entry.Title = "title";
            entry.Categories = new List<string>(new string[] { "cat1", "cat2", "cat3" });
            string serializedLogEntryText = new BinaryLogFormatter().Format(entry);
            LogEntry deserializedEntry = BinaryLogFormatter.Deserialize(serializedLogEntryText);
            Assert.IsNotNull(deserializedEntry);
            Assert.IsFalse(ReferenceEquals(entry, deserializedEntry));
            Assert.AreEqual(entry.Categories.Count, deserializedEntry.Categories.Count);
            foreach (string category in entry.Categories)
            {
                Assert.IsTrue(deserializedEntry.Categories.Contains(category));
            }
            Assert.AreEqual(entry.Message, deserializedEntry.Message);
            Assert.AreEqual(entry.Title, deserializedEntry.Title);
        }
        [TestMethod]
        public void CanDeserializeFormattedCustomEntry()
        {
            CustomLogEntry entry = new CustomLogEntry();
            entry.TimeStamp = DateTime.MaxValue;
            entry.Title = "My custom message title";
            entry.Message = "My custom message body";
            entry.Categories = new List<string>(new string[] { "CustomFormattedCategory", "OtherCategory" });
            entry.AcmeCoField1 = "apple";
            entry.AcmeCoField2 = "orange";
            entry.AcmeCoField3 = "lemon";
            string serializedLogEntryText = new BinaryLogFormatter().Format(entry);
            CustomLogEntry deserializedEntry =
                (CustomLogEntry)BinaryLogFormatter.Deserialize(serializedLogEntryText);
            Assert.IsNotNull(deserializedEntry);
            Assert.IsFalse(ReferenceEquals(entry, deserializedEntry));
            Assert.AreEqual(entry.Categories.Count, deserializedEntry.Categories.Count);
            foreach (string category in entry.Categories)
            {
                Assert.IsTrue(deserializedEntry.Categories.Contains(category));
            }
            Assert.AreEqual(entry.Message, deserializedEntry.Message);
            Assert.AreEqual(entry.Title, deserializedEntry.Title);
            Assert.AreEqual(entry.AcmeCoField1, deserializedEntry.AcmeCoField1);
            Assert.AreEqual(entry.AcmeCoField2, deserializedEntry.AcmeCoField2);
            Assert.AreEqual(entry.AcmeCoField3, deserializedEntry.AcmeCoField3);
        }
    }
}
