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
using Sheng.SailingEase.ComponentModel.Design.UndoEngine;
using Sheng.SailingEase.ComponentModel.Design;
namespace Sheng.SailingEase.Components.Window.DesignComponent
{
    class SEUndoTransactionFormDesigner : SEUndoTransaction
    {
        public bool ComponentChanged
        {
            get
            {
                bool _componentChanged = false;
                foreach (SEUndoUnitFormDesigner unit in from unit in _undoStack.Union(_redoStack) where unit is SEUndoUnitFormDesigner select unit)
                {
                    if (unit.ComponentChanged)
                    {
                        _componentChanged = true;
                        break;
                    }
                }
                return _componentChanged;
            }
        }
    }
}
