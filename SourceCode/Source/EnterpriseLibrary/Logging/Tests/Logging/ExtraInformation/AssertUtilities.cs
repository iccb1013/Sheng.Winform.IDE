/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation.Tests
{
    static class AssertUtilities
    {
        internal static void AssertStringDoesNotContain(string o,
                                                        string s,
                                                        string message)
        {
            Assert.IsNotNull(o);
            Assert.IsNotNull(s);
            Assert.IsNotNull(message);
            Assert.IsFalse(o.StartsWith(s), string.Format("\nIn {2}, the string:\n\t{0}\ncontains\n\t{1}\nwhen it should not.\n", o, s, message));
        }
    }
}
