/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design.Tests
{
    [TestClass]
    public class ExceptionTypeNodeNameFormatterFixture
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullConfigurationDataInCreateNameMethodTrows()
        {
            ExceptionTypeNodeNameFormatter nameFormatter = new ExceptionTypeNodeNameFormatter();
            nameFormatter.CreateName((ExceptionTypeData)null);
        }
        [TestMethod]
        public void PassingConfigurationWithNullTypeReturnsEmptyString()
        {
            ExceptionTypeNodeNameFormatter nameFormatter = new ExceptionTypeNodeNameFormatter();
            ExceptionTypeData exceptionTypeData = new ExceptionTypeData("someName", (Type)null, PostHandlingAction.NotifyRethrow);
            string name = nameFormatter.CreateName(exceptionTypeData);
            Assert.IsNotNull(name);
            Assert.AreEqual(0, name.Length);
        }
        [TestMethod]
        public void PassingTypeStringReturnsFirstSegment()
        {
            ExceptionTypeNodeNameFormatter nameFormatter = new ExceptionTypeNodeNameFormatter();
            ExceptionTypeData exceptionTypeData = new ExceptionTypeData("someName", "a, b, c, d", PostHandlingAction.NotifyRethrow);
            string name = nameFormatter.CreateName(exceptionTypeData);
            Assert.IsNotNull(name);
            Assert.AreEqual("a", name);
        }
        [TestMethod]
        public void PassingTypeReturnsTypeName()
        {
            ExceptionTypeNodeNameFormatter nameFormatter = new ExceptionTypeNodeNameFormatter();
            ExceptionTypeData exceptionTypeData = new ExceptionTypeData("someName", typeof(Exception), PostHandlingAction.NotifyRethrow);
            string name = nameFormatter.CreateName(exceptionTypeData);
            Assert.IsNotNull(name);
            Assert.AreEqual("Exception", name);
        }
        [TestMethod]
        public void PassingTypeStringReturnsFirstSegmentAndTrimsSpaces()
        {
            ExceptionTypeNodeNameFormatter nameFormatter = new ExceptionTypeNodeNameFormatter();
            ExceptionTypeData exceptionTypeData = new ExceptionTypeData("someName", "  a, b, c, d", PostHandlingAction.NotifyRethrow);
            string name = nameFormatter.CreateName(exceptionTypeData);
            Assert.IsNotNull(name);
            Assert.AreEqual("a", name);
        }
    }
}
