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
	public abstract class LogFilter : ILogFilter
	{
		private string name;
		public LogFilter(string name)
		{
			this.name = name;
		}
		public abstract bool Filter(LogEntry log);
		public string Name
		{
			get { return name; }
		}
	}
}
