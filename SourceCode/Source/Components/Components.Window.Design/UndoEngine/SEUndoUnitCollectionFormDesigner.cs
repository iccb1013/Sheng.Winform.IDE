/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Sheng.SailingEase.ComponentModel.Design.UndoEngine;
using Sheng.SailingEase.Core.Development;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.ComponentModel.Design;
namespace Sheng.SailingEase.Components.Window.DesignComponent
{
    class SEUndoUnitCollectionFormDesigner : SEUndoUnitCollection
    {
        public bool ComponentChanged
        {
            get
            {
                for (int i = 0; i < this.Count; i++)
                {
                    SEUndoUnitFormDesigner unit = this[i] as SEUndoUnitFormDesigner;
                    if (unit != null)
                    {
                        if (unit.ComponentChanged)
                            return true;
                    }
                }
                return false;
            }
        }
        public void Clearup()
        {
           
            if (ComponentChanged == false)
                return;
            SEUndoUnitAbstract[] unitArray = new SEUndoUnitAbstract[this.Count];
            this.CopyTo(unitArray, 0);
            foreach (SEUndoUnitAbstract unit in unitArray)
            {
                SEUndoUnitFormDesigner undoUnitFormDesigner = unit as SEUndoUnitFormDesigner;
                if (undoUnitFormDesigner == null)
                {
                    continue;
                }
                if (undoUnitFormDesigner.ComponentChanged)
                {
                    if (undoUnitFormDesigner.Type == SEUndoUnitFormDesigner.UndoUnitType.ComponentAdded)
                    {
                        IShellControlDev shellControlDev;
                        shellControlDev = undoUnitFormDesigner.Components as IShellControlDev;
                        Debug.Assert(shellControlDev != null, "Component没有实现IShellControlDev");
                        if (shellControlDev != null)
                        {
                            undoUnitFormDesigner.Entity = shellControlDev.Entity;
                        }
                    }
                }
                else
                {
                    this.Remove(unit);
                }
            }
        }
        public int Add(string name, SEUndoUnitFormDesigner.UndoUnitType type, EntityBase entity, params SEUndoMember[] members)
        {
            SEUndoUnitFormDesigner undoUnit = new SEUndoUnitFormDesigner(name);
            undoUnit.Type = type;
            undoUnit.Entity = entity;
            undoUnit.Members.AddRange(members);
            return Add(undoUnit);
        }
    }
}
