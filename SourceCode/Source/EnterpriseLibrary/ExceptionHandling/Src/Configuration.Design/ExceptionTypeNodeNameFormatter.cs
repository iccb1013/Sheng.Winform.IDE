/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design
{
    public class ExceptionTypeNodeNameFormatter : TypeNodeNameFormatter
    {
        public string CreateName(ExceptionTypeData exceptionTypeConfiguration)
        {
            if (exceptionTypeConfiguration == null) 
                throw new ArgumentNullException("exceptionTypeConfiguration");
            return CreateName(exceptionTypeConfiguration.TypeName);
        }
    }
}
