/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Formatters
{
	[ConfigurationElementType(typeof(BinaryLogFormatterData))]
	public class BinaryLogFormatter : LogFormatter
	{
		public BinaryLogFormatter()
		{ }
		public override string Format(LogEntry log)
		{
			using (MemoryStream binaryStream = new MemoryStream())
			{
				GetFormatter().Serialize(binaryStream, log);
				return Convert.ToBase64String(binaryStream.ToArray());
			}
		}
		public static LogEntry Deserialize(string serializedLogEntry)
		{
			using (MemoryStream binaryStream = new MemoryStream(Convert.FromBase64String(serializedLogEntry)))
			{
				return (LogEntry)GetFormatter().Deserialize(binaryStream);
			}
		}
		private static BinaryFormatter GetFormatter()
		{
			return new BinaryFormatter();
		}
	}
}
