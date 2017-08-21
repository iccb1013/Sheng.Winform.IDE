/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Tests
{
    [ExceptionShielding()]
    class MockServiceWithShielding : IMockService
    {
        public void Test()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
