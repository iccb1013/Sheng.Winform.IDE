/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.ComponentModel;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
	public class AssemblyQualifiedTypeNameConverter : ConfigurationConverterBase
	{
		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
		{
            if (value != null)
            {
                Type typeValue = value as Type;
                if (typeValue == null)
                {
                    throw new ArgumentException(string.Format(Resources.Culture, Resources.ExceptionCanNotConvertType, typeof(Type).Name));
                }
                if (typeValue != null) return (typeValue).AssemblyQualifiedName;
            }
            return null;
		}
		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
            string stringValue = (string)value;
            if (!string.IsNullOrEmpty(stringValue))
            {
                Type result = Type.GetType(stringValue, false);
                if (result == null)
                {
                    throw new ArgumentException(string.Format(Resources.Culture, Resources.ExceptionInvalidType, stringValue));
                }
                return result;
            }
            return null;
		}		
	}
}
