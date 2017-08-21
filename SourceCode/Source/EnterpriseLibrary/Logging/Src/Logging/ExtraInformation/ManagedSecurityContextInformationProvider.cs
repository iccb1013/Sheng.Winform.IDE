/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Threading;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation
{
    public class ManagedSecurityContextInformationProvider : IExtraInformationProvider
    {
        public void PopulateDictionary(IDictionary<string, object> dict)
        {
            dict.Add(Properties.Resources.ManagedSecurity_AuthenticationType, AuthenticationType);
            dict.Add(Properties.Resources.ManagedSecurity_IdentityName, IdentityName);
            dict.Add(Properties.Resources.ManagedSecurity_IsAuthenticated, IsAuthenticated.ToString());
        }
        public string AuthenticationType
        {
            get { return Thread.CurrentPrincipal.Identity.AuthenticationType; }
        }
        public string IdentityName
        {
            get { return Thread.CurrentPrincipal.Identity.Name; }
        }
        public bool IsAuthenticated
        {
            get { return Thread.CurrentPrincipal.Identity.IsAuthenticated; }
        }
    }
}
