/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
{
	public class ExceptionPolicyFactory : LocatorNameTypeFactoryBase<ExceptionPolicyImpl>
	{
		public ExceptionPolicyFactory()
			: base()
		{
		}
        public ExceptionPolicyFactory(IConfigurationSource configurationSource)
			: base(configurationSource)
		{
		}
	}
}
