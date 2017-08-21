/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration
{
	[Assembler(typeof(CacheManagerAssembler))]
	[ContainerPolicyCreator(typeof(CacheManagerPolicyCreator))]
	public class CacheManagerData : CacheManagerDataBase
	{
		private const string expirationPollFrequencyInSecondsProperty = "expirationPollFrequencyInSeconds";
		private const string maximumElementsInCacheBeforeScavengingProperty = "maximumElementsInCacheBeforeScavenging";
		private const string numberToRemoveWhenScavengingProperty = "numberToRemoveWhenScavenging";
		private const string backingStoreNameProperty = "backingStoreName";
		public CacheManagerData()
		{
		}
		public CacheManagerData(string name, int expirationPollFrequencyInSeconds, int maximumElementsInCacheBeforeScavenging, int numberToRemoveWhenScavenging, string cacheStorage)
			: base(name, typeof(CacheManager))
		{
			this.ExpirationPollFrequencyInSeconds = expirationPollFrequencyInSeconds;
			this.MaximumElementsInCacheBeforeScavenging = maximumElementsInCacheBeforeScavenging;
			this.NumberToRemoveWhenScavenging = numberToRemoveWhenScavenging;
			this.CacheStorage = cacheStorage;
		}
		[ConfigurationProperty(expirationPollFrequencyInSecondsProperty, IsRequired = true)]
		public int ExpirationPollFrequencyInSeconds
		{
			get { return (int)base[expirationPollFrequencyInSecondsProperty]; }
			set { base[expirationPollFrequencyInSecondsProperty] = value; }
		}
		[ConfigurationProperty(maximumElementsInCacheBeforeScavengingProperty, IsRequired = true)]
		public int MaximumElementsInCacheBeforeScavenging
		{
			get { return (int)base[maximumElementsInCacheBeforeScavengingProperty]; }
			set { base[maximumElementsInCacheBeforeScavengingProperty] = value; }
		}
		[ConfigurationProperty(numberToRemoveWhenScavengingProperty, IsRequired = true)]
		public int NumberToRemoveWhenScavenging
		{
			get { return (int)base[numberToRemoveWhenScavengingProperty]; }
			set { base[numberToRemoveWhenScavengingProperty] = value; }
		}
		[ConfigurationProperty(backingStoreNameProperty, IsRequired = true)]
		public string CacheStorage
		{
			get { return (string)base[backingStoreNameProperty]; }
			set { base[backingStoreNameProperty] = value; }
		}
	}
}
