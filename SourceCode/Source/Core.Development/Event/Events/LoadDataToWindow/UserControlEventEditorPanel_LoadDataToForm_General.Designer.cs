/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Core.Development
{
    partial class UserControlEventEditorPanel_LoadDataToForm_General
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
            this.txtCode = new Sheng.SailingEase.Controls.SETextBox();
            this.lblCode = new System.Windows.Forms.Label();
            this.txtName = new Sheng.SailingEase.Controls.SETextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.radioButtonDataEntity = new System.Windows.Forms.RadioButton();
            this.radioButtonSql = new System.Windows.Forms.RadioButton();
            this.lblNotice = new Sheng.SailingEase.Controls.SEAdvLabel();
            this.seGroupBox1 = new Sheng.SailingEase.Controls.SEGroupBox();
            this.seGroupBox1.SuspendLayout();
            this.SuspendLayout();
            this.txtCode.AllowEmpty = false;
            this.txtCode.HighLight = true;
            this.txtCode.LimitMaxValue = false;
            this.txtCode.Location = new System.Drawing.Point(275, 32);
            this.txtCode.MaxLength = 100;
            this.txtCode.MaxValue = ((long)(2147483647));
            this.txtCode.Name = "txtCode";
            this.txtCode.Regex = "";
            this.txtCode.RegexMsg = "RegexMsg_OnlyAllowMonogram";
            this.txtCode.Size = new System.Drawing.Size(120, 21);
            this.txtCode.TabIndex = 1;
            this.txtCode.Title = "LabelCode";
            this.txtCode.ValueCompareTo = null;
            this.txtCode.WaterText = "";
            this.lblCode.AutoSize = true;
            this.lblCode.Location = new System.Drawing.Point(210, 35);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(389, 12);
            this.lblCode.TabIndex = 26;
            this.lblCode.Text = "${UserControlEventEditorPanel_LoadDataToForm_General_LabelCode}:";
            this.txtName.AllowEmpty = false;
            this.txtName.HighLight = true;
            this.txtName.LimitMaxValue = false;
            this.txtName.Location = new System.Drawing.Point(69, 32);
            this.txtName.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.txtName.MaxLength = 100;
            this.txtName.MaxValue = ((long)(2147483647));
            this.txtName.Name = "txtName";
            this.txtName.Regex = "";
            this.txtName.RegexMsg = null;
            this.txtName.Size = new System.Drawing.Size(120, 21);
            this.txtName.TabIndex = 0;
            this.txtName.Title = "LabelName";
            this.txtName.ValueCompareTo = null;
            this.txtName.WaterText = "";
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(3, 35);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(389, 12);
            this.lblName.TabIndex = 24;
            this.lblName.Text = "${UserControlEventEditorPanel_LoadDataToForm_General_LabelName}:";
            this.radioButtonDataEntity.AutoSize = true;
            this.radioButtonDataEntity.Location = new System.Drawing.Point(6, 20);
            this.radioButtonDataEntity.Name = "radioButtonDataEntity";
            this.radioButtonDataEntity.Size = new System.Drawing.Size(473, 16);
            this.radioButtonDataEntity.TabIndex = 2;
            this.radioButtonDataEntity.TabStop = true;
            this.radioButtonDataEntity.Text = "${UserControlEventEditorPanel_LoadDataToForm_General_RadioButtonDataEntity}";
            this.radioButtonDataEntity.UseVisualStyleBackColor = true;
            this.radioButtonSql.AutoSize = true;
            this.radioButtonSql.Location = new System.Drawing.Point(159, 20);
            this.radioButtonSql.Name = "radioButtonSql";
            this.radioButtonSql.Size = new System.Drawing.Size(431, 16);
            this.radioButtonSql.TabIndex = 3;
            this.radioButtonSql.TabStop = true;
            this.radioButtonSql.Text = "${UserControlEventEditorPanel_LoadDataToForm_General_RadioButtonSql}";
            this.radioButtonSql.UseVisualStyleBackColor = true;
            this.lblNotice.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNotice.BorderColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblNotice.FillColorEnd = System.Drawing.Color.Empty;
            this.lblNotice.FillColorStart = System.Drawing.SystemColors.Info;
            this.lblNotice.FillMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.lblNotice.FillStyle = Sheng.SailingEase.Controls.FillStyle.Solid;
            this.lblNotice.Location = new System.Drawing.Point(5, 256);
            this.lblNotice.Name = "lblNotice";
            this.lblNotice.ShowBorder = true;
            this.lblNotice.SingleLine = false;
            this.lblNotice.Size = new System.Drawing.Size(452, 41);
            this.lblNotice.TabIndex = 31;
            this.lblNotice.Text = " ${UserControlEventEditorPanel_LoadDataToForm_General_LabelNotice}";
            this.lblNotice.TextHorizontalCenter = false;
            this.lblNotice.TextVerticalCenter = true;
            this.seGroupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.seGroupBox1.Controls.Add(this.radioButtonSql);
            this.seGroupBox1.Controls.Add(this.radioButtonDataEntity);
            this.seGroupBox1.HighLight = false;
            this.seGroupBox1.Location = new System.Drawing.Point(5, 62);
            this.seGroupBox1.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.seGroupBox1.Name = "seGroupBox1";
            this.seGroupBox1.Size = new System.Drawing.Size(452, 60);
            this.seGroupBox1.TabIndex = 32;
            this.seGroupBox1.TabStop = false;
            this.seGroupBox1.Text = "${UserControlEventEditorPanel_LoadDataToForm_General_GroupBoxLoadMode}";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.seGroupBox1);
            this.Controls.Add(this.lblNotice);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.lblCode);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.Name = "UserControlEventEditorPanel_LoadDataToForm_General";
            this.Load += new System.EventHandler(this.UserControlEventEditorPanel_LoadDataToForm_General_Load);
            this.Controls.SetChildIndex(this.lblName, 0);
            this.Controls.SetChildIndex(this.txtName, 0);
            this.Controls.SetChildIndex(this.lblCode, 0);
            this.Controls.SetChildIndex(this.txtCode, 0);
            this.Controls.SetChildIndex(this.lblNotice, 0);
            this.Controls.SetChildIndex(this.seGroupBox1, 0);
            this.seGroupBox1.ResumeLayout(false);
            this.seGroupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private Sheng.SailingEase.Controls.SETextBox txtCode;
        private System.Windows.Forms.Label lblCode;
        private Sheng.SailingEase.Controls.SETextBox txtName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.RadioButton radioButtonDataEntity;
        private System.Windows.Forms.RadioButton radioButtonSql;
        private Sheng.SailingEase.Controls.SEAdvLabel lblNotice;
        private Sheng.SailingEase.Controls.SEGroupBox seGroupBox1;
    }
}
