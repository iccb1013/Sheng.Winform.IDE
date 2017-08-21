/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests
{
    public class MockException : ArgumentNullException
    {
        public readonly string FieldString = "MockFieldString";
		private string setOnlyString;
		private const string setOnlyStringValue = "SetOnlyString";
		private const string mockException = "MOCK EXCEPTION";
		private const string mockPropertyString = "MockPropertyString";
        public MockException() : base(mockException)
        {
			setOnlyString = string.Empty;
        }
		public MockException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
        public string PropertyString
        {
            get { return mockPropertyString; }
        }
		public string SetOnlyString
		{
			set { setOnlyString = setOnlyStringValue; }
		}
    }
}
