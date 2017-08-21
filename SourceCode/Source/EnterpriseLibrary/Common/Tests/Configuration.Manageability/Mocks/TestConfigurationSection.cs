/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests.Mocks
{
	public class TestsConfigurationSection : SerializableConfigurationSection
	{
		private const String valuePropertyName = "value";
		public TestsConfigurationSection()
			: base()
		{ }
		public TestsConfigurationSection(String value)
			: base()
		{
			this.Value = value;
		}
		public override string ToString()
		{
			return this.Value.ToString();
		}
		[ConfigurationProperty(valuePropertyName)]
		public String Value
		{
			get { return (String)base[valuePropertyName]; }
			set { base[valuePropertyName] = value; }
		}
	}
}
