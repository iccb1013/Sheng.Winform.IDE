/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder
{
	public class NameTypeFactoryBase<T>
	{
		private IConfigurationSource configurationSource;
		protected NameTypeFactoryBase()
			: this(ConfigurationSourceFactory.Create())
		{
		}
		protected NameTypeFactoryBase(IConfigurationSource configurationSource)
		{
			this.configurationSource = configurationSource;
		}
		public T CreateDefault()
		{
			return EnterpriseLibraryFactory.BuildUp<T>(configurationSource);
		}
		public T Create(string name)
		{
			return EnterpriseLibraryFactory.BuildUp<T>(name, configurationSource);
		}
	}
}
