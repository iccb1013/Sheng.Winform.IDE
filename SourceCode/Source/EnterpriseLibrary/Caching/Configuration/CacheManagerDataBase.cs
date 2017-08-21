/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using System;
namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration
{
	[Assembler(typeof(TypeInstantiationAssembler<ICacheManager, CacheManagerDataBase>))]
	public class CacheManagerDataBase : NameTypeConfigurationElement
	{
		public CacheManagerDataBase()
		{
		}
		public CacheManagerDataBase(string name, Type type)
			: base(name, type)
		{
		}
	}
}
