/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests.Mocks
{
	public class MockCustomProviderData : NameTypeConfigurationElement, ICustomProviderData
	{
		public NameValueCollection attributesCollection;
		public MockCustomProviderData()
		{
			this.attributesCollection = new NameValueCollection();
		}
		public NameValueCollection Attributes
		{
			get { return attributesCollection; }
		}
	}
}
