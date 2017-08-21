/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
	[Assembler(typeof(TextFormatterAssembler))]
	public class TextFormatterData : FormatterData
	{
		private const string templateProperty = "template";
		public TextFormatterData()
		{
		}
		public TextFormatterData(string templateData)
			: this("unnamed", templateData)
		{
		}
		public TextFormatterData(string name, string templateData)
			: this(name, typeof(TextFormatter), templateData)
		{
		}
		public TextFormatterData(string name, Type formatterType, string templateData)
			: base(name, formatterType)
		{
			this.Template = templateData;
		}
		[ConfigurationProperty(templateProperty, IsRequired= true)]
		public string Template
		{
			get
			{
				return (string)this[templateProperty];
			}
			set
			{
				this[templateProperty] = value;
			}
		}
	}
	public class TextFormatterAssembler : IAssembler<ILogFormatter, FormatterData>
	{
		public ILogFormatter Assemble(IBuilderContext context, FormatterData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			TextFormatterData castedObjectConfiguration = (TextFormatterData)objectConfiguration;
			ILogFormatter createdObject	= new TextFormatter(castedObjectConfiguration.Template);
			return createdObject;
		}
	}
}
