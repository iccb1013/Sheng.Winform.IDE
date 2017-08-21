/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    public interface IProtectedConfigurationSource
    {
        void Add(IConfigurationParameter saveParameter, string sectionName, ConfigurationSection configurationSection, string protectionProviderName);
    }
}
