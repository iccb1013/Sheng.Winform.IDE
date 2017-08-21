/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Kernal;
using Sheng.SailingEase.Core;
namespace Sheng.SailingEase.Windows.Forms.Development
{
    [ToolboxItem(false)]
    partial class UserControlSEComboBoxExDevDataRule : UserControlViewBase
    {
        private UserControlSEComboBoxExDevDataRule_Enum _enum = new UserControlSEComboBoxExDevDataRule_Enum()
        {
            Dock = DockStyle.Fill
        };
        private UserControlSEComboBoxExDevDataRule_DataEntity _dataEntity = new UserControlSEComboBoxExDevDataRule_DataEntity()
        {
            Dock = DockStyle.Fill
        };
        private FormElementComboBoxEntityDev _entity;
        private IWindowDesignService _windowDesignService = ServiceUnity.WindowDesignService;
        public UserControlSEComboBoxExDevDataRule(FormElementComboBoxEntityDev entity)
        {
            InitializeComponent();
            Unity.ApplyResource(this);
            this._entity = entity;
        }
        private void SEComboBoxExDevDataRule_Load(object sender, EventArgs e)
        {
            this.ddlDataSourceMode.DataSource = EnumDescConverter.Get(typeof(UIElementComboBoxEntity.EnumComboBoxDataSourceMode));
            this.ddlDataSourceMode.SelectedValue = (int)this._entity.DataSourceMode;
            this._enum.SelectedId = this._entity.EnumId;
            this._dataEntity.SelectedDataEntityId = this._entity.DataEntityId;
            this._dataEntity.TextDataItemId = this._entity.TextDataItemId;
            this._dataEntity.ValueDataItemId = this._entity.ValueDataItemId;
        }
        private void ddlDataSourceMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.panel.Controls.Clear();
            if ((UIElementComboBoxEntity.EnumComboBoxDataSourceMode)Convert.ToInt32(this.ddlDataSourceMode.SelectedValue) ==
                UIElementComboBoxEntity.EnumComboBoxDataSourceMode.Enum)
            {
                this.panel.Controls.Add(this._enum);
            }
            else
            {
                this.panel.Controls.Add(this._dataEntity);
            }
        }
        public void UpdateEntity()
        {
            FormElementComboBoxEntityDev.ModifyDataRuleCommand command =
                new FormElementComboBoxEntityDev.ModifyDataRuleCommand(this._entity)
                {
                    DataSourceMode =
                       (UIElementComboBoxEntity.EnumComboBoxDataSourceMode)Convert.ToInt32(this.ddlDataSourceMode.SelectedValue),
                    DataEntityId = this._dataEntity.SelectedDataEntityId,
                    TextDataItemId = this._dataEntity.TextDataItemId,
                    ValueDataItemId = this._dataEntity.ValueDataItemId,
                    EnumId = this._enum.SelectedId
                };
            _windowDesignService.ExecuteCommand(command);
        }
        public void Clear()
        {
            _windowDesignService.ExecuteCommand(new FormElementComboBoxEntityDev.ClearDataRuleCommand(this._entity));
        }
    }
}
