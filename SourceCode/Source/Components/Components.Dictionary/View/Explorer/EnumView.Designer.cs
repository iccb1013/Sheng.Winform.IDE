/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Components.DictionaryComponent.View
{
    partial class EnumView
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
            this.dataGridViewEnumItem = new Sheng.SailingEase.Controls.SEDataGridView();
            this.dataGridViewEnum = new Sheng.SailingEase.Controls.SEDataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEnumItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEnum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            this.dataGridViewEnumItem.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewEnumItem.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewEnumItem.Location = new System.Drawing.Point(232, 70);
            this.dataGridViewEnumItem.Name = "dataGridViewEnumItem";
            this.dataGridViewEnumItem.RowTemplate.Height = 23;
            this.dataGridViewEnumItem.Size = new System.Drawing.Size(472, 344);
            this.dataGridViewEnumItem.TabIndex = 0;
            this.dataGridViewEnumItem.WaterText = "";
            this.dataGridViewEnum.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.dataGridViewEnum.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewEnum.Location = new System.Drawing.Point(3, 4);
            this.dataGridViewEnum.Name = "dataGridViewEnum";
            this.dataGridViewEnum.RowTemplate.Height = 23;
            this.dataGridViewEnum.Size = new System.Drawing.Size(223, 386);
            this.dataGridViewEnum.TabIndex = 1;
            this.dataGridViewEnum.WaterText = "";
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(293, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 25);
            this.label1.TabIndex = 14;
            this.label1.Text = "枚举";
            this.pictureBox1.Location = new System.Drawing.Point(232, 9);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(55, 55);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 13;
            this.pictureBox1.TabStop = false;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(708, 394);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.dataGridViewEnum);
            this.Controls.Add(this.dataGridViewEnumItem);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "EnumView";
            this.Text = "DictionaryView";
            this.Load += new System.EventHandler(this.DictionaryView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEnumItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEnum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private Controls.SEDataGridView dataGridViewEnumItem;
        private Controls.SEDataGridView dataGridViewEnum;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}
