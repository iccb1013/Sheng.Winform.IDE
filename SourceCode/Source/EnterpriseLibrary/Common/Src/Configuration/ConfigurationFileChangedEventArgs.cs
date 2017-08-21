/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    [Serializable]
    public class ConfigurationFileChangedEventArgs : ConfigurationChangedEventArgs
    {
        private readonly string configurationFile;
        public ConfigurationFileChangedEventArgs(string configurationFile, string sectionName) : base(sectionName)
        {
            this.configurationFile = configurationFile;
        }
        public string ConfigurationFile
        {
            get { return configurationFile; }
        }
    }
}
