/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    public class ConfigurationSourceElement : NameTypeConfigurationElement
    {
        public ConfigurationSourceElement() 
        {
        }
        public ConfigurationSourceElement(string name, Type type)
            : base(name, type)
		{
		}
		public virtual IConfigurationSource CreateSource()
		{
			throw new ConfigurationErrorsException(Resources.ExceptionBaseConfigurationSourceElementIsInvalid);
		}
	}
}
