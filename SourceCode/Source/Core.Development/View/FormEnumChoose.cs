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
using System.Text;
using System.Windows.Forms;
using Sheng.SailingEase.Infrastructure;
namespace Sheng.SailingEase.Core.Development
{
    partial class FormEnumChoose : FormViewBase
    {
        private IDictionaryComponentService _dictionaryComponentService = ServiceUnity.DictionaryComponentService;
        private BindingList<EnumEntity> _bindingList;
        public EnumEntity SelectedEnumEntity
        {
            get;
            set;
        }
        public FormEnumChoose()
        {
            InitializeComponent();
            UIHelper.ProcessDataGridView(this.dataGridViewEnum);
            Unity.ApplyResource(this);
        }
        private void FormEnumChoose_Load(object sender, EventArgs e)
        {
            this.dataGridViewEnum.AutoGenerateColumns = false;
            _bindingList = new BindingList<EnumEntity>(_dictionaryComponentService.GetEnumEntityList());
            this.dataGridViewEnum.DataSource = _bindingList;
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (dataGridViewEnum.SelectedRows.Count == 0)
            {
                this.SelectedEnumEntity = null;
            }
            else
            {
                EnumEntity enumEntity = (EnumEntity)dataGridViewEnum.SelectedRows[0].DataBoundItem;
                this.SelectedEnumEntity = enumEntity;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
