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
using Sheng.SailingEase.Controls.Extensions;
using Sheng.SailingEase.Localisation;
namespace Sheng.SailingEase.Components.Window.DesignComponent
{
    class ContainerMenu : ContextMenuStripCodon
    {
        private Func<IToolStripItemCodon, bool> _getEnabled;
        public ContainerMenu(MenuCommandService menuCommandService)
            : base("ContainerMenu")
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
            this.Items.Add(new DesignerContextMenuStripItemCodon(new ContextMenuCommandTabOrder(menuCommandService), IconsLibrary.TabOrder)
            {
                IsEnabled = _getEnabled
            });
            this.Items.Add(new DesignerContextMenuStripItemCodon(new ContextMenuCommandLockControls(menuCommandService))
            {
                IsEnabled = _getEnabled
            });
            this.Items.Add(new ToolStripSeparatorCodon());
            this.Items.Add(new DesignerContextMenuStripItemCodon(new ContextMenuCommandPaste(menuCommandService), IconsLibrary.Paste)
            {
                IsEnabled = delegate(IToolStripItemCodon codon)
                {
                    DesignerContextMenuStripItemCodon contextMenuCodon = codon as DesignerContextMenuStripItemCodon;
                    return contextMenuCodon.ContextMenuCommand.Enabled;
                }
            });
            this.Items.Add(new ToolStripSeparatorCodon());
            this.Items.Add(new DesignerContextMenuStripItemCodon(new ContextMenuCommandProperties(menuCommandService), IconsLibrary.Property)
            {
                IsEnabled = _getEnabled
            });
        }
    }
}
