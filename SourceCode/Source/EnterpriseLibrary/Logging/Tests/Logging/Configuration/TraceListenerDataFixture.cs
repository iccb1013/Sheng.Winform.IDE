/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Tests
{
    [TestClass]
    public class TraceListenerDataFixture
    {
        [TestInitialize]
        public void TestInitialize()
        {
            AppDomain.CurrentDomain.SetData("APPBASE", Environment.CurrentDirectory);
        }
        [TestMethod]
        public void CanDeserializeSerializedDefaultConfiguration()
        {
            TraceListenerData data = new TraceListenerData();
            Assert.AreEqual(data.TraceOutputOptions, TraceOptions.None);
            Assert.AreEqual(data.Filter, SourceLevels.All);
        }
    }
}
