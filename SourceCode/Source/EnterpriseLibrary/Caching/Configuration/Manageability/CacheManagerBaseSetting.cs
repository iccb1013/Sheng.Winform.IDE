/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Management.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Manageability
{
	[ManagementEntity]
	public abstract partial class CacheManagerBaseSetting : NamedConfigurationSetting
	{
		protected CacheManagerBaseSetting(string name)
			: base(name)
		{ 
		}
		protected override bool SaveChanges(System.Configuration.ConfigurationElement sourceElement)
		{
			return false;
		}
	}
}
