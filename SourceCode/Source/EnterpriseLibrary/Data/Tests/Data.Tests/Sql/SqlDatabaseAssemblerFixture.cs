/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests.Sql
{
    [TestClass]
    public class SqlDatabaseAssemblerFixture
    {
        ConfigurationReflectionCache reflectionCache;
        IConfigurationSource configurationSource;
        [TestInitialize]
        public void SetUp()
        {
            reflectionCache = new ConfigurationReflectionCache();
            configurationSource = new SystemConfigurationSource();
        }
        [TestMethod]
        public void CanGetAssemblerForSqlDatabase()
        {
            IDatabaseAssembler assembler
                = new DatabaseCustomFactory().GetAssembler(typeof(SqlDatabase), "ignore", reflectionCache);
            Assert.IsNotNull(assembler);
        }
        [TestMethod]
        public void AssemblerCanAssembleSqlDatabase()
        {
            ConnectionStringSettings settings
                = new ConnectionStringSettings("name", "test;", DbProviderMapping.DefaultSqlProviderName);
            IDatabaseAssembler assembler
                = new DatabaseCustomFactory().GetAssembler(typeof(SqlDatabase), settings.Name, reflectionCache);
            Database database = assembler.Assemble(settings.Name, settings, configurationSource);
            Assert.IsNotNull(database);
            Assert.AreSame(typeof(SqlDatabase), database.GetType());
            Assert.AreEqual(settings.ConnectionString, database.ConnectionStringWithoutCredentials);
        }
    }
}
