/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
	[Assembler(typeof(BinaryLogFormatterAssembler))]
	public class BinaryLogFormatterData : FormatterData
	{
		public BinaryLogFormatterData()
			: base()
		{ }
		public BinaryLogFormatterData(string name)
			: base(name, typeof(BinaryLogFormatter))
		{ }
	}
	public class BinaryLogFormatterAssembler : IAssembler<ILogFormatter, FormatterData>
	{
		public ILogFormatter Assemble(IBuilderContext context, FormatterData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			return new BinaryLogFormatter();
		}
	}
}
