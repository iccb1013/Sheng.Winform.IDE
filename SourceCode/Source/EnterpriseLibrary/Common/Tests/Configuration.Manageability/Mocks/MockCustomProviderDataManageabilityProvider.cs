/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests.Mocks
{
	public class MockCustomProviderDataManageabilityProvider
		: CustomProviderDataManageabilityProvider<MockCustomProviderData>
	{
		public MockCustomProviderDataManageabilityProvider()
			: base("{0}")
		{ }
		protected override void GenerateWmiObjects(MockCustomProviderData configurationObject, 
			System.Collections.Generic.ICollection<ConfigurationSetting> wmiSettings)
		{
			wmiSettings.Add(
				new MockCustomProviderSetting(configurationObject.Name, 
					configurationObject.TypeName,
					CustomDataWmiMapperHelper.GenerateAttributesArray(configurationObject.Attributes)));
		}
	}
}
