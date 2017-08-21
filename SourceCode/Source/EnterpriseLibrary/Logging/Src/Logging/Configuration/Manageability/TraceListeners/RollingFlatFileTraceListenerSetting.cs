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
    public class RollingFlatFileTraceListenerSetting : TraceListenerSetting
    {
        string fileName;
        string footer;
        string formatter;
        string header;
        string rollFileExistsBehavior;
        string rollInterval;
        int rollSizeKB;
        string timeStampPattern;
        public RollingFlatFileTraceListenerSetting(RollingFlatFileTraceListenerData sourceElement,
                                                   string name,
                                                   string fileName,
                                                   string header,
                                                   string footer,
                                                   string formatter,
                                                   string rollFileExistsBehavior,
                                                   string rollInterval,
                                                   int rollSizeKB,
                                                   string timeStampPattern,
												   string traceOutputOptions,
												   string filter)
            : base(sourceElement, name, traceOutputOptions, filter)
        {
            this.fileName = fileName;
            this.header = header;
            this.footer = footer;
            this.formatter = formatter;
            this.rollFileExistsBehavior = rollFileExistsBehavior;
            this.rollInterval = rollInterval;
            this.rollSizeKB = rollSizeKB;
            this.timeStampPattern = timeStampPattern;
        }
        [ManagementConfiguration]
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }
        [ManagementConfiguration]
        public string Footer
        {
            get { return footer; }
            set { footer = value; }
        }
        [ManagementConfiguration]
        public string Formatter
        {
            get { return formatter; }
            set { formatter = value; }
        }
        [ManagementConfiguration]
        public string Header
        {
            get { return header; }
            set { header = value; }
        }
        [ManagementConfiguration]
        public string RollFileExistsBehavior
        {
            get { return rollFileExistsBehavior; }
            set { rollFileExistsBehavior = value; }
        }
        [ManagementConfiguration]
        public string RollInterval
        {
            get { return rollInterval; }
            set { rollInterval = value; }
        }
        [ManagementConfiguration]
        public int RollSizeKB
        {
            get { return rollSizeKB; }
            set { rollSizeKB = value; }
        }
        [ManagementConfiguration]
        public string TimeStampPattern
        {
            get { return timeStampPattern; }
            set { timeStampPattern = value; }
        }
        [ManagementBind]
        public static RollingFlatFileTraceListenerSetting BindInstance(string ApplicationName,
                                                                       string SectionName,
                                                                       string Name)
        {
            return BindInstance<RollingFlatFileTraceListenerSetting>(ApplicationName, SectionName, Name);
        }
        [ManagementEnumerator]
        public static IEnumerable<RollingFlatFileTraceListenerSetting> GetInstances()
        {
            return GetInstances<RollingFlatFileTraceListenerSetting>();
        }
        protected override bool SaveChanges(ConfigurationElement sourceElement)
        {
            return RollingFlatFileTraceListenerDataWmiMapper.SaveChanges(this, sourceElement);
        }
    }
}
