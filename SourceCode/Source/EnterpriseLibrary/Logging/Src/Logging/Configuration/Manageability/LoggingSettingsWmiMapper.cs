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
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability
{
    public static class LoggingSettingsWmiMapper
    {
        public static void GenerateWmiObjects(LoggingSettings configurationObject,
            ICollection<ConfigurationSetting> wmiSettings)
        {
            wmiSettings.Add(
                new LoggingBlockSetting(
                    configurationObject,
                    configurationObject.DefaultCategory,
                    configurationObject.LogWarningWhenNoCategoriesMatch,
                    configurationObject.TracingEnabled,
                    configurationObject.RevertImpersonation));
        }
        public static void GenerateTraceSourceDataWmiObjects(TraceSourceData traceSourceData,
            ICollection<ConfigurationSetting> wmiSettings, string sourceKind)
        {
            string[] referencedTraceListeners = new string[traceSourceData.TraceListeners.Count];
            for (int i = 0; i < traceSourceData.TraceListeners.Count; i++)
            {
                referencedTraceListeners[i]
                    = traceSourceData.TraceListeners.Get(i).Name;
            }
            wmiSettings.Add(
                new TraceSourceSetting(traceSourceData,
                    traceSourceData.Name,
                    traceSourceData.DefaultLevel.ToString(),
                    referencedTraceListeners,
                    sourceKind));
        }
        internal static bool SaveChanges(LoggingBlockSetting setting, ConfigurationElement sourceElement)
        {
            LoggingSettings section = (LoggingSettings)sourceElement;
            section.DefaultCategory = setting.DefaultCategory;
            section.LogWarningWhenNoCategoriesMatch = setting.LogWarningWhenNoCategoriesMatch;
            section.TracingEnabled = setting.TracingEnabled;
            section.RevertImpersonation = setting.RevertImpersonation;
            return true;
        }
        internal static bool SaveChanges(TraceSourceSetting setting, ConfigurationElement sourceElement)
        {
            TraceSourceData element = (TraceSourceData)sourceElement;
            element.DefaultLevel = ParseHelper.ParseEnum<SourceLevels>(setting.DefaultLevel, false);
            element.TraceListeners.Clear();
            foreach (string traceListenerName in setting.TraceListeners)
            {
                element.TraceListeners.Add(new TraceListenerReferenceData(traceListenerName));
            }
            return true;
        }
        internal static void RegisterWmiTypes()
        {
            ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(LoggingBlockSetting),
                typeof(TraceSourceSetting));
        }
    }
}
