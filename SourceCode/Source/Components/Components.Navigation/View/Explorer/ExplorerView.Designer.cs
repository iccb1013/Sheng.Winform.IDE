/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Components.NavigationComponent.View
{
    partial class ExplorerView
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {
            Sheng.SailingEase.Controls.Docking.DockPanelSkin dockPanelSkin1 = new Sheng.SailingEase.Controls.Docking.DockPanelSkin();
            Sheng.SailingEase.Controls.Docking.AutoHideStripSkin autoHideStripSkin1 = new Sheng.SailingEase.Controls.Docking.AutoHideStripSkin();
            Sheng.SailingEase.Controls.Docking.DockPanelGradient dockPanelGradient1 = new Sheng.SailingEase.Controls.Docking.DockPanelGradient();
            Sheng.SailingEase.Controls.Docking.TabGradient tabGradient1 = new Sheng.SailingEase.Controls.Docking.TabGradient();
            Sheng.SailingEase.Controls.Docking.DockPaneStripSkin dockPaneStripSkin1 = new Sheng.SailingEase.Controls.Docking.DockPaneStripSkin();
            Sheng.SailingEase.Controls.Docking.DockPaneStripGradient dockPaneStripGradient1 = new Sheng.SailingEase.Controls.Docking.DockPaneStripGradient();
            Sheng.SailingEase.Controls.Docking.TabGradient tabGradient2 = new Sheng.SailingEase.Controls.Docking.TabGradient();
            Sheng.SailingEase.Controls.Docking.DockPanelGradient dockPanelGradient2 = new Sheng.SailingEase.Controls.Docking.DockPanelGradient();
            Sheng.SailingEase.Controls.Docking.TabGradient tabGradient3 = new Sheng.SailingEase.Controls.Docking.TabGradient();
            Sheng.SailingEase.Controls.Docking.DockPaneStripToolWindowGradient dockPaneStripToolWindowGradient1 = new Sheng.SailingEase.Controls.Docking.DockPaneStripToolWindowGradient();
            Sheng.SailingEase.Controls.Docking.TabGradient tabGradient4 = new Sheng.SailingEase.Controls.Docking.TabGradient();
            Sheng.SailingEase.Controls.Docking.TabGradient tabGradient5 = new Sheng.SailingEase.Controls.Docking.TabGradient();
            Sheng.SailingEase.Controls.Docking.DockPanelGradient dockPanelGradient3 = new Sheng.SailingEase.Controls.Docking.DockPanelGradient();
            Sheng.SailingEase.Controls.Docking.TabGradient tabGradient6 = new Sheng.SailingEase.Controls.Docking.TabGradient();
            Sheng.SailingEase.Controls.Docking.TabGradient tabGradient7 = new Sheng.SailingEase.Controls.Docking.TabGradient();
            this.dockPanel = new Sheng.SailingEase.Controls.Docking.DockPanel();
            this.SuspendLayout();
            this.dockPanel.ActiveAutoHideContent = null;
            this.dockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel.DockBackColor = System.Drawing.SystemColors.Control;
            this.dockPanel.DocumentStyle = Sheng.SailingEase.Controls.Docking.DocumentStyle.DockingSdi;
            this.dockPanel.Location = new System.Drawing.Point(0, 0);
            this.dockPanel.Name = "dockPanel";
            this.dockPanel.Size = new System.Drawing.Size(284, 262);
            dockPanelGradient1.EndColor = System.Drawing.SystemColors.ControlLight;
            dockPanelGradient1.StartColor = System.Drawing.SystemColors.ControlLight;
            autoHideStripSkin1.DockStripGradient = dockPanelGradient1;
            tabGradient1.EndColor = System.Drawing.SystemColors.Control;
            tabGradient1.StartColor = System.Drawing.SystemColors.Control;
            tabGradient1.TextColor = System.Drawing.SystemColors.ControlDarkDark;
            autoHideStripSkin1.TabGradient = tabGradient1;
            dockPanelSkin1.AutoHideStripSkin = autoHideStripSkin1;
            tabGradient2.EndColor = System.Drawing.SystemColors.ControlDarkDark;
            tabGradient2.StartColor = System.Drawing.SystemColors.ControlDarkDark;
            tabGradient2.TextColor = System.Drawing.SystemColors.HighlightText;
            dockPaneStripGradient1.ActiveTabGradient = tabGradient2;
            dockPanelGradient2.EndColor = System.Drawing.SystemColors.Control;
            dockPanelGradient2.StartColor = System.Drawing.SystemColors.Control;
            dockPaneStripGradient1.DockStripGradient = dockPanelGradient2;
            tabGradient3.EndColor = System.Drawing.SystemColors.ControlLight;
            tabGradient3.StartColor = System.Drawing.SystemColors.ControlLight;
            tabGradient3.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripGradient1.InactiveTabGradient = tabGradient3;
            dockPaneStripSkin1.DocumentGradient = dockPaneStripGradient1;
            tabGradient4.EndColor = System.Drawing.SystemColors.ActiveCaption;
            tabGradient4.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient4.StartColor = System.Drawing.SystemColors.GradientActiveCaption;
            tabGradient4.TextColor = System.Drawing.SystemColors.ActiveCaptionText;
            dockPaneStripToolWindowGradient1.ActiveCaptionGradient = tabGradient4;
            tabGradient5.EndColor = System.Drawing.SystemColors.Control;
            tabGradient5.StartColor = System.Drawing.SystemColors.Control;
            tabGradient5.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripToolWindowGradient1.ActiveTabGradient = tabGradient5;
            dockPanelGradient3.EndColor = System.Drawing.SystemColors.ControlLight;
            dockPanelGradient3.StartColor = System.Drawing.SystemColors.ControlLight;
            dockPaneStripToolWindowGradient1.DockStripGradient = dockPanelGradient3;
            tabGradient6.EndColor = System.Drawing.SystemColors.GradientInactiveCaption;
            tabGradient6.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient6.StartColor = System.Drawing.SystemColors.GradientInactiveCaption;
            tabGradient6.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripToolWindowGradient1.InactiveCaptionGradient = tabGradient6;
            tabGradient7.EndColor = System.Drawing.Color.Transparent;
            tabGradient7.StartColor = System.Drawing.Color.Transparent;
            tabGradient7.TextColor = System.Drawing.SystemColors.ControlDarkDark;
            dockPaneStripToolWindowGradient1.InactiveTabGradient = tabGradient7;
            dockPaneStripSkin1.ToolWindowGradient = dockPaneStripToolWindowGradient1;
            dockPanelSkin1.DockPaneStripSkin = dockPaneStripSkin1;
            this.dockPanel.Skin = dockPanelSkin1;
            this.dockPanel.TabIndex = 3;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.dockPanel);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "ExplorerView";
            this.Text = "NavigationView";
            this.Load += new System.EventHandler(this.ExplorerView_Load);
            this.ResumeLayout(false);
        }
        private Controls.Docking.DockPanel dockPanel;
    }
}
