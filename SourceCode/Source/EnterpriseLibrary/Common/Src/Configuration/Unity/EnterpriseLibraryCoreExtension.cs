/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity
{
	public class EnterpriseLibraryCoreExtension : UnityContainerExtension
	{
		private readonly IConfigurationSource configurationSource;
		public EnterpriseLibraryCoreExtension()
			: this(ConfigurationSourceFactory.Create())
		{ }
		public EnterpriseLibraryCoreExtension(IConfigurationSource configurationSource)
		{
			this.configurationSource = configurationSource;
		}
		protected override void Initialize()
		{
			Context.Policies.Set<IConfigurationObjectPolicy>(
				new ConfigurationObjectPolicy(this.configurationSource), 
				typeof(IConfigurationSource));
			Context.Policies.Set<IReflectionCachePolicy>(
				new ReflectionCachePolicy(new ConfigurationReflectionCache()),
				typeof(IReflectionCachePolicy));
			Context.Strategies.AddNew<InstrumentationStrategy>(UnityBuildStage.PostInitialization);
		}
	}
}
