/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity
{
	public sealed class DelegateBuildPlanPolicy : IBuildPlanPolicy
	{
		private readonly Func<IBuilderContext, object> factoryDelegate;
		public DelegateBuildPlanPolicy(Func<IBuilderContext, object> factoryDelegate)
		{
			Guard.ArgumentNotNull(factoryDelegate, "factoryDelegate");
			this.factoryDelegate = factoryDelegate;
		}
		void IBuildPlanPolicy.BuildUp(IBuilderContext context)
		{
			if (context.Existing == null)
			{
				context.Existing = factoryDelegate(context);
			}
		}
	}
}
