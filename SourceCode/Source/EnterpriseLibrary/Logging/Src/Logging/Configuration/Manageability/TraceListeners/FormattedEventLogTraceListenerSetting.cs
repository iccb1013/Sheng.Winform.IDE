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
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.TraceListeners
{
    [ManagementEntity]
    public class FormattedEventLogTraceListenerSetting : TraceListenerSetting
    {
        string formatter;
        string log;
        string machineName;
        string source;
		public FormattedEventLogTraceListenerSetting(FormattedEventLogTraceListenerData sourceElement,
                                                     string name,
                                                     string source,
                                                     string log,
                                                     string machineName,
                                                     string formatter,
													 string traceOutputOptions,
													 string filter)
            : base(sourceElement, name, traceOutputOptions, filter)
        {
            this.source = source;
            this.log = log;
            this.machineName = machineName;
            this.formatter = formatter;
        }
        [ManagementConfiguration]
        public string Formatter
        {
            get { return formatter; }
            set { formatter = value; }
        }
        [ManagementConfiguration]
        public string Log
        {
            get { return log; }
            set { log = value; }
        }
        [ManagementConfiguration]
        public string MachineName
        {
            get { return machineName; }
            set { machineName = value; }
        }
        [ManagementConfiguration]
        public string Source
        {
            get { return source; }
            set { source = value; }
        }
        [ManagementBind]
        public static FormattedEventLogTraceListenerSetting BindInstance(string ApplicationName,
                                                                         string SectionName,
                                                                         string Name)
        {
            return BindInstance<FormattedEventLogTraceListenerSetting>(ApplicationName, SectionName, Name);
        }
        [ManagementEnumerator]
        public static IEnumerable<FormattedEventLogTraceListenerSetting> GetInstances()
        {
            return GetInstances<FormattedEventLogTraceListenerSetting>();
        }
        protected override bool SaveChanges(ConfigurationElement sourceElement)
        {
            return FormattedEventLogTraceListenerDataWmiMapper.SaveChanges(this, sourceElement);
        }
    }
}
