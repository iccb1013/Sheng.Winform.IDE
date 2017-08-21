/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF
{
    public class ExceptionShielding
    {
        public const string DefaultExceptionPolicy = "WCF Exception Shielding";
        public const string FaultAction = "http://www.microsoft.com/practices/servicefactory/2006/01/wcf/exceptionShielding/fault";
        public const string HandlingInstanceIdPropertyMappingName = "Guid";
    }
}
