/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Configuration;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
	[ConfigurationElementType(typeof(SystemConfigurationSourceElement))]
	public class SystemConfigurationSource : IConfigurationSource
	{
		private static SystemConfigurationSourceImplementation implementation = new SystemConfigurationSourceImplementation(true);
		public SystemConfigurationSource()
		{
		}
		public ConfigurationSection GetSection(string sectionName)
		{
			return implementation.GetSection(sectionName);
		}
		public void AddSectionChangeHandler(string sectionName, ConfigurationChangedEventHandler handler)
		{
			implementation.AddSectionChangeHandler(sectionName, handler);
		}
		public void RemoveSectionChangeHandler(string sectionName, ConfigurationChangedEventHandler handler)
		{
			implementation.RemoveSectionChangeHandler(sectionName, handler);
		}
		public static void ResetImplementation(bool refreshing)
		{
			SystemConfigurationSourceImplementation currentImplementation = implementation;
			implementation = new SystemConfigurationSourceImplementation(refreshing);
			currentImplementation.Dispose();
		}
		public static BaseFileConfigurationSourceImplementation Implementation
		{
			get { return implementation; }
		}
		public void Save(string fileName, string section, ConfigurationSection configurationSection)
		{
			ValidateArgumentsAndFileExists(fileName, section, configurationSection);
			ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
			fileMap.ExeConfigFilename = fileName;
			System.Configuration.Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
			config.Sections.Remove(section);
			config.Sections.Add(section, configurationSection);
			config.Save();
		}
		public void Remove(string fileName, string section)
		{
			if (string.IsNullOrEmpty(fileName)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "fileName");
			if (string.IsNullOrEmpty(section)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "section");
			ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
			fileMap.ExeConfigFilename = fileName;
			System.Configuration.Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
			config.Sections.Remove(section);
			config.Save();
		}
		public void Add(IConfigurationParameter addParameter, string sectionName, ConfigurationSection configurationSection)
		{
			FileConfigurationParameter parameter = addParameter as FileConfigurationParameter;
			if (null == parameter) throw new ArgumentException(string.Format(Resources.Culture, Resources.ExceptionUnexpectedType, typeof(FileConfigurationParameter).Name), "saveParameter");
			Save(parameter.FileName, sectionName, configurationSection);
		}
		public void Remove(IConfigurationParameter removeParameter, string sectionName)
		{
			FileConfigurationParameter parameter = removeParameter as FileConfigurationParameter;
			if (null == parameter) throw new ArgumentException(string.Format(Resources.Culture, Resources.ExceptionUnexpectedType, typeof(FileConfigurationParameter).Name), "saveParameter");
			Remove(parameter.FileName, sectionName);
		}
		private static void ValidateArgumentsAndFileExists(string fileName, string section, ConfigurationSection configurationSection)
		{
			if (string.IsNullOrEmpty(fileName)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "fileName");
			if (string.IsNullOrEmpty(section)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "section");
			if (null == configurationSection) throw new ArgumentNullException("configurationSection");
			if (!File.Exists(fileName)) throw new FileNotFoundException(string.Format(Resources.ExceptionConfigurationFileNotFound, section), fileName);
		}
	}
}
