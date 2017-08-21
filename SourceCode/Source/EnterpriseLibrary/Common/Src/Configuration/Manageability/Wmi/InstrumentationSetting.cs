/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Configuration;
using System.Management.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
    [ManagementEntity]
    public partial class InstrumentationSetting : ConfigurationSectionSetting
    {
        bool eventLoggingEnabled;
        bool performanceCountersEnabled;
        bool wmiEnabled;
        public InstrumentationSetting(ConfigurationElement sourceElement,
                                      bool eventLoggingEnabled,
                                      bool performanceCountersEnabled,
                                      bool wmiEnabled)
            : base(sourceElement)
        {
            this.eventLoggingEnabled = eventLoggingEnabled;
            this.performanceCountersEnabled = performanceCountersEnabled;
            this.wmiEnabled = wmiEnabled;
        }
        [ManagementConfiguration]
        public bool EventLoggingEnabled
        {
            get { return eventLoggingEnabled; }
            set { eventLoggingEnabled = value; }
        }
        [ManagementConfiguration]
        public bool PerformanceCountersEnabled
        {
            get { return performanceCountersEnabled; }
            set { performanceCountersEnabled = value; }
        }
        [ManagementConfiguration]
        public bool WmiEnabled
        {
            get { return wmiEnabled; }
            set { wmiEnabled = value; }
        }
        [ManagementBind]
        public static InstrumentationSetting BindInstance(string ApplicationName,
                                                          string SectionName)
        {
            return BindInstance<InstrumentationSetting>(ApplicationName, SectionName);
        }
        [ManagementEnumerator]
        public static IEnumerable<InstrumentationSetting> GetInstances()
        {
            return GetInstances<InstrumentationSetting>();
        }
        protected override bool SaveChanges(ConfigurationElement sourceElement)
        {
            return InstrumentationConfigurationSectionWmiMapper.SaveChanges(this, sourceElement);
        }
    }
}
