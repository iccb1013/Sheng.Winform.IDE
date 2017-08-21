/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.TraceListeners
{
    public class WmiTraceListenerDataManageabilityProvider
        : TraceListenerDataManageabilityProvider<WmiTraceListenerData>
    {
        public WmiTraceListenerDataManageabilityProvider()
        {
            WmiTraceListenerDataWmiMapper.RegisterWmiTypes();
        }
        protected override void AddElementAdministrativeTemplateParts(AdmContentBuilder contentBuilder,
                                                                      WmiTraceListenerData configurationObject,
                                                                      IConfigurationSource configurationSource,
                                                                      String elementPolicyKeyName)
        {
            AddTraceOptionsPart(contentBuilder, configurationObject.TraceOutputOptions);
			AddFilterPart(contentBuilder, configurationObject.Filter);
		}
        protected override void GenerateWmiObjects(WmiTraceListenerData configurationObject,
                                                   ICollection<ConfigurationSetting> wmiSettings)
        {
            WmiTraceListenerDataWmiMapper.GenerateWmiObjects(configurationObject, wmiSettings);
        }
        protected override void OverrideWithGroupPolicies(WmiTraceListenerData configurationObject,
                                                          IRegistryKey policyKey)
        {
            TraceOptions? traceOutputOptionsOverride = policyKey.GetEnumValue<TraceOptions>(TraceOutputOptionsPropertyName);
			SourceLevels? filterOverride = policyKey.GetEnumValue<SourceLevels>(FilterPropertyName);
            configurationObject.TraceOutputOptions = traceOutputOptionsOverride.Value;
			configurationObject.Filter = filterOverride.Value;
		}
    }
}
