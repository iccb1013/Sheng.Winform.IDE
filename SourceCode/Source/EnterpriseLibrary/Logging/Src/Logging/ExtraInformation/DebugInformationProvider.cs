/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security;
using Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation.Helpers;
using Microsoft.Practices.EnterpriseLibrary.Logging.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation
{
    public class DebugInformationProvider : IExtraInformationProvider
    {
        IDebugUtils debugUtils;
        public DebugInformationProvider()
            : this(new DebugUtils()) {}
        public DebugInformationProvider(IDebugUtils debugUtils)
        {
            this.debugUtils = debugUtils;
        }
        public void PopulateDictionary(IDictionary<string, object> dict)
        {
            string value;
            try
            {
                value = debugUtils.GetStackTraceWithSourceInfo(new StackTrace(true));
            }
            catch (SecurityException)
            {
                value = String.Format(Resources.Culture, Resources.ExtendedPropertyError, Resources.DebugInfo_StackTraceSecurityException);
            }
            catch
            {
                value = String.Format(Resources.Culture, Resources.ExtendedPropertyError, Resources.DebugInfo_StackTraceException);
            }
            dict.Add(Resources.DebugInfo_StackTrace, value);
        }
    }
}
