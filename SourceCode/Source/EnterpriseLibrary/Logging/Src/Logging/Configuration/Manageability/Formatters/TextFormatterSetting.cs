/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Configuration;
using System.Management.Instrumentation;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Formatters
{
    [ManagementEntity]
    public partial class TextFormatterSetting : FormatterSetting
    {
        string template;
        public TextFormatterSetting(TextFormatterData sourceElement,
                                    string name,
                                    string template)
            : base(sourceElement, name)
        {
            this.template = template;
        }
        [ManagementConfiguration]
        public string Template
        {
            get { return template; }
            set { template = value; }
        }
        [ManagementBind]
        public static TextFormatterSetting BindInstance(string ApplicationName,
                                                        string SectionName,
                                                        string Name)
        {
            return BindInstance<TextFormatterSetting>(ApplicationName, SectionName, Name);
        }
        [ManagementEnumerator]
        public static IEnumerable<TextFormatterSetting> GetInstances()
        {
            return GetInstances<TextFormatterSetting>();
        }
        protected override bool SaveChanges(ConfigurationElement sourceElement)
        {
            return TextFormatterDataWmiMapper.SaveChanges(this, sourceElement);
        }
    }
}
