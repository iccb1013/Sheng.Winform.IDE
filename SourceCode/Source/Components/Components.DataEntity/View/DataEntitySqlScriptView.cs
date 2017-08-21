/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sheng.SailingEase.Controls;
using Sheng.SailingEase.Controls.TextEditor.Document;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Localisation;
using Sheng.SailingEase.Core;
namespace Sheng.SailingEase.Components.DataEntityComponent.View
{
    [ToolboxItem(false)]
    partial class DataEntitySqlScriptView : UserControlViewBase
    {
        private List<DataEntity> _dataEntitys;
        public DataEntitySqlScriptView()
        {
            InitializeComponent();
            this.toolStrip1.Renderer = ToolStripRenders.WhiteToSilverGray;
            ApplyIconResource();
        }
        private void ApplyIconResource()
        {
            this.toolStripButtonSave.Image = IconsLibrary.Save;
        }
        public DataEntitySqlScriptView(List<DataEntity> entityList)
            :this()
        {
            this._dataEntitys = entityList;
        }
        private void UserControlDataEntityCreateSql_Load(object sender, EventArgs e)
        {
            this.txtSql.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy("SQL");
            StringBuilder sb = new StringBuilder();
            foreach (DataEntityDev entity in _dataEntitys)
            {
                sb.Append(entity.GetSql());
                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine);
            }
            this.txtSql.Text = sb.ToString() ;
        }
        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtSql.SaveFile(saveFileDialog.FileName);
            }
        }
    }
}
