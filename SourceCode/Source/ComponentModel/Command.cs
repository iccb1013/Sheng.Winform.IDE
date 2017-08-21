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
namespace Sheng.SailingEase.ComponentModel
{
   
    public interface ICommand
    {
        Func<bool> CanExcuteHandler { set; }
        bool CanExcute();
        void Excute();
    }
    public abstract class AbstactCommand<TArgument> : ICommand
    {
        private Func<bool> _canExcuteHandler;
        public Func<bool> CanExcuteHandler { set { _canExcuteHandler = value; } }
        private Func<TArgument> _getArgumentHandler;
        public Func<TArgument> GetArgumentHandler { set { _getArgumentHandler = value; } }
        private Action _preExcuteHandler;
        public Action PreExcuteHandler { set { _preExcuteHandler = value; } }
        private Action _afterExcuteHandler;
        public Action AfterExcuteHandler { set { _afterExcuteHandler = value; } }
        private Action _excuteHandler;
        public Action ExcuteHandler { set { _excuteHandler = value; } }
        public AbstactCommand()
        {
        }
        public bool CanExcute()
        {
            if (_canExcuteHandler == null)
                return true;
            else
                return _canExcuteHandler();
        }
        public TArgument GetArgument()
        {
            Debug.Assert(_getArgumentHandler != null, "GetArgumentHandler 为 null");
            if (_getArgumentHandler == null)
                return default(TArgument);
            else
                return _getArgumentHandler();
        }
        public void PreExcute()
        {
            if (_preExcuteHandler != null)
                _preExcuteHandler();
        }
        public void AfterExcute()
        {
            if (_afterExcuteHandler != null)
                _afterExcuteHandler();
        }
        public void Excute()
        {
            if (_excuteHandler != null)
            {
                PreExcute();
                _excuteHandler();
                AfterExcute();
            }
        }
    }
    public abstract class AbstactCommand : AbstactCommand<object>
    {
    }
}
