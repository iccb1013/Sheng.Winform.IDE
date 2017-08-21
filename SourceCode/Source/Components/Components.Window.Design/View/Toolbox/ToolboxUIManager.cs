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
using System.ComponentModel;
namespace Sheng.SailingEase.Components.Window.DesignComponent
{
    internal class ToolboxUIManager
    {
        private Toolbox _toolbox;
        private Toolbox Toolbox
        {
            get
            {
                return _toolbox;
            }
        }
        public ToolboxUIManager(Toolbox toolbox)
        {
            _toolbox = toolbox;
        }
        public void FillToolbox()
        {
            CreateControls();
            ConfigureControls();
        }
        private void CreateControls()
        {
            Toolbox.Controls.Clear();
            Toolbox.ToolboxItemContainer = new ToolboxItemContainer[Toolbox.Tabs.Count];
            Toolbox.TabPageArray = new ToolboxTabButton[Toolbox.Tabs.Count];
        }
        private void ConfigureControls()
        {
            Toolbox.SuspendLayout();
            for (int i = Toolbox.Tabs.Count - 1; i >= 0; i--)
            {
                ToolboxItemContainer toolboxItemContainer = new ToolboxItemContainer(this.Toolbox);
                toolboxItemContainer.Name = "ToolboxItemContainer" + i.ToString();
                toolboxItemContainer.Dock = DockStyle.Top;
                toolboxItemContainer.TabIndex = Toolbox.Tabs.Count + 1;
                toolboxItemContainer.ToolboxItems = Toolbox.Tabs[i].ToolboxItems;
                Toolbox.ToolboxItemContainer[i] = toolboxItemContainer;
                Toolbox.Controls.Add(toolboxItemContainer);
                ToolboxTabButton toolboxTabButton = new ToolboxTabButton();
                toolboxTabButton.ItemContainer = toolboxItemContainer;
                toolboxTabButton.Dock = System.Windows.Forms.DockStyle.Top;
                toolboxTabButton.Name = Toolbox.Tabs[i].Name;
                toolboxTabButton.TabIndex = i + 1;
                toolboxTabButton.Text = Toolbox.Tabs[i].Name;
                toolboxTabButton.Tag = i;
                Toolbox.TabPageArray[i] = toolboxTabButton;
                Toolbox.Controls.Add(toolboxTabButton);
            }
            Toolbox.ResumeLayout();
        }
    }
}
