/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Configuration;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Text;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
	public class SerializableConfigurationSection : ConfigurationSection, IXmlSerializable
	{
		public XmlSchema GetSchema()
		{
			return null;
		}
		public void ReadXml(XmlReader reader)
		{
			reader.Read();
			DeserializeSection(reader);
		}
		public void WriteXml(XmlWriter writer)
		{
			String serialized = SerializeSection(this, "SerializableConfigurationSection", ConfigurationSaveMode.Full);
			writer.WriteRaw(serialized);
		}
	}
}
