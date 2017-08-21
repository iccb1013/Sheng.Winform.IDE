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
using Sheng.SailingEase.Components.Window.DesignComponent.Localisation;
using Sheng.SailingEase.Controls.Docking;
using Sheng.SailingEase.Controls.Extensions;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Kernal;
namespace Sheng.SailingEase.Components.Window.DesignComponent
{
    class HostingTabPageContextMenuStrip
    {
        private IWorkbenchService _workbenchService;
        private static InstanceLazy<HostingTabPageContextMenuStrip> _instance =
            new InstanceLazy<HostingTabPageContextMenuStrip>(() => new HostingTabPageContextMenuStrip());
        public static HostingTabPageContextMenuStrip Instance
        {
            get { return _instance.Value; }
        }
        public ContextMenuStripView MenuStrip
        {
            get;
            private set;
        }
        private HostingTabPageContextMenuStrip()
        {
            _workbenchService = ServiceUnity.WorkbenchService;
            ContextMenuStripCodon contextMenuStripCodon = new ContextMenuStripCodon("HostingTabPageContextMenuStrip");
            contextMenuStripCodon.Items.Add(new ToolStripMenuItemCodon("Close", Language.Current.HostingTabPageContextMenuStrip_Close,
                delegate(object sender, ToolStripItemCodonEventArgs codon)
                {
                    _workbenchService.CloseView(FormHostingContainer.Instance.ActiveHosting);
                }));
            contextMenuStripCodon.Items.Add(new ToolStripMenuItemCodon("CloseAll", Language.Current.HostingTabPageContextMenuStrip_CloseAll,
               delegate(object sender, ToolStripItemCodonEventArgs codon)
               {
                   IDockContent[] dockContent = FormHostingContainer.Instance.Hostings.ToArray();
                   for (int i = 0; i < dockContent.Length; i++)
                   {
                       IView view = dockContent[i] as IView;
                       _workbenchService.CloseView(view);
                   }
               }));
            contextMenuStripCodon.Items.Add(new ToolStripMenuItemCodon("CloseAllButThis", Language.Current.HostingTabPageContextMenuStrip_CloseAllButThis,
               delegate(object sender, ToolStripItemCodonEventArgs codon)
               {
                   IDockContent[] dockContent = FormHostingContainer.Instance.Hostings.ToArray();
                   for (int i = 0; i < dockContent.Length; i++)
                   {
                       if (dockContent[i] != FormHostingContainer.Instance.ActiveHosting)
                       {
                           IView view = dockContent[i] as IView;
                           _workbenchService.CloseView(view);
                       }
                   }
               }));
            MenuStrip = new ContextMenuStripView(contextMenuStripCodon);
        }
    }
}
