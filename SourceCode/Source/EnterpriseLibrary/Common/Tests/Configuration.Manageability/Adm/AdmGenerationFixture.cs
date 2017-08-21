/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests.Adm
{
    [TestClass]
    public class AdmGenerationFixture
    {
        AdmContent content;
        StringWriter writer;
        [TestInitialize]
        public void SetUp()
        {
            writer = new StringWriter();
        }
        [TestMethod]
        public void CanWriteEmptyContent()
        {
            content = new AdmContent();
            content.Write(writer);
        }
        [TestMethod]
        public void CanWriteContentWithCategory()
        {
            content = new AdmContent();
            content.AddCategory(new AdmCategory("category"));
            content.Write(writer);
        }
    }
}
