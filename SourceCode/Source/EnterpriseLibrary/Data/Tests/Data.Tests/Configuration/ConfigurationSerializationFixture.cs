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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Tests
{
    [TestClass]
    public class ConfigurationSerializationFixture
    {
        const string providerName1 = "provider 1";
        const string providerName2 = "provider 2";
        const string databaseName1 = "database 1";
        [TestInitialize]
        public void TestInitialize()
        {
            AppDomain.CurrentDomain.SetData("APPBASE", Environment.CurrentDirectory);
        }
        [TestMethod]
        public void CanDeserializeSerializedConfiguration()
        {
            DatabaseSettings settings = new DatabaseSettings();
            DbProviderMapping mappingData1 = new DbProviderMapping(providerName1, typeof(OracleDatabase));
            DbProviderMapping mappingData2 = new DbProviderMapping(providerName2, typeof(SqlDatabase));
            settings.DefaultDatabase = databaseName1;
            settings.ProviderMappings.Add(mappingData1);
            settings.ProviderMappings.Add(mappingData2);
            IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
            sections[DatabaseSettings.SectionName] = settings;
            IConfigurationSource configurationSource
                = ConfigurationTestHelper.SaveSectionsInFileAndReturnConfigurationSource(sections);
            DatabaseSettings roSettigs = (DatabaseSettings)configurationSource.GetSection(DatabaseSettings.SectionName);
            Assert.IsNotNull(roSettigs);
            Assert.AreEqual(2, roSettigs.ProviderMappings.Count);
            Assert.AreEqual(databaseName1, roSettigs.DefaultDatabase);
            Assert.IsNotNull(roSettigs.ProviderMappings.Get(providerName1));
            Assert.AreSame(typeof(OracleDatabase), roSettigs.ProviderMappings.Get(providerName1).DatabaseType);
            Assert.AreEqual(providerName1, roSettigs.ProviderMappings.Get(providerName1).DbProviderName);
        }
    }
}
