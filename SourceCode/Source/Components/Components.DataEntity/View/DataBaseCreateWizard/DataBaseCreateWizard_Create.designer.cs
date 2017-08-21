/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Components.DataEntityComponent.View
{
    partial class DataBaseCreateWizard_Create
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
            this.panelCreate = new Sheng.SailingEase.Controls.SEPanel();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.panelError = new Sheng.SailingEase.Controls.SEPanel();
            this.txtExceptionMsg = new Sheng.SailingEase.Controls.SETextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panelCreate.SuspendLayout();
            this.panelError.SuspendLayout();
            this.SuspendLayout();
            this.panelCreate.BorderColor = System.Drawing.Color.Black;
            this.panelCreate.Controls.Add(this.progressBar1);
            this.panelCreate.Controls.Add(this.label1);
            this.panelCreate.FillColorEnd = System.Drawing.Color.Empty;
            this.panelCreate.FillColorStart = System.Drawing.Color.Empty;
            this.panelCreate.FillMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.panelCreate.FillStyle = Sheng.SailingEase.Controls.FillStyle.Solid;
            this.panelCreate.HighLight = false;
            this.panelCreate.Location = new System.Drawing.Point(116, 3);
            this.panelCreate.Name = "panelCreate";
            this.panelCreate.ShowBorder = false;
            this.panelCreate.Size = new System.Drawing.Size(508, 250);
            this.panelCreate.TabIndex = 0;
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(54, 29);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(400, 15);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 1;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(341, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "${UserControlCreateDataBaseWizardStepCreate_LabelCreate}";
            this.panelError.BorderColor = System.Drawing.Color.Black;
            this.panelError.Controls.Add(this.txtExceptionMsg);
            this.panelError.Controls.Add(this.label2);
            this.panelError.FillColorEnd = System.Drawing.Color.Empty;
            this.panelError.FillColorStart = System.Drawing.Color.Empty;
            this.panelError.FillMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.panelError.FillStyle = Sheng.SailingEase.Controls.FillStyle.Solid;
            this.panelError.HighLight = false;
            this.panelError.Location = new System.Drawing.Point(20, 32);
            this.panelError.Name = "panelError";
            this.panelError.ShowBorder = false;
            this.panelError.Size = new System.Drawing.Size(508, 250);
            this.panelError.TabIndex = 1;
            this.txtExceptionMsg.AllowEmpty = true;
            this.txtExceptionMsg.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtExceptionMsg.BackColor = System.Drawing.SystemColors.Control;
            this.txtExceptionMsg.HighLight = true;
            this.txtExceptionMsg.LimitMaxValue = false;
            this.txtExceptionMsg.Location = new System.Drawing.Point(46, 33);
            this.txtExceptionMsg.MaxValue = ((long)(2147483647));
            this.txtExceptionMsg.Multiline = true;
            this.txtExceptionMsg.Name = "txtExceptionMsg";
            this.txtExceptionMsg.ReadOnly = true;
            this.txtExceptionMsg.Regex = "";
            this.txtExceptionMsg.RegexMsg = null;
            this.txtExceptionMsg.Size = new System.Drawing.Size(416, 144);
            this.txtExceptionMsg.TabIndex = 2;
            this.txtExceptionMsg.Title = null;
            this.txtExceptionMsg.ValueCompareTo = null;
            this.txtExceptionMsg.WaterText = "";
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(-2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(347, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "${UserControlCreateDataBaseWizardStepCreate_LabelFailure}";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelError);
            this.Controls.Add(this.panelCreate);
            this.Name = "UserControlCreateDataBaseWizardStepCreate";
            this.Size = new System.Drawing.Size(657, 370);
            this.panelCreate.ResumeLayout(false);
            this.panelCreate.PerformLayout();
            this.panelError.ResumeLayout(false);
            this.panelError.PerformLayout();
            this.ResumeLayout(false);
        }
        private Sheng.SailingEase.Controls.SEPanel panelCreate;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label1;
        private Sheng.SailingEase.Controls.SEPanel panelError;
        private System.Windows.Forms.Label label2;
        private Sheng.SailingEase.Controls.SETextBox txtExceptionMsg;
    }
}
