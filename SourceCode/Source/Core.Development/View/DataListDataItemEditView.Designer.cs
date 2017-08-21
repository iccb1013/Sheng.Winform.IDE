/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Core.Development
{
    partial class DataListDataItemEditView
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
            this.ddlDataColumn = new Sheng.SailingEase.Controls.SEComboBox();
            this.lblDataColumn = new System.Windows.Forms.Label();
            this.btnOK = new Sheng.SailingEase.Controls.SEButton();
            this.btnCancel = new Sheng.SailingEase.Controls.SEButton();
            this.lblDataSource = new System.Windows.Forms.Label();
            this.dataSourceSelector = new Sheng.SailingEase.Core.Development.View.DataSourceSelector();
            this.SuspendLayout();
            this.ddlDataColumn.AllowEmpty = false;
            this.ddlDataColumn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlDataColumn.CustomValidate = null;
            this.ddlDataColumn.DisplayMember = "Text";
            this.ddlDataColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlDataColumn.FormattingEnabled = true;
            this.ddlDataColumn.HighLight = true;
            this.ddlDataColumn.Location = new System.Drawing.Point(115, 43);
            this.ddlDataColumn.Name = "ddlDataColumn";
            this.ddlDataColumn.Size = new System.Drawing.Size(352, 20);
            this.ddlDataColumn.TabIndex = 56;
            this.ddlDataColumn.Title = "";
            this.ddlDataColumn.ValueMember = "Value";
            this.ddlDataColumn.WaterText = "";
            this.lblDataColumn.AutoSize = true;
            this.lblDataColumn.Location = new System.Drawing.Point(26, 46);
            this.lblDataColumn.Name = "lblDataColumn";
            this.lblDataColumn.Size = new System.Drawing.Size(269, 12);
            this.lblDataColumn.TabIndex = 55;
            this.lblDataColumn.Text = "${DataListDataItemEditView_LabelDataColumn}:";
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Enabled = false;
            this.btnOK.Location = new System.Drawing.Point(381, 145);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 57;
            this.btnOK.Text = "${DataListDataItemEditView_ButtonOK}";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(462, 145);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 58;
            this.btnCancel.Text = "${DataListDataItemEditView_ButtonCancel}";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.lblDataSource.AutoSize = true;
            this.lblDataSource.Location = new System.Drawing.Point(26, 79);
            this.lblDataSource.Name = "lblDataSource";
            this.lblDataSource.Size = new System.Drawing.Size(269, 12);
            this.lblDataSource.TabIndex = 59;
            this.lblDataSource.Text = "${DataListDataItemEditView_LabelDataSource}:";
            this.dataSourceSelector.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataSourceSelector.Location = new System.Drawing.Point(115, 73);
            this.dataSourceSelector.Name = "dataSourceSelector";
            this.dataSourceSelector.Size = new System.Drawing.Size(352, 25);
            this.dataSourceSelector.TabIndex = 60;
            this.dataSourceSelector.WindowEntity = null;
            this.dataSourceSelector.DataSourceChanged += new Sheng.SailingEase.Core.Development.View.DataSourceSelector.OnDataSourceChangedHandler(this.dataSourceSelector_DataSourceChanged);
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(549, 180);
            this.Controls.Add(this.dataSourceSelector);
            this.Controls.Add(this.lblDataSource);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.ddlDataColumn);
            this.Controls.Add(this.lblDataColumn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DataListDataItemEditView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "${DataListDataItemEditView}";
            this.Load += new System.EventHandler(this.DataListDataItemEditView_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private Controls.SEComboBox ddlDataColumn;
        private System.Windows.Forms.Label lblDataColumn;
        private Controls.SEButton btnOK;
        private Controls.SEButton btnCancel;
        private System.Windows.Forms.Label lblDataSource;
        private View.DataSourceSelector dataSourceSelector;
    }
}
