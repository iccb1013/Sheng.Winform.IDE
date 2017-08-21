/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity
{
	public sealed class FixedPropertySelectorPolicy : IPropertySelectorPolicy
	{
		private readonly IEnumerable<SelectedProperty> selectedProperties;
		public FixedPropertySelectorPolicy(IEnumerable<SelectedProperty> selectedProperties)
		{
			this.selectedProperties = selectedProperties;
		}
		IEnumerable<SelectedProperty> IPropertySelectorPolicy.SelectProperties(IBuilderContext context)
		{
			return selectedProperties;
		}
	}
}
