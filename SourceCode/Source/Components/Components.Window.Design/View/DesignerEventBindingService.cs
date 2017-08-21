/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Design;
namespace Sheng.SailingEase.Components.Window.DesignComponent
{
	class DesignerEventBindingService : EventBindingService
	{
		public DesignerEventBindingService(IServiceProvider provider)
			: base(provider)
		{
		}
		protected override string CreateUniqueMethodName(System.ComponentModel.IComponent component, System.ComponentModel.EventDescriptor e)
		{
			ISelectionService selectionService = this.GetService(typeof(ISelectionService)) as ISelectionService;
			if (selectionService.SelectionCount == 1)
			{
				FormDesignerEvents formEvents = new FormDesignerEvents();
				formEvents.ShowDialog();
			}
			return String.Empty;
		}
		protected override System.Collections.ICollection GetCompatibleMethods(System.ComponentModel.EventDescriptor e)
		{
			throw new NotImplementedException();
		}
		protected override bool ShowCode(System.ComponentModel.IComponent component, System.ComponentModel.EventDescriptor e, string methodName)
		{
			throw new NotImplementedException();
		}
		protected override bool ShowCode(int lineNumber)
		{
			throw new NotImplementedException();
		}
		protected override bool ShowCode()
		{
			throw new NotImplementedException();
		}
	}
}
