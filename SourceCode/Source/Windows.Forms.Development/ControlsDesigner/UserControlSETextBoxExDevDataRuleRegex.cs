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
using Sheng.SailingEase.Windows.Forms.Development.Localisation;
using Sheng.SailingEase.RegexTool;
namespace Sheng.SailingEase.Windows.Forms.Development
{
    partial class UserControlSETextBoxExDevDataRuleRegex : UserControlViewBase
    {
        private  FormElementTextBoxEntityDev _entity;
        private IWindowDesignService _windowDesignService = ServiceUnity.WindowDesignService;
        public UserControlSETextBoxExDevDataRuleRegex(FormElementTextBoxEntityDev entity)
        {
            InitializeComponent();
            ApplyLanguageResource();
            Unity.ApplyResource(this);
            this._entity = entity;
        }
        private void UserControlSETextBoxExDevDataRuleRegex_Load(object sender, EventArgs e)
        {
            this.txtRegex.Text = this._entity.Regex;
            this.txtRegexMsg.Text = this._entity.RegexMsg;
        }
        private void ApplyLanguageResource()
        {
            this.txtRegex.Title = Language.Current.UserControlSETextBoxExDevDataRuleRegex_LabelRegex;
            this.txtRegexMsg.Title = Language.Current.UserControlSETextBoxExDevDataRuleRegex_LabelRegexMsg;
        }
        public void UpdateEntity()
        {
            FormElementTextBoxEntityDev.ModifyDataRuleCommand command =
                new FormElementTextBoxEntityDev.ModifyDataRuleCommand(this._entity);
            command.Regex = this.txtRegex.Text;
            command.RegexMsg = this.txtRegexMsg.Text;
            _windowDesignService.ExecuteCommand(command);
        }
        private void btnRegexLib_Click(object sender, EventArgs e)
        {
            using (FormRegexLib formRegexLib = new FormRegexLib())
            {
                if (formRegexLib.ShowDialog() == DialogResult.OK)
                {
                    this.txtRegex.Text = formRegexLib.Regex;
                }
            }
        }
        private void btnTool_Click(object sender, EventArgs e)
        {
            using (FormMain formRegex = new FormMain())
            {
                formRegex.Regex = this.txtRegex.Text;
                if (formRegex.ShowDialog() == DialogResult.OK)
                {
                    this.txtRegex.Text = formRegex.Regex;
                }
            }
        }
    }
}
