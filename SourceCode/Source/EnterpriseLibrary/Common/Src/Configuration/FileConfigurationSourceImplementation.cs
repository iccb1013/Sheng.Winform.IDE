/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
	public class FileConfigurationSourceImplementation : BaseFileConfigurationSourceImplementation
	{
		private string configurationFilepath;
		private ExeConfigurationFileMap fileMap;
		private System.Configuration.Configuration cachedConfiguration;
		private object cachedConfigurationLock = new object();
		public FileConfigurationSourceImplementation(string configurationFilepath)
			: this(configurationFilepath, true)
		{
		}
		public FileConfigurationSourceImplementation(string configurationFilepath, bool refresh)
			: base(configurationFilepath, refresh)
		{
			this.configurationFilepath = configurationFilepath;
			this.fileMap = new ExeConfigurationFileMap();
			fileMap.ExeConfigFilename = configurationFilepath;
		}
		public override ConfigurationSection GetSection(string sectionName)
		{
			System.Configuration.Configuration configuration = GetConfiguration();
			ConfigurationSection configurationSection = configuration.GetSection(sectionName) as ConfigurationSection;
			SetConfigurationWatchers(sectionName, configurationSection);
			return configurationSection;
		}
		protected override void RefreshAndValidateSections(IDictionary<string, string> localSectionsToRefresh, IDictionary<string, string> externalSectionsToRefresh, out ICollection<string> sectionsToNotify, out IDictionary<string, string> sectionsWithChangedConfigSource)
		{
			UpdateCache();
			sectionsToNotify = new List<string>();
			sectionsWithChangedConfigSource = new Dictionary<string, string>();
			foreach (KeyValuePair<string, string> sectionMapping in localSectionsToRefresh)
			{
				ConfigurationSection section = cachedConfiguration.GetSection(sectionMapping.Key) as ConfigurationSection;
				string refreshedConfigSource = section != null ? section.SectionInformation.ConfigSource : NullConfigSource;
				if (!sectionMapping.Value.Equals(refreshedConfigSource))
				{
					sectionsWithChangedConfigSource.Add(sectionMapping.Key, refreshedConfigSource);
				}
				sectionsToNotify.Add(sectionMapping.Key);
			}
			foreach (KeyValuePair<string, string> sectionMapping in externalSectionsToRefresh)
			{
				ConfigurationSection section = cachedConfiguration.GetSection(sectionMapping.Key) as ConfigurationSection;
				string refreshedConfigSource = section != null ? section.SectionInformation.ConfigSource : NullConfigSource;
				if (!sectionMapping.Value.Equals(refreshedConfigSource))
				{
					sectionsWithChangedConfigSource.Add(sectionMapping.Key, refreshedConfigSource);
					sectionsToNotify.Add(sectionMapping.Key);
				}
			}
		}
		protected override void RefreshExternalSections(string[] sectionsToRefresh)
		{
			UpdateCache();
		}
		private System.Configuration.Configuration GetConfiguration()
		{
			if (cachedConfiguration == null)
			{
				lock (cachedConfigurationLock)
				{
					if (cachedConfiguration == null)
					{
						cachedConfiguration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
					}
				}
			}
			return cachedConfiguration;
		}
		internal void UpdateCache()
		{
			System.Configuration.Configuration newConfiguration
				= ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
			lock (cachedConfigurationLock)
			{
				cachedConfiguration = newConfiguration;
			}
		}
	}
}
