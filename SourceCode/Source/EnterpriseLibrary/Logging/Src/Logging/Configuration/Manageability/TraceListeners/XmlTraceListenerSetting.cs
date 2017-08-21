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
    public class XmlTraceListenerSetting : TraceListenerSetting
    {
        string fileName;
		public XmlTraceListenerSetting(XmlTraceListenerData sourceElement,
                                       string name,
                                       string fileName,
									   string traceOutputOptions,
									   string filter)
            : base(sourceElement, name, traceOutputOptions, filter)
        {
            this.fileName = fileName;
        }
        [ManagementConfiguration]
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }
        [ManagementBind]
        public static XmlTraceListenerSetting BindInstance(string ApplicationName,
                                                           string SectionName,
                                                           string Name)
        {
            return BindInstance<XmlTraceListenerSetting>(ApplicationName, SectionName, Name);
        }
        [ManagementEnumerator]
        public static IEnumerable<XmlTraceListenerSetting> GetInstances()
        {
            return GetInstances<XmlTraceListenerSetting>();
        }
        protected override bool SaveChanges(ConfigurationElement sourceElement)
        {
            return XmlTraceListenerDataWmiMapper.SaveChanges(this, sourceElement);
        }
    }
}
