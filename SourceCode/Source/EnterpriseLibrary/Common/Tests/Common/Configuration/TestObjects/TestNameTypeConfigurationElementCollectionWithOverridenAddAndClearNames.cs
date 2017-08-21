/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.TestObjects
{
	public class TestNameTypeConfigurationElementCollectionWithOverridenAddAndClearNames<T, TCustomElementData>
        : NameTypeConfigurationElementCollection<T, TCustomElementData>
		where T : NameTypeConfigurationElement, new()
        where TCustomElementData : T, new()
	{
		public TestNameTypeConfigurationElementCollectionWithOverridenAddAndClearNames()
		{
			this.AddElementName = "overridenAdd";
			this.ClearElementName = "overridenClear";
		}
	}
}
