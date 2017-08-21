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
using Sheng.SIMBE.Core.Entity;
using Sheng.SIMBE.Core;
using Sheng.SIMBE.Core.Event;
using Sheng.SIMBE.IDE.EventDev;
using Sheng.SIMBE.IDE.WarningChecker;
namespace Sheng.SailingEase.Components.NavigationComponent
{
    class ToolStripLabelEntityDev : ToolStripLabelEntity, IFormElementEntityDev,IWarningable
    {
        public ToolStripLabelEntityDev()
        {
            base.EventTypesAdapter = EventDevTypes.Instance;
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
            get { return typeof(Sheng.SIMBE.IDE.ShellControl.SEToolStripLabelDev); }
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
            WarningChecker.ToolStripLabelEntityDevChecker.CheckWarning(this);
        }
    }
}
