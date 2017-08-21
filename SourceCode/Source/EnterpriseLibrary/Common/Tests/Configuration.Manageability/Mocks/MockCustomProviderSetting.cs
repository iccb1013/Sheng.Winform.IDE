/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests.Mocks
{
	public class MockCustomProviderSetting : NamedConfigurationSetting
	{
		public String providerTypeName;
		public String[] attributes;
		public MockCustomProviderSetting(String name, String providerTypeName, String[] attributes)
			: base(name)
		{
			this.providerTypeName = providerTypeName;
			this.attributes = attributes;
		}
	}
}
