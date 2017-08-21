/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sheng.SailingEase.Controls.Extensions;
using Sheng.SailingEase.Localisation;
namespace Sheng.SailingEase.Components.Window.DesignComponent
{
    class SelectionMenu : ContextMenuStripCodon
    {
        private Func<IToolStripItemCodon, bool> _getEnabled;
        public SelectionMenu(MenuCommandService menuCommandService)
            : base("SelectionMenu")
        {
            _getEnabled = delegate(IToolStripItemCodon codon)
            {
                DesignerContextMenuStripItemCodon contextMenuCodon = codon as DesignerContextMenuStripItemCodon;
                return contextMenuCodon.ContextMenuCommand.Enabled;
            };
            this.Items.Add(new DesignerContextMenuStripItemCodon(new ContextMenuCommandShowEvents(menuCommandService), IconsLibrary.Event)
            {
                IsEnabled = _getEnabled
            });
            this.Items.Add(new ToolStripSeparatorCodon());
            this.Items.Add(new DesignerContextMenuStripItemCodon(new ContextMenuCommandBringToFront(menuCommandService), IconsLibrary.BringToFrontHS)
            {
                IsEnabled = _getEnabled
            });
            this.Items.Add(new DesignerContextMenuStripItemCodon(new ContextMenuCommandSendToBack(menuCommandService), IconsLibrary.SendToBackHS)
            {
                IsEnabled = _getEnabled
            });
            this.Items.Add(new ToolStripSeparatorCodon());
            this.Items.Add(new DesignerContextMenuStripItemCodon(new ContextMenuCommandLockControls(menuCommandService))
            {
                IsEnabled = _getEnabled
            });
            this.Items.Add(new ToolStripSeparatorCodon());
            foreach (DesignerVerb verb in menuCommandService.Verbs)
            {
                this.Items.Add(new DesignerContextMenuStripItemCodon(verb));
            }
            if (menuCommandService.Verbs.Count > 0)
            {
                this.Items.Add(new ToolStripSeparatorCodon());
            }
            this.Items.Add(new DesignerContextMenuStripItemCodon(new ContextMenuCommandCut(menuCommandService), IconsLibrary.Cut)
            {
                IsEnabled = _getEnabled
            });
            this.Items.Add(new DesignerContextMenuStripItemCodon(new ContextMenuCommandCopy(menuCommandService), IconsLibrary.Copy)
            {
                IsEnabled = _getEnabled
            });
            this.Items.Add(new DesignerContextMenuStripItemCodon(new ContextMenuCommandPaste(menuCommandService), IconsLibrary.Paste)
            {
                IsEnabled = _getEnabled
            });
            this.Items.Add(new DesignerContextMenuStripItemCodon(new ContextMenuCommandDelete(menuCommandService), IconsLibrary.Delete)
            {
                IsEnabled = _getEnabled
            });
            this.Items.Add(new ToolStripSeparatorCodon());
            this.Items.Add(new DesignerContextMenuStripItemCodon(new ContextMenuCommandProperties(menuCommandService), IconsLibrary.Property)
            {
                IsEnabled = _getEnabled
            });
        }
    }
}
