/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
{
    public class ExceptionHandlingConfigurationView
    {
        IConfigurationSource configurationSource;
        public ExceptionHandlingConfigurationView(IConfigurationSource configurationSource)
        {
            if (configurationSource == null) throw new ArgumentNullException("configurationSource");
            this.configurationSource = configurationSource;
        }
        public ExceptionHandlingSettings ExceptionHandlingSettings
        {
            get { return ExceptionHandlingSettings.GetExceptionHandlingSettings(configurationSource); }
        }
        public ExceptionPolicyData GetExceptionPolicyData(string policyName)
        {
            if (string.IsNullOrEmpty(policyName)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty);
            ExceptionHandlingSettings settings = ExceptionHandlingSettings;
            if (settings.ExceptionPolicies.Contains(policyName))
            {
                return settings.ExceptionPolicies.Get(policyName);
            }
            throw new ConfigurationErrorsException(string.Format(Resources.Culture, Resources.ExceptionSimpleProviderNotFound, policyName));
        }
    }
}
