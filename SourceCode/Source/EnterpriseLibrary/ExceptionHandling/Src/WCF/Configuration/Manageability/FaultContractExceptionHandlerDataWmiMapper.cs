/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration.Manageability
{
	public static class FaultContractExceptionHandlerDataWmiMapper
	{
		public static void GenerateWmiObjects(FaultContractExceptionHandlerData configurationObject,
			ICollection<ConfigurationSetting> wmiSettings)
		{
			string[] attributesArray = GenerateAttributesArray(configurationObject.Attributes);
			wmiSettings.Add(
				new FaultContractExceptionHandlerSetting(configurationObject,
					configurationObject.Name,
					configurationObject.ExceptionMessage,
					configurationObject.FaultContractType,
					attributesArray));
		}
		public static bool SaveChanges(FaultContractExceptionHandlerSetting faultContractExceptionHandlerSetting, ConfigurationElement sourceElement)
		{
			FaultContractExceptionHandlerData element = (FaultContractExceptionHandlerData)sourceElement;
			element.Attributes.Clear();
			element.ExceptionMessage = faultContractExceptionHandlerSetting.ExceptionMessage;
			element.FaultContractType = faultContractExceptionHandlerSetting.FaultContractType;
			foreach (string attribute in faultContractExceptionHandlerSetting.Attributes)
			{
				string[] splittedAttribute = attribute.Split('=');
				element.PropertyMappings.Add(new FaultContractExceptionHandlerMappingData(splittedAttribute[0], splittedAttribute[1]));
			}
			return true;
		}
		private static String[] GenerateAttributesArray(NameValueCollection attributes)
		{
			String[] attributesArray = new String[attributes.Count];
			int i = 0;
			foreach (String key in attributes.AllKeys)
			{
				attributesArray[i++] = KeyValuePairEncoder.EncodeKeyValuePair(key, attributes.Get(key));
			}
			return attributesArray;
		}
		internal static void RegisterWmiTypes()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(FaultContractExceptionHandlerSetting));
		}
	}
}
