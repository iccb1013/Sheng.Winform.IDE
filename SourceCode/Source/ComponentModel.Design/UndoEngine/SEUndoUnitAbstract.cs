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
namespace Sheng.SailingEase.ComponentModel.Design.UndoEngine
{
    public abstract class SEUndoUnitAbstract
    {
       
        internal virtual bool MergeAction
        {
            get { return true; }
        }
       
        public Action<SEUndoUnitAbstract, SEUndoEngine.Type> Action { get; set; }
        public abstract string Name { get; }
        public virtual SEUndoEngine UndoEngine { get; set; }
       
        public virtual void Undo()
        {
            Undo(true);
        }
        public abstract void Undo(bool action);
        public virtual void Redo()
        {
            Redo(true);
        }
        public abstract void Redo(bool action);
    }
}
