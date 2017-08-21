/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Sources;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design
{
	sealed class LoggingCommandRegistrar : CommandRegistrar
	{	
		public LoggingCommandRegistrar(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
		public override void Register()
		{
			AddLoggingCommand();
			AddDefaultCommands(typeof(LoggingSettingsNode));
			AddWmiTraceListenerCommand();
			AddDefaultCommands(typeof(WmiTraceListenerNode));
			AddSystemDiagnosticsTraceListenerCommand();
			AddDefaultCommands(typeof(SystemDiagnosticsTraceListenerNode));
			AddMsmqTraceListenerCommand();
			AddDefaultCommands(typeof(MsmqTraceListenerNode));
			AddFlatFileTraceListenerCommand();
			AddDefaultCommands(typeof(FlatFileTraceListenerNode));
			AddFormattedEventLogTraceListenerCommand();
			AddDefaultCommands(typeof(FormattedEventLogTraceListenerNode));
			AddEmailTraceListenerCommand();
			AddDefaultCommands(typeof(EmailTraceListenerNode));
			AddCustomTraceListenerCommand();
			AddDefaultCommands(typeof(CustomTraceListenerNode));
			AddTraceListenerReferenceCommand();
			AddDefaultCommands(typeof(TraceListenerReferenceNode));
			AddCategoryCommand();
			AddDefaultCommands(typeof(CategoryTraceSourceNode));
			AddCategoryFilterCommand();
			AddDefaultCommands(typeof(CategoryFilterNode));
			AddMoveUpDownCommands(typeof(CategoryFilterNode));
			AddLogEnabledFilterCommand();
			AddDefaultCommands(typeof(LogEnabledFilterNode));
			AddMoveUpDownCommands(typeof(LogEnabledFilterNode));
			AddPriorityFilterCommand();
			AddDefaultCommands(typeof(PriorityFilterNode));
			AddMoveUpDownCommands(typeof(PriorityFilterNode));
			AddCustomLogFilterCommand();
			AddDefaultCommands(typeof(CustomLogFilterNode));
			AddMoveUpDownCommands(typeof(CustomLogFilterNode));
			AddTextFormatterCommand();
			AddDefaultCommands(typeof(TextFormatterNode));
			AddBinaryFormatterCommand();
			AddDefaultCommands(typeof(BinaryFormatterNode));
			AddCustomFormatterCommand();
			AddDefaultCommands(typeof(CustomFormatterNode));
			AddXmlTraceListenerNodeCommand();
			AddDefaultCommands(typeof(XmlTraceListenerNode));
			AddRollingTraceListenerNodeCommand();
			AddDefaultCommands(typeof(RollingTraceListenerNode));
		}
		private void AddCategoryCommand()
		{
			AddMultipleChildNodeCommand(Resources.CategoryTraceSourceUICommandText,
				Resources.CategoryTraceSourceUICommandLongText,
				typeof(CategoryTraceSourceNode),
				typeof(CategoryTraceSourceCollectionNode));
		}
		private void AddCustomFormatterCommand()
		{
			AddMultipleChildNodeCommand(Resources.CustomFormatterUICommandText,
				Resources.CustomFormatterUICommandLongText,
				typeof(CustomFormatterNode),
				typeof(FormatterCollectionNode));
		}
		private void AddBinaryFormatterCommand()
		{
			AddMultipleChildNodeCommand(Resources.BinaryFormatterUICommandText,
				Resources.BinaryFormatterUICommandLongText,
				typeof(BinaryFormatterNode),
				typeof(FormatterCollectionNode));
		}
		private void AddTextFormatterCommand()
		{
			AddMultipleChildNodeCommand(Resources.TextFormatterUICommandText,
				Resources.TextFormatterUICommandLongText,
				typeof(TextFormatterNode),
				typeof(FormatterCollectionNode));
		}
		private void AddPriorityFilterCommand()
		{
			AddMultipleChildNodeCommand(Resources.PriorityFilterUICommandText,
				Resources.PriorityFilterUICommandLongText,
				typeof(PriorityFilterNode),
				typeof(LogFilterCollectionNode));
		}
		private void AddCustomLogFilterCommand()
		{
			AddMultipleChildNodeCommand(Resources.CustomLogFilterUICommandText,
				Resources.CustomLogFilterUICommandLongText,
				typeof(CustomLogFilterNode),
				typeof(LogFilterCollectionNode));
		}
		private void AddLogEnabledFilterCommand()
		{
			AddMultipleChildNodeCommand(Resources.LogEnabledFilterUICommandText,
				Resources.LogEnabledFilterUICommandLongText,
				typeof(LogEnabledFilterNode),
				typeof(LogFilterCollectionNode));
		}
		private void AddCategoryFilterCommand()
		{
			AddMultipleChildNodeCommand(Resources.CategoryFilterUICommandText,
				Resources.CategoryFilterUICommandLongText,
				typeof(CategoryFilterNode),
				typeof(LogFilterCollectionNode));
		}
		private void AddTraceListenerReferenceCommand()
		{
			ConfigurationUICommand item = ConfigurationUICommand.CreateMultipleUICommand(ServiceProvider, Resources.TraceListenerReferenceUICommandText,
				Resources.TraceListenerReferenceUICommandLongText,
				new AddChildNodeCommand(ServiceProvider, typeof(TraceListenerReferenceNode)),
				typeof(TraceListenerReferenceNode));
			AddUICommand(item, typeof(AllTraceSourceNode));
			AddUICommand(item, typeof(ErrorsTraceSourceNode));
			AddUICommand(item, typeof(NotProcessedTraceSourceNode));
			AddUICommand(item, typeof(CategoryTraceSourceNode));
		}
		private void AddCustomTraceListenerCommand()
		{
			AddMultipleChildNodeCommand(Resources.CustomTraceListenerUICommandText,
				Resources.CustomTraceListenerUICommandLongText,
				typeof(CustomTraceListenerNode),
				typeof(TraceListenerCollectionNode));
		}
		private void AddFormattedEventLogTraceListenerCommand()
		{
			AddMultipleChildNodeCommand(Resources.FormattedEventLogTraceListenerUICommandText,
				Resources.FormattedEventLogTraceListenerUICommandLongText,
				typeof(FormattedEventLogTraceListenerNode),
				typeof(TraceListenerCollectionNode));
		}
		private void AddEmailTraceListenerCommand()
		{
			AddMultipleChildNodeCommand(Resources.EmailTraceListenerUICommandText,
				Resources.EmailTraceListenerUICommandLongText,
				typeof(EmailTraceListenerNode),
				typeof(TraceListenerCollectionNode));
		}
		private void AddFlatFileTraceListenerCommand()
		{
			AddMultipleChildNodeCommand(Resources.FlatFileTraceListenerUICommandText,
				Resources.FlatFileTraceListenerUICommandLongText,
				typeof(FlatFileTraceListenerNode),
				typeof(TraceListenerCollectionNode));
		}
		private void AddMsmqTraceListenerCommand()
		{
			AddMultipleChildNodeCommand(Resources.MsmqTraceListenerUICommandText,
				Resources.MsmqTraceListenerUICommandLongText,
				typeof(MsmqTraceListenerNode),
				typeof(TraceListenerCollectionNode));
		}
		private void AddSystemDiagnosticsTraceListenerCommand()
		{
			AddMultipleChildNodeCommand(Resources.SystemDiagnosticsTraceListenerUICommandText,
				Resources.SystemDiagnosticsTraceListenerUICommandLongText,
				typeof(SystemDiagnosticsTraceListenerNode),
				typeof(TraceListenerCollectionNode));
		}
		private void AddWmiTraceListenerCommand()
		{
			AddMultipleChildNodeCommand(Resources.WmiTraceListenerUICommandText,
				Resources.WmiTraceListenerUICommandLongText,
				typeof(WmiTraceListenerNode),
				typeof(TraceListenerCollectionNode));
		}
		private void AddLoggingCommand()
		{
			ConfigurationUICommand item = ConfigurationUICommand.CreateSingleUICommand(ServiceProvider, 
				Resources.LogSettingsCommandText,
				Resources.LogSettingsCommandLongText,
				new AddLoggingSettingsNodeCommand(ServiceProvider),
                typeof(LoggingSettingsNode));
			AddUICommand(item, typeof(ConfigurationApplicationNode));
		}
		private void AddXmlTraceListenerNodeCommand()
		{
			AddMultipleChildNodeCommand(
				Resources.XmlTraceListenerNodeUICommandText,
				Resources.XmlTraceListenerNodeUICommandLongText,
				typeof(XmlTraceListenerNode),
				typeof(TraceListenerCollectionNode));
		}
		private void AddRollingTraceListenerNodeCommand()
		{
			AddMultipleChildNodeCommand(
				Resources.RollingTraceListenerNodeUICommandText,
				Resources.RollingTraceListenerNodeUICommandLongText,
				typeof(RollingTraceListenerNode),
				typeof(Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners.TraceListenerCollectionNode));
		}
	}
}
