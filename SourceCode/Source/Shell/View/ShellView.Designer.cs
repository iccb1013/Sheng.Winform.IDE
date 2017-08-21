namespace Sheng.SailingEase.Shell.View
{
    partial class ShellView
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
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
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.dockPanel = new Sheng.SailingEase.Controls.Docking.DockPanel();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusMessage,
            this.toolStripStatusLabel1});
            this.statusStrip.Location = new System.Drawing.Point(0, 274);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(550, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusMessage
            // 
            this.toolStripStatusMessage.Name = "toolStripStatusMessage";
            this.toolStripStatusMessage.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(535, 17);
            this.toolStripStatusLabel1.Spring = true;
            // 
            // dockPanel
            // 
            this.dockPanel.ActiveAutoHideContent = null;
            this.dockPanel.AllowEndUserNestedDocking = false;
            this.dockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel.DockBackColor = System.Drawing.SystemColors.Control;
            this.dockPanel.DocumentStyle = Sheng.SailingEase.Controls.Docking.DocumentStyle.DockingWindow;
            this.dockPanel.Location = new System.Drawing.Point(0, 0);
            this.dockPanel.Name = "dockPanel";
            this.dockPanel.Size = new System.Drawing.Size(550, 274);
            dockPanelGradient1.EndColor = System.Drawing.SystemColors.ControlLight;
            dockPanelGradient1.StartColor = System.Drawing.SystemColors.ControlLight;
            autoHideStripSkin1.DockStripGradient = dockPanelGradient1;
            tabGradient1.EndColor = System.Drawing.SystemColors.Control;
            tabGradient1.StartColor = System.Drawing.SystemColors.Control;
            tabGradient1.TextColor = System.Drawing.SystemColors.ControlDarkDark;
            autoHideStripSkin1.TabGradient = tabGradient1;
            dockPanelSkin1.AutoHideStripSkin = autoHideStripSkin1;
            tabGradient2.EndColor = System.Drawing.SystemColors.Window;
            tabGradient2.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient2.StartColor = System.Drawing.SystemColors.Window;
            tabGradient2.TextColor = System.Drawing.SystemColors.ControlText;
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
            this.dockPanel.TabIndex = 13;
            // 
            // ShellView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(550, 296);
            this.Controls.Add(this.dockPanel);
            this.Controls.Add(this.statusStrip);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.IsMdiContainer = true;
            this.Name = "ShellView";
            this.Text = "${FormMain}";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ShellView_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ShellView_FormClosed);
            this.Load += new System.EventHandler(this.ShellView_Load);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusMessage;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private Sheng.SailingEase.Controls.Docking.DockPanel dockPanel;


    }
}

