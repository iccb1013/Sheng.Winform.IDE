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
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design
{
	sealed class ExceptionHandlingCommandRegistrar : CommandRegistrar
	{		
		public ExceptionHandlingCommandRegistrar(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
		public override void Register()
		{
			AddExceptionHandlingCommand();
			AddDefaultCommands(typeof(ExceptionHandlingSettingsNode));
			AddExceptionTypeNodeCommand();
			AddDefaultCommands(typeof(ExceptionTypeNode));
			AddExceptionPolicyNodeCommand();
			AddDefaultCommands(typeof(ExceptionPolicyNode));
			AddReplaceHandlerNodeCommand();
			AddDefaultCommands(typeof(ReplaceHandlerNode));
			AddMoveUpDownCommands(typeof(ReplaceHandlerNode));
			AddWrapHandlerNodeCommand();
			AddDefaultCommands(typeof(WrapHandlerNode));
			AddMoveUpDownCommands(typeof(WrapHandlerNode));
			AddCustomHandlerNodeCommand();
			AddDefaultCommands(typeof(CustomHandlerNode));
			AddMoveUpDownCommands(typeof(CustomHandlerNode));		
		}
		private void AddCustomHandlerNodeCommand()
		{
			AddMultipleChildNodeCommand(Resources.DefaultCustomHandlerNodeName,
				string.Format(Resources.Culture, Resources.GenericCreateStatusText, Resources.DefaultCustomHandlerNodeName),
				typeof(CustomHandlerNode), typeof(ExceptionTypeNode));
		}
		private void AddWrapHandlerNodeCommand()
		{
			AddMultipleChildNodeCommand(Resources.DefaultWrapHandlerNodeName,
				string.Format(Resources.Culture, Resources.GenericCreateStatusText, Resources.DefaultWrapHandlerNodeName),
				typeof(WrapHandlerNode), typeof(ExceptionTypeNode));			
		}
		private void AddReplaceHandlerNodeCommand()
		{
			AddMultipleChildNodeCommand(Resources.DefaultReplaceHandlerNodeName,
				string.Format(Resources.Culture, Resources.GenericCreateStatusText, Resources.DefaultReplaceHandlerNodeName),
				typeof(ReplaceHandlerNode), typeof(ExceptionTypeNode));
		}
		private void AddExceptionTypeNodeCommand()
		{
            ConfigurationUICommand cmd = ConfigurationUICommand.CreateMultipleUICommand(ServiceProvider,
                Resources.ExceptionTypeNodeMenuText,
                Resources.ExceptionTypeNodeStatusText,
                new AddExceptionTypeNodeCommand(ServiceProvider, typeof(ExceptionTypeNode)),
                typeof(ExceptionTypeNode));
            AddUICommand(cmd, typeof(ExceptionPolicyNode));		
		}
		private void AddExceptionPolicyNodeCommand()
		{
			AddMultipleChildNodeCommand(Resources.ExceptionPolicyNodeMenuText,
				Resources.ExceptionPolicyNodeStatusText,
				typeof(ExceptionPolicyNode), typeof(ExceptionHandlingSettingsNode));	
		}
		private void AddExceptionHandlingCommand()
		{
			AddSingleChildNodeCommand(Resources.ExceptionHandlingUICommandText,
				Resources.ExceptionHandlingUICommandLongText,
				typeof(ExceptionHandlingSettingsNode), typeof(ConfigurationApplicationNode));
		}		
	}
}
