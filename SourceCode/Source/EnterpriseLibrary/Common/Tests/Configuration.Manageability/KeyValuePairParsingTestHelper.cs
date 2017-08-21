/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests
{
	public static class KeyValuePairParsingTestHelper
	{
		public static void ExtractKeyValueEntries(String attributes, IDictionary<String, String> attributesDictionary)
		{
			KeyValuePairParser.ExtractKeyValueEntries(attributes, attributesDictionary);
		}
		public static String EncodeKeyValueEntry(String key, String value)
		{
			return KeyValuePairEncoder.EncodeKeyValuePair(key, value);
		}
	}
}
