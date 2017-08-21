/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using System.Reflection;
using System.ComponentModel;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF
{
	[ConfigurationElementType(typeof(FaultContractExceptionHandlerData))]
	public class FaultContractExceptionHandler : IExceptionHandler
	{
		private NameValueCollection attributes;
		private Type faultContractType;
		private string exceptionMessage;
		public FaultContractExceptionHandler(Type faultContractType, NameValueCollection attributes)
			: this(faultContractType, null, attributes)
		{
		}
		public FaultContractExceptionHandler(Type faultContractType, string exceptionMessage, NameValueCollection attributes)
		{
			if (faultContractType == null)
			{
				throw new ArgumentNullException("faultContractType");
			}
			this.faultContractType = faultContractType;
			this.attributes = attributes;
			this.exceptionMessage = exceptionMessage;
		}
		public Exception HandleException(Exception exception, Guid handlingInstanceId)
		{
			EnsureDefaultConstructor();
			object fault = Activator.CreateInstance(faultContractType);
			PopulateFaultContractFromException(fault, exception, handlingInstanceId);
			return new FaultContractWrapperException(
				fault, handlingInstanceId, ExceptionUtility.GetMessage(exception, exceptionMessage, handlingInstanceId));
		}
		private void EnsureDefaultConstructor()
		{
			if (faultContractType.GetConstructor(Type.EmptyTypes) == null)
			{
				throw new MissingMethodException(
					string.Format(CultureInfo.CurrentCulture, Properties.Resources.NoDefaultParameterInFaultContract, faultContractType.FullName));
			}
		}
		private void PopulateFaultContractFromException(object fault, Exception exception, Guid handlingInstanceId)
		{
			if (this.attributes != null)
			{
				foreach (PropertyInfo property in this.faultContractType.GetProperties())
				{
					if (PropertyIsMappedInAttributes(property))
					{
						string configProperty = this.attributes[property.Name];
						if (!string.IsNullOrEmpty(configProperty))
						{
							configProperty = configProperty.Replace("{", "").Replace("}", "");
							if (PropertyIsGuid(property, configProperty))
							{
								property.SetValue(fault, handlingInstanceId, null);
							}
							else
							{
								PropertyInfo mappedExceptionProperty = GetMappedProperty(exception, configProperty);
								if (PropertiesMatch(property, mappedExceptionProperty))
								{
									property.SetValue(fault,
										mappedExceptionProperty.GetValue(exception, null),
										null);
								}
							}
						}
					}
					else
					{
						if (PropertyNamesMatch(property, exception))
						{
							property.SetValue(fault,
								GetExceptionProperty(property, exception).GetValue(exception, null),
								null);
						}
					}
				}
			}
		}
		private bool PropertyIsGuid(PropertyInfo property, string configPropertyName)
		{
			return
				configPropertyName.Equals(
					ExceptionShielding.HandlingInstanceIdPropertyMappingName,
					StringComparison.InvariantCultureIgnoreCase)
				&& property.PropertyType == typeof(Guid);
		}
		private bool PropertiesMatch(PropertyInfo faultProperty, PropertyInfo exceptionProperty)
		{
			return exceptionProperty != null
				&& exceptionProperty.PropertyType == faultProperty.PropertyType
				&& faultProperty.CanWrite
				&& faultProperty.GetIndexParameters().Length == 0
				&& exceptionProperty.CanRead
				&& exceptionProperty.GetIndexParameters().Length == 0;
		}
		private bool PropertyNamesMatch(PropertyInfo faultProperty, Exception exception)
		{
			PropertyInfo exceptionProperty = GetExceptionProperty(faultProperty, exception);
			return PropertiesMatch(faultProperty, exceptionProperty);
		}
		private bool PropertyIsMappedInAttributes(PropertyInfo property)
		{
			return Array.Exists(this.attributes.AllKeys,
				delegate(string s) { return s == property.Name; });
		}
		private PropertyInfo GetMappedProperty(Exception exception, string configProperty)
		{
			return exception.GetType().GetProperty(configProperty);
		}
		private PropertyInfo GetExceptionProperty(PropertyInfo property, Exception exception)
		{
			return exception.GetType().GetProperty(property.Name);
		}
	}
}
