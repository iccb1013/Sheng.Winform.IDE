/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
	public class NameTypeConfigurationElementCollection<T, TCustomElementData> : PolymorphicConfigurationElementCollection<T>
		where T : NameTypeConfigurationElement, new()
        where TCustomElementData : T, new()
	{
		private const string typeAttribute = "type";
		protected override Type RetrieveConfigurationElementType(XmlReader reader)
		{
			Type configurationElementType = null;
			if (reader.AttributeCount > 0)
			{
				for (bool go = reader.MoveToFirstAttribute(); go; go = reader.MoveToNextAttribute())
				{
					if (typeAttribute.Equals(reader.Name))
					{
						Type providerType = Type.GetType(reader.Value, false);
						if (providerType == null)
						{
                            configurationElementType = typeof(TCustomElementData);
                            break;
						}
						Attribute attribute = Attribute.GetCustomAttribute(providerType, typeof(ConfigurationElementTypeAttribute));
						if (attribute == null)
						{
							throw new ConfigurationErrorsException(string.Format(Resources.Culture, Resources.ExceptionNoConfigurationElementAttribute, providerType.Name));
						}
						configurationElementType = ((ConfigurationElementTypeAttribute)attribute).ConfigurationType;
						break;
					}
				}
				if (configurationElementType == null)
				{
					throw new ConfigurationErrorsException(string.Format(Resources.Culture, Resources.ExceptionNoTypeAttribute, reader.Name));
				}
				reader.MoveToElement();
			}
			return configurationElementType;
		}
	}
}
