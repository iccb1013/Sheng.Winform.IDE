/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
	public sealed class CustomFactoryAttribute : Attribute
	{
		private Type factoryType;
		public CustomFactoryAttribute(Type factoryType)
		{
			if (factoryType == null)
				throw new ArgumentNullException("factoryType");
			if (!typeof(ICustomFactory).IsAssignableFrom(factoryType))
				throw new ArgumentException(string.Format(Resources.Culture, Resources.ExceptionTypeNotCustomFactory, factoryType), "factoryType");
			this.factoryType = factoryType;
		}
		public Type FactoryType
		{
			get { return factoryType; }
		}
	}
}
