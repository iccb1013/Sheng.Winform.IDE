/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder
{
	public class LocatorNameTypeFactoryBase<T>
	{
		private readonly IReadWriteLocator locator;
		private readonly ILifetimeContainer lifetimeContainer;
		private readonly IConfigurationSource configurationSource;
		protected LocatorNameTypeFactoryBase()
			: this(ConfigurationSourceFactory.Create())
		{
		}
		protected LocatorNameTypeFactoryBase(IConfigurationSource configurationSource)
		{
			this.configurationSource = configurationSource;
			locator = new Locator();
			lifetimeContainer = new LifetimeContainer();
		}
		public T CreateDefault()
		{
			return EnterpriseLibraryFactory.BuildUp<T>(locator, lifetimeContainer, configurationSource);
		}
		public T Create(string name)
		{
			return EnterpriseLibraryFactory.BuildUp<T>(locator, lifetimeContainer, name, configurationSource);
		}
	}
}
