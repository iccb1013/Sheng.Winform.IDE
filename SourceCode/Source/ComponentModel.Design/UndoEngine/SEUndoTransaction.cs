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
using System.Diagnostics;
namespace Sheng.SailingEase.ComponentModel.Design.UndoEngine
{
    public class SEUndoTransaction : SEUndoUnitAbstract
    {
        protected Stack<SEUndoUnitAbstract> _undoStack = new Stack<SEUndoUnitAbstract>();
        protected Stack<SEUndoUnitAbstract> _redoStack = new Stack<SEUndoUnitAbstract>();
        private string _name;
        public override string Name
        {
            get
            {
                if (this._name == null || this._name == String.Empty)
                {
                    if (this._undoStack.Count == 0 && this._redoStack.Count == 0)
                        return String.Empty;
                    else
                        if (_undoStack.Count > 0)
                        {
                            return _undoStack.Peek().Name;
                        }
                        else
                        {
                            return _redoStack.Peek().Name;
                        }
                }
                else
                {
                    return this._name;
                }
            }
        }
        public override SEUndoEngine UndoEngine
        {
            get
            {
                return base.UndoEngine;
            }
            set
            {
                if (base.UndoEngine != null && base.UndoEngine.Equals(value) == false)
                {
                    foreach (SEUndoUnitAbstract undoUnit in this._undoStack.ToArray())
                    {
                        undoUnit.UndoEngine = value;
                    }
                    foreach (SEUndoUnitAbstract undoUnit in this._redoStack.ToArray())
                    {
                        undoUnit.UndoEngine = value;
                    }
                }
                base.UndoEngine = value;
            }
        }
        public SEUndoTransaction()
        {
        }
        public SEUndoTransaction(string name)
        {
            this._name = name;
        }
        public SEUndoTransaction(SEUndoEngine undoEngine, SEUndoUnitCollection units)
        {
            this.UndoEngine = undoEngine;
            foreach (SEUndoUnitAbstract undoUnit in units)
            {
                AddUndoUnit(undoUnit);
            }
            this.Action = units.Action;
        }
        public SEUndoUnitAbstract[] GetUndoUnits()
        {
            return this._undoStack.ToArray();
        }
        public SEUndoUnitAbstract[] GetRedoUnits()
        {
            return this._redoStack.ToArray();
        }
        public SEUndoUnitAbstract[] GetAllUnits()
        {
            return this._undoStack.ToArray().Union(this._redoStack.ToArray()).ToArray();
        }
        public override void Undo(bool action)
        {
            try
            {
                while (_undoStack.Count > 0)
                {
                    UndoUnit();
                }
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }
            if (action && Action != null)
            {
                Action(this,SEUndoEngine.Type.Undo);
            }
        }
        public override void Redo(bool action)
        {
            try
            {
                while (_redoStack.Count > 0)
                {
                    RedoUnit();
                }
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }
            if (action && Action != null)
            {
                Action(this,SEUndoEngine.Type.Redo);
            }
        }
        public override string ToString()
        {
            return base.ToString();
        }
        public void SetName(string name)
        {
            this._name = name;
        }
        public void AddUndoUnit(SEUndoUnitAbstract unit)
        {
            if (unit == null)
                return;
            if (_redoStack.Count > 0)
            {
                _redoStack.Clear();
            }
            unit.UndoEngine = this.UndoEngine;
            _undoStack.Push(unit);
        }
        public void AddUndoUnit(SEUndoUnitCollection units)
        {
            if (units == null || units.Count == 0)
                return;
            foreach (SEUndoUnitAbstract undoUnit in units)
            {
                AddUndoUnit(undoUnit);
            }
        }
        private SEUndoUnitAbstract UndoUnit()
        {
            if (_undoStack.Count == 0)
                return null;
            SEUndoUnitAbstract unit;
            unit = _undoStack.Pop();
            unit.Undo();
            _redoStack.Push(unit);
            Debug.WriteLine("Undo:" + unit.ToString());
            return unit;
        }
        private SEUndoUnitAbstract RedoUnit()
        {
            if (_redoStack.Count == 0)
                return null;
            SEUndoUnitAbstract unit = _redoStack.Pop();
            unit.Redo();
            _undoStack.Push(unit);
            Debug.WriteLine("Redo:" + unit.ToString());
            return unit;
        }
    }
}
