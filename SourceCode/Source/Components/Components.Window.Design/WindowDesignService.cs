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
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Kernal;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.ComponentModel.Design;
using Sheng.SailingEase.ComponentModel.Design.UndoEngine;
namespace Sheng.SailingEase.Components.Window.DesignComponent
{
    class WindowDesignService : IWindowDesignService
    {
        IWorkbenchService _workbenchService = ServiceUnity.WorkbenchService;
        private static InstanceLazy<WindowDesignService> _instance =
           new InstanceLazy<WindowDesignService>(() => new WindowDesignService());
        public static WindowDesignService Instance
        {
            get { return _instance.Value; }
        }
        private WindowDesignService()
        {
        }
        public void OpenDesigner(WindowEntity entity)
        {
            _workbenchService.Show(FormHostingContainer.Instance);
            FormHostingContainer.Instance.Create(entity);
        }
        public bool InDesigner(WindowEntity entity)
        {
            throw new NotImplementedException();
        }
        public void ExecuteCommand(CommandAbstract command)
        {
            FormHostingContainer.Instance.ExecuteCommand(command);
        }
        public void ExecuteCommand(CommandAbstract command, Action<SEUndoUnitAbstract, SEUndoEngine.Type> action)
        {
            FormHostingContainer.Instance.ExecuteCommand(command, action);
        }
        public void MakeDirty()
        {
            FormHostingContainer.Instance.MakeDirty();
        }
        public void AddUndoUnit(SEUndoUnitAbstract undoUnit)
        {
            FormHostingContainer.Instance.AddUndoUnit(undoUnit);
        }
    }
}
