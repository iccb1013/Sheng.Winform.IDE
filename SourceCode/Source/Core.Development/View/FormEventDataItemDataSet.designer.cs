/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Core.Development
{
    partial class FormEventDataItemDataSet
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
            this.ddlDataSource = new FormElementComboBox();
            this.ddlDataItem = new Sheng.SailingEase.Controls.SEComboBox();
            this.ddlDataSourceType = new Sheng.SailingEase.Controls.SEComboBox();
            this.lblDataSource = new System.Windows.Forms.Label();
            this.btnOK = new Sheng.SailingEase.Controls.SEButton();
            this.btnCancel = new Sheng.SailingEase.Controls.SEButton();
            this.lblDataColumn = new System.Windows.Forms.Label();
            this.txtDataItem = new Sheng.SailingEase.Controls.SETextBox();
            this.SuspendLayout();
            this.ddlDataSource.AllowDataSource = EnumEventDataSource.FormElement;
            this.ddlDataSource.AllowEmpty = false;
            this.ddlDataSource.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.ddlDataSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlDataSource.FormattingEnabled = true;
            this.ddlDataSource.FormEntity = null;
            this.ddlDataSource.HighLight = true;
            this.ddlDataSource.Location = new System.Drawing.Point(101, 79);
            this.ddlDataSource.MaxDropDownItems = 16;
            this.ddlDataSource.Name = "ddlDataSource";
            this.ddlDataSource.SelectedFormElementId = "";
            this.ddlDataSource.Size = new System.Drawing.Size(347, 22);
            this.ddlDataSource.TabIndex = 64;
            this.ddlDataSource.Title = null;
            this.ddlDataSource.WaterText = "";
            this.ddlDataItem.AllowEmpty = false;
            this.ddlDataItem.DisplayMember = "Name";
            this.ddlDataItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlDataItem.FormattingEnabled = true;
            this.ddlDataItem.HighLight = true;
            this.ddlDataItem.Location = new System.Drawing.Point(101, 26);
            this.ddlDataItem.Name = "ddlDataItem";
            this.ddlDataItem.Size = new System.Drawing.Size(347, 20);
            this.ddlDataItem.TabIndex = 62;
            this.ddlDataItem.Title = "";
            this.ddlDataItem.ValueMember = "Id";
            this.ddlDataItem.WaterText = "";
            this.ddlDataSourceType.AllowEmpty = false;
            this.ddlDataSourceType.DisplayMember = "Text";
            this.ddlDataSourceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlDataSourceType.FormattingEnabled = true;
            this.ddlDataSourceType.HighLight = true;
            this.ddlDataSourceType.Location = new System.Drawing.Point(101, 53);
            this.ddlDataSourceType.Name = "ddlDataSourceType";
            this.ddlDataSourceType.Size = new System.Drawing.Size(347, 20);
            this.ddlDataSourceType.TabIndex = 61;
            this.ddlDataSourceType.Title = "";
            this.ddlDataSourceType.ValueMember = "Value";
            this.ddlDataSourceType.WaterText = "";
            this.ddlDataSourceType.SelectedIndexChanged += new System.EventHandler(this.ddlDataSourceType_SelectedIndexChanged);
            this.lblDataSource.AutoSize = true;
            this.lblDataSource.Location = new System.Drawing.Point(12, 56);
            this.lblDataSource.Name = "lblDataSource";
            this.lblDataSource.Size = new System.Drawing.Size(269, 12);
            this.lblDataSource.TabIndex = 60;
            this.lblDataSource.Text = "${FormEventDataItemDataSet_LabelDataSource}:";
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(345, 147);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 58;
            this.btnOK.Text = "${FormEventDataItemDataSet_ButtonOK}";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(426, 147);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 59;
            this.btnCancel.Text = "${FormEventDataItemDataSet_ButtonCancel}";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.lblDataColumn.AutoSize = true;
            this.lblDataColumn.Location = new System.Drawing.Point(12, 29);
            this.lblDataColumn.Name = "lblDataColumn";
            this.lblDataColumn.Size = new System.Drawing.Size(257, 12);
            this.lblDataColumn.TabIndex = 57;
            this.lblDataColumn.Text = "${FormEventDataItemDataSet_LabelDataItem}:";
            this.txtDataItem.AllowEmpty = true;
            this.txtDataItem.HighLight = true;
            this.txtDataItem.LimitMaxValue = false;
            this.txtDataItem.Location = new System.Drawing.Point(12, 149);
            this.txtDataItem.MaxLength = 100;
            this.txtDataItem.MaxValue = ((long)(2147483647));
            this.txtDataItem.Name = "txtDataItem";
            this.txtDataItem.Regex = "";
            this.txtDataItem.RegexMsg = "";
            this.txtDataItem.Size = new System.Drawing.Size(100, 21);
            this.txtDataItem.TabIndex = 65;
            this.txtDataItem.Title = "DataItemCode";
            this.txtDataItem.ValueCompareTo = null;
            this.txtDataItem.Visible = false;
            this.txtDataItem.WaterText = "";
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(513, 182);
            this.Controls.Add(this.lblDataSource);
            this.Controls.Add(this.lblDataColumn);
            this.Controls.Add(this.txtDataItem);
            this.Controls.Add(this.ddlDataSource);
            this.Controls.Add(this.ddlDataItem);
            this.Controls.Add(this.ddlDataSourceType);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormEventDataItemDataSet";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "${FormEventDataItemDataSet}";
            this.Load += new System.EventHandler(this.FormEventDataItemDataSet_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private FormElementComboBox ddlDataSource;
        private Sheng.SailingEase.Controls.SEComboBox ddlDataItem;
        private Sheng.SailingEase.Controls.SEComboBox ddlDataSourceType;
        private System.Windows.Forms.Label lblDataSource;
        private Sheng.SailingEase.Controls.SEButton btnOK;
        private Sheng.SailingEase.Controls.SEButton btnCancel;
        private System.Windows.Forms.Label lblDataColumn;
        private Sheng.SailingEase.Controls.SETextBox txtDataItem;
    }
}
