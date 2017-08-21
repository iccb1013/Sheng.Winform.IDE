/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests.Mocks
{
	public class MockWmiPublisher : IWmiPublisher
	{
		private IDictionary<ConfigurationSetting, ConfigurationSetting> publishedInstances;
		public MockWmiPublisher()
		{
			publishedInstances = new Dictionary<ConfigurationSetting, ConfigurationSetting>();
		}
		public ICollection<ConfigurationSetting> GetPublishedInstances()
		{
			return publishedInstances.Keys;
		}
		public void Publish(ConfigurationSetting instance)
		{
			publishedInstances[instance] = instance;
		}
		public void Revoke(ConfigurationSetting instance)
		{
			publishedInstances.Remove(instance);
		}
	}
}
