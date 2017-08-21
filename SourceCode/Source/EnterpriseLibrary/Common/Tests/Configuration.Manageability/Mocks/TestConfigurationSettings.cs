/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests.Mocks
{
	public class TestConfigurationSettings : ConfigurationSetting
	{
		public TestConfigurationSettings(string value)
		{
			this.Value = value;
		}
		public string Value;
		protected override bool SaveChanges(ConfigurationElement sourceElement)
		{
			((TestsConfigurationSection)sourceElement).Value = this.Value;
			return true;	
		}
		public override void Publish()
		{
			throw new System.NotImplementedException();
		}
		public override void Revoke()
		{
			throw new System.NotImplementedException();
		}
	}
}
