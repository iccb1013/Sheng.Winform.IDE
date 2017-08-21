/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sheng.SailingEase.Controls;
using Sheng.SailingEase.Controls.Extensions;
namespace Sheng.SailingEase.Infrastructure
{
    public partial class WorkbenchViewBase : SEFormDock, IWorkbenchView
    {
        private IWorkbenchService _workbenchService;
        public WorkbenchViewBase()
        {
            InitializeComponent();
            this.DockAreas = SailingEase.Controls.Docking.DockAreas.Document;
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
           
            if (DesignMode)
                return;
            _workbenchService = ServiceUnity.Container.Resolve<IWorkbenchService>();
        }
        protected override void OnVisibleChanged(EventArgs e)
        {
           
            if (this.Disposing == false)
            {
                if (this.Visible)
                {
                    if (ToolStripList != null && ToolStripList.Count > 0)
                    {
                        foreach (ToolStripCodon toolStripCodon in ToolStripList)
                        {
                            _workbenchService.ActivateToolStrip(toolStripCodon);
                        }
                    }
                }
                else
                {
                    if (ToolStripList != null && ToolStripList.Count > 0)
                    {
                        foreach (ToolStripCodon toolStripCodon in ToolStripList)
                        {
                            _workbenchService.DeactiveToolStrip(toolStripCodon);
                        }
                    }
                }
            }
            base.OnVisibleChanged(e);
        }
        protected override void OnClosed(EventArgs e)
        {
            _workbenchService.DestroyToolStrip(ToolStripList);
            base.OnClosed(e);
        }
        public virtual List<ToolStripCodon> ToolStripList
        {
            get { return null; }
        }
        public virtual string Title
        {
            get { return this.TabText; }
        }
        private bool _single = false;
        public virtual bool Single
        {
            get { return _single; }
            set { _single = value; }
        }
        private object _singleKey = null;
        public virtual object SingleKey
        {
            get { return _singleKey; }
            set { _singleKey = value; }
        }
        public virtual bool CompareSingleKey(object singleKey)
        {
            if (_singleKey != null)
                return _singleKey.Equals(singleKey);
            else
                return false;
        }
    }
}
