/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
{
    public static class ExceptionUtility
    {
        private const string HandlingInstanceToken = "{handlingInstanceID}";        
        public static string FormatExceptionMessage(string message, Guid handlingInstanceId)
        {
            return message.Replace(HandlingInstanceToken, handlingInstanceId.ToString());
        }
		public static string FormatExceptionHandlingExceptionMessage(string policyName, Exception offendingException, Exception chainException, Exception originalException)
        {
            StringBuilder message = new StringBuilder();
            StringWriter writer = null;
			string result = null;
            try
            {
                writer = new StringWriter(message, CultureInfo.CurrentCulture);
                if (policyName.Length > 0)
                {
                    writer.WriteLine(string.Format(Resources.Culture, Resources.PolicyName, policyName));
                }
                FormatHandlingException(writer, Resources.OffendingException, offendingException);
                FormatHandlingException(writer, Resources.OriginalException, originalException);
                FormatHandlingException(writer, Resources.ChainException, chainException);
            }
            finally
            {
                if (writer != null)
                {
					result = writer.ToString();
                    writer.Close();
                }
            }
			return result;
        }
        private static void FormatHandlingException(StringWriter writer, string header, Exception ex)
        {
            if (ex != null)
            {
                writer.WriteLine();
                writer.WriteLine(header);
                writer.Write(writer.NewLine);
                TextExceptionFormatter formatter = new TextExceptionFormatter(writer, ex);
                formatter.Format();
            }
        }
    }
}
