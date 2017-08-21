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
using Sheng.SailingEase.Core;
using Sheng.SailingEase.ComponentModel.Design;
using Sheng.SailingEase.ComponentModel.Design.UndoEngine;
namespace Sheng.SailingEase.Infrastructure
{
    public interface IWindowDesignService
    {
        void OpenDesigner(WindowEntity entity);
        bool InDesigner(WindowEntity entity);
        void ExecuteCommand(CommandAbstract command);
        void ExecuteCommand(CommandAbstract command, Action<SEUndoUnitAbstract, SEUndoEngine.Type> action);
        void MakeDirty();
        void AddUndoUnit(SEUndoUnitAbstract undoUnit);
    }
}
