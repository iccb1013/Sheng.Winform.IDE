/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Configuration;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
	public class TraceListenerDataCollection : PolymorphicConfigurationElementCollection<TraceListenerData>
	{
		protected override Type RetrieveConfigurationElementType(XmlReader reader)
		{
			Type configurationElementType = null;
			if (reader.AttributeCount > 0)
			{
				for (bool go = reader.MoveToFirstAttribute(); go; go = reader.MoveToNextAttribute())
				{
					if (TraceListenerData.listenerDataTypeProperty.Equals(reader.Name))
					{
						configurationElementType = Type.GetType(reader.Value);
						if (configurationElementType == null)
						{
							throw new ConfigurationErrorsException(
								string.Format(
									Resources.Culture,
									Resources.ExceptionTraceListenerConfigurationElementTypeNotFound,
									reader.ReadOuterXml()));
						}
						break;
					}
				}
				if (configurationElementType == null)
				{
					throw new ConfigurationErrorsException(
						string.Format(
							Resources.Culture,
							Resources.ExceptionTraceListenerConfigurationElementMissingTypeAttribute,
							reader.ReadOuterXml()));
				}
				reader.MoveToElement();
			}
			return configurationElementType;
		}
	}
}
