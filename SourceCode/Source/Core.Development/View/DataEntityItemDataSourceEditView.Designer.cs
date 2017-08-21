/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Core.Development
{
    partial class DataEntityItemDataSourceEditView
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
            this.dataSourceSelector = new Sheng.SailingEase.Core.Development.View.DataSourceSelector();
            this.lblDataSource = new System.Windows.Forms.Label();
            this.btnOK = new Sheng.SailingEase.Controls.SEButton();
            this.btnCancel = new Sheng.SailingEase.Controls.SEButton();
            this.ddlDataItem = new Sheng.SailingEase.Controls.SEComboBox();
            this.lblDataItem = new System.Windows.Forms.Label();
            this.SuspendLayout();
            this.dataSourceSelector.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataSourceSelector.Location = new System.Drawing.Point(115, 73);
            this.dataSourceSelector.Name = "dataSourceSelector";
            this.dataSourceSelector.Size = new System.Drawing.Size(352, 25);
            this.dataSourceSelector.TabIndex = 66;
            this.dataSourceSelector.WindowEntity = null;
            this.dataSourceSelector.DataSourceChanged += new Sheng.SailingEase.Core.Development.View.DataSourceSelector.OnDataSourceChangedHandler(this.dataSourceSelector_DataSourceChanged);
            this.lblDataSource.AutoSize = true;
            this.lblDataSource.Location = new System.Drawing.Point(26, 79);
            this.lblDataSource.Name = "lblDataSource";
            this.lblDataSource.Size = new System.Drawing.Size(317, 12);
            this.lblDataSource.TabIndex = 65;
            this.lblDataSource.Text = "${DataEntityItemDataSourceEditView_LabelDataSource}:";
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Enabled = false;
            this.btnOK.Location = new System.Drawing.Point(381, 145);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 63;
            this.btnOK.Text = "${DataEntityItemDataSourceEditView_ButtonOK}";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(462, 145);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 64;
            this.btnCancel.Text = "${DataEntityItemDataSourceEditView_ButtonCancel}";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.ddlDataItem.AllowEmpty = false;
            this.ddlDataItem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlDataItem.CustomValidate = null;
            this.ddlDataItem.DisplayMember = "Text";
            this.ddlDataItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlDataItem.FormattingEnabled = true;
            this.ddlDataItem.HighLight = true;
            this.ddlDataItem.Location = new System.Drawing.Point(115, 43);
            this.ddlDataItem.Name = "ddlDataItem";
            this.ddlDataItem.Size = new System.Drawing.Size(352, 20);
            this.ddlDataItem.TabIndex = 62;
            this.ddlDataItem.Title = "";
            this.ddlDataItem.ValueMember = "Value";
            this.ddlDataItem.WaterText = "";
            this.lblDataItem.AutoSize = true;
            this.lblDataItem.Location = new System.Drawing.Point(26, 46);
            this.lblDataItem.Name = "lblDataItem";
            this.lblDataItem.Size = new System.Drawing.Size(305, 12);
            this.lblDataItem.TabIndex = 61;
            this.lblDataItem.Text = "${DataEntityItemDataSourceEditView_LabelDataItem}:";
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(549, 180);
            this.Controls.Add(this.lblDataSource);
            this.Controls.Add(this.lblDataItem);
            this.Controls.Add(this.dataSourceSelector);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.ddlDataItem);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DataEntityItemDataSourceEditView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "${DataEntityItemDataSourceEditView}";
            this.Load += new System.EventHandler(this.DataEntityItemDataSourceEditView_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private View.DataSourceSelector dataSourceSelector;
        private System.Windows.Forms.Label lblDataSource;
        private Controls.SEButton btnOK;
        private Controls.SEButton btnCancel;
        private Controls.SEComboBox ddlDataItem;
        private System.Windows.Forms.Label lblDataItem;
    }
}
