/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Storage;
using System.IO;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
	public abstract class ConfigurationSourceWatcher : IDisposable
	{
		private string configurationSource;
		private IList<string> watchedSections;
		protected ConfigurationChangeWatcher configWatcher = null;
		public ConfigurationSourceWatcher(string configSource, bool refresh, ConfigurationChangedEventHandler changed)
		{
			this.configurationSource = configSource;
			this.watchedSections = new List<string>();
		}
		public string ConfigSource
		{
			get { return configurationSource; }
			set { configurationSource = value; }
		}
		public IList<string> WatchedSections
		{
			get { return watchedSections; }
			set { watchedSections = value; }
		}
		public void StartWatching()
		{
			if (this.configWatcher != null)
			{
				this.configWatcher.StartWatching();
			}
		}
		public void StopWatching()
		{
			if (this.configWatcher != null)
			{
				this.configWatcher.StopWatching();
			}
		}
		public ConfigurationChangeWatcher Watcher
		{
			get { return this.configWatcher; }
		}
		void IDisposable.Dispose()
		{
			if (this.configWatcher != null)
			{
				this.configWatcher.Dispose();
			}
		}
	}
}
