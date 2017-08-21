/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
    public interface IManageabilityHelper
    {
        event EventHandler<ConfigurationSettingChangedEventArgs> ConfigurationSettingChanged;
        void UpdateConfigurationManageability(IConfigurationAccessor configurationAccessor);
        void UpdateConfigurationSectionManageability(IConfigurationAccessor configurationAccessor,
                                                     string sectionName);
    }
}
