/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Filters
{
	[ConfigurationElementType(typeof(LogEnabledFilterData))]
	public class LogEnabledFilter : LogFilter
	{
		private bool enabled = false;
		public LogEnabledFilter(string name, bool enabled)
			: base(name)
		{
			this.enabled = enabled;
		}
		public override bool Filter(LogEntry log)
		{
			return enabled;
		}
		public bool Enabled
		{
			get { return this.enabled; }
			set { this.enabled = value; }
		}
	}
}
