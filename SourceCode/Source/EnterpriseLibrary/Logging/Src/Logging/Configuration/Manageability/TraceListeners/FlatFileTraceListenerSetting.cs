/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Configuration;
using System.Management.Instrumentation;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.TraceListeners
{
    [ManagementEntity]
    public partial class FlatFileTraceListenerSetting : TraceListenerSetting
    {
        string fileName;
        string footer;
        string formatter;
        string header;
		public FlatFileTraceListenerSetting(FlatFileTraceListenerData sourceElement,
                                            string name,
                                            string fileName,
                                            string header,
                                            string footer,
                                            string formatter,
                                            string traceOutputOptions,
											string filter)
            : base(sourceElement, name, traceOutputOptions, filter)
        {
            this.fileName = fileName;
            this.header = header;
            this.footer = footer;
            this.formatter = formatter;
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
        [ManagementBind]
        public static FlatFileTraceListenerSetting BindInstance(string ApplicationName,
                                                                string SectionName,
                                                                string Name)
        {
            return BindInstance<FlatFileTraceListenerSetting>(ApplicationName, SectionName, Name);
        }
        [ManagementEnumerator]
        public static IEnumerable<FlatFileTraceListenerSetting> GetInstances()
        {
            return GetInstances<FlatFileTraceListenerSetting>();
        }
        protected override bool SaveChanges(ConfigurationElement sourceElement)
        {
            return FlatFileTraceListenerDataWmiMapper.SaveChanges(this, sourceElement);
        }
    }
}
