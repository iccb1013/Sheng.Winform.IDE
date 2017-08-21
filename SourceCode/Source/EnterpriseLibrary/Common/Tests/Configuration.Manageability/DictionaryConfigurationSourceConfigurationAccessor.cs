/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests
{
	public class DictionaryConfigurationSourceConfigurationAccessor : IConfigurationAccessor
	{
		private DictionaryConfigurationSource configurationSource;
		public DictionaryConfigurationSourceConfigurationAccessor(DictionaryConfigurationSource configurationSource)
		{
			this.configurationSource = configurationSource;
		}
		public ConfigurationSection GetSection(String sectionName)
		{
			return configurationSource.GetSection(sectionName);
		}
		public void RemoveSection(String sectionName)
		{
			configurationSource.Remove(sectionName);
		}
		public IEnumerable<String> GetRequestedSectionNames()
		{
			throw new Exception("The method or operation is not implemented.");
		}
	}
}
