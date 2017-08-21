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
    public static class EmailTraceListenerDataWmiMapper
    {
        public static void GenerateWmiObjects(EmailTraceListenerData configurationObject,
                                              ICollection<ConfigurationSetting> wmiSettings)
        {
            wmiSettings.Add(
                new EmailTraceListenerSetting(configurationObject,
                                              configurationObject.Name,
                                              configurationObject.Formatter,
                                              configurationObject.FromAddress,
                                              configurationObject.SmtpPort,
                                              configurationObject.SmtpServer,
                                              configurationObject.SubjectLineEnder,
                                              configurationObject.SubjectLineStarter,
                                              configurationObject.ToAddress,
                                              configurationObject.TraceOutputOptions.ToString(),
											  configurationObject.Filter.ToString()));
        }
        internal static void RegisterWmiTypes()
        {
            ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(EmailTraceListenerSetting));
        }
        internal static bool SaveChanges(EmailTraceListenerSetting setting,
                                         ConfigurationElement sourceElement)
        {
            EmailTraceListenerData element = (EmailTraceListenerData)sourceElement;
            element.Formatter = setting.Formatter;
            element.FromAddress = setting.FromAddress;
            element.SmtpPort = setting.SmtpPort;
            element.SmtpServer = setting.SmtpServer;
            element.SubjectLineEnder = setting.SubjectLineEnder;
            element.SubjectLineStarter = setting.SubjectLineStarter;
            element.ToAddress = setting.ToAddress;
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
