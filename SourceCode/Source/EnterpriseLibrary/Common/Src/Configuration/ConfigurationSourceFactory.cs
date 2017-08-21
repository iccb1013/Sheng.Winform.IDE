/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
	public static class ConfigurationSourceFactory
	{
		public static IConfigurationSource Create(string name)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");
			ConfigurationSourceSection configurationSourceSection
				= ConfigurationSourceSection.GetConfigurationSourceSection();
			if (configurationSourceSection == null)
			{
				throw new ConfigurationErrorsException(Resources.ExceptionConfigurationSourceSectionNotFound);
			}
			ConfigurationSourceElement objectConfiguration
				= configurationSourceSection.Sources.Get(name);
			if (objectConfiguration == null)
			{
				throw new ConfigurationErrorsException(
					string.Format(
						Resources.Culture,
						Resources.ExceptionNamedConfigurationNotFound,
						name,
						"ConfigurationSourceFactory"));
			}
			IConfigurationSource source = objectConfiguration.CreateSource();
			return source;
		}
		public static IConfigurationSource Create()
		{
			ConfigurationSourceSection configurationSourceSection
				= ConfigurationSourceSection.GetConfigurationSourceSection();
			if (configurationSourceSection != null)
			{
				string systemSourceName = configurationSourceSection.SelectedSource;
				if (!string.IsNullOrEmpty(systemSourceName))
				{
					return Create(systemSourceName);
				}
				else
				{
					throw new ConfigurationErrorsException(Resources.ExceptionSystemSourceNotDefined);
				}
			}
			return new SystemConfigurationSource();
		}
	}
}
