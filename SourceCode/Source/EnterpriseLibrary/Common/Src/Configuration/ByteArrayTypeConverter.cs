/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.ComponentModel;
using System.Globalization;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
	public class ByteArrayTypeConverter : TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return typeof(string) == sourceType;
		}
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			return Convert.FromBase64String((string) value);
		}
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return typeof(byte[]) == destinationType;
		}
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			return Convert.ToBase64String((byte[])value);
		}
	}
}
