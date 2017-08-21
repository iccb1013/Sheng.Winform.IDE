/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    public class ConfigurationSourceSection : SerializableConfigurationSection
    {
        private const string selectedSourceProperty = "selectedSource";
        private const string sourcesProperty = "sources";
        public const string SectionName = "enterpriseLibrary.ConfigurationSource";        
        public static ConfigurationSourceSection GetConfigurationSourceSection()
        {
			return (ConfigurationSourceSection)ConfigurationManager.GetSection(SectionName);
        }
        [ConfigurationProperty(selectedSourceProperty, IsRequired=true)]
        public string SelectedSource
        {
            get
            {
                return (string)this[selectedSourceProperty];
            }
			set
			{
				this[selectedSourceProperty] = value;
			}
        }
        [ConfigurationProperty(sourcesProperty, IsRequired = true)]
        public NameTypeConfigurationElementCollection<ConfigurationSourceElement, ConfigurationSourceElement> Sources
        {
            get
            {
                return (NameTypeConfigurationElementCollection<ConfigurationSourceElement, ConfigurationSourceElement>)this[sourcesProperty];
            }           
        }
    }
}
