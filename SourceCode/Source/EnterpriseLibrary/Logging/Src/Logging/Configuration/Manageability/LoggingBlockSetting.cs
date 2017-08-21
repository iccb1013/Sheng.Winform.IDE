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
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability
{
    [ManagementEntity]
    public partial class LoggingBlockSetting : ConfigurationSectionSetting
    {
        string defaultCategory;
        bool logWarningWhenNoCategoriesMatch;
        bool tracingEnabled;
        bool revertImpersonation;
        public LoggingBlockSetting(
            LoggingSettings sourceElement,
            string defaultCategory,
            bool logWarningWhenNoCategoriesMatch,
            bool tracingEnabled,
            bool revertImpersonation)
            : base(sourceElement)
        {
            this.defaultCategory = defaultCategory;
            this.logWarningWhenNoCategoriesMatch = logWarningWhenNoCategoriesMatch;
            this.tracingEnabled = tracingEnabled;
            this.revertImpersonation = revertImpersonation;
        }
        [ManagementConfiguration]
        public string DefaultCategory
        {
            get { return defaultCategory; }
            set { defaultCategory = value; }
        }
        [ManagementConfiguration]
        public bool LogWarningWhenNoCategoriesMatch
        {
            get { return logWarningWhenNoCategoriesMatch; }
            set { logWarningWhenNoCategoriesMatch = value; }
        }
        [ManagementConfiguration]
        public bool TracingEnabled
        {
            get { return tracingEnabled; }
            set { tracingEnabled = value; }
        }
        [ManagementConfiguration]
        public bool RevertImpersonation
        {
            get { return revertImpersonation; }
            set { revertImpersonation = value; }
        }
        [ManagementBind]
        public static LoggingBlockSetting BindInstance(string ApplicationName,
                                                       string SectionName)
        {
            return BindInstance<LoggingBlockSetting>(ApplicationName, SectionName);
        }
        [ManagementEnumerator]
        public static IEnumerable<LoggingBlockSetting> GetInstances()
        {
            return GetInstances<LoggingBlockSetting>();
        }
        protected override bool SaveChanges(ConfigurationElement sourceElement)
        {
            return LoggingSettingsWmiMapper.SaveChanges(this, sourceElement);
        }
    }
}
