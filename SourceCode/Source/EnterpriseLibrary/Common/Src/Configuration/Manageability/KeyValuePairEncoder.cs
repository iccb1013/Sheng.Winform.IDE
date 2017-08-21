/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Text;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
	public class KeyValuePairEncoder
	{
		private StringBuilder builder;
		public KeyValuePairEncoder()
		{
			this.builder = new StringBuilder();
		}
		public void AppendKeyValuePair(String key, String value)
		{
			builder.Append(EncodeKeyValuePair(key, value, true));
			builder.Append(';');
		}
		public String GetEncodedKeyValuePairs()
		{
			return builder.ToString();
		}
		public static String EncodeKeyValuePair(String key, String value)
		{
			return EncodeKeyValuePair(key, value, false);
		}
		public static String EncodeKeyValuePair(String key, String value, bool escapeSemicolons)
		{
			return key
				+ "="
				+ (escapeSemicolons ? value.Replace(";", ";;") : value);
		}
	}
}
