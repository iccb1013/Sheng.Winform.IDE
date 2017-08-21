/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
	public static class KeyValuePairParser
	{
		private static Regex KeyValueEntryRegex
			= new Regex(@"
						(?<name>[^;=]+)= 		# match the value name - anything but ; or =, followed by =
						(?<value>.*?)			# followed by anything as the value, but non greedy
						(						# until either
							$					# the string ends
								|				# or
							(?<!;);$			# the string ends after a non escaped ;
								|				# or
							(?<!;);(?!;)) 		# a ; that is not before or after another ; (;;) is the escaped ;"
						, RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace | RegexOptions.ExplicitCapture);
		public static void ExtractKeyValueEntries(String attributes, IDictionary<String, String> attributesDictionary)
		{
			foreach (Match match in KeyValueEntryRegex.Matches(attributes))
			{
				attributesDictionary.Add(match.Groups["name"].Value.Trim(), match.Groups["value"].Value.Replace(";;", ";"));
			}
		}
		public static void DecodeKeyValuePair(string keyValue, out string key, out string value)
		{
			key = string.Empty;
			value = string.Empty;
			if (keyValue != null)
			{
				Match match = KeyValueEntryRegex.Match(keyValue);
				if (match.Success)
				{
					key = match.Groups["name"].Value.Trim();
					value = match.Groups["value"].Value.Replace(";;", ";");
				}
				else
				{
					key = keyValue;
				}
			}
		}
	}
}
