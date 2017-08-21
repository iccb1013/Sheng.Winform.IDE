/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Core.Development
{
    partial class UserControlEventEditorPanel_DataListRefresh_General
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
            this.radioButtonDataEntity = new System.Windows.Forms.RadioButton();
            this.radioButtonSql = new System.Windows.Forms.RadioButton();
            this.seGroupBox1 = new Sheng.SailingEase.Controls.SEGroupBox();
            this.seGroupBox1.SuspendLayout();
            this.SuspendLayout();
            this.radioButtonDataEntity.AutoSize = true;
            this.radioButtonDataEntity.Location = new System.Drawing.Point(6, 20);
            this.radioButtonDataEntity.Name = "radioButtonDataEntity";
            this.radioButtonDataEntity.Size = new System.Drawing.Size(485, 16);
            this.radioButtonDataEntity.TabIndex = 0;
            this.radioButtonDataEntity.TabStop = true;
            this.radioButtonDataEntity.Text = "${UserControlEventEditorPanel_DataListRefresh_General_RadioButtonDataEntity}";
            this.radioButtonDataEntity.UseVisualStyleBackColor = true;
            this.radioButtonSql.AutoSize = true;
            this.radioButtonSql.Location = new System.Drawing.Point(178, 20);
            this.radioButtonSql.Name = "radioButtonSql";
            this.radioButtonSql.Size = new System.Drawing.Size(443, 16);
            this.radioButtonSql.TabIndex = 1;
            this.radioButtonSql.TabStop = true;
            this.radioButtonSql.Text = "${UserControlEventEditorPanel_DataListRefresh_General_RadioButtonSql}";
            this.radioButtonSql.UseVisualStyleBackColor = true;
            this.seGroupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.seGroupBox1.Controls.Add(this.radioButtonSql);
            this.seGroupBox1.Controls.Add(this.radioButtonDataEntity);
            this.seGroupBox1.HighLight = false;
            this.seGroupBox1.Location = new System.Drawing.Point(6, 32);
            this.seGroupBox1.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.seGroupBox1.Name = "seGroupBox1";
            this.seGroupBox1.Size = new System.Drawing.Size(451, 60);
            this.seGroupBox1.TabIndex = 25;
            this.seGroupBox1.TabStop = false;
            this.seGroupBox1.Text = "${UserControlEventEditorPanel_DataListRefresh_General_GroupBoxRefreshMode}";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.seGroupBox1);
            this.Name = "UserControlEventEditorPanel_DataListRefresh_General";
            this.Controls.SetChildIndex(this.seGroupBox1, 0);
            this.seGroupBox1.ResumeLayout(false);
            this.seGroupBox1.PerformLayout();
            this.ResumeLayout(false);
        }
        private System.Windows.Forms.RadioButton radioButtonDataEntity;
        private System.Windows.Forms.RadioButton radioButtonSql;
        private Sheng.SailingEase.Controls.SEGroupBox seGroupBox1;
    }
}
