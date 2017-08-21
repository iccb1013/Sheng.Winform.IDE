/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.ComponentModel;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters
{
    public sealed class TextFormatterNode : FormatterNode
    {
        private string template;
        public TextFormatterNode() 
            :this(new TextFormatterData(Resources.TextFormatterNode, DefaultValues.TextFormatterFormat))
        {
        }
        public TextFormatterNode(TextFormatterData textFormatterData) 
        {
			if (null == textFormatterData) throw new ArgumentNullException("textFormatterData");
            this.template = textFormatterData.Template;
			Rename(textFormatterData.Name);
        }
        [Required, Editor(typeof(TemplateEditor), typeof(UITypeEditor))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public Template Template
        {
            get { return new Template(template); }
            set { template = value.Text; }
        }
		public override FormatterData  FormatterData
		{
			get { return new TextFormatterData(Name, template); }
		}
    }
}
