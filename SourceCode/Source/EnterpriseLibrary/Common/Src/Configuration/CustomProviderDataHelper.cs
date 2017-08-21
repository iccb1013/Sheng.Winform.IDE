/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Collections.Specialized;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
	public class CustomProviderDataHelper<T>
		where T : NameTypeConfigurationElement, IHelperAssistedCustomConfigurationData<T>
	{
		private IHelperAssistedCustomConfigurationData<T> helpedCustomProviderData;
		private NameValueCollection attributes;
		private object lockObject = new object();
		protected internal ConfigurationPropertyCollection propertiesCollection;
		public CustomProviderDataHelper(T helpedCustomProviderData)
		{
			propertiesCollection = new ConfigurationPropertyCollection();
            foreach (ConfigurationProperty propertyInfo in helpedCustomProviderData.Properties)
            {
                propertiesCollection.Add(propertyInfo);
            }
			this.helpedCustomProviderData 
				= helpedCustomProviderData as IHelperAssistedCustomConfigurationData<T>;
		}
		public bool HandleIsModified()
		{
			return UpdatePropertyCollection() || helpedCustomProviderData.BaseIsModified();
		}
		public bool HandleOnDeserializeUnrecognizedAttribute(string name, string value)
		{
			Attributes.Add(name, value);
			return true;
		}
		public void HandleReset(ConfigurationElement parentElement)
		{
			T parentProviders = parentElement as T;
			if (parentProviders != null)
				parentProviders.Helper.UpdatePropertyCollection(); 
			helpedCustomProviderData.BaseReset(parentElement);
		}
		public void HandleSetAttributeValue(string key, string value)
		{
			Attributes.Add(key, value);
			UpdatePropertyCollection();
		}
		public void HandleUnmerge(ConfigurationElement sourceElement, ConfigurationElement parentElement, ConfigurationSaveMode saveMode)
		{
			T parentProviders = parentElement as T;
			if (parentProviders != null)
				parentProviders.Helper.UpdatePropertyCollection(); 
			T sourceProviders = sourceElement as T;
			if (sourceProviders != null)
				sourceProviders.Helper.UpdatePropertyCollection(); 
			helpedCustomProviderData.BaseUnmerge(sourceElement, parentElement, saveMode);
			UpdatePropertyCollection();
		}
		public NameValueCollection Attributes
		{
			get
			{
				CreateAttributes();
				return (NameValueCollection)attributes;
			}
		}
		public ConfigurationPropertyCollection Properties
		{
			get
			{
				UpdatePropertyCollection();
				return propertiesCollection;
			}
		}
		protected internal virtual bool IsKnownPropertyName(string propertyName)
		{
            return ((NameTypeConfigurationElement)helpedCustomProviderData).Properties.Contains(propertyName);
		}
		private void AddAttributesFromConfigurationProperties()
		{
			foreach (ConfigurationProperty property in propertiesCollection)
			{
				if (!IsKnownPropertyName(property.Name))
				{
					attributes.Add(property.Name, (string)helpedCustomProviderData.BaseGetPropertyValue(property));
				}
			}
		}
		private ConfigurationProperty CreateProperty(string propertyName)
		{
			ConfigurationProperty property = new ConfigurationProperty(propertyName, typeof(string), null);
			propertiesCollection.Add(property);
			return property;
		}
		private void CreateAttributes()
		{
			if (attributes == null)
			{
				lock (lockObject)
				{
					if (attributes == null)
					{
						attributes = new NameValueCollection(StringComparer.InvariantCulture);
						AddAttributesFromConfigurationProperties();
					}
				}
			}
		}
		private bool CopyPropertiesToAttributes()
		{
			bool isModified = false;
			foreach (string key in attributes)
			{
				string valueInCollection = attributes[key];
				string valueInBag = GetPropertyValue(key);
				if (valueInBag == null || valueInCollection != valueInBag)
				{
					SetPropertyValue(key, valueInCollection);
					isModified = true;
				}
			}
			return isModified;
		}
		private void CreateRemoveList(List<string> removeList)
		{
			foreach (ConfigurationProperty property in propertiesCollection)
			{
				if (IsKnownPropertyName(property.Name)) continue;
				if (attributes.Get(property.Name) == null)
				{
					removeList.Add(property.Name);
				}
			}
		}
		private ConfigurationProperty GetProperty(string propertyName)
		{
			if (!propertiesCollection.Contains(propertyName)) return null;
			return propertiesCollection[propertyName];
		}
		private string GetPropertyValue(string propertyName)
		{
			ConfigurationProperty property = GetProperty(propertyName);
			if (property != null) return (string)helpedCustomProviderData.BaseGetPropertyValue(property);
			return string.Empty;
		}
		private bool RemoveDeletedConfigurationProperties()
		{
			List<string> removeList = new List<string>();
			CreateRemoveList(removeList);
			foreach (string propertyName in removeList)
			{
				propertiesCollection.Remove(propertyName);
			}
			return removeList.Count > 0;
		}
		private void SetPropertyValue(string propertyName, string value)
		{
			ConfigurationProperty property = GetProperty(propertyName);
			if (property == null)
			{
				property = CreateProperty(propertyName);
			}
			helpedCustomProviderData.BaseSetPropertyValue(property, value);
		}
		private bool UpdatePropertyCollection()
		{
			if (attributes == null) return false;
			bool isModified = RemoveDeletedConfigurationProperties();
			isModified |= CopyPropertiesToAttributes();
			return isModified;
		}
	}
}
