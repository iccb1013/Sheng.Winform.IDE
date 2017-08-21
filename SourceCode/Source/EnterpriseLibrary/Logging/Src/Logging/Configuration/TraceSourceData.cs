/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Configuration;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
	public class TraceSourceData : NamedConfigurationElement
	{
		private const string defaultLevelProperty = "switchValue";
		private const string traceListenersProperty = "listeners";
        private const string autoFlushProperty = "autoFlush";
		public TraceSourceData()
		{
		}
		public TraceSourceData(string name, SourceLevels defaultLevel)
			: base(name)
		{
			this.DefaultLevel = defaultLevel;
		}
        public TraceSourceData(string name, SourceLevels defaultLevel, bool autoFlush)
            : this(name, defaultLevel)
        {
            this.AutoFlush = autoFlush;
        }
        [ConfigurationProperty(defaultLevelProperty, IsRequired = true, DefaultValue = SourceLevels.All)]
		public SourceLevels DefaultLevel
		{
			get { return (SourceLevels)base[defaultLevelProperty]; }
			set { base[defaultLevelProperty] = value; }
		}
        [ConfigurationProperty(autoFlushProperty, IsRequired = false, DefaultValue = LogSource.DefaultAutoFlushProperty)]
        public bool AutoFlush
        {
            get { return (bool)base[autoFlushProperty]; }
            set { base[autoFlushProperty] = value; }
        }
		[ConfigurationProperty(traceListenersProperty)]
		public NamedElementCollection<TraceListenerReferenceData> TraceListeners
		{
			get { return (NamedElementCollection<TraceListenerReferenceData>)base[traceListenersProperty]; }
		}
	}
}
