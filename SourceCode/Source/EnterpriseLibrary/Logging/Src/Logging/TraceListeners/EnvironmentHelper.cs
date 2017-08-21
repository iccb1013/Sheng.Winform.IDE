/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.IO;
using System.Security;
using System.Text.RegularExpressions;
using Microsoft.Practices.EnterpriseLibrary.Logging.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners
{
    public class EnvironmentHelper
    {
        public static string ReplaceEnvironmentVariables(string fileName)
        {
            try
            {
                string variables = Environment.ExpandEnvironmentVariables(fileName);
                Regex filter = new Regex("%(.*?)%", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
                string filePath = filter.Replace(variables, "");
                if (Path.GetDirectoryName(filePath) == null)
                {
                    filePath = Path.GetFileName(filePath);
                }
                return filePath;
            }
            catch (SecurityException)
            {
                throw new InvalidOperationException(Resources.ExceptionReadEnvironmentVariablesDenied);
            }
        }
    }
}
