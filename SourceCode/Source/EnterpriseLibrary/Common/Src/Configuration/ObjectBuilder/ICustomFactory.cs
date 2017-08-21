/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder
{
	public interface ICustomFactory
	{
		object CreateObject(IBuilderContext context, string name, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache);
	}
}
