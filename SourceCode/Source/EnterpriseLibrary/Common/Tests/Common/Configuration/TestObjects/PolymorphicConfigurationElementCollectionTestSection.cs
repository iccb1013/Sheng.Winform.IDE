/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.TestObjects
{
	public class PolymorphicConfigurationElementCollectionTestSection : ConfigurationSection
	{
		[ConfigurationProperty("withOverrides")]
		public TestNameTypeConfigurationElementCollectionWithOverridenAddAndClearNames<BasePolymorphicObjectData, CustomPolymorphicObjectData> WithOverrides
		{
            get { return (TestNameTypeConfigurationElementCollectionWithOverridenAddAndClearNames<BasePolymorphicObjectData, CustomPolymorphicObjectData>)this["withOverrides"]; }
			set { this["withOverrides"] = value; }
		}
		[ConfigurationProperty("withoutOverrides")]
        public NameTypeConfigurationElementCollection<BasePolymorphicObjectData, CustomPolymorphicObjectData> WithoutOverrides
		{
            get { return (NameTypeConfigurationElementCollection<BasePolymorphicObjectData, CustomPolymorphicObjectData>)this["withoutOverrides"]; }
			set { this["withoutOverrides"] = value; }
		}
	}
}
