/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Controls
{
    partial class SEMessageBoxCmdLink
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SEMessageBoxCmdLink));
            this.buttonToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.panelIcon = new System.Windows.Forms.Panel();
            this.imageListIcons = new System.Windows.Forms.ImageList(this.components);
            this.rtbMessage = new System.Windows.Forms.RichTextBox();
            this.chbSaveResponse = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            this.panelIcon.BackColor = System.Drawing.Color.Transparent;
            this.panelIcon.Location = new System.Drawing.Point(30, 31);
            this.panelIcon.Name = "panelIcon";
            this.panelIcon.Size = new System.Drawing.Size(32, 35);
            this.panelIcon.TabIndex = 6;
            this.panelIcon.Visible = false;
            this.imageListIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListIcons.ImageStream")));
            this.imageListIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListIcons.Images.SetKeyName(0, "");
            this.imageListIcons.Images.SetKeyName(1, "");
            this.imageListIcons.Images.SetKeyName(2, "");
            this.imageListIcons.Images.SetKeyName(3, "");
            this.rtbMessage.BackColor = System.Drawing.SystemColors.Control;
            this.rtbMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbMessage.Location = new System.Drawing.Point(222, 31);
            this.rtbMessage.Name = "rtbMessage";
            this.rtbMessage.ReadOnly = true;
            this.rtbMessage.Size = new System.Drawing.Size(100, 52);
            this.rtbMessage.TabIndex = 7;
            this.rtbMessage.Text = "";
            this.rtbMessage.Visible = false;
            this.chbSaveResponse.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chbSaveResponse.Location = new System.Drawing.Point(78, 83);
            this.chbSaveResponse.Name = "chbSaveResponse";
            this.chbSaveResponse.Size = new System.Drawing.Size(104, 17);
            this.chbSaveResponse.TabIndex = 5;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(381, 309);
            this.Controls.Add(this.panelIcon);
            this.Controls.Add(this.rtbMessage);
            this.Controls.Add(this.chbSaveResponse);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SEMessageBoxCmdLink";
            this.Text = "SEMessageBoxCmdLink";
            this.ResumeLayout(false);
        }
        private System.Windows.Forms.ToolTip buttonToolTip;
        private System.Windows.Forms.Panel panelIcon;
        private System.Windows.Forms.ImageList imageListIcons;
        private System.Windows.Forms.RichTextBox rtbMessage;
        private System.Windows.Forms.CheckBox chbSaveResponse;
    }
}
