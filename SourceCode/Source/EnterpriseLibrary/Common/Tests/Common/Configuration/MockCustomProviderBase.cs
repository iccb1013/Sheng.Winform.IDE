/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Specialized;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration
{
	public abstract class MockCustomProviderBase
	{
		public const string AttributeKey = "attributeKey";
		public string customValue;
		public MockCustomProviderBase(NameValueCollection attributes)
		{
			this.customValue = attributes[AttributeKey];
		}	
	}
}
