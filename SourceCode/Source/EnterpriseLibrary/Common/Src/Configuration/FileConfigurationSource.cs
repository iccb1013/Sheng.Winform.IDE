//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
	/// <summary>
	/// Represents a configuration source that retrieves configuration information from an arbitrary file.
	/// </summary>
	/// <remarks>
	/// This configuration source uses a <see cref="System.Configuration.Configuration"/> object to deserialize configuration, so 
	/// the configuration file must be a valid .NET Framework configuration file.
	/// </remarks>
	[ConfigurationElementType(typeof(FileConfigurationSourceElement))]
	public class FileConfigurationSource : IConfigurationSource, IProtectedConfigurationSource
	{
		private static Dictionary<string, FileConfigurationSourceImplementation> implementationByFilepath = new Dictionary<string, FileConfigurationSourceImplementation>(StringComparer.OrdinalIgnoreCase);
		private string configurationFilepath;
		private static object lockObject = new object();

		/// <summary>
		/// Initializes a new instance of the <see cref="FileConfigurationSource"/> class with a configuration file path.
		/// </summary>
		/// <param name="configurationFilepath">The configuration file path. The path can be absolute or relative.</param>
		public FileConfigurationSource(string configurationFilepath)
		{
			if (string.IsNullOrEmpty(configurationFilepath)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "configurationFilepath");
			this.configurationFilepath = RootConfigurationFilePath(configurationFilepath);

			if (!File.Exists(this.configurationFilepath)) throw new FileNotFoundException(string.Format(Resources.Culture, Resources.ExceptionConfigurationLoadFileNotFound, this.configurationFilepath));
			EnsureImplementation(this.configurationFilepath);
		}

		/// <summary>
		/// Retrieves the specified <see cref="ConfigurationSection"/>.
		/// </summary>
		/// <param name="sectionName">The name of the section to be retrieved.</param>
		/// <returns>The specified <see cref="ConfigurationSection"/>, or <see langword="null"/> (<b>Nothing</b> in Visual Basic)
		/// if a section by that name is not found.</returns>
		public ConfigurationSection GetSection(string sectionName)
		{
			return implementationByFilepath[configurationFilepath].GetSection(sectionName);
		}

		/// <summary>
		/// Adds a handler to be called when changes to section <code>sectionName</code> are detected.
		/// This call should always be followed by a <see cref="RemoveSectionChangeHandler"/>. Failure to remove change
		/// handlers will result in .Net resource leaks.
		/// </summary>
		/// <param name="sectionName">The name of the section to watch for.</param>
		/// <param name="handler">The handler.</param>
		public void AddSectionChangeHandler(string sectionName, ConfigurationChangedEventHandler handler)
		{
			implementationByFilepath[configurationFilepath].AddSectionChangeHandler(sectionName, handler);
		}

		/// <summary>
		/// Remove a handler to be called when changes to section <code>sectionName</code> are detected.
		/// This class should always follow a call to <see cref="AddSectionChangeHandler"/>. Failure
		/// to call these methods in pairs will result in .Net resource leaks.
		/// </summary>
		/// <param name="sectionName">The name of the section to watch for.</param>
		/// <param name="handler">The handler.</param>
		public void RemoveSectionChangeHandler(string sectionName, ConfigurationChangedEventHandler handler)
		{
			implementationByFilepath[configurationFilepath].RemoveSectionChangeHandler(sectionName, handler);
		}

		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Adds or replaces <paramref name="configurationSection"/> under name <paramref name="section"/> in the configuration 
		/// file named <paramref name="fileName" /> and saves the configuration file.
		/// </summary>
		/// <param name="fileName">The name of the configuration file.</param>
		/// <param name="section">The name for the section.</param>
		/// <param name="configurationSection">The configuration section to add or replace.</param>
		public void Save(string fileName, string section, ConfigurationSection configurationSection)
		{
            ValidateArgumentsAndFileExists(fileName, section, configurationSection);

            InternalSave(fileName, section, configurationSection, string.Empty);
		}

        /// <summary>
        /// TODO: add comment
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="section"></param>
        /// <param name="configurationSection"></param>
        /// <param name="protectionProvider"></param>
        public void Save(string fileName, string section, ConfigurationSection configurationSection, string protectionProvider)
        {
            ValidateArgumentsAndFileExists(fileName, section, configurationSection);
            if (string.IsNullOrEmpty(protectionProvider)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "protectionProvider");

            InternalSave(fileName, section, configurationSection, protectionProvider);
        }

        private void InternalSave(string fileName, string section, ConfigurationSection configurationSection, string protectionProvider)
        {
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = fileName;
            System.Configuration.Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            if (typeof(ConnectionStringsSection) == configurationSection.GetType())
            {
                config.Sections.Remove(section);
                UpdateConnectionStrings(section, configurationSection, config, protectionProvider);
            }
            else if (typeof(AppSettingsSection) == configurationSection.GetType())
            {
                UpdateApplicationSettings(section, configurationSection, config, protectionProvider);
            }
            else
            {
                config.Sections.Remove(section);
                config.Sections.Add(section, configurationSection);
                ProtectConfigurationSection(configurationSection, protectionProvider);
            }

            config.Save();

            UpdateImplementation(fileName);
        }

        private static void ProtectConfigurationSection(ConfigurationSection configurationSection, string protectionProvider)
        {
            if (!string.IsNullOrEmpty(protectionProvider))
            {
                if(configurationSection.SectionInformation.ProtectionProvider == null
                    || configurationSection.SectionInformation.ProtectionProvider.Name != protectionProvider)
                {
                    configurationSection.SectionInformation.ProtectSection(protectionProvider);
                }
            }
            else
            {
                if(configurationSection.SectionInformation.ProtectionProvider != null)
                {
                    configurationSection.SectionInformation.UnprotectSection();
                }
            }
        }
        
        private void UpdateApplicationSettings(string section, ConfigurationSection configurationSection, System.Configuration.Configuration config, string protectionProvider)
        {
            AppSettingsSection current = config.AppSettings;
            if (current == null)
            {
                config.Sections.Add(section, configurationSection);
                ProtectConfigurationSection(configurationSection, protectionProvider);
            }
            else
            {
                AppSettingsSection newApplicationSettings = configurationSection as AppSettingsSection;
                if (current.File != newApplicationSettings.File)
                {
                    current.File = newApplicationSettings.File;
                }

                List<string> newKeys = new List<string>(newApplicationSettings.Settings.AllKeys);
                List<string> currentKeys = new List<string>(current.Settings.AllKeys);

                foreach (string keyInCurrent in currentKeys)
                {
                    if (!newKeys.Contains(keyInCurrent))
                    {
                        current.Settings.Remove(keyInCurrent);
                    }
                }

                foreach (string newKey in newKeys)
                {
                    if (!currentKeys.Contains(newKey))
                    {
                        current.Settings.Add(newKey, newApplicationSettings.Settings[newKey].Value);
                    }
                    else
                    {
                        if (current.Settings[newKey].Value != newApplicationSettings.Settings[newKey].Value)
                        {
                            current.Settings[newKey].Value = newApplicationSettings.Settings[newKey].Value;
                        }
                    }
                }
                ProtectConfigurationSection(current, protectionProvider);
            }

        }
		private void UpdateConnectionStrings(string section, ConfigurationSection configurationSection, System.Configuration.Configuration config, string protectionProvider)
		{
			ConnectionStringsSection current = config.ConnectionStrings;
			if (current == null)
			{
				config.Sections.Add(section, configurationSection);
                ProtectConfigurationSection(configurationSection, protectionProvider);
			}
			else
			{
				ConnectionStringsSection newConnectionStrings = (ConnectionStringsSection)configurationSection;
				foreach (ConnectionStringSettings connectionString in newConnectionStrings.ConnectionStrings)
				{
					if (current.ConnectionStrings[connectionString.Name] == null)
					{
						current.ConnectionStrings.Add(connectionString);
					}
				}
                ProtectConfigurationSection(current, protectionProvider);
			}
		}

		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Removes the configuration section named <paramref name="section"/> from the configuration file named
		/// <paramref name="fileName"/> and saves the configuration file.
		/// </summary>
		/// <param name="fileName">The name of the configuration file.</param>
		/// <param name="section">The name for the section.</param>
		public void Remove(string fileName, string section)
		{
			if (string.IsNullOrEmpty(fileName)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "fileName");
			if (string.IsNullOrEmpty(section)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "section");

			ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
			fileMap.ExeConfigFilename = fileName;
			System.Configuration.Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
			if (config.Sections.Get(section) != null)
			{
				config.Sections.Remove(section);
				config.Save();
				UpdateImplementation(fileName);
			}
		}

		/// <summary>
		/// Adds a <see cref="ConfigurationSection"/> to the configuration source location specified by 
		/// <paramref name="saveParameter"/> and saves the configuration source.
		/// </summary>
		/// <remarks>
		/// If a configuration section with the specified name already exists in the location specified by 
		/// <paramref name="saveParameter"/> it will be replaced.
		/// </remarks>
		/// <param name="saveParameter">The <see cref="IConfigurationParameter"/> that represents the location where 
		/// to save the updated configuration. Must be an instance of <see cref="FileConfigurationParameter"/>.</param>
		/// <param name="sectionName">The name by which the <paramref name="configurationSection"/> should be added.</param>
		/// <param name="configurationSection">The configuration section to add.</param>
		public void Add(IConfigurationParameter saveParameter, string sectionName, ConfigurationSection configurationSection)
		{
			FileConfigurationParameter parameter = saveParameter as FileConfigurationParameter;
			if (null == parameter) throw new ArgumentException(string.Format(Resources.Culture, Resources.ExceptionUnexpectedType, typeof(FileConfigurationParameter).Name), "saveParameter");

			Save(parameter.FileName, sectionName, configurationSection);
		}


        /// <summary>
        /// TODO: add comment
        /// </summary>
        /// <param name="saveParameter"></param>
        /// <param name="sectionName"></param>
        /// <param name="configurationSection"></param>
        /// <param name="protectionProviderName"></param>
        public void Add(IConfigurationParameter saveParameter, string sectionName, ConfigurationSection configurationSection, string protectionProviderName)
        {
            FileConfigurationParameter parameter = saveParameter as FileConfigurationParameter;
            if (null == parameter) throw new ArgumentException(string.Format(Resources.Culture, Resources.ExceptionUnexpectedType, typeof(FileConfigurationParameter).Name), "saveParameter");

            Save(parameter.FileName, sectionName, configurationSection, protectionProviderName);
            
        }

		/// <summary>
		/// Removes a <see cref="ConfigurationSection"/> from the configuration source location specified by 
		/// <paramref name="removeParameter"/> and saves the configuration source.
		/// </summary>
		/// <param name="removeParameter">The <see cref="IConfigurationParameter"/> that represents the location where 
		/// to save the updated configuration. Must be an instance of <see cref="FileConfigurationParameter"/>.</param>
		/// <param name="sectionName">The name of the section to remove.</param>
		public void Remove(IConfigurationParameter removeParameter, string sectionName)
		{
			FileConfigurationParameter parameter = removeParameter as FileConfigurationParameter;
			if (null == parameter) throw new ArgumentException(string.Format(Resources.Culture, Resources.ExceptionUnexpectedType, typeof(FileConfigurationParameter).Name), "saveParameter");

			Remove(parameter.FileName, sectionName);
		}

        /// <summary>
        /// Flushes the internal cache of the FileConfigurationSource.
        /// </summary>
        /// <param name="configurationFilepath">The path to the configuration that should have its caches flushed.</param>
        /// <param name="refreshing">tue if a ConfigurationFileWatcher should be setup to monitor changes to the configuration file, otherwise false.</param>
		public static void ResetImplementation(string configurationFilepath, bool refreshing)
		{
			string rootedConfigurationFilepath = RootConfigurationFilePath(configurationFilepath);
			FileConfigurationSourceImplementation currentImplementation = null;
			implementationByFilepath.TryGetValue(rootedConfigurationFilepath, out currentImplementation);
			implementationByFilepath[rootedConfigurationFilepath] = new FileConfigurationSourceImplementation(rootedConfigurationFilepath, refreshing);

			if (currentImplementation != null)
			{
				currentImplementation.Dispose();
			}
		}

        /// <summary>
        /// Gets the implemenation of the source.
        /// </summary>
        /// <value>
        /// The implemenation of the source.
        /// </value>
		public BaseFileConfigurationSourceImplementation Implementation
		{
			get { return implementationByFilepath[configurationFilepath]; }
		}

        /// <summary>
        /// Get the implemenation for the configuration file.
        /// </summary>
        /// <param name="configurationFilepath">The path of the configuration file.</param>
        /// <returns>A file configuration source implmentation.</returns>
		public static BaseFileConfigurationSourceImplementation GetImplementation(string configurationFilepath)
		{
			string rootedConfigurationFilepath = RootConfigurationFilePath(configurationFilepath);
			EnsureImplementation(rootedConfigurationFilepath);
			return implementationByFilepath[rootedConfigurationFilepath];
		}

		private static void ValidateArgumentsAndFileExists(string fileName, string section, ConfigurationSection configurationSection)
		{
			if (string.IsNullOrEmpty(fileName)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "fileName");
			if (string.IsNullOrEmpty(section)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "section");
			if (null == configurationSection) throw new ArgumentNullException("configurationSection");

			if (!File.Exists(fileName)) throw new FileNotFoundException(string.Format(Resources.Culture, Resources.ExceptionConfigurationFileNotFound, section), fileName);
		}

		private static string RootConfigurationFilePath(string configurationFile)
		{
			string rootedConfigurationFile = (string)configurationFile.Clone();
			if (!Path.IsPathRooted(rootedConfigurationFile))
			{
				rootedConfigurationFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, rootedConfigurationFile);
			}
			return rootedConfigurationFile;
		}

		private static void EnsureImplementation(string rootedConfigurationFile)
		{
			if (!implementationByFilepath.ContainsKey(rootedConfigurationFile))
			{
				lock (lockObject)
				{
					if (!implementationByFilepath.ContainsKey(rootedConfigurationFile))
					{
						FileConfigurationSourceImplementation implementation = new FileConfigurationSourceImplementation(rootedConfigurationFile);
						implementationByFilepath.Add(rootedConfigurationFile, implementation);
					}
				}
			}
		}

		private static void UpdateImplementation(string fileName)
		{
			FileConfigurationSourceImplementation implementation;
			implementationByFilepath.TryGetValue(fileName, out implementation);
			if (implementation != null)
			{
				implementation.UpdateCache();
			}
		}
    }
}
