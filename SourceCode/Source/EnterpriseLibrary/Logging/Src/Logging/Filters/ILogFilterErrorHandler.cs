/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Filters
{
	public interface ILogFilterErrorHandler
	{
		bool FilterCheckingFailed(Exception ex, LogEntry logEntry, ILogFilter filter);
	}
}
