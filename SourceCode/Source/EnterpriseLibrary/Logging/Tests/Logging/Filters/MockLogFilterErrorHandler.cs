/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Filters.Tests
{
	internal class MockLogFilterErrorHandler : ILogFilterErrorHandler
	{
		internal ICollection<ILogFilter> failingFilters = new List<ILogFilter>();
		private bool returnValue = false;
		internal MockLogFilterErrorHandler(bool returnValue)
		{
			this.returnValue = returnValue;
		}
		public bool FilterCheckingFailed(System.Exception ex, LogEntry logEntry, ILogFilter filter)
		{
			failingFilters.Add(filter);
			return returnValue;
		}
	}
}
