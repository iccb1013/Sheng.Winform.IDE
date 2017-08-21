/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
	public interface IConfigurationSource
	{
		ConfigurationSection GetSection(string sectionName);
		void Add(IConfigurationParameter saveParameter, string sectionName, ConfigurationSection configurationSection);
		void Remove(IConfigurationParameter removeParameter, string sectionName);
		void AddSectionChangeHandler(string sectionName, ConfigurationChangedEventHandler handler);
		void RemoveSectionChangeHandler(string sectionName, ConfigurationChangedEventHandler handler);
	}
	public interface IConfigurationParameter
	{
	}
}
