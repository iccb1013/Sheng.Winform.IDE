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
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=false)]
	public sealed class AssemblerAttribute : Attribute
	{
		private Type assemblerType;
		public AssemblerAttribute(Type assemblerType)
		{
			if (assemblerType == null)
				throw new ArgumentNullException("assemblerType");
			this.assemblerType = assemblerType;
		}
		public Type AssemblerType
		{
			get { return assemblerType; }
		}
	}
}
