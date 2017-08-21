/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Tests
{
	public class TestEvent : BaseWmiEvent
	{
		private string text;
		public TestEvent(string text)
		{
			this.text = text;
		}
		public string Text
		{
			get { return text; }
		}
	}
}
