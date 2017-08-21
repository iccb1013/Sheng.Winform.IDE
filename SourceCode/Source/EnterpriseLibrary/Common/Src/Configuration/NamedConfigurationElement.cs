/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Configuration;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
	public class NamedConfigurationElement : ConfigurationElement, IObjectWithName
	{
		public const string nameProperty = "name";
		public NamedConfigurationElement()
		{ }
		public NamedConfigurationElement(string name)
		{
			Name = name;
		}
		[ConfigurationProperty(nameProperty, IsKey = true, DefaultValue = "Name", IsRequired= true)]				
		[StringValidator(MinLength=1)]
		public string Name
		{
			get { return (string)this[nameProperty]; }
			set { this[nameProperty] = value; }
		}
		public void DeserializeElement(XmlReader reader)
		{
			base.DeserializeElement(reader, false);
		}
	}
}
