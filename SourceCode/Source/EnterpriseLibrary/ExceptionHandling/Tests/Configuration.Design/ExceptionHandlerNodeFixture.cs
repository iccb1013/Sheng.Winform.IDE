/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design.Tests
{
    [TestClass]
    public class ExceptionHandlerNodeFixture
    {
        [TestMethod]
        public void ExceptionHandlerDataTest()
        {
            string name = "some name";
            ExceptionHandlerData data = new ExceptionHandlerData();
            data.Name = name;
            ExceptionHandlerNode node = new ExceptionHandlerNodeImpl(data);
            Assert.AreEqual(name, node.Name);
        }
        [TestMethod]
        public void ExceptionHandlerNodeDataTest()
        {
            string name = "some name";
            ExceptionHandlerData exceptionHandlerData = new ExceptionHandlerData();
            exceptionHandlerData.Name = name;
            ExceptionHandlerNode exceptionHandlerNode = new ExceptionHandlerNodeImpl(exceptionHandlerData);
            ExceptionHandlerData nodeData = exceptionHandlerNode.ExceptionHandlerData;
            Assert.AreEqual(name, nodeData.Name);
        }
        class ExceptionHandlerNodeImpl : ExceptionHandlerNode
        {
            ExceptionHandlerData data;
            public ExceptionHandlerNodeImpl(ExceptionHandlerData data)
            {
                this.data = data;
                Rename(data.Name);
            }
            public override ExceptionHandlerData ExceptionHandlerData
            {
                get { return data; }
            }
        }
    }
}
