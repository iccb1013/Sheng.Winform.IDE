/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Text.RegularExpressions;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation
{
    public class AppDomainNameFormatter : IPerformanceCounterNameFormatter
    {
        private string applicationInstanceName;
        private const string InvalidCharacters = @"\()/#*";
        public AppDomainNameFormatter()
        {
        }
        public AppDomainNameFormatter(string applicationInstanceName)
        {
            this.applicationInstanceName = applicationInstanceName;
        }
		public string CreateName(string nameSuffix)
        {
            string replacePattern = "[\\\\()#/\\*]*";
            string appDomainFriendlyName = string.IsNullOrEmpty(this.applicationInstanceName) ? AppDomain.CurrentDomain.FriendlyName : this.applicationInstanceName;
            Regex filter = new Regex(replacePattern);
            appDomainFriendlyName = filter.Replace(appDomainFriendlyName, string.Empty);
			PerformanceCounterInstanceName instanceName = new PerformanceCounterInstanceName(appDomainFriendlyName, nameSuffix);
			return instanceName.ToString();        	
        }
    }
}
