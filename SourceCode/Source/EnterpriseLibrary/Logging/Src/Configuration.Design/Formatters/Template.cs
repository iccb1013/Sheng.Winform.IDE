/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes")]
	public class Template : IEnvironmentalOverridesSerializable
    {
        private string text;
        public Template()
        {
        }
        public Template(string text)
        {
            this.text = text;
        }
        public string Text
        {
            get { return text; }
            set { text = value; }
        }		
        public override string ToString()
        {
            return Resources.TemplatePlaceHolder;
        }
        void IEnvironmentalOverridesSerializable.DesializeFromString(string serializedContents)
        {
            text = serializedContents;
        }
        string IEnvironmentalOverridesSerializable.SerializeToString()
        {
            return text;
        }
    }
}
