/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Formatters
{
    [TestClass]
    public class ExceptionFormatterFixture
    {
        [TestMethod]
        public void SkippedNonReadableProperty()
        {
            ExceptionFormatter formatter = new ExceptionFormatter();
            Exception nonReadablePropertyException = new ExceptionWithNonReadableProperty("MyException");
            string message = formatter.GetMessage(nonReadablePropertyException);
            Assert.IsTrue(message.Length > 0);
        }
    }
    internal class ExceptionWithNonReadableProperty : Exception
    {
        public ExceptionWithNonReadableProperty(string message)
            : base(message)
        { }
        public string NonReadableProperty
        {
            set { ; }
        }
    }
}
