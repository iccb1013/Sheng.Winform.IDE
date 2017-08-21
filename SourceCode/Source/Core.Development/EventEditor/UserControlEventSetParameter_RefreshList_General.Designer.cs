/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SIMBE.IDE.UI.EventSet
{
    partial class UserControlEventSetParameter_RefreshList_General
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
            this.txtCode = new Sheng.SIMBE.SEControl.SETextBox();
            this.lblCode = new System.Windows.Forms.Label();
            this.txtName = new Sheng.SIMBE.SEControl.SETextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.ddlDataList = new Sheng.SIMBE.SEControl.SEComboBox();
            this.lblDataList = new System.Windows.Forms.Label();
            this.lblTitle = new Sheng.SIMBE.SEControl.SEAdvLabel();
            this.radioButtonDataEntity = new System.Windows.Forms.RadioButton();
            this.radioButtonSql = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            this.txtCode.AllowEmpty = false;
            this.txtCode.HighLight = true;
            this.txtCode.Location = new System.Drawing.Point(275, 32);
            this.txtCode.MaxLength = 100;
            this.txtCode.Name = "txtCode";
            this.txtCode.Regex = "";
            this.txtCode.RegexMsg = "RegexMsg_OnlyAllowMonogram";
            this.txtCode.Size = new System.Drawing.Size(120, 21);
            this.txtCode.TabIndex = 7;
            this.txtCode.Title = "LabelCode";
            this.txtCode.ValueCompareTo = null;
            this.lblCode.AutoSize = true;
            this.lblCode.Location = new System.Drawing.Point(210, 35);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(59, 12);
            this.lblCode.TabIndex = 6;
            this.lblCode.Text = "LabelCode";
            this.txtName.AllowEmpty = false;
            this.txtName.HighLight = true;
            this.txtName.Location = new System.Drawing.Point(69, 32);
            this.txtName.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.txtName.MaxLength = 100;
            this.txtName.Name = "txtName";
            this.txtName.Regex = "";
            this.txtName.RegexMsg = null;
            this.txtName.Size = new System.Drawing.Size(120, 21);
            this.txtName.TabIndex = 5;
            this.txtName.Title = "LabelName";
            this.txtName.ValueCompareTo = null;
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(3, 35);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(59, 12);
            this.lblName.TabIndex = 4;
            this.lblName.Text = "LabelName";
            this.ddlDataList.AllowEmpty = false;
            this.ddlDataList.DisplayMember = "Name";
            this.ddlDataList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlDataList.FormattingEnabled = true;
            this.ddlDataList.HighLight = true;
            this.ddlDataList.Location = new System.Drawing.Point(69, 59);
            this.ddlDataList.Name = "ddlDataList";
            this.ddlDataList.Size = new System.Drawing.Size(326, 20);
            this.ddlDataList.TabIndex = 13;
            this.ddlDataList.Title = "LabelDataList";
            this.ddlDataList.ValueMember = "Value";
            this.ddlDataList.SelectedIndexChanged += new System.EventHandler(this.ddlDataList_SelectedIndexChanged);
            this.lblDataList.AutoSize = true;
            this.lblDataList.Location = new System.Drawing.Point(4, 62);
            this.lblDataList.Name = "lblDataList";
            this.lblDataList.Size = new System.Drawing.Size(83, 12);
            this.lblDataList.TabIndex = 12;
            this.lblDataList.Text = "LabelDataList";
            this.lblTitle.BorderColor = System.Drawing.Color.Black;
            this.lblTitle.FillColorEnd = System.Drawing.Color.Empty;
            this.lblTitle.FillColorStart = System.Drawing.Color.White;
            this.lblTitle.FillMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.lblTitle.FillStyle = Sheng.SIMBE.SEControl.FillStyle.LinearGradient;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.ShowBorder = false;
            this.lblTitle.SingleLine = true;
            this.lblTitle.Size = new System.Drawing.Size(460, 23);
            this.lblTitle.TabIndex = 14;
            this.lblTitle.Text = " LabelTitle";
            this.lblTitle.TextHorizontalCenter = false;
            this.lblTitle.TextVerticalCenter = true;
            this.radioButtonDataEntity.AutoSize = true;
            this.radioButtonDataEntity.Location = new System.Drawing.Point(6, 89);
            this.radioButtonDataEntity.Name = "radioButtonDataEntity";
            this.radioButtonDataEntity.Size = new System.Drawing.Size(149, 16);
            this.radioButtonDataEntity.TabIndex = 15;
            this.radioButtonDataEntity.TabStop = true;
            this.radioButtonDataEntity.Text = "RadioButtonDataEntity";
            this.radioButtonDataEntity.UseVisualStyleBackColor = true;
            this.radioButtonSql.AutoSize = true;
            this.radioButtonSql.Location = new System.Drawing.Point(178, 89);
            this.radioButtonSql.Name = "radioButtonSql";
            this.radioButtonSql.Size = new System.Drawing.Size(107, 16);
            this.radioButtonSql.TabIndex = 16;
            this.radioButtonSql.TabStop = true;
            this.radioButtonSql.Text = "RadioButtonSql";
            this.radioButtonSql.UseVisualStyleBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radioButtonDataEntity);
            this.Controls.Add(this.radioButtonSql);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.ddlDataList);
            this.Controls.Add(this.lblDataList);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.lblCode);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.Name = "UserControlEventSetParameter_RefreshList_General";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private Sheng.SIMBE.SEControl.SETextBox txtCode;
        private System.Windows.Forms.Label lblCode;
        private Sheng.SIMBE.SEControl.SETextBox txtName;
        private System.Windows.Forms.Label lblName;
        private Sheng.SIMBE.SEControl.SEComboBox ddlDataList;
        private System.Windows.Forms.Label lblDataList;
        private Sheng.SIMBE.SEControl.SEAdvLabel lblTitle;
        private System.Windows.Forms.RadioButton radioButtonDataEntity;
        private System.Windows.Forms.RadioButton radioButtonSql;
    }
}
