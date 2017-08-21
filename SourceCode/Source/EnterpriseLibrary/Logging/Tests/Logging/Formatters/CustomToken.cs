/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Formatters.Tests
{
    public class CustomToken : TokenFunction
    {
        public CustomToken() : base("[[AcmeDBLookup{", "}]]")
        {
        }
        public override string FormatToken(string tokenTemplate, LogEntry log)
        {
            return "1234";
        }
    }
}
