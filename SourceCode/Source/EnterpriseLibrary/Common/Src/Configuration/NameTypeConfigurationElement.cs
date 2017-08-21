/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
	public class NameTypeConfigurationElement : NamedConfigurationElement, IObjectWithNameAndType
	{
		private AssemblyQualifiedTypeNameConverter typeConverter = new AssemblyQualifiedTypeNameConverter();
		public const string typeProperty = "type";
		public NameTypeConfigurationElement()
		{
		}
		public NameTypeConfigurationElement(string name, Type type)
			: base(name)
		{
			this.Type = type;
		}
		public Type Type
		{
			get { return (Type)typeConverter.ConvertFrom(TypeName); }
			set { TypeName = typeConverter.ConvertToString(value); }
		}
		[ConfigurationProperty(typeProperty, IsRequired = true)]
		public string TypeName
		{
			get { return (string)this[typeProperty]; }
			set { this[typeProperty] = value; }
		}
		internal new ConfigurationPropertyCollection Properties
		{
			get { return base.Properties; }
		}
	}
}
