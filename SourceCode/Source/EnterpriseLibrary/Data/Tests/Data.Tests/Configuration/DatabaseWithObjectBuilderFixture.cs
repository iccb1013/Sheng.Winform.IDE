/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Data.Common;
using System.Data.Odbc;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests.Configuration
{
    [TestClass]
    public class DatabaseWithObjectBuildUperFixture
    {
        public const string OracleTestStoredProcedureInPackageWithTranslation = "TESTPACKAGETOTRANSLATEGETCUSTOMERDETAILS";
		public const string OracleTestTranslatedStoredProcedureInPackageWithTranslation = "TESTPACKAGE.TESTPACKAGETOTRANSLATEGETCUSTOMERDETAILS";
		public const string OracleTestStoredProcedureInPackageWithoutTranslation = "TESTPACKAGETOKEEPGETCUSTOMERDETAILS";
        [TestMethod]
        public void CanCreateSqlDatabase()
        {
            Database database = EnterpriseLibraryFactory.BuildUp<Database>("Service_Dflt");
            Assert.IsNotNull(database);
            Assert.AreSame(typeof(SqlDatabase), database.GetType());
        }
        [TestMethod]
        public void CanCreateOracleDatabase()
        {
            Database database = EnterpriseLibraryFactory.BuildUp<Database>("OracleTest");
            Assert.IsNotNull(database);
            Assert.AreSame(typeof(OracleDatabase), database.GetType());
            DbCommand dbCommand1 = database.GetStoredProcCommand(OracleTestStoredProcedureInPackageWithTranslation);
            Assert.AreEqual((object)OracleTestTranslatedStoredProcedureInPackageWithTranslation, dbCommand1.CommandText);
            DbCommand dbCommand2 = database.GetStoredProcCommand(OracleTestStoredProcedureInPackageWithoutTranslation);
            Assert.AreEqual((object)OracleTestStoredProcedureInPackageWithoutTranslation, dbCommand2.CommandText);
        }
        [TestMethod]
        public void CanCreateGenericDatabase()
        {
            Database database = EnterpriseLibraryFactory.BuildUp<Database>("OdbcDatabase");
            Assert.IsNotNull(database);
            Assert.AreSame(typeof(GenericDatabase), database.GetType());
            DbCommand command = database.GetStoredProcCommand("ignore");
            Assert.AreSame(typeof(OdbcCommand), command.GetType());
        }
        [TestMethod]
        public void CreatedDatabaseIsInstrumented()
        {
            Database database = EnterpriseLibraryFactory.BuildUp<Database>("Service_Dflt");
        }
    }
}
