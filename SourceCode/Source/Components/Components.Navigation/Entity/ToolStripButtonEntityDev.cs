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
using Sheng.SailingEase.Core.Development;
namespace Sheng.SailingEase.Components.NavigationComponent
{
    class ToolStripButtonEntityDev : ToolStripButtonEntity, IFormElementEntityDev, IWarningable
    {
        public ToolStripButtonEntityDev()
        {
            base.EventTypesAdapter = EventDevTypes.Instance;
        }
        private object objLock = new object();
        private static EventTypeCollection _eventProvide;
        public override EventTypeCollection EventProvide
        {
            get
            {
                if (_eventProvide == null)
                    lock (objLock)
                    {
                        _eventProvide = new EventTypeCollection();
                        _eventProvide.Add(typeof(LockProgramEvent));
                        _eventProvide.Add(typeof(ReLoginEvent));
                        _eventProvide.Add(typeof(ExitEvent));
                        _eventProvide.Add(typeof(OpenFormEvent));
                        _eventProvide.Add(typeof(StartProcessEvent));
                        _eventProvide.Add(typeof(OpenSystemFormEvent));
                        _eventProvide.Add(typeof(CallAddInEvent));
                        _eventProvide.Add(typeof(CallEntityMethodEvent));
                    }
                return _eventProvide;
            }
        }
        public System.ComponentModel.IComponent Component
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        public Type DesignerControlType
        {
            get { return typeof(Sheng.SIMBE.IDE.ShellControl.SEToolStripButtonDev); }
        }
        public string WarningSignName
        {
            get { return this.Name; }
        }
        [NonSerialized]
        private WarningSign _warning;
        public WarningSign Warning
        {
            get
            {
                if (_warning == null)
                    _warning = new WarningSign(this);
                return _warning;
            }
            set { _warning = value; }
        }
        public void CheckWarning()
        {
            WarningChecker.ToolStripButtonEntityDevChecker.CheckWarning(this);
        }
    }
}
