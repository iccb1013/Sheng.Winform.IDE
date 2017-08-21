/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity.Fluent
{
	public interface IFinishPoliciesBuilding
	{
		void AddPoliciesToPolicyList(IPolicyList policyList);
	}
}
