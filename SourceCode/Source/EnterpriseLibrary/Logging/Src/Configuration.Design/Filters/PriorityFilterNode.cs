/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Filters
{
    public sealed class PriorityFilterNode : LogFilterNode
    {
		private int maximumPriority;
		private int minimumPriority;
        public PriorityFilterNode()
            : this(new PriorityFilterData(Resources.PriorityFilterNode, -1))
        {
        }
        public PriorityFilterNode(PriorityFilterData priorityFilterData)
            : base(null == priorityFilterData ? string.Empty : priorityFilterData.Name)
        {
			if (null == priorityFilterData) throw new ArgumentNullException("priorityFilterData");
			this.minimumPriority = priorityFilterData.MinimumPriority;
			this.maximumPriority = priorityFilterData.MaximumPriority;			
        }
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("PriorityFilterNodeMaximumPriorityDescription", typeof(Resources))]
        [AssertRange(0, RangeBoundaryType.Inclusive, Int32.MaxValue, RangeBoundaryType.Inclusive)]
        [PriorityFilterMaximumPriorityValidationAttribute]
		public int? MaximumPriority
		{
			get { return (maximumPriority == Int32.MaxValue) ? (int?)null : maximumPriority; }
			set { maximumPriority = value ?? Int32.MaxValue; }
		}
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("PriorityFilterNodeMinimumPriorityDescription", typeof(Resources))]
        [AssertRange(0, RangeBoundaryType.Inclusive, Int32.MaxValue, RangeBoundaryType.Inclusive)]
		public int? MinimumPriority
		{
			get { return (minimumPriority == -1) ? (int?)null : minimumPriority; }
			set { minimumPriority = value ?? -1; }
		}
		public override LogFilterData LogFilterData
		{
			get 
			{
				PriorityFilterData data = new PriorityFilterData(Name, minimumPriority);
				data.MaximumPriority = maximumPriority;
				return data;
			}
		}
    }
}
