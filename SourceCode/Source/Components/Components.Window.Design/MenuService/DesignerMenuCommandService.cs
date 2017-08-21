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
using System.Windows.Forms.Design;
using Sheng.SailingEase.Controls;
using Sheng.SailingEase.Controls.Extensions;
using CommandID = System.ComponentModel.Design.CommandID;
using MenuCommand = System.ComponentModel.Design.MenuCommand;
namespace Sheng.SailingEase.Components.Window.DesignComponent
{
    class DesignerMenuCommandService : MenuCommandService
    {
        public DesignerMenuCommandService( IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            AbstractContextMenuCommand properties = new ContextMenuCommandProperties(this);
            this.AddCommand(new MenuCommand(properties.CommandCallBack, properties.CommandID));
        }
        public override void ShowContextMenu(CommandID menuID, int x, int y)
        {
            ContextMenuStripCodon contextMenuStripCodon = null;
            if (menuID == MenuCommands.ComponentTrayMenu)
            {
            }
            else if (menuID == MenuCommands.ContainerMenu)
            {
                contextMenuStripCodon = new ContainerMenu(this);
            }
            else if (menuID == MenuCommands.SelectionMenu)
            {
                contextMenuStripCodon = new SelectionMenu(this);
            }
            else if (menuID == MenuCommands.TraySelectionMenu)
            {
            }
            else
            {
                throw new Exception("ShowContextMenu Error");
            }
            if (contextMenuStripCodon != null)
            {
                contextMenuStripCodon.View.Renderer = ToolStripRenders.Default;
                contextMenuStripCodon.View.Show(x, y);
            }
        }
    }
}
