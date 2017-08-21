/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Formatters
{
    public class XmlLogFormatter : LogFormatter
    {
        private const string DefaultValue = "";
        public override string Format(LogEntry log)
        {
            StringBuilder result = new StringBuilder();
            using (XmlWriter writer = new XmlTextWriter(new StringWriter(result)))
            {
                Format(log, writer);
            }
            return result.ToString();
        }
        private void Format(object obj, XmlWriter writer)
        {
            if (obj == null) return;
            writer.WriteStartElement(CreateRootName(obj));
            if (Type.GetTypeCode(obj.GetType()) == TypeCode.Object)
            {
                foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
                {
                    if (!propertyInfo.CanRead || propertyInfo.GetIndexParameters().Length > 0)
                        continue;
                    writer.WriteStartElement(propertyInfo.Name);
                    if (typeof(IEnumerable).IsAssignableFrom(propertyInfo.PropertyType) && Type.GetTypeCode(propertyInfo.PropertyType) == TypeCode.Object)
                    {
                        IEnumerable values = (IEnumerable)propertyInfo.GetValue(obj, null);
                        if (values != null)
                        {
                            foreach (object value in values)
                            {
                                Format(value, writer);
                            }
                        }
                    }
                    else
                    {
                        writer.WriteString(ConvertToString(propertyInfo, obj));
                    }
                    writer.WriteEndElement();
                }
            }
            else
            {
                writer.WriteString(obj.ToString());
            }
            writer.WriteEndElement();
        }
        private string CreateRootName(object obj)
        {
            string name = obj.GetType().Name;
            name = name.Replace('`', '_');
            name = name.Replace('[', '_');
            name = name.Replace(']', '_');
            name = name.Replace(',', '_');
            return name;
        }
        private string ConvertToString(PropertyInfo propertyInfo, object obj)
        {
            object value = propertyInfo.GetValue(obj, null);
            return value != null ? value.ToString() : DefaultValue;
        }
    }
}
