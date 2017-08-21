/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Configuration;
using System.Collections.Specialized;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
	public interface ICustomProviderData
	{
		string Name	{ get; }
		NameValueCollection Attributes { get; }
	}
}
