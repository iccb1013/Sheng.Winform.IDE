/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Core.Development
{
    partial class DataEntityTreeChooseView
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
            this.treeViewDataEntity = new Sheng.SailingEase.Controls.SETreeView();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.txtSelectedName = new Sheng.SailingEase.Controls.SETextBox();
            this.btnClear = new Sheng.SailingEase.Controls.SEButton();
            this.SuspendLayout();
            this.treeViewDataEntity.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewDataEntity.HideSelection = false;
            this.treeViewDataEntity.HotTracking = true;
            this.treeViewDataEntity.Location = new System.Drawing.Point(12, 42);
            this.treeViewDataEntity.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.treeViewDataEntity.Name = "treeViewDataEntity";
            this.treeViewDataEntity.ShowLines = false;
            this.treeViewDataEntity.Size = new System.Drawing.Size(368, 333);
            this.treeViewDataEntity.TabIndex = 0;
            this.treeViewDataEntity.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewDataEntity_AfterSelect);
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(305, 381);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "${DataEntityTreeChooseView_ButtonCancel}";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(224, 381);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "${DataEntityTreeChooseView_ButtonOK}";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            this.txtSelectedName.AllowEmpty = true;
            this.txtSelectedName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSelectedName.CustomValidate = null;
            this.txtSelectedName.HighLight = true;
            this.txtSelectedName.LimitMaxValue = false;
            this.txtSelectedName.Location = new System.Drawing.Point(12, 12);
            this.txtSelectedName.MaxValue = ((long)(2147483647));
            this.txtSelectedName.Name = "txtSelectedName";
            this.txtSelectedName.ReadOnly = true;
            this.txtSelectedName.Regex = "";
            this.txtSelectedName.RegexMsg = null;
            this.txtSelectedName.Size = new System.Drawing.Size(368, 21);
            this.txtSelectedName.TabIndex = 3;
            this.txtSelectedName.Title = null;
            this.txtSelectedName.ValueCompareTo = null;
            this.txtSelectedName.WaterText = "";
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClear.Location = new System.Drawing.Point(12, 381);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 4;
            this.btnClear.Text = "${DataEntityTreeChooseView_ButtonClear}";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(392, 416);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.txtSelectedName);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.treeViewDataEntity);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DataEntityTreeChooseView";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "${DataEntityTreeChooseView}";
            this.Load += new System.EventHandler(this.FormDataEntityTreeChoose_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private Sheng.SailingEase.Controls.SETreeView treeViewDataEntity;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private Sheng.SailingEase.Controls.SETextBox txtSelectedName;
        private Sheng.SailingEase.Controls.SEButton btnClear;
    }
}
