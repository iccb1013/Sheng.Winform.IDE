/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Configuration;
using System.ComponentModel;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
	public class DictionaryConfigurationSource : IConfigurationSource
	{
		protected internal Dictionary<string, ConfigurationSection> sections;
		protected internal EventHandlerList eventHandlers = new EventHandlerList();
		public DictionaryConfigurationSource()
		{
			this.sections = new Dictionary<string, ConfigurationSection>();
		}
		public ConfigurationSection GetSection(string sectionName)
		{
			if (sections.ContainsKey(sectionName))
			{
				return sections[sectionName];
			}
			return null;
		}
		public void Add(string name, ConfigurationSection section)
		{
			sections.Add(name, section);
		}
		public bool Remove(string name)
		{
			return sections.Remove(name);
		}
		public void Remove(IConfigurationParameter removeParameter, string sectionName)
		{
			Remove(sectionName);
		}
		public bool Contains(string name)
		{
			return sections.ContainsKey(name);
		}
		public void Add(IConfigurationParameter saveParameter, string sectionName, ConfigurationSection configurationSection)
		{
			Add(sectionName, configurationSection);
		}
		public void AddSectionChangeHandler(string sectionName, ConfigurationChangedEventHandler handler)
		{
			eventHandlers.AddHandler(sectionName, handler);
		}
		public void RemoveSectionChangeHandler(string sectionName, ConfigurationChangedEventHandler handler)
		{
			eventHandlers.RemoveHandler(sectionName, handler);
		}
	}
}
