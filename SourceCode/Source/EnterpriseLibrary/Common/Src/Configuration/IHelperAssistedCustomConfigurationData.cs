/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
	public interface IHelperAssistedCustomConfigurationData<T> : ICustomProviderData
		where T : NameTypeConfigurationElement, IHelperAssistedCustomConfigurationData<T>
	{
		CustomProviderDataHelper<T> Helper { get; }
		object BaseGetPropertyValue(ConfigurationProperty property);
		bool BaseIsModified();
		void BaseReset(ConfigurationElement parentElement);
		void BaseSetPropertyValue(ConfigurationProperty property, object value);
		void BaseUnmerge(ConfigurationElement sourceElement, ConfigurationElement parentElement, ConfigurationSaveMode saveMode);
	}
}
