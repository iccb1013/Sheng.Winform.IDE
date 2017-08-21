/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.Data.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration
{
	public sealed class DatabaseAssemblerAttribute : Attribute
	{
		private Type assemblerType;
		public DatabaseAssemblerAttribute(Type assemblerType)
		{
			if (assemblerType == null)
				throw new ArgumentNullException("assemblerType");
			if (!typeof(IDatabaseAssembler).IsAssignableFrom(assemblerType))
				throw new ArgumentException(string.Format(Resources.Culture, Resources.ExceptionTypeNotDatabaseAssembler, assemblerType), "assemblerType");
			this.assemblerType = assemblerType;
		}
		public Type AssemblerType
		{
			get { return assemblerType; }
		}
	}
}
