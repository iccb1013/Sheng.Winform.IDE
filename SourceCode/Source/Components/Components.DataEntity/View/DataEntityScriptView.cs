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
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Core;
namespace Sheng.SailingEase.Components.DataEntityComponent.View
{
    partial class DataEntityScriptView : WorkbenchViewBase
    {
        public DataEntityScriptView(List<DataEntity> entityList)
        {
            InitializeComponent();
            DataEntitySqlScriptView userControlDataEntityCreateSql = new DataEntitySqlScriptView(entityList);
            userControlDataEntityCreateSql.Dock = DockStyle.Fill;
            this.Controls.Add(userControlDataEntityCreateSql);
        }
    }
}
