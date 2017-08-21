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
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.TraceListeners
{
    public class XmlTraceListenerDataManageabilityProvider
        : TraceListenerDataManageabilityProvider<XmlTraceListenerData>
    {
        public const String FileNamePropertyName = "fileName";
        public XmlTraceListenerDataManageabilityProvider()
        {
            XmlTraceListenerDataWmiMapper.RegisterWmiTypes();
        }
        protected override void AddElementAdministrativeTemplateParts(AdmContentBuilder contentBuilder,
                                                                      XmlTraceListenerData configurationObject,
                                                                      IConfigurationSource configurationSource,
                                                                      String elementPolicyKeyName)
        {
            contentBuilder.AddEditTextPart(Resources.XmlTraceListenerFileNamePartName,
                                           FileNamePropertyName,
                                           configurationObject.FileName,
                                           255,
                                           true);
            AddTraceOptionsPart(contentBuilder, configurationObject.TraceOutputOptions);
			AddFilterPart(contentBuilder, configurationObject.Filter);
		}
        protected override void GenerateWmiObjects(XmlTraceListenerData configurationObject,
                                                   ICollection<ConfigurationSetting> wmiSettings)
        {
            XmlTraceListenerDataWmiMapper.GenerateWmiObjects(configurationObject, wmiSettings);
        }
        protected override void OverrideWithGroupPolicies(XmlTraceListenerData configurationObject,
                                                          IRegistryKey policyKey)
        {
            String fileNameOverride = policyKey.GetStringValue(FileNamePropertyName);
            TraceOptions? traceOutputOptionsOverride = policyKey.GetEnumValue<TraceOptions>(TraceOutputOptionsPropertyName);
			SourceLevels? filterOverride = policyKey.GetEnumValue<SourceLevels>(FilterPropertyName);
            configurationObject.FileName = fileNameOverride;
            configurationObject.TraceOutputOptions = traceOutputOptionsOverride.Value;
			configurationObject.Filter = filterOverride.Value;
		}
    }
}
