/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Filters
{
	[ConfigurationElementType(typeof(PriorityFilterData))]    
	public class PriorityFilter : LogFilter
    {
        private int minimumPriority;
        private int maximumPriority;
        public PriorityFilter(string name, int minimumPriority)
            : this(name, minimumPriority, int.MaxValue)
        {
        }
		public PriorityFilter(string name, int minimumPriority, int maximumPriority)
			: base(name)
        {
            this.minimumPriority = minimumPriority;
            this.maximumPriority = maximumPriority;
		}
        public override bool Filter(LogEntry log)
        {
			return ShouldLog(log.Priority);
        }
		public bool ShouldLog(int priority)
		{
			if (priority < 0)
			{
				priority = this.minimumPriority;
			}
			return (this.maximumPriority >= priority && priority >= this.minimumPriority);
		}
		public int MinimumPriority
		{
            get { return minimumPriority; }
		}
		public int MaximumPriority
		{
			get { return maximumPriority; }
		}
    }
}
