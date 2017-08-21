

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sheng.SailingEase.ComponentModel.Design.UndoEngine
{
    /// <summary>
    /// 撤销/重复引擎
    /// </summary>
    public class SEUndoEngine
    {
        #region 事件和相关委托

        /// <summary>
        /// 执行多步撤销以后的事件委托
        /// </summary>
        /// <param name="undoUnit"></param>
        public delegate void OnStepUndoHandler(int step);
        /// <summary>
        /// 执行多步撤销以后的事件
        /// </summary>
        public event OnStepUndoHandler OnStepUndo;

        /// <summary>
        /// 执行多步重做以后的事件委托
        /// </summary>
        /// <param name="undoUnit"></param>
        public delegate void OnStepRedoHandler(int step);
        /// <summary>
        /// 执行多步重做以后的事件
        /// </summary>
        public event OnStepRedoHandler OnStepRedo;

        /// <summary>
        /// 执行撤销以后的事件委托
        /// </summary>
        /// <param name="undoUnit"></param>
        public delegate void OnUndoHandler(SEUndoUnitAbstract undoUnit);
        /// <summary>
        /// 执行撤销以后的事件
        /// 如果一次执行多步，可能每步都会触发（根据执行时的bool参数）
        /// </summary>
        public event OnUndoHandler OnUndo;

        /// <summary>
        /// 执行重做以后的事件委托
        /// </summary>
        /// <param name="undoUnit"></param>
        public delegate void OnRedoHandler(SEUndoUnitAbstract undoUnit);
        /// <summary>
        /// 执行重做以后的事件
        /// 如果一次执行多步，可能每步都会触发（根据执行时的bool参数）
        /// </summary>
        public event OnRedoHandler OnRedo;

        /// <summary>
        /// 添加新的单元以后的事件委托
        /// </summary>
        /// <param name="undoUnit"></param>
        public delegate void OnAddUndoUnitHandler(SEUndoUnitAbstract undoUnit);
        /// <summary>
        /// 添加新的实现ISEUndoUnit的工作单元（包括事务，事务也是实现ISEUndoUnit的）以后的事件
        /// </summary>
        public event OnAddUndoUnitHandler OnAddUndoUnit;

        // /// <summary>
        ///// 事务完成后的事件
        ///// 撤销，重做完成后会引发一个事务结束事件，添加工作单元完成不会引发此事件
        ///// 事务中的单元集合，可能只有一个工作单元，也可能有多个，一系列的工作单元
        // /// </summary>
        //public event OnTransactionClosedHandler OnTransactionClosed;

        #endregion

        #region 公开属性

        private bool _working = false;
        /// <summary>
        /// 是否正在撤销或重做
        /// 如果正在撤销或重做，不应再把这些操作记录在SEUndoEngine中
        /// 在向SEUndoEngine中放入新的可撤销单元时应判断这个属性
        /// </summary>
        public bool Working
        {
            get { return this._working; }
            set { this._working = value; }
        }

        public bool EnableUndo
        {
            get { return _undoStack.Count > 0; }
        }

        public bool EnableRedo
        {
            get { return _redoStack.Count > 0; }
        }

        #endregion

        #region 私有成员

        /// <summary>
        /// 撤销集合
        /// </summary>
        private Stack<SEUndoUnitAbstract> _undoStack = new Stack<SEUndoUnitAbstract>();

        /// <summary>
        /// 重做集合
        /// </summary>
        private Stack<SEUndoUnitAbstract> _redoStack = new Stack<SEUndoUnitAbstract>();

        #endregion

        #region 公开方法

        #region Undo

        /// <summary>
        /// 撤销
        /// </summary>
        public void Undo()
        {
            Undo(true);
        }

        /// <summary>
        /// 撤销
        /// 并指定是否引发OnUndo事件
        /// </summary>
        /// <param name="undoEvent"></param>
        public void Undo(bool undoEvent)
        {
            this.Working = true;

            SEUndoUnitAbstract unit = null;

            Debug.Assert(_undoStack.Count > 0, "_undoStack.Count <= 0");

            if (_undoStack.Count > 0)
            {
                try
                {
                    unit = _undoStack.Pop();
                    unit.Undo();
                    _redoStack.Push(unit);

                    Debug.WriteLine("Undo:" + unit.ToString());

                    //触发撤销后的事件
                    if (undoEvent && this.OnUndo != null )
                        OnUndo(unit);
                }
                catch (Exception ex)
                {
                    Debug.Assert(false, ex.Message);
                }
            }

            this.Working = false;
        }

        /// <summary>
        /// 多步撤销
        /// 默认不在每一步Undo中引发事件
        /// </summary>
        /// <param name="step"></param>
        public void Undo(int step)
        {
            Undo(step, false);
        }

        /// <summary>
        /// 多步撤销，并指定是否在每一步Undo中引发事件
        /// </summary>
        /// <param name="step"></param>
        /// <param name="undoEvent"></param>
        public void Undo(int step, bool undoEvent)
        {
            this.Working = true;

            if (step < 1)
                return;

            Dictionary<SEUndoUnitAbstract, Action<SEUndoUnitAbstract, SEUndoEngine.Type>> _actions
                = new Dictionary<SEUndoUnitAbstract, Action<SEUndoUnitAbstract, Type>>();

            #region 循环执行指定的步数

            for (int i = 0; i < step; i++)
            {
                Debug.Assert(_undoStack.Count > 0, "_undoStack.Count <= 0");

                SEUndoUnitAbstract unit = null;

                if (_undoStack.Count > 0)
                {
                    try
                    {
                        unit = _undoStack.Pop();
                        if (unit.MergeAction && unit.Action != null)
                        {
                            if ((from c in _actions where c.Key.GetType().Equals(unit.GetType()) select c).Count() == 0)
                            {
                                _actions.Add(unit, unit.Action);
                            }

                            unit.Undo(false);
                        }
                        else
                        {
                            unit.Undo(true);
                        }
                        _redoStack.Push(unit);

                        Debug.WriteLine("Undo:" + unit.ToString());

                        //触发撤销后的事件
                        if (undoEvent && this.OnUndo != null)
                            OnUndo(unit);
                    }
                    catch (Exception ex)
                    {
                        Debug.Assert(false, ex.Message);
                    }
                }
            }

            #endregion

            foreach (KeyValuePair<SEUndoUnitAbstract, Action<SEUndoUnitAbstract, SEUndoEngine.Type>> pair in _actions)
            {
                pair.Value(pair.Key, SEUndoEngine.Type.Undo);
            }

            if (OnStepUndo != null)
                OnStepUndo(step);

            this.Working = false;
        }

        #endregion

        #region Redo

        /// <summary>
        /// 重做
        /// </summary>
        public void Redo()
        {
            Redo(true);
        }

        /// <summary>
        /// 重做
        /// 并指定是否引发OnRedo事件
        /// </summary>
        /// <param name="redoEvent"></param>
        public void Redo(bool redoEvent)
        {
            this.Working = true;

            SEUndoUnitAbstract unit = null;

            Debug.Assert(_redoStack.Count > 0, "_redoStack.Count <= 0");

            if (_redoStack.Count > 0)
            {
                try
                {
                    unit = _redoStack.Pop();
                    unit.Redo();
                    _undoStack.Push(unit);

                    Debug.WriteLine("Redo:" + unit.ToString());

                    //触发重做后的事件
                    if (redoEvent && this.OnRedo != null )
                        OnRedo(unit);
                }
                catch (Exception ex)
                {
                    Debug.Assert(false, ex.Message);
                }
            }

            this.Working = false;
        }

        /// <summary>
        /// 多步重做
        /// 默认不在每一步Redo中引发事件
        /// </summary>
        /// <param name="step"></param>
        public void Redo(int step)
        {
            Redo(step, false);
        }

        /// <summary>
        /// 多步重做，并指定是否在每一步Redo中引发事件
        /// </summary>
        /// <param name="step"></param>
        /// <param name="redoEvent"></param>
        public void Redo(int step, bool redoEvent)
        {
            this.Working = true;

            if (step < 1)
                return;

            Dictionary<SEUndoUnitAbstract, Action<SEUndoUnitAbstract, SEUndoEngine.Type>> _actions
               = new Dictionary<SEUndoUnitAbstract, Action<SEUndoUnitAbstract, Type>>();

            for (int i = 0; i < step; i++)
            {
                Debug.Assert(_redoStack.Count > 0, "_redoStack.Count <= 0");

                SEUndoUnitAbstract unit = null;

                if (_redoStack.Count > 0)
                {
                    try
                    {
                        unit = _redoStack.Pop();
                        if (unit.MergeAction && unit.Action != null)
                        {
                            if ((from c in _actions where c.Key.GetType().Equals(unit.GetType()) select c).Count() == 0)
                            {
                                _actions.Add(unit, unit.Action);
                            }

                            unit.Redo(false);
                        }
                        else
                        {
                            unit.Redo(true);
                        }
                        _undoStack.Push(unit);

                        Debug.WriteLine("Redo:" + unit.ToString());

                        //触发重做后的事件
                        if (this.OnRedo != null && redoEvent)
                            OnRedo(unit);
                    }
                    catch (Exception ex)
                    {
                        Debug.Assert(false, ex.Message);
                    }
                }
            }

            foreach (KeyValuePair<SEUndoUnitAbstract, Action<SEUndoUnitAbstract, SEUndoEngine.Type>> pair in _actions)
            {
                pair.Value(pair.Key, SEUndoEngine.Type.Redo);
            }

            if (OnStepRedo != null)
                OnStepRedo(step);

            this.Working = false;
        }

        #endregion

        #region AddUndoUnit

        /// <summary>
        /// 将 SEUndoUnit 添加到撤消堆栈中。
        /// </summary>
        /// <param name="unit"></param>
        public void AddUndoUnit(SEUndoUnitAbstract unit)
        {
            if (this.Working)
                return;

            if (unit == null)
                return;

            //放入新的可撤销单元时，必须清除当前的重做堆栈,否则会在使用重做功能时会发生逻辑问题
            //看文件头上的注释
            if (EnableRedo)
            {
                _redoStack.Clear();
            }

            unit.UndoEngine = this;

            _undoStack.Push(unit);

            //触发添加单元后的事件
            if (this.OnAddUndoUnit != null)
                OnAddUndoUnit(unit);
        }

        /// <summary>
        /// 添加一系列工作单元
        /// 这些工作单元将组成一个事务
        /// </summary>
        /// <param name="units"></param>
        public void AddUndoUnit(SEUndoUnitCollection units)
        {
            if (this.Working)
                return;

            if (units == null || units.Count == 0)
                return;

            if (units.Count == 1)
            {
                this.AddUndoUnit(units[0]);
            }
            else
            {
                SEUndoTransaction transaction = new SEUndoTransaction(this, units);
                this.AddUndoUnit(transaction);
            }
        }

        #endregion

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

        #endregion

        public enum Type
        {
            Undo = 0,
            Redo = 1
        }
    }
}
