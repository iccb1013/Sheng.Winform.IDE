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
    public static class CustomExceptionHandlerDataWmiMapper
    {
        public static void GenerateWmiObjects(CustomHandlerData data,
                                              ICollection<ConfigurationSetting> wmiSettings)
        {
            wmiSettings.Add(
                new CustomHandlerSetting(data,
                                         data.Name,
                                         data.Type.AssemblyQualifiedName,
                                         CustomDataWmiMapperHelper.GenerateAttributesArray(data.Attributes)));
        }
        internal static void RegisterWmiTypes()
        {
            ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(CustomHandlerSetting));
        }
        internal static bool SaveChanges(CustomHandlerSetting customHandlerSetting,
                                         ConfigurationElement sourceElement)
        {
            CustomHandlerData element = (CustomHandlerData)sourceElement;
            element.Attributes.Clear();
            foreach (string attribute in customHandlerSetting.Attributes)
            {
                string[] splittedAttribute = attribute.Split('=');
                element.Attributes.Add(splittedAttribute[0], splittedAttribute[1]);
            }
            return true;
        }
    }
}
