/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation.Helpers;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation
{
    public class ComPlusInformationProvider : IExtraInformationProvider
    {
        public ComPlusInformationProvider()
        {
            contextUtils = new ContextUtils();
        }
        internal ComPlusInformationProvider(IContextUtils contextUtils)
        {
            this.contextUtils = contextUtils;
        }
        public void PopulateDictionary(IDictionary<string, object> dict)
        {
            dict.Add(Properties.Resources.ComPlusInfo_ActivityId, ActivityId);
            dict.Add(Properties.Resources.ComPlusInfo_ApplicationId, ApplicationId);
            dict.Add(Properties.Resources.ComPlusInfo_TransactionID, TransactionId);
            dict.Add(Properties.Resources.ComPlusInfo_DirectCallerAccountName, DirectCallerAccountName);
            dict.Add(Properties.Resources.ComPlusInfo_OriginalCallerAccountName, OriginalCallerAccountName);
        }
        public string OriginalCallerAccountName
        {
            get { return GetSafeProperty(new ContextAccessorDelegate(contextUtils.GetOriginalCallerAccountName)); }
        }
        public string DirectCallerAccountName
        {
            get { return GetSafeProperty(new ContextAccessorDelegate(contextUtils.GetDirectCallerAccountName)); }
        }
        public string TransactionId
        {
            get { return GetSafeProperty(new ContextAccessorDelegate(contextUtils.GetTransactionId)); }
        }
        public string ApplicationId
        {
            get { return GetSafeProperty(new ContextAccessorDelegate(contextUtils.GetApplicationId)); }
        }
        public string ActivityId
        {
            get { return GetSafeProperty(new ContextAccessorDelegate(contextUtils.GetActivityId)); }
        }
        private string GetSafeProperty(ContextAccessorDelegate accessorDelegate)
        {
            string result;
            try
            {
                result = accessorDelegate();
            }
            catch (Exception e)
            {
				result = String.Format(Properties.Resources.Culture, Properties.Resources.ExtendedPropertyError, e.Message);
            }            
            return result;
        }
        private delegate string ContextAccessorDelegate();
        private IContextUtils contextUtils;
    }
}
