/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration
{
	public class InstrumentationConfigurationSection : SerializableConfigurationSection
    {
		private const string performanceCountersEnabled = "performanceCountersEnabled";
        private const string eventLoggingEnabled = "eventLoggingEnabled";
        private const string wmiEnabled = "wmiEnabled";
        private const string applicationInstanceName = "applicationInstanceName";
        public const string SectionName = "instrumentationConfiguration";
		internal bool InstrumentationIsEntirelyDisabled
		{
			get { return (PerformanceCountersEnabled || EventLoggingEnabled || WmiEnabled) == false; }
		}
        public InstrumentationConfigurationSection(bool performanceCountersEnabled, bool eventLoggingEnabled, bool wmiEnabled):
            this(performanceCountersEnabled, eventLoggingEnabled, wmiEnabled, "")
        {
            this.PerformanceCountersEnabled = performanceCountersEnabled;
            this.EventLoggingEnabled = eventLoggingEnabled;
            this.WmiEnabled = wmiEnabled;
        }
        public InstrumentationConfigurationSection(bool performanceCountersEnabled, bool eventLoggingEnabled, bool wmiEnabled, string applicationInstanceName)
        {
            this.PerformanceCountersEnabled = performanceCountersEnabled;
            this.EventLoggingEnabled = eventLoggingEnabled;
            this.WmiEnabled = wmiEnabled;
            this.ApplicationInstanceName = applicationInstanceName;
        }
        public InstrumentationConfigurationSection()
        {
        }
        [ConfigurationProperty(performanceCountersEnabled, IsRequired = false, DefaultValue = false)]
        public bool PerformanceCountersEnabled
        {
            get { return (bool)this[performanceCountersEnabled]; }
            set { this[performanceCountersEnabled] = value; }
        }
        [ConfigurationProperty(eventLoggingEnabled, IsRequired = false, DefaultValue = false)]
        public bool EventLoggingEnabled
        {
            get { return (bool)this[eventLoggingEnabled]; }
            set { this[eventLoggingEnabled] = value; }
        }
        [ConfigurationProperty(wmiEnabled, IsRequired = false, DefaultValue = false)]
        public bool WmiEnabled
        {
            get { return (bool)this[wmiEnabled]; }
            set { this[wmiEnabled] = value; }
        }
        [ConfigurationProperty(applicationInstanceName, IsRequired = false, DefaultValue="")]
        public string ApplicationInstanceName
        {
            get { return (string)this[applicationInstanceName]; }
            set { this[applicationInstanceName] = value; }
        }
    }
}
