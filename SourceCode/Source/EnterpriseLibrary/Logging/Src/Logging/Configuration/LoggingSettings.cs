/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.ComponentModel;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    public class LoggingSettings : SerializableConfigurationSection
    {
        private const string tracingEnabledProperty = "tracingEnabled";
        private const string nameProperty = "name";
        private const string traceListenerDataCollectionProperty = "listeners";
        private const string formatterDataCollectionProperty = "formatters";
        private const string logFiltersProperty = "logFilters";
        private const string traceSourcesProrperty = "categorySources";
        private const string defaultCategoryProperty = "defaultCategory";
        private const string logWarningsWhenNoCategoriesMatchProperty = "logWarningsWhenNoCategoriesMatch";
        private const string specialTraceSourcesProperty = "specialSources";
        private const string revertImpersonationProperty = "revertImpersonation";
        public const string SectionName = "loggingConfiguration";
        public LoggingSettings()
            : this(string.Empty)
        {
        }
        public LoggingSettings(string name)
            : this(name, false, string.Empty)
        {
        }
        public LoggingSettings(string name, bool tracingEnabled, string defaultCategory)
        {
            this.Name = name;
            this.TracingEnabled = tracingEnabled;
            this.DefaultCategory = defaultCategory;
        }
        public static LoggingSettings GetLoggingSettings(IConfigurationSource configurationSource)
        {
            return (LoggingSettings)configurationSource.GetSection(LoggingSettings.SectionName);
        }
        [ConfigurationProperty(tracingEnabledProperty)]
        public bool TracingEnabled
        {
            get
            {
                return (bool)this[tracingEnabledProperty];
            }
            set
            {
                this[tracingEnabledProperty] = value;
            }
        }
        [ConfigurationProperty(nameProperty)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string Name
        {
            get
            {
                return (string)this[nameProperty];
            }
            set
            {
                this[nameProperty] = value;
            }
        }
        [ConfigurationProperty(defaultCategoryProperty, IsRequired = true)]
        public string DefaultCategory
        {
            get
            {
                return (string)this[defaultCategoryProperty];
            }
            set
            {
                this[defaultCategoryProperty] = value;
            }
        }
        [ConfigurationProperty(traceListenerDataCollectionProperty)]
        public TraceListenerDataCollection TraceListeners
        {
            get
            {
                return (TraceListenerDataCollection)base[traceListenerDataCollectionProperty];
            }
        }
        [ConfigurationProperty(formatterDataCollectionProperty)]
        public NameTypeConfigurationElementCollection<FormatterData, CustomFormatterData> Formatters
        {
            get
            {
                return (NameTypeConfigurationElementCollection<FormatterData, CustomFormatterData>)base[formatterDataCollectionProperty];
            }
        }
        [ConfigurationProperty(logFiltersProperty)]
        public NameTypeConfigurationElementCollection<LogFilterData, CustomLogFilterData> LogFilters
        {
            get
            {
                return (NameTypeConfigurationElementCollection<LogFilterData, CustomLogFilterData>)base[logFiltersProperty];
            }
        }
        [ConfigurationProperty(traceSourcesProrperty)]
        public NamedElementCollection<TraceSourceData> TraceSources
        {
            get
            {
                return (NamedElementCollection<TraceSourceData>)base[traceSourcesProrperty];
            }
        }
        [ConfigurationProperty(specialTraceSourcesProperty, IsRequired = true)]
        public SpecialTraceSourcesData SpecialTraceSources
        {
            get
            {
                return (SpecialTraceSourcesData)base[specialTraceSourcesProperty];
            }
            set
            {
                base[specialTraceSourcesProperty] = value;
            }
        }
        [ConfigurationProperty(logWarningsWhenNoCategoriesMatchProperty)]
        public bool LogWarningWhenNoCategoriesMatch
        {
            get
            {
                return (bool)this[logWarningsWhenNoCategoriesMatchProperty];
            }
            set
            {
                this[logWarningsWhenNoCategoriesMatchProperty] = value;
            }
        }
        [ConfigurationProperty(revertImpersonationProperty, DefaultValue = true, IsRequired = false)]
        public bool RevertImpersonation
        {
            get
            {
                return (bool)this[revertImpersonationProperty];
            }
            set
            {
                this[revertImpersonationProperty] = value;
            }
        }
    }
}
