/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Management.Instrumentation;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests
{
	[Serializable]
	public class CustomLogEntry : LogEntry
	{
		public CustomLogEntry()
			: base()
		{
		}
		public string AcmeCoField1 = string.Empty;
		public string AcmeCoField2 = string.Empty;
		public string AcmeCoField3 = string.Empty;
		private string propertyValue = "myPropertyValue";
		public string MyProperty
		{
			get { return propertyValue; }
			set { propertyValue = value; }
		}
		public string MyPropertyThatReturnsNull
		{
			get { return null; }
			set { }
		}
		[IgnoreMember]
		public string PropertyNotReadable
		{
			set { }
		}
		[IgnoreMember]
		public string this[int index]
		{
			get { return null; }
			set { }
		}
		public string MyPropertyThatThrowsException
		{
			get { throw new Exception(); }
			set { }
		}
	}
}
