/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Storage
{
	[HasInstallableResourcesAttribute()]
	[EventLogDefinition("Application", eventSourceName)]
	public class ConfigurationChangeFileWatcher : ConfigurationChangeWatcher, IConfigurationChangeWatcher
	{
		private const string eventSourceName = "Enterprise Library Configuration";
		private string configurationSectionName;
		private string configFilePath;
		public ConfigurationChangeFileWatcher(string configFilePath, string configurationSectionName)
		{
			if (string.IsNullOrEmpty(configFilePath)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "configFilePath");
			if (null == configurationSectionName) throw new ArgumentNullException("configurationSectionName");
			this.configurationSectionName = configurationSectionName;
			this.configFilePath = configFilePath;
		}
		~ConfigurationChangeFileWatcher()
		{
			Disposing(false);
		}
		public override string SectionName
		{
			get { return configurationSectionName; }
		}
		protected override DateTime GetCurrentLastWriteTime()
		{
			if (File.Exists(configFilePath) == true)
			{
				return File.GetLastWriteTime(configFilePath);
			}
			else
			{
				return DateTime.MinValue;
			}
		}
		protected override string BuildThreadName()
		{
			return "_ConfigurationFileWatcherThread : " + configFilePath;
		}
		protected override ConfigurationChangedEventArgs BuildEventData()
		{
			return new ConfigurationFileChangedEventArgs(Path.GetFullPath(configFilePath), configurationSectionName);
		}
		protected override string GetEventSourceName()
		{
			return eventSourceName;
		}
	}
}
