/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Security;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    public class SystemConfigurationSourceImplementation : BaseFileConfigurationSourceImplementation
    {
        public SystemConfigurationSourceImplementation()
            : base(SafeGetCurrentConfigurationFile())
        {
        }
        public SystemConfigurationSourceImplementation(bool refresh)
            : base(SafeGetCurrentConfigurationFile(), refresh)
        {
        }
        private static string SafeGetCurrentConfigurationFile()
        {
            try
            {
                return AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            }
            catch (SecurityException)
            {
                return null;
            }
        }
        public override ConfigurationSection GetSection(string sectionName)
        {
            ConfigurationSection configurationSection = ConfigurationManager.GetSection(sectionName) as ConfigurationSection;
            SetConfigurationWatchers(sectionName, configurationSection);
            return configurationSection;
        }
        protected override void RefreshAndValidateSections(IDictionary<string, string> localSectionsToRefresh, IDictionary<string, string> externalSectionsToRefresh, out ICollection<string> sectionsToNotify, out IDictionary<string, string> sectionsWithChangedConfigSource)
        {
            sectionsToNotify = new List<string>();
            sectionsWithChangedConfigSource = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> sectionMapping in localSectionsToRefresh)
            {
                ConfigurationManager.RefreshSection(sectionMapping.Key);
                ConfigurationSection section = ConfigurationManager.GetSection(sectionMapping.Key) as ConfigurationSection;
                string refreshedConfigSource = section != null ? section.SectionInformation.ConfigSource : NullConfigSource;
                if (!sectionMapping.Value.Equals(refreshedConfigSource))
                {
                    sectionsWithChangedConfigSource.Add(sectionMapping.Key, refreshedConfigSource);
                }
                sectionsToNotify.Add(sectionMapping.Key);
            }
            foreach (KeyValuePair<string, string> sectionMapping in externalSectionsToRefresh)
            {
                ConfigurationManager.RefreshSection(sectionMapping.Key);
                ConfigurationSection section = ConfigurationManager.GetSection(sectionMapping.Key) as ConfigurationSection;
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
            foreach (string sectionName in sectionsToRefresh)
            {
                ConfigurationManager.RefreshSection(sectionName);
            }
        }
    }
}
