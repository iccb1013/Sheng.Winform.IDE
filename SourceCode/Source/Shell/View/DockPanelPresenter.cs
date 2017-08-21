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
using Sheng.SailingEase.Controls.Docking;
using System.Drawing;
namespace Sheng.SailingEase.Shell.View
{
    class DockPanelPresenter
    {
        DockPanel _dockPanel;
        public DockPanelPresenter(DockPanel dockpanel)
        {
            _dockPanel = dockpanel;
            SetSkin();
        }
        private void SetSkin()
        {
            DockPanelSkin dockPanelSkin = new DockPanelSkin();
            DockPaneStripSkin dockPaneStripSkin = new DockPaneStripSkin();
            DockPaneStripGradient dockPaneStripGradient = new DockPaneStripGradient();
            DockPanelGradient dockPanelGradient = new DockPanelGradient();
            dockPanelGradient.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            dockPanelGradient.EndColor = Color.FromArgb(235, 241, 250); 
            dockPanelGradient.StartColor =Color.FromArgb(245, 248, 252);
            dockPaneStripGradient.DockStripGradient = dockPanelGradient;
            dockPaneStripGradient.ActiveTabGradient.StartColor = Color.White;
            dockPaneStripGradient.ActiveTabGradient.EndColor = Color.White;
            dockPaneStripSkin.DocumentGradient = dockPaneStripGradient;
            dockPanelSkin.DockPaneStripSkin = dockPaneStripSkin;
            this._dockPanel.Skin = dockPanelSkin;
        }
    }
}
