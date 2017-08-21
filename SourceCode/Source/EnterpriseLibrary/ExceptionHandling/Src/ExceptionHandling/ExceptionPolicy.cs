/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
{
    public static class ExceptionPolicy
    {
        private static readonly ExceptionPolicyFactory defaultFactory = new ExceptionPolicyFactory();
        public static bool HandleException(Exception exceptionToHandle, string policyName)
        {
            if (exceptionToHandle == null) throw new ArgumentNullException("exceptionToHandle");
            if (string.IsNullOrEmpty(policyName)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty);
            return HandleException(exceptionToHandle, policyName, defaultFactory);
        }
        private static bool HandleException(Exception exceptionToHandle, string policyName, ExceptionPolicyFactory policyFactory)
        {
            ExceptionPolicyImpl policy = GetExceptionPolicy(exceptionToHandle, policyName, policyFactory);
            return policy.HandleException(exceptionToHandle);
        }
		public static bool HandleException(Exception exceptionToHandle, string policyName, out Exception exceptionToThrow)
        {
            if (exceptionToHandle == null) throw new ArgumentNullException("exceptionToHandle");
            if (string.IsNullOrEmpty(policyName)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty);
            return HandleException(exceptionToHandle, policyName, defaultFactory, out exceptionToThrow);
        }
        private static bool HandleException(Exception exceptionToHandle, string policyName, ExceptionPolicyFactory policyFactory, out Exception exceptionToThrow)
        {
            try
            {
                bool retrowAdviced = HandleException(exceptionToHandle, policyName, policyFactory);
                exceptionToThrow = null;
                return retrowAdviced;
            }
            catch (Exception exception)
            {
                exceptionToThrow = exception;
                return true;
            }
        }
        private static ExceptionPolicyImpl GetExceptionPolicy(Exception exception, string policyName, ExceptionPolicyFactory factory)
        {
            try
            {
                return factory.Create(policyName);
            }
            catch (ConfigurationErrorsException configurationException)
            {
                try
                {
                    DefaultExceptionHandlingEventLogger logger = EnterpriseLibraryFactory.BuildUp<DefaultExceptionHandlingEventLogger>();
                    logger.LogConfigurationError(configurationException, policyName);
                }
                catch { }
                throw;
            }
            catch (Exception ex)
            {
                try
                {
                    string exceptionMessage = ExceptionUtility.FormatExceptionHandlingExceptionMessage(policyName, ex, null, exception);
                    DefaultExceptionHandlingEventLogger logger = EnterpriseLibraryFactory.BuildUp<DefaultExceptionHandlingEventLogger>();
                    logger.LogInternalError(policyName, exceptionMessage);
                }
                catch { }
                throw new ExceptionHandlingException(ex.Message, ex);
            }
        }
    }
}
