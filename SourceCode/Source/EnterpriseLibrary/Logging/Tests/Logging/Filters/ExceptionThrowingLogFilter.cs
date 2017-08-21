/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Filters.Tests
{
	class ExceptionThrowingLogFilter : ILogFilter
	{
		private string name;
		internal ExceptionThrowingLogFilter(string name)
		{
			this.name = name;
		}
		public bool Filter(LogEntry log)
		{
			throw new Exception("exception during evaluation.");
		}
		public string Name
		{
			get { return name; }
		}
	}
}
