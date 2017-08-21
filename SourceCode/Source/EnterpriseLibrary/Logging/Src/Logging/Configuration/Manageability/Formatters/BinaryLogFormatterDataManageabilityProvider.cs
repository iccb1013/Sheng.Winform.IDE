/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Formatters
{
    public class BinaryLogFormatterDataManageabilityProvider
        : ConfigurationElementManageabilityProviderBase<BinaryLogFormatterData>
    {
        public BinaryLogFormatterDataManageabilityProvider()
        {
            BinaryLogFormatterDataWmiMapper.RegisterWmiTypes();
        }
        protected override string ElementPolicyNameTemplate
        {
            get { return Resources.FormatterPolicyNameTemplate; }
        }
        protected override void AddElementAdministrativeTemplateParts(AdmContentBuilder contentBuilder,
                                                                      BinaryLogFormatterData configurationObject,
                                                                      IConfigurationSource configurationSource,
                                                                      String elementPolicyKeyName)
        {
        }
        protected override void GenerateWmiObjects(BinaryLogFormatterData configurationObject,
                                                   ICollection<ConfigurationSetting> wmiSettings)
        {
            BinaryLogFormatterDataWmiMapper.GenerateWmiObjects(configurationObject, wmiSettings);
        }
        protected override void OverrideWithGroupPolicies(BinaryLogFormatterData configurationObject,
                                                          IRegistryKey policyKey)
        {
        }
    }
}
