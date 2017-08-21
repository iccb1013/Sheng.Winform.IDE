/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
    public class ConfigurationInstanceConfigurationAccessor : IConfigurationAccessor
    {
        readonly System.Configuration.Configuration configuration;
        readonly IDictionary<string, bool> requestedSections;
        public ConfigurationInstanceConfigurationAccessor(System.Configuration.Configuration configuration)
        {
            this.configuration = configuration;
            requestedSections = new Dictionary<string, bool>();
        }
        public IEnumerable<string> GetRequestedSectionNames()
        {
            String[] requestedSectionNames = new String[requestedSections.Keys.Count];
            requestedSections.Keys.CopyTo(requestedSectionNames, 0);
            return requestedSectionNames;
        }
        public ConfigurationSection GetSection(string sectionName)
        {
            ConfigurationSection section = configuration.GetSection(sectionName);
            requestedSections[sectionName] = section != null;
            return section;
        }
        public void RemoveSection(string sectionName)
        {
            configuration.Sections.Remove(sectionName);
        }
    }
}
