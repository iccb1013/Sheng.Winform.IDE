/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder
{
	public class ConfigurationObjectPolicy : IConfigurationObjectPolicy
	{
		private IConfigurationSource configurationSource;
		public ConfigurationObjectPolicy(IConfigurationSource configurationSource)
		{
			this.configurationSource = configurationSource;
		}
		public IConfigurationSource ConfigurationSource
		{
			get { return configurationSource; }
		}
	}
}
