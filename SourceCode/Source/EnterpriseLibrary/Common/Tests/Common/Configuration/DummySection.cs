/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests
{
    public class DummySection : SerializableConfigurationSection
    {
        private const string nameProperty = "name";
        private const string valueProperty = "value";
        public DummySection()
        {
        }
        [ConfigurationProperty(nameProperty)]
        public string Name
        {
            get { return (string)base[nameProperty]; }
            set { base[nameProperty] = value; }
        }
        [ConfigurationProperty(valueProperty)]
        public int Value
        {
            get { return (int)base[valueProperty]; }
            set { base[valueProperty] = value; }
        }
    }
}
