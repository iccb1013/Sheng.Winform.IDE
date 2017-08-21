/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Logging.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Filters
{
	public class LogFilterHelper
	{
		private ICollection<ILogFilter> filters;
		private ILogFilterErrorHandler handler;
		public LogFilterHelper(ICollection<ILogFilter> filters, ILogFilterErrorHandler handler)
		{
			this.filters = filters;
			this.handler = handler;
		}
		public bool CheckFilters(LogEntry log)
		{
			bool passFilters = true;
			foreach (ILogFilter filter in this.filters)
			{
				try
				{
					bool passed = filter.Filter(log);
					passFilters &= passed;
					if (!passFilters)
					{
						break;
					}
				}
				catch (Exception ex)
				{
					if (!this.handler.FilterCheckingFailed(ex, log, filter))
					{
						return false;
					}
				}
			}
			return passFilters;
		}
		public T GetFilter<T>()
			where T : class, ILogFilter
		{
			foreach (ILogFilter filter in this.filters)
			{
				if (filter is T)
				{
					return filter as T;
				}
			}
			return null;
		}
		public T GetFilter<T>(string name)
			where T : class, ILogFilter
		{
			if (string.IsNullOrEmpty(name)) throw new ArgumentException(Resources.ExceptionNullOrEmptyString, "name");
			foreach (ILogFilter filter in this.filters)
			{
				if (filter is T && name.Equals(filter.Name))
				{
					return filter as T;
				}
			}
			return null;
		}
		public ILogFilter GetFilter(string name)
		{
			if (string.IsNullOrEmpty(name)) throw new ArgumentException(Resources.ExceptionNullOrEmptyString, "name");
			foreach (ILogFilter filter in this.filters)
			{
				if (name.Equals(filter.Name))
				{
					return filter;
				}
			}
			return null;
		}
	}
}
