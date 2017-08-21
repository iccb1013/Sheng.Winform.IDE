/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    public class FileConfigurationParameter : IConfigurationParameter
    {
        private readonly string fileName;
        public FileConfigurationParameter(string fileName)
        {
            this.fileName = fileName;
        }
        public string FileName
        {
            get { return fileName; }
        }
    }
}
