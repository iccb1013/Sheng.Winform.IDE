/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
	public class CacheCapacityScavengingPolicy
	{
		private readonly int maximumElementsInCacheBeforeScavenging;
		public CacheCapacityScavengingPolicy(int maximumElementsInCacheBeforeScavenging)
		{
			this.maximumElementsInCacheBeforeScavenging = maximumElementsInCacheBeforeScavenging;
		}
		public int MaximumItemsAllowedBeforeScavenging
		{
			get { return this.maximumElementsInCacheBeforeScavenging; }
		}
		public bool IsScavengingNeeded(int currentCacheItemCount)
		{
			return currentCacheItemCount > MaximumItemsAllowedBeforeScavenging;
		}
	}
}
