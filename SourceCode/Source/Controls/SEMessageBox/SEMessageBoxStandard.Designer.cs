/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Controls
{
    partial class SEMessageBoxStandard
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
            this.components = new System.ComponentModel.Container();
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(SEMessageBoxStandard));
            this.panelIcon = new System.Windows.Forms.Panel();
            this.chbSaveResponse = new System.Windows.Forms.CheckBox();
            this.imageListIcons = new System.Windows.Forms.ImageList(this.components);
            this.buttonToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.rtbMessage = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            this.panelIcon.BackColor = System.Drawing.Color.Transparent;
            this.panelIcon.Location = new System.Drawing.Point(8, 8);
            this.panelIcon.Name = "panelIcon";
            this.panelIcon.Size = new System.Drawing.Size(32, 32);
            this.panelIcon.TabIndex = 3;
            this.panelIcon.Visible = false;
            this.chbSaveResponse.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chbSaveResponse.Location = new System.Drawing.Point(56, 56);
            this.chbSaveResponse.Name = "chbSaveResponse";
            this.chbSaveResponse.Size = new System.Drawing.Size(104, 16);
            this.chbSaveResponse.TabIndex = 0;
            this.imageListIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth16Bit;
            this.imageListIcons.ImageSize = new System.Drawing.Size(32, 32);
            this.imageListIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListIcons.ImageStream")));
            this.imageListIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.rtbMessage.BackColor = System.Drawing.SystemColors.Control;
            this.rtbMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbMessage.Location = new System.Drawing.Point(200, 8);
            this.rtbMessage.Name = "rtbMessage";
            this.rtbMessage.ReadOnly = true;
            this.rtbMessage.Size = new System.Drawing.Size(100, 48);
            this.rtbMessage.TabIndex = 4;
            this.rtbMessage.Text = "";
            this.rtbMessage.Visible = false;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.ClientSize = new System.Drawing.Size(322, 224);
            this.Controls.Add(this.rtbMessage);
            this.Controls.Add(this.chbSaveResponse);
            this.Controls.Add(this.panelIcon);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MessageBoxExForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.ResumeLayout(false);
        }
        private System.Windows.Forms.ToolTip buttonToolTip;
        private System.Windows.Forms.Panel panelIcon;
        private System.Windows.Forms.ImageList imageListIcons;
        private System.Windows.Forms.RichTextBox rtbMessage;
        private System.Windows.Forms.CheckBox chbSaveResponse;
    }
}
