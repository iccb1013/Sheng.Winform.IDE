/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm
{
	public static class AdministrativeTemplateGenerator
	{
		public static AdmContent GenerateAdministrativeTemplateContent(
			IConfigurationSource configurationSource,
			String applicationName,
			IDictionary<string, ConfigurationSectionManageabilityProvider> manageabilityProviders)
		{
			AdmContentBuilder contentBuilder = new AdmContentBuilder();
			contentBuilder.StartCategory(applicationName);
			foreach (KeyValuePair<string, ConfigurationSectionManageabilityProvider> kvp in manageabilityProviders)
			{
				ConfigurationSection configurationSection = configurationSource.GetSection(kvp.Key);
				if (configurationSection != null)
				{
					kvp.Value.AddAdministrativeTemplateDirectives(contentBuilder,
						configurationSection,
						configurationSource,
						applicationName);
				}
			}
			contentBuilder.EndCategory();
			return contentBuilder.GetContent();
		}
	}
}
