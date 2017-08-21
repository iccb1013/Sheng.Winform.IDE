/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.TestObjects
{
	public class BasePolymorphicObjectData : NameTypeConfigurationElement
	{
		public BasePolymorphicObjectData()
			: base()
		{ }
		public BasePolymorphicObjectData(string name, Type type)
			: base(name, type)
		{ }
	}
}
