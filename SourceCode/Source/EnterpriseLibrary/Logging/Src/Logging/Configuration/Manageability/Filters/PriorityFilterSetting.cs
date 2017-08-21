/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Configuration;
using System.Management.Instrumentation;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Filters
{
    [ManagementEntity]
    public class PriorityFilterSetting : LogFilterSetting
    {
        int maximumPriority;
        int minimumPriority;
        public PriorityFilterSetting(PriorityFilterData sourceElement,
                                     string name,
                                     int maximumPriority,
                                     int minimumPriority)
            : base(sourceElement, name)
        {
            this.maximumPriority = maximumPriority;
            this.minimumPriority = minimumPriority;
        }
        [ManagementConfiguration]
        public int MaximumPriority
        {
            get { return maximumPriority; }
            set { maximumPriority = value; }
        }
        [ManagementConfiguration]
        public int MinimumPriority
        {
            get { return minimumPriority; }
            set { minimumPriority = value; }
        }
        [ManagementBind]
        public static PriorityFilterSetting BindInstance(string ApplicationName,
                                                         string SectionName,
                                                         string Name)
        {
            return BindInstance<PriorityFilterSetting>(ApplicationName, SectionName, Name);
        }
        [ManagementEnumerator]
        public static IEnumerable<PriorityFilterSetting> GetInstances()
        {
            return GetInstances<PriorityFilterSetting>();
        }
        protected override bool SaveChanges(ConfigurationElement sourceElement)
        {
            return PriorityFilterDataWmiMapper.SaveChanges(this, sourceElement);
        }
    }
}
