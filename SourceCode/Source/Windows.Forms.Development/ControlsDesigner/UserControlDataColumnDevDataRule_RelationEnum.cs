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
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Core.Development;
namespace Sheng.SailingEase.Windows.Forms.Development
{
    [ToolboxItem(false)]
    partial class UserControlDataColumnDevDataRule_RelationEnum : UserControlViewBase, IFormElementDataListColumnDataRuleParameterSetControl
    {
        private EnumEntity _selectedEntity;
        private IDictionaryComponentService _dictionaryComponentService =
            ServiceUnity.DictionaryComponentService;
        public string SelectedId
        {
            get
            {
                if (_selectedEntity == null)
                    return String.Empty;
                return this._selectedEntity.Id;
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    txtEnumName.Text = String.Empty;
                }
                else
                {
                    _selectedEntity = _dictionaryComponentService.GetEnumEntity(value);
                    if (_selectedEntity == null)
                    {
                        txtEnumName.Text = String.Empty;
                    }
                    else
                    {
                        txtEnumName.Text = _selectedEntity.Name;
                    }
                }
            }
        }
        public UserControlDataColumnDevDataRule_RelationEnum()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            ApplyLanguageResource();
        }
        private void ApplyLanguageResource()
        {
            this.txtEnumName.Title = ""; 
        }
        private void btnBrowseEnum_Click(object sender, EventArgs e)
        {
            bool result = DialogUnity.EnumChoose(out _selectedEntity);
            if (result == false)
            {
                return;
            }
            txtEnumName.Text = _selectedEntity.Name;
        }
        public UIElementDataListColumnDataRuleAbstract GetParameter()
        {
            FormElementDataListColumnDataRulesDev.RelationEnumDev dataRule = new FormElementDataListColumnDataRulesDev.RelationEnumDev();
            dataRule.EnumId = this.SelectedId;
            return dataRule;
        }
        public void SetParameter(UIElementDataListColumnDataRuleAbstract dataRule)
        {
            FormElementDataListColumnDataRulesDev.RelationEnumDev rule = dataRule as FormElementDataListColumnDataRulesDev.RelationEnumDev;
            if (rule == null)
            {
                Debug.Assert(false, "FormElementDataColumnDataRuleAbstract 参数不正确");
                throw new ArgumentException();
            }
            this.SelectedId = rule.EnumId;
        }
    }
}
