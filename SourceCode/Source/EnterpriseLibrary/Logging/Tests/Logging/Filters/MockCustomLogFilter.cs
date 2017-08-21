/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Filters.Tests
{
	[ConfigurationElementType(typeof(CustomLogFilterData))]
	public class MockCustomLogFilter : MockCustomProviderBase, ILogFilter
	{
		public MockCustomLogFilter(NameValueCollection attributes)
			: base(attributes)
		{
		}
		public bool Filter(LogEntry log)
		{
			return true;
		}
		public string Name
		{
			get { return string.Empty; }
		}
	}
}
