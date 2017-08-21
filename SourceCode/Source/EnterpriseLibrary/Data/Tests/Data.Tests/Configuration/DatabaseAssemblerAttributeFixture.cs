/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests.Configuration
{
    [TestClass]
    public class DatabaseAssemblerAttributeFixture
    {
        [TestMethod]
        public void CreationOfAttributeWithCorrectTypeSucceeds()
        {
            DatabaseAssemblerAttribute attribute = new DatabaseAssemblerAttribute(typeof(SqlDatabaseAssembler));
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreationOfAttributeWithNullTypeThrows()
        {
            new DatabaseAssemblerAttribute(null);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreationOfAttributeWithInvalidTypeThrows()
        {
            new DatabaseAssemblerAttribute(typeof(object));
        }
    }
}
