/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
namespace Microsoft.Practices.EnterpriseLibrary.Tests
{
	[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
	public class DeploymentItemAttribute : Attribute
	{
		public DeploymentItemAttribute(string ignored)
		{ }
	}
}
