/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Specialized;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
	public static class CustomDataWmiMapperHelper
	{
		public static String[] GenerateAttributesArray(NameValueCollection attributes)
		{
			String[] attributesArray = new String[attributes.Count];
			int i = 0;
			foreach (String key in attributes.AllKeys)
			{
				attributesArray[i++] = KeyValuePairEncoder.EncodeKeyValuePair(key, attributes.Get(key));
			}
			return attributesArray;
		}
		public static NameValueCollection GenerateAttributesCollection(string[] attributes)
		{
			NameValueCollection attributeCollection = new NameValueCollection(attributes.Length);
			foreach (string attribute in attributes)
			{
				string key;
				string value;
				KeyValuePairParser.DecodeKeyValuePair(attribute, out key, out value);
				attributeCollection.Add(key, value);
			}
			return attributeCollection;
		}
		public static void UpdateAttributes(string[] encodedAttributes, NameValueCollection attributes)
		{
			attributes.Clear();
			NameValueCollection generatedAttributes = GenerateAttributesCollection(encodedAttributes);
			foreach (string key in generatedAttributes.Keys)
			{
				attributes.Add(key, generatedAttributes[key]);
			}
		}
	}
}
