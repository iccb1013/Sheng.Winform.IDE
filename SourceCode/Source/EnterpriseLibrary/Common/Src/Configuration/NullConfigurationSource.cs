/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
	public class NullConfigurationSource : IConfigurationSource
	{
		public ConfigurationSection GetSection(string sectionName)
		{
			return null;
		}
		public void Add(IConfigurationParameter saveParameter, string sectionName, ConfigurationSection configurationSection)
        {            
        }
		public void Remove(IConfigurationParameter removeParameter, string sectionName)
        {
        }
        public void AddSectionChangeHandler(string sectionName, ConfigurationChangedEventHandler handler)
        {
        }
        public void RemoveSectionChangeHandler(string sectionName, ConfigurationChangedEventHandler handler)
        {
        }
    }
}
