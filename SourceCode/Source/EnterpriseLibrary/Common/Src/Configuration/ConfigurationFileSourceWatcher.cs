/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Storage;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
	public class ConfigurationFileSourceWatcher : ConfigurationSourceWatcher
	{
		private string configurationFilepath;
		public ConfigurationFileSourceWatcher(string configurationFilepath, string configSource, bool refresh, ConfigurationChangedEventHandler changed)
			: base(configSource, refresh, changed)
		{
			this.configurationFilepath = configurationFilepath;
			if (refresh)
			{
				SetUpWatcher(changed);
			}
		}
		private void SetUpWatcher(ConfigurationChangedEventHandler changed)
		{
			this.configWatcher = new ConfigurationChangeFileWatcher(GetFullFileName(this.configurationFilepath, this.ConfigSource), this.ConfigSource);
			this.configWatcher.ConfigurationChanged += changed;
		}
		public static string GetFullFileName(string configurationFilepath, string configSource)
		{
			if (string.Empty == configSource)
			{
				return configurationFilepath;
			}
			else
			{
				if (!Path.IsPathRooted(configSource))
				{
					return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configSource);
				}
				else
				{
					return configSource;
				}
			}
		}
	}
}
