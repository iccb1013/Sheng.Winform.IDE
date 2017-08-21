/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design
{
    public class AddExceptionTypeNodeCommand : AddChildNodeCommand
    {
        public AddExceptionTypeNodeCommand(IServiceProvider serviceProvider, Type childType) : base(serviceProvider, childType)
        {
        }
        protected override void ExecuteCore(ConfigurationNode node)
        {
			Type selectedType = SelectedType;
			if (null == selectedType) return;
			base.ExecuteCore(node);
			ExceptionTypeNode typeNode = (ExceptionTypeNode)ChildNode;
			typeNode.PostHandlingAction = PostHandlingAction.NotifyRethrow;			
			try
			{
				typeNode.Type = selectedType.AssemblyQualifiedName;
			}
			catch (InvalidOperationException)
			{
				typeNode.Remove();
				UIService.ShowError(string.Format(Resources.Culture, Resources.DuplicateExceptionTypeErrorMessage, selectedType != null ? selectedType.Name : string.Empty));
			}		
        }
		protected virtual Type SelectedType
		{
			get
			{
				TypeSelectorUI selector = new TypeSelectorUI(
					typeof(Exception),
					typeof(Exception),
					TypeSelectorIncludes.BaseType |
						TypeSelectorIncludes.AbstractTypes);
				DialogResult result = selector.ShowDialog();
				if (result == DialogResult.OK)
				{
					return selector.SelectedType;
				}
				return null;
			}
		}
    }
}
