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
using System.Windows.Forms;
using Sheng.SailingEase.Kernal;
using Sheng.SailingEase.Controls;
using Sheng.SailingEase.Shell.Localisation;
using Sheng.SailingEase.Infrastructure;
namespace Sheng.SailingEase.Shell
{
    class GlobalTabPageContextMenuStrip : ContextMenuStrip
    {
        private static InstanceLazy<GlobalTabPageContextMenuStrip> _instance =
            new InstanceLazy<GlobalTabPageContextMenuStrip>(() => new GlobalTabPageContextMenuStrip());
        public static GlobalTabPageContextMenuStrip Instance
        {
            get { return _instance.Value; }
        }
        private IWorkbenchService _workbenchService;
        private GlobalTabPageContextMenuStrip()
        {
            _workbenchService = ServiceUnity.WorkbenchService;
            this.Renderer = ToolStripRenders.Default;
            ToolStripMenuItem close = new ToolStripMenuItem(Language.Current.GlobalTabPageContextMenuStrip_Close);
            close.Click += new EventHandler(close_Click);
            ToolStripMenuItem closeAll = new ToolStripMenuItem(Language.Current.GlobalTabPageContextMenuStrip_CloseAll);
            closeAll.Click += new EventHandler(closeAll_Click);
            ToolStripMenuItem closeAllButThis = new ToolStripMenuItem(Language.Current.GlobalTabPageContextMenuStrip_CloseAllButThis);
            closeAllButThis.Click += new EventHandler(closeAllButThis_Click);
            this.Items.Add(close);
            this.Items.Add(closeAll);
            this.Items.Add(closeAllButThis);
        }
        void closeAllButThis_Click(object sender, EventArgs e)
        {
            _workbenchService.CloseAllViewButThis();
        }
        void closeAll_Click(object sender, EventArgs e)
        {
            _workbenchService.CloseAllView();
        }
        void close_Click(object sender, EventArgs e)
        {
            _workbenchService.CloseView();
        }
    }
}
