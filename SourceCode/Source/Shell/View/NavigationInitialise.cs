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
using Sheng.SailingEase.Controls.Extensions;
using Sheng.SailingEase.Shell.Localisation;
using System.Windows.Forms;
namespace Sheng.SailingEase.Shell.View
{
    class NavigationInitialise
    {
        public static void InitialiseMenu(MenuStripAreoView menu)
        {
            menu.RegisterItem("Main", new ToolStripMenuItemCodon("File", Language.Current.Navigation_Menu_File));
            menu.RegisterItem("Main", new ToolStripMenuItemCodon("Edit", Language.Current.Navigation_Menu_Edit));
            menu.RegisterItem("Main", new ToolStripMenuItemCodon("View", Language.Current.Navigation_Menu_View));
            menu.RegisterItem("Main", new ToolStripMenuItemCodon("Tool", Language.Current.Navigation_Menu_Tool));
            menu.RegisterItem("Main", new ToolStripMenuItemCodon("Build", Language.Current.Navigation_Menu_Build));
            menu.RegisterItem("Main", new ToolStripMenuItemCodon("Help", Language.Current.Navigation_Menu_Help));
            menu.RegisterItem("Main/File", new ToolStripMenuItemCodon("Exit", Language.Current.Navigation_Menu_File_Exit,
                (sender, codon) => { Application.Exit(); }) { Description = "退出应用程序" });
        }
        public static void InitialiseToolStrip(ToolStripView toolStrip)
        {
        }
    }
}
