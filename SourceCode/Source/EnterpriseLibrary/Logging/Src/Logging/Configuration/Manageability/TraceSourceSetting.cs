/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Management.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using System.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability
{
	[ManagementEntity]
	public partial class TraceSourceSetting : NamedConfigurationSetting
	{
		private string defaultLevel;
		private string[] traceListeners;
		private string kind;
		public TraceSourceSetting(TraceSourceData sourceElement,
			string name, 
			string defaultLevel, 
			string[] traceListeners, 
			string kind)
			: base(sourceElement, name)
		{
			this.defaultLevel = defaultLevel;
			this.traceListeners = traceListeners;
			this.kind = kind;
		}
		[ManagementConfiguration]
		public string DefaultLevel
		{
			get { return defaultLevel; }
			set { defaultLevel = value; }
		}
		[ManagementConfiguration]
		public string[] TraceListeners
		{
			get { return traceListeners; }
			set { traceListeners = value; }
		}
		[ManagementProbe]
		public string Kind
		{
			get { return kind; }
		}
        [ManagementEnumerator]
        public static IEnumerable<TraceSourceSetting> GetInstances()
        {
            return NamedConfigurationSetting.GetInstances<TraceSourceSetting>();
        }
        [ManagementBind]
        public static TraceSourceSetting BindInstance(string ApplicationName, string SectionName, string Name)
        {
            return NamedConfigurationSetting.BindInstance<TraceSourceSetting>(ApplicationName, SectionName, Name);
        }
		protected override bool SaveChanges(ConfigurationElement sourceElement)
		{
			return LoggingSettingsWmiMapper.SaveChanges(this, sourceElement);
		}
	}
}
