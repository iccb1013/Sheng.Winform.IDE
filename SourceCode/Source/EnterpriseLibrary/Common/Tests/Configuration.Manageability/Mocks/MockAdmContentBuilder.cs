/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests.Mocks
{
    public class MockAdmContentBuilder : AdmContentBuilder
    {
        public MockAdmContentBuilder()
            : base(new MockAdmContent()) {}
        public MockAdmContent GetMockContent()
        {
            return GetContent() as MockAdmContent;
        }
    }
}
