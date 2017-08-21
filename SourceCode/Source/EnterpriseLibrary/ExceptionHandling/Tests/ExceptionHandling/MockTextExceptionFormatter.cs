/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests
{
    public class MockTextExceptionFormatter : TextExceptionFormatter
    {
        public NameValueCollection properties = new NameValueCollection();
        public NameValueCollection fields = new NameValueCollection();
        public MockTextExceptionFormatter(TextWriter writer, Exception exception, Guid handlingInstanceId)
            : base(writer, exception, handlingInstanceId)
        {
        }
        protected override void WritePropertyInfo(PropertyInfo propertyInfo, object propertyValue)
        {
            if (propertyValue != null)
            {
                properties[propertyInfo.Name] = propertyValue.ToString();
            }
        }
        protected override void WriteFieldInfo(FieldInfo field, object fieldValue)
        {
            if (fieldValue != null)
            {
                fields[field.Name] = fieldValue.ToString();
            }
        }
    }
}
