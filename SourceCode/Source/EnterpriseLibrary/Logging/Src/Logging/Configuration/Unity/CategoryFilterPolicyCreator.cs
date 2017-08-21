/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Unity
{
	public class CategoryFilterPolicyCreator : IContainerPolicyCreator
	{
		void IContainerPolicyCreator.CreatePolicies(
			IPolicyList policyList,
			string instanceName,
			ConfigurationElement configurationObject,
			IConfigurationSource configurationSource)
		{
			CategoryFilterData castConfigurationObject = (CategoryFilterData)configurationObject;
			new PolicyBuilder<CategoryFilter, CategoryFilterData>(
				instanceName,
				castConfigurationObject,
				c => new CategoryFilter(
						c.Name,
						new List<string>(from cfe in castConfigurationObject.CategoryFilters select cfe.Name),
						c.CategoryFilterMode))
				.AddPoliciesToPolicyList(policyList);
		}
	}
}
