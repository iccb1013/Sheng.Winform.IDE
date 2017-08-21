/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Specialized;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners
{
	public class MockCustomTraceListenerWithInvalidConstructor
		: CustomTraceListener
	{
		internal const string AttributeKey = "attribute";
		internal readonly static string[] SupportedAttributes = new string[] { AttributeKey };
		internal string initData;
		public MockCustomTraceListenerWithInvalidConstructor(string initData, string ignored)
		{
			this.initData = initData;
		}
		internal string Attribute
		{
			get { return Attributes[AttributeKey]; }
		}
		public override void Write(string message)
		{
			throw new Exception("The method or operation is not implemented.");
		}
		public override void WriteLine(string message)
		{
			throw new Exception("The method or operation is not implemented.");
		}
		protected override string[] GetSupportedAttributes()
		{
			return SupportedAttributes;
		}
	}
}
