/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Components.DictionaryComponent.View
{
    partial class EnumItemEntityEditView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EnumItemEntityEditView));
            this.lblValue = new System.Windows.Forms.Label();
            this.lblText = new System.Windows.Forms.Label();
            this.txtValue = new Sheng.SailingEase.Controls.SETextBox();
            this.txtText = new Sheng.SailingEase.Controls.SETextBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonEquals = new System.Windows.Forms.ToolStripButton();
            this.btnOK = new Sheng.SailingEase.Controls.SEButton();
            this.btnCancel = new Sheng.SailingEase.Controls.SEButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            this.lblValue.AutoSize = true;
            this.lblValue.Location = new System.Drawing.Point(22, 49);
            this.lblValue.Name = "lblValue";
            this.lblValue.Size = new System.Drawing.Size(191, 12);
            this.lblValue.TabIndex = 15;
            this.lblValue.Text = "${EnumItemEditView_LabelValue}:";
            this.lblText.AutoSize = true;
            this.lblText.Location = new System.Drawing.Point(22, 22);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(185, 12);
            this.lblText.TabIndex = 14;
            this.lblText.Text = "${EnumItemEditView_LabelText}:";
            this.txtValue.AllowEmpty = false;
            this.txtValue.CustomValidate = null;
            this.txtValue.HighLight = true;
            this.txtValue.LimitMaxValue = false;
            this.txtValue.Location = new System.Drawing.Point(97, 46);
            this.txtValue.MaxLength = 100;
            this.txtValue.MaxValue = ((long)(2147483647));
            this.txtValue.Name = "txtValue";
            this.txtValue.ReadOnly = true;
            this.txtValue.Regex = "";
            this.txtValue.RegexMsg = "";
            this.txtValue.Size = new System.Drawing.Size(215, 21);
            this.txtValue.TabIndex = 13;
            this.txtValue.Title = "LabelValue";
            this.txtValue.ValueCompareTo = null;
            this.txtValue.WaterText = "";
            this.txtText.AllowEmpty = false;
            this.txtText.CustomValidate = null;
            this.txtText.HighLight = true;
            this.txtText.LimitMaxValue = false;
            this.txtText.Location = new System.Drawing.Point(97, 19);
            this.txtText.MaxLength = 100;
            this.txtText.MaxValue = ((long)(2147483647));
            this.txtText.Name = "txtText";
            this.txtText.Regex = "";
            this.txtText.RegexMsg = null;
            this.txtText.Size = new System.Drawing.Size(244, 21);
            this.txtText.TabIndex = 12;
            this.txtText.Title = "LabelText";
            this.txtText.ValueCompareTo = null;
            this.txtText.WaterText = "";
            this.txtText.TextChanged += new System.EventHandler(this.txtText_TextChanged);
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonEquals});
            this.toolStrip1.Location = new System.Drawing.Point(318, 46);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(26, 25);
            this.toolStrip1.TabIndex = 16;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStripButtonEquals.Checked = true;
            this.toolStripButtonEquals.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripButtonEquals.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonEquals.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonEquals.Image")));
            this.toolStripButtonEquals.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonEquals.Name = "toolStripButtonEquals";
            this.toolStripButtonEquals.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonEquals.Text = "=";
            this.toolStripButtonEquals.Click += new System.EventHandler(this.toolStripButtonEquals_Click);
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(237, 121);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 17;
            this.btnOK.Text = "${EnumItemEditView_ButtonOK}";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(318, 121);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 18;
            this.btnCancel.Text = "${EnumItemEditView_ButtonCancel}";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(405, 156);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.lblValue);
            this.Controls.Add(this.lblText);
            this.Controls.Add(this.txtValue);
            this.Controls.Add(this.txtText);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormEnumItemAdd";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "${EnumItemEditView}";
            this.Load += new System.EventHandler(this.EnumItemEditView_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private System.Windows.Forms.Label lblValue;
        private System.Windows.Forms.Label lblText;
        private Sheng.SailingEase.Controls.SETextBox txtValue;
        private Sheng.SailingEase.Controls.SETextBox txtText;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonEquals;
        private Sheng.SailingEase.Controls.SEButton btnOK;
        private Sheng.SailingEase.Controls.SEButton btnCancel;
    }
}
