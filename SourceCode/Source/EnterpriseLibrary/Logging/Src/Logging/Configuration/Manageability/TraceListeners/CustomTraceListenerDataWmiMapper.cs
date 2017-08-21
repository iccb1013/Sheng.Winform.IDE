/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.TraceListeners
{
    public static class CustomTraceListenerDataWmiMapper
    {
        public static void GenerateWmiObjects(CustomTraceListenerData data,
                                              ICollection<ConfigurationSetting> wmiSettings)
        {
            wmiSettings.Add(
                new CustomTraceListenerSetting(data,
                                               data.Name,
                                               data.Type.AssemblyQualifiedName,
                                               data.InitData,
                                               CustomDataWmiMapperHelper.GenerateAttributesArray(data.Attributes),
                                               data.TraceOutputOptions.ToString(),
											   data.Filter.ToString(),
                                               data.Formatter));
        }
        internal static void RegisterWmiTypes()
        {
            ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(CustomTraceListenerSetting));
        }
        internal static bool SaveChanges(CustomTraceListenerSetting setting,
                                         ConfigurationElement sourceElement)
        {
            BasicCustomTraceListenerData element = (BasicCustomTraceListenerData)sourceElement;
            element.TypeName = setting.ListenerType;
            CustomDataWmiMapperHelper.UpdateAttributes(setting.Attributes, element.Attributes);
            if (element is CustomTraceListenerData)
            {
                ((CustomTraceListenerData)element).Formatter = setting.Formatter;
            }
            element.InitData = setting.InitData;
            element.TraceOutputOptions = ParseHelper.ParseEnum<TraceOptions>(setting.TraceOutputOptions, false);
            SourceLevels filter;
            if(ParseHelper.TryParseEnum(setting.Filter, out filter))
            {
                element.Filter = filter;
            }
            return true;
        }
    }
}
