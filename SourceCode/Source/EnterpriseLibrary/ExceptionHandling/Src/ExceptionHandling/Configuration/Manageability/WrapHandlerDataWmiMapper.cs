/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Manageability
{
    public static class WrapHandlerDataWmiMapper
    {
        public static void GenerateWmiObjects(WrapHandlerData configurationObject,
                                              ICollection<ConfigurationSetting> wmiSettings)
        {
            wmiSettings.Add(
                new WrapHandlerSetting(configurationObject,
                                       configurationObject.Name,
                                       configurationObject.ExceptionMessage,
                                       configurationObject.WrapExceptionType.AssemblyQualifiedName));
        }
        internal static void RegisterWmiTypes()
        {
            ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(WrapHandlerSetting));
        }
        public static bool SaveChanges(WrapHandlerSetting wrapHandlerSettingSetting,
                                       ConfigurationElement sourceElement)
        {
            WrapHandlerData element = (WrapHandlerData)sourceElement;
            element.WrapExceptionTypeName = wrapHandlerSettingSetting.WrapExceptionType;
            element.ExceptionMessage = wrapHandlerSettingSetting.ExceptionMessage;
            return true;
        }
    }
}
