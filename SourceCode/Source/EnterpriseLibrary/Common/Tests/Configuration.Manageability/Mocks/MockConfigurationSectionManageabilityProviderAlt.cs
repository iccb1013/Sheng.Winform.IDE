/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests.Mocks
{
	public class MockConfigurationSectionManageabilityProviderAlt : MockConfigurationSectionManageabilityProviderBase
	{
		public MockConfigurationSectionManageabilityProviderAlt(IDictionary<Type, ConfigurationElementManageabilityProvider> subProviders)
			: base(subProviders)
		{ }
	}
}
