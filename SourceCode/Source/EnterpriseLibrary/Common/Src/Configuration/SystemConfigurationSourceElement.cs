/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    public class SystemConfigurationSourceElement : ConfigurationSourceElement
    {
		public SystemConfigurationSourceElement()
			: this(Resources.SystemConfigurationSourceName)
        {
        }
        public SystemConfigurationSourceElement(string name)
            : base(name, typeof(SystemConfigurationSource))
		{				
        }
		public override IConfigurationSource CreateSource()
		{
			IConfigurationSource createdObject = new SystemConfigurationSource();
			return createdObject;
		}
	}
}
