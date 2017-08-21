/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
	public abstract class PolymorphicConfigurationElementCollection<T> : NamedElementCollection<T>
		where T : NamedConfigurationElement, new()
	{
		private Dictionary<string, Type> configurationElementTypeMapping;
		private T currentElement;
        protected override void Reset(ConfigurationElement parentElement)
        {
            CreateTypesMap((PolymorphicConfigurationElementCollection<T>)parentElement);
            base.Reset(parentElement);
            ReleaseTypesMap();
        }
		protected override bool OnDeserializeUnrecognizedElement(string elementName, XmlReader reader)
		{
			if (this.AddElementName.Equals(elementName))
			{
				Type configurationElementType = RetrieveConfigurationElementType(reader);
				currentElement = (T)Activator.CreateInstance(configurationElementType);
				currentElement.DeserializeElement(reader);
				base.Add(currentElement);
				return true;
			}
			return base.OnDeserializeUnrecognizedElement(elementName, reader);
		}
		protected abstract Type RetrieveConfigurationElementType(XmlReader reader);			
		protected override ConfigurationElement CreateNewElement()
		{
			if (currentElement != null)
			{
				return currentElement;
			}
			else
			{
				return new T();
			}
		}
		protected override ConfigurationElement CreateNewElement(string elementName)
		{
			if (configurationElementTypeMapping != null)
			{
				Type configurationElementType = configurationElementTypeMapping[elementName];
				if (configurationElementType != null)
				{
					return Activator.CreateInstance(configurationElementType) as ConfigurationElement;
				}
			}
			return base.CreateNewElement(elementName);
		}
		protected override void Unmerge(ConfigurationElement sourceElement, ConfigurationElement parentElement, ConfigurationSaveMode saveMode)
		{
		    CreateTypesMap((PolymorphicConfigurationElementCollection<T>)sourceElement);
		    base.Unmerge(sourceElement, parentElement, saveMode);
		    ReleaseTypesMap();
		}
		private void CreateTypesMap(PolymorphicConfigurationElementCollection<T> sourceCollection)
		{
		    configurationElementTypeMapping = new Dictionary<string, Type>(sourceCollection.Count);
		    foreach (T configurationElementSettings in sourceCollection)
		    {
		        configurationElementTypeMapping.Add(configurationElementSettings.Name, configurationElementSettings.GetType());
		    }
		}
		private void ReleaseTypesMap()
		{
		    configurationElementTypeMapping = null;
		}
	}
}
