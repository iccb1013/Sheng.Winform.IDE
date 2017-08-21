/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Components.Window.DesignComponent
{
    partial class FormPropertyGrid
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
            this.panelPropertyGrid = new Sheng.SailingEase.Controls.SEPanel();
            this.propertyGrid = new ComponentModel.Design.PropertyGridPad();
            this.comboBoxFormElementList = new Sheng.SailingEase.Controls.SEComboBox();
            this.panelPropertyGrid.SuspendLayout();
            this.SuspendLayout();
            this.panelPropertyGrid.BorderColor = System.Drawing.Color.Black;
            this.panelPropertyGrid.Controls.Add(this.propertyGrid);
            this.panelPropertyGrid.CustomValidate = null;
            this.panelPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPropertyGrid.FillColorEnd = System.Drawing.Color.Empty;
            this.panelPropertyGrid.FillColorStart = System.Drawing.Color.Empty;
            this.panelPropertyGrid.FillMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.panelPropertyGrid.FillStyle = Sheng.SailingEase.Controls.FillStyle.Solid;
            this.panelPropertyGrid.HighLight = false;
            this.panelPropertyGrid.Location = new System.Drawing.Point(0, 20);
            this.panelPropertyGrid.Name = "panelPropertyGrid";
            this.panelPropertyGrid.ShowBorder = false;
            this.panelPropertyGrid.Size = new System.Drawing.Size(237, 312);
            this.panelPropertyGrid.TabIndex = 3;
            this.panelPropertyGrid.Title = null;
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.SelectedObjects = null;
            this.propertyGrid.ShowDescription = true;
            this.propertyGrid.Size = new System.Drawing.Size(237, 312);
            this.propertyGrid.TabIndex = 0;
            this.propertyGrid.Verbs = null;
            this.comboBoxFormElementList.AllowEmpty = true;
            this.comboBoxFormElementList.CustomValidate = null;
            this.comboBoxFormElementList.DisplayMember = "Text";
            this.comboBoxFormElementList.Dock = System.Windows.Forms.DockStyle.Top;
            this.comboBoxFormElementList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFormElementList.FormattingEnabled = true;
            this.comboBoxFormElementList.HighLight = true;
            this.comboBoxFormElementList.Location = new System.Drawing.Point(0, 0);
            this.comboBoxFormElementList.MaxDropDownItems = 16;
            this.comboBoxFormElementList.Name = "comboBoxFormElementList";
            this.comboBoxFormElementList.Size = new System.Drawing.Size(237, 20);
            this.comboBoxFormElementList.TabIndex = 2;
            this.comboBoxFormElementList.Title = null;
            this.comboBoxFormElementList.ValueMember = "Value";
            this.comboBoxFormElementList.WaterText = "";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(237, 332);
            this.Controls.Add(this.panelPropertyGrid);
            this.Controls.Add(this.comboBoxFormElementList);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "FormPropertyGrid";
            this.TabText = "FormPropertyGrid";
            this.Text = "FormPropertyGrid";
            this.panelPropertyGrid.ResumeLayout(false);
            this.ResumeLayout(false);
        }
        private Sheng.SailingEase.Controls.SEPanel panelPropertyGrid;
        private Sheng.SailingEase.Controls.SEComboBox comboBoxFormElementList;
        private Sheng.SailingEase.ComponentModel.Design.PropertyGridPad propertyGrid;
    }
}
