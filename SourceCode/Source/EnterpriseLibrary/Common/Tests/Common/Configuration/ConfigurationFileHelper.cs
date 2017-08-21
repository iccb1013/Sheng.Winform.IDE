/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration
{
	public class ConfigurationFileHelper : IDisposable
	{
		private System.Configuration.Configuration configuration;
		private IConfigurationSource configurationSource;
		private string configurationFileName;
		public ConfigurationFileHelper(IDictionary<string, ConfigurationSection> sections)
		{
			configurationFileName = Path.GetTempFileName();
			File.Copy("test.exe.config", configurationFileName, true);
			configuration = GetConfigurationForCustomFile(configurationFileName);
			SaveSections(configuration, sections);
			configurationSource = GetConfigurationSourceForCustomFile(configurationFileName);
		}
		public static IConfigurationSource GetConfigurationSourceForCustomFile(string fileName)
		{
			FileConfigurationSource.ResetImplementation(fileName, false);
			return new FileConfigurationSource((fileName));
		}
		public static System.Configuration.Configuration GetConfigurationForCustomFile(string fileName)
		{
			ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
			fileMap.ExeConfigFilename = fileName;
			File.SetAttributes(fileMap.ExeConfigFilename, FileAttributes.Normal);
			return ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
		}
		private static void SaveSections(System.Configuration.Configuration configuration,
									IDictionary<string, ConfigurationSection> sections)
		{
			foreach (string sectionName in sections.Keys)
			{
				configuration.Sections.Remove(sectionName);
				configuration.Sections.Add(sectionName, sections[sectionName]);
			}
			configuration.Save();
		}
		public IConfigurationSource ConfigurationSource
		{
			get { return this.configurationSource; }
		}
		public void Dispose()
		{
			FileConfigurationSource.ResetImplementation(this.configurationFileName, false);
			File.Delete(this.configurationFileName);
		}
	}
}
