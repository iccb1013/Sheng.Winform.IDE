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
namespace Sheng.SailingEase.Windows.Forms.Development
{
    partial class FormSEComboBoxExDevDataRule : FormViewBase
    {
        private  FormElementComboBoxEntityDev _entity;
        private UserControlSEComboBoxExDevDataRule _dataRule;
        private IWindowDesignService _windowDesignService = ServiceUnity.WindowDesignService;
        public FormSEComboBoxExDevDataRule(FormElementComboBoxEntityDev entity)
        {
            InitializeComponent();
            Unity.ApplyResource(this);
            this._entity = entity;
        }
        private void FormSEComboBoxExDevDataRule_Load(object sender, EventArgs e)
        {
            this._dataRule = new UserControlSEComboBoxExDevDataRule(_entity);
            this._dataRule.Dock = DockStyle.Fill;
            this.panel.Controls.Add(this._dataRule);
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            this._dataRule.UpdateEntity();
            _windowDesignService.MakeDirty();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            this._dataRule.Clear();
            _windowDesignService.MakeDirty();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
