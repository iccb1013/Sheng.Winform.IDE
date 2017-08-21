/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Configuration;
using System.IO;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests
{
	public static class ConfigurationTestHelper
	{
		public static IConfigurationSource SaveSectionsAndReturnConfigurationSource(IDictionary<string, ConfigurationSection> sections)
		{
			System.Configuration.Configuration configuration 
				= ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			SaveSections(configuration, sections);
			return new SystemConfigurationSource();
		}
		public static IConfigurationSource SaveSectionsInFileAndReturnConfigurationSource(IDictionary<string, ConfigurationSection> sections)
		{
			System.Configuration.Configuration configuration
				= GetConfigurationForCustomFile("test.exe.config");
			SaveSections(configuration, sections);
			return GetConfigurationSourceForCustomFile("test.exe.config");
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
	}
}
