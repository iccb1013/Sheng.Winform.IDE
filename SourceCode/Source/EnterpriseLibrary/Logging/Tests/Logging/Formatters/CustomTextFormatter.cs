/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Formatters.Tests
{
	public class CustomTextFormatter : TextFormatter
	{
		public CustomTextFormatter(string template)
			: base(template, GetExtraTokenHandlersDictionary())
		{ }
		private static IDictionary<string, TokenHandler<LogEntry>> GetExtraTokenHandlersDictionary()
		{
			Dictionary<string, TokenHandler<LogEntry>> tokenHandlers = new Dictionary<string, TokenHandler<LogEntry>>();
			tokenHandlers["field1"] = GenericTextFormatter<LogEntry>.CreateSimpleTokenHandler(le => ((CustomLogEntry)le).AcmeCoField1);
			tokenHandlers["field2"] = GenericTextFormatter<LogEntry>.CreateSimpleTokenHandler(le => ((CustomLogEntry)le).AcmeCoField2);
			tokenHandlers["field3"] = GenericTextFormatter<LogEntry>.CreateSimpleTokenHandler(le => ((CustomLogEntry)le).AcmeCoField3);
			return tokenHandlers;
		}
		public override string Format(LogEntry log)
		{
			StringBuilder templateBuilder = new StringBuilder(base.Format(log));
			CustomToken custom = new CustomToken();
			custom.Format(templateBuilder, log);
			return templateBuilder.ToString();
		}
	}
}
