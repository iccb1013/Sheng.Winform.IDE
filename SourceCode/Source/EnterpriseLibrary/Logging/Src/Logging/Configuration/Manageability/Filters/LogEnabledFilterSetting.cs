/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Configuration;
using System.Management.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Filters
{
    [ManagementEntity]
    public class LogEnabledFilterSetting : LogFilterSetting
    {
        bool enabled;
        public LogEnabledFilterSetting(LogEnabledFilterData sourceElement,
                                       string name,
                                       bool enabled)
            : base(sourceElement, name)
        {
            this.enabled = enabled;
        }
        [ManagementConfiguration]
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }
        [ManagementBind]
        public static LogEnabledFilterSetting BindInstance(string ApplicationName,
                                                           string SectionName,
                                                           string Name)
        {
            return BindInstance<LogEnabledFilterSetting>(ApplicationName, SectionName, Name);
        }
        [ManagementEnumerator]
        public static IEnumerable<LogEnabledFilterSetting> GetInstances()
        {
            return GetInstances<LogEnabledFilterSetting>();
        }
        protected override bool SaveChanges(ConfigurationElement sourceElement)
        {
            return LogEnabledFilterDataWmiMapper.SaveChanges(this, sourceElement);
        }
    }
}
