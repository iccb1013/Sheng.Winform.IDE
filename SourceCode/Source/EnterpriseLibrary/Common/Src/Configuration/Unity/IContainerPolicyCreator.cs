/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Configuration;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity
{
	public interface IContainerPolicyCreator
	{
		void CreatePolicies(IPolicyList policyList,
							string instanceName,
							ConfigurationElement configurationObject,
		                    IConfigurationSource configurationSource);
	}
}
