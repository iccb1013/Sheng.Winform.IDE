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
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Formatters.Tests
{
	[ConfigurationElementType(typeof(CustomFormatterData))]
	public class MockCustomLogFormatter
		: MockCustomProviderBase, ILogFormatter
	{
		public MockCustomLogFormatter(NameValueCollection attributes)
			: base(attributes)
		{
		}
		public string Format(LogEntry log)
		{
			throw new Exception("The method or operation is not implemented.");
		}
	}
}
