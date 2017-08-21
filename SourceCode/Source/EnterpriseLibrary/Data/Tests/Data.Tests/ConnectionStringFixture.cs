/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests
{
    [TestClass]
    public class ConnectionStringFixture
    {
        static readonly string userName = "User";
        static readonly string password = "Password";
        static readonly string userIdTokens = "user id=,uid=";
        static readonly string passwordTokens = "password=,pwd=";
        static ConnectionString connectionString;
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConnectionStringIsNullThrows()
        {
            connectionString = new ConnectionString(null, userIdTokens, passwordTokens);
            string password = connectionString.Password;
            Assert.IsTrue(password != null);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EmptyConnectionStringThrows()
        {
            connectionString = new ConnectionString(string.Empty, userIdTokens, passwordTokens);
            Assert.AreEqual(0, connectionString.UserName.Length);
            Assert.AreEqual(0, connectionString.Password.Length);
        }
        [TestMethod]
        public void CanGetCredentialsFromRealSqlDataClass()
        {
            string initialConnectionString = String.Format(@"server=(local)\SQLEXPRESS; database=JoeRandom; uid={0}; pwd={1}; ;", userName, password);
            connectionString = new ConnectionString(initialConnectionString, userIdTokens, passwordTokens);
            Assert.AreEqual(userName, connectionString.UserName);
            Assert.AreEqual(password, connectionString.Password);
        }
        [TestMethod]
        public void NoUserOrPasswordDefinedReturnsAnEmptyString()
        {
            string initialConnectionString = @"server=(local)\SQLEXPRESS; database=JoeRandom; Integrated Security=true";
            connectionString = new ConnectionString(initialConnectionString, userIdTokens, passwordTokens);
            Assert.AreEqual(string.Empty, connectionString.UserName);
            Assert.AreEqual(string.Empty, connectionString.Password);
        }
        [TestMethod]
        public void CreateNewConnectionStringTest()
        {
            string initialConnectionString = String.Format(@"server=(local)\SQLEXPRESS; database=JoeRandom; uid={0}; pwd={1}; ;", userName, password);
            connectionString = new ConnectionString(initialConnectionString, userIdTokens, passwordTokens).CreateNewConnectionString(initialConnectionString);
            Assert.AreEqual(userName, connectionString.UserName);
            Assert.AreEqual(password, connectionString.Password);
        }
        [TestMethod]
        public void CanGetCredentialsUsingAlternatePatternsForUidAndPwd()
        {
            string initialConnectionString = String.Format(@"server=(local)\SQLEXPRESS; database=JoeRandom; user id={0}; password={1}; ;", userName, password);
            connectionString = new ConnectionString(initialConnectionString, userIdTokens, passwordTokens);
            Assert.AreEqual(userName, connectionString.UserName);
            Assert.AreEqual(password, connectionString.Password);
        }
        [TestMethod]
        public void CanAddCredentialsToConnectionStringThatDoesNotHaveThem()
        {
            string initialConnectionString = @"server=(local)\SQLEXPRESS; database=RandomData; ; ;";
            connectionString = new ConnectionString(initialConnectionString, userIdTokens, passwordTokens);
            connectionString.UserName = userName;
            connectionString.Password = password;
            string actualConnectionString = String.Format(@"server=(local)\SQLEXPRESS; database=RandomData; ; ;user id={0};password={1};",
                                                          userName, password);
            Assert.AreEqual(actualConnectionString, connectionString.ToString());
        }
        [TestMethod]
        public void CanSetUserIdAndPasswordInConnectionStringThatAlreadyHasOne()
        {
            string initialConnectionString = String.Format(@"server=(local)\sqlexpress; database=JoeRandom; user id={0}; password={1}; ;", "Kill", "Bill");
            ConnectionString newConnectionString = new ConnectionString(initialConnectionString, userIdTokens, passwordTokens);
            newConnectionString.UserName = userName;
            newConnectionString.Password = password;
            string actualConnectionString = String.Format(@"server=(local)\sqlexpress; database=JoeRandom; user id={0}; password={1}; ;", userName, password);
            Assert.AreEqual(actualConnectionString, newConnectionString.ToString());
        }
        [TestMethod]
        public void RemovingCredentialsFromConnectionStringWithoutThemIsOk()
        {
            string initialConnectionString = @"server=(local)\sqlexpress;database=RandomData;";
            ConnectionString newConnectionString = new ConnectionString(initialConnectionString, userIdTokens, passwordTokens);
            string expectedConnectionString = @"server=(local)\sqlexpress;database=randomdata;";
            string strippedConnectionString = newConnectionString.ToStringNoCredentials();
            Assert.AreEqual(expectedConnectionString, strippedConnectionString);
        }
        [TestMethod]
        public void WillRemoveCredentialsFromConnectionString()
        {
            string initialConnectionString = @"server=(local)\sqlexpress;database=RandomData;user id=Bill;pwd=goodPassword";
            ConnectionString newConnectionString = new ConnectionString(initialConnectionString,
                                                                        userIdTokens, passwordTokens);
            string expectedConnectionString = @"server=(local)\sqlexpress;database=randomdata;";
            string strippedConnectionString = newConnectionString.ToStringNoCredentials();
            Assert.AreEqual(expectedConnectionString, strippedConnectionString);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructConnectionStrigWithNullUserIdTokensThrows()
        {
            string initialConnectionString = @"server=(local)\SQLEXPRESS;database=RandomData;user id=Bill;pwd=goodPassword";
            ConnectionString newConnectionString = new ConnectionString(initialConnectionString, null, passwordTokens);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructConnectionStrigNullPasswordTokensThrows()
        {
            string initialConnectionString = @"server=(local)\SQLEXPRESS;database=RandomData;user id=Bill;pwd=goodPassword";
            ConnectionString newConnectionString = new ConnectionString(initialConnectionString, userIdTokens, null);
        }
    }
}
