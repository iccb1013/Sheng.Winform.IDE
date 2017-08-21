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
    partial class UserControlDataColumnDevDataRule_RelationDataEntity : UserControlViewBase, IFormElementDataListColumnDataRuleParameterSetControl
    {
        IDataEntityComponentService _dataEntityComponentService = ServiceUnity.DataEntityComponentService;
        string _selectedId;
        string _selectedName;
        public string SelectedDataEntityId
        {
            get
            {
                return _selectedId;
            }
            set
            {
                DataEntity dataEntity = null;
                if (String.IsNullOrEmpty(value) == false)
                    dataEntity = _dataEntityComponentService.GetDataEntity(value);
                if (dataEntity == null)
                {
                    _selectedId = String.Empty;
                    _selectedName = String.Empty;
                    this.txtDataEntityName.Text = String.Empty;
                }
                else
                {
                    _selectedId = value;
                    _selectedName = dataEntity.Name;
                    this.txtDataEntityName.Text = dataEntity.Name;
                    ddlTextDataItem.DataSource = dataEntity.Items.ToList();
                    ddlValueDataItem.DataSource = dataEntity.Items.ToList();
                }
            }
        }
        public string TextDataItemId
        {
            get
            {
                if (this.ddlTextDataItem.SelectedValue == null)
                {
                    return String.Empty;
                }
                return this.ddlTextDataItem.SelectedValue.ToString();
            }
            set
            {
                this.ddlTextDataItem.SelectedValue = value;
            }
        }
        public string ValueDataItemId
        {
            get
            {
                if (this.ddlValueDataItem.SelectedValue == null)
                {
                    return String.Empty;
                }
                return this.ddlValueDataItem.SelectedValue.ToString();
            }
            set
            {
                this.ddlValueDataItem.SelectedValue = value;
            }
        }
        public UserControlDataColumnDevDataRule_RelationDataEntity()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            Unity.ApplyResource(this);
            ApplyLanguageResource();
        }
        private void ApplyLanguageResource()
        {
            this.txtDataEntityName.Title = "";
        }
        private void btnBrowseDataEntity_Click(object sender, EventArgs e)
        {
            DataEntityTreeChooseArgs dialogArgs = new DataEntityTreeChooseArgs()
            {
                ShowDataItem = false,
                SelectedId = _selectedId,
                SelectedName = _selectedName
            };
            DataEntityTreeChooseResult result = DialogUnity.DataEntityChoose(dialogArgs);
            if (result.DialogResult == false)
            {
                return;
            }
            _selectedId = result.SelectedId;
            _selectedName = result.SelectedName;
            txtDataEntityName.Text = _selectedName;
            DataEntity dataEntity = result.SelectedDataEntity;
            ddlTextDataItem.DataSource = dataEntity.Items.ToList();
            ddlValueDataItem.DataSource = dataEntity.Items.ToList();
        }
        public UIElementDataListColumnDataRuleAbstract GetParameter()
        {
            FormElementDataListColumnDataRulesDev.RelationDataEntityDev dataRule = new FormElementDataListColumnDataRulesDev.RelationDataEntityDev();
            dataRule.DataEntityId = this.SelectedDataEntityId;
            dataRule.DisplayItemId = this.TextDataItemId;
            dataRule.ValueItemId = this.ValueDataItemId;
            return dataRule;
        }
        public void SetParameter(UIElementDataListColumnDataRuleAbstract dataRule)
        {
            FormElementDataListColumnDataRulesDev.RelationDataEntityDev rule = dataRule as FormElementDataListColumnDataRulesDev.RelationDataEntityDev;
            if (rule == null)
            {
                Debug.Assert(false, "FormElementDataColumnDataRuleAbstract 参数不正确");
                throw new ArgumentException();
            }
            this.SelectedDataEntityId = rule.DataEntityId;
            this.TextDataItemId = rule.DisplayItemId;
            this.ValueDataItemId = rule.ValueItemId;
        }
    }
}
