/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Windows.Forms.Development
{
    partial class UserControlDataColumnDevDataRule_RelationDataEntity
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
            this.btnBrowseDataEntity = new Sheng.SailingEase.Controls.SEButton();
            this.txtDataEntityName = new Sheng.SailingEase.Controls.SETextBox();
            this.lblTextDataItem = new System.Windows.Forms.Label();
            this.ddlTextDataItem = new Sheng.SailingEase.Controls.SEComboBox();
            this.ddlValueDataItem = new Sheng.SailingEase.Controls.SEComboBox();
            this.lblValueDataItem = new System.Windows.Forms.Label();
            this.SuspendLayout();
            this.btnBrowseDataEntity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseDataEntity.Location = new System.Drawing.Point(347, -1);
            this.btnBrowseDataEntity.Name = "btnBrowseDataEntity";
            this.btnBrowseDataEntity.Size = new System.Drawing.Size(31, 23);
            this.btnBrowseDataEntity.TabIndex = 10;
            this.btnBrowseDataEntity.Text = "...";
            this.btnBrowseDataEntity.UseVisualStyleBackColor = true;
            this.btnBrowseDataEntity.Click += new System.EventHandler(this.btnBrowseDataEntity_Click);
            this.txtDataEntityName.AllowEmpty = false;
            this.txtDataEntityName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDataEntityName.HighLight = true;
            this.txtDataEntityName.LimitMaxValue = false;
            this.txtDataEntityName.Location = new System.Drawing.Point(3, 1);
            this.txtDataEntityName.MaxValue = ((long)(2147483647));
            this.txtDataEntityName.Name = "txtDataEntityName";
            this.txtDataEntityName.ReadOnly = true;
            this.txtDataEntityName.Regex = "";
            this.txtDataEntityName.RegexMsg = null;
            this.txtDataEntityName.Size = new System.Drawing.Size(338, 21);
            this.txtDataEntityName.TabIndex = 9;
            this.txtDataEntityName.Title = "TextBoxDataEntityNameTitle";
            this.txtDataEntityName.ValueCompareTo = null;
            this.txtDataEntityName.WaterText = "";
            this.lblTextDataItem.AutoSize = true;
            this.lblTextDataItem.Location = new System.Drawing.Point(1, 31);
            this.lblTextDataItem.Name = "lblTextDataItem";
            this.lblTextDataItem.Size = new System.Drawing.Size(35, 12);
            this.lblTextDataItem.TabIndex = 12;
            this.lblTextDataItem.Text = "值项:";
            this.ddlTextDataItem.AllowEmpty = false;
            this.ddlTextDataItem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlTextDataItem.DisplayMember = "Name";
            this.ddlTextDataItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlTextDataItem.FormattingEnabled = true;
            this.ddlTextDataItem.HighLight = true;
            this.ddlTextDataItem.Location = new System.Drawing.Point(77, 54);
            this.ddlTextDataItem.Name = "ddlTextDataItem";
            this.ddlTextDataItem.Size = new System.Drawing.Size(301, 20);
            this.ddlTextDataItem.TabIndex = 13;
            this.ddlTextDataItem.Title = "LabelTextDataItem";
            this.ddlTextDataItem.ValueMember = "Id";
            this.ddlTextDataItem.WaterText = "";
            this.ddlValueDataItem.AllowEmpty = false;
            this.ddlValueDataItem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlValueDataItem.DisplayMember = "Name";
            this.ddlValueDataItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlValueDataItem.FormattingEnabled = true;
            this.ddlValueDataItem.HighLight = true;
            this.ddlValueDataItem.Location = new System.Drawing.Point(77, 28);
            this.ddlValueDataItem.Name = "ddlValueDataItem";
            this.ddlValueDataItem.Size = new System.Drawing.Size(301, 20);
            this.ddlValueDataItem.TabIndex = 14;
            this.ddlValueDataItem.Title = "LabelValueDataItem";
            this.ddlValueDataItem.ValueMember = "Id";
            this.ddlValueDataItem.WaterText = "";
            this.lblValueDataItem.AutoSize = true;
            this.lblValueDataItem.Location = new System.Drawing.Point(1, 57);
            this.lblValueDataItem.Name = "lblValueDataItem";
            this.lblValueDataItem.Size = new System.Drawing.Size(47, 12);
            this.lblValueDataItem.TabIndex = 15;
            this.lblValueDataItem.Text = "显示项:";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblValueDataItem);
            this.Controls.Add(this.ddlValueDataItem);
            this.Controls.Add(this.ddlTextDataItem);
            this.Controls.Add(this.lblTextDataItem);
            this.Controls.Add(this.btnBrowseDataEntity);
            this.Controls.Add(this.txtDataEntityName);
            this.Name = "UserControlDataColumnDevDataRule_RelationDataEntity";
            this.Size = new System.Drawing.Size(381, 80);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private Sheng.SailingEase.Controls.SEButton btnBrowseDataEntity;
        private Sheng.SailingEase.Controls.SETextBox txtDataEntityName;
        private System.Windows.Forms.Label lblTextDataItem;
        private Sheng.SailingEase.Controls.SEComboBox ddlTextDataItem;
        private Sheng.SailingEase.Controls.SEComboBox ddlValueDataItem;
        private System.Windows.Forms.Label lblValueDataItem;
    }
}
