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
using Sheng.SailingEase.Core.Development.Localisation;
using Sheng.SailingEase.Localisation;
namespace Sheng.SailingEase.Core.Development
{
    partial class FormEventDataItemDataSet : FormViewBase
    {
        private IDataEntityComponentService _dataEntityComponentService;
        public string DataSourceLabel
        {
            get
            {
                return this.lblDataSource.Text;
            }
            set
            {
                this.lblDataSource.Text = value + ":";
            }
        }
       private EnumEventDataSource _allowDataSourceType = EnumEventDataSource.FormElement;
        public EnumEventDataSource AllowDataSourceType
        {
            get
            {
                return this._allowDataSourceType;
            }
            set
            {
                this._allowDataSourceType = value;
            }
        }
        public UIElementEntityTypeCollection AllowFormElementControlType
        {
            get
            {
                return this.ddlDataSource.AllowFormElementControlType;
            }
            set
            {
                this.ddlDataSource.AllowFormElementControlType = value;
            }
        }
        private WindowEntity formEntity;
        private string dataEntityId;
        public string DataEntityId
        {
            get
            {
                return this.dataEntityId;
            }
            set
            {
                this.dataEntityId = value;
                if (this.dataEntityId != null && this.dataEntityId != String.Empty)
                {
                    DataEntity dataEntity = _dataEntityComponentService.GetDataEntity(dataEntityId);
                    this.ddlDataItem.DataSource = dataEntity.Items;
                    this.ddlDataItem.Visible = true;
                    this.ddlDataItem.AllowEmpty = false;
                    this.txtDataItem.Visible = false;
                    this.txtDataItem.AllowEmpty = true;
                }
                else
                {
                    this.ddlDataItem.Visible = false;
                    this.ddlDataItem.AllowEmpty = true;
                    this.txtDataItem.Visible = true;
                    this.txtDataItem.AllowEmpty = false;
                }
            }
        }
        private DataTable dtDataSourceType;
        public string SelectedDataItemId
        {
            get
            {
                if (this.DataEntityId != null && this.DataEntityId != String.Empty)
                {
                    if (this.ddlDataItem.SelectedValue == null)
                        return String.Empty;
                    return this.ddlDataItem.SelectedValue.ToString();
                }
                else
                {
                    return this.txtDataItem.Text;
                }
            }
        }
        public string SelectedDataItemName
        {
            get
            {
                if (this.DataEntityId != null && this.DataEntityId != String.Empty)
                {
                    return this.ddlDataItem.Text;
                }
                else
                {
                    return this.txtDataItem.Text;
                }
            }
        }
        public object SelectedDataSourceItem
        {
            get
            {
                return this.ddlDataSource.SelectedItem;
            }
        }
        public string SelectedDataSourceVisibleString
        {
            get
            {
                return this.ddlDataSource.SelectedDataSourceVisibleString;
            }
        }
        public string SelectedDataSourceString
        {
            get
            {
                return this.ddlDataSource.SelectedDataSourceString;
            }
        }
        public FormEventDataItemDataSet(WindowEntity formEntity)
        {
            InitializeComponent();
            _dataEntityComponentService = ServiceUnity.Container.Resolve<IDataEntityComponentService>();
            Unity.ApplyResource(this);
            ApplyLanguageResource();
            this.txtDataItem.Location = this.ddlDataItem.Location;
            this.txtDataItem.Size = this.ddlDataItem.Size;
            this.formEntity = formEntity;
        }
        private void ApplyLanguageResource()
        {
            this.ddlDataItem.Title = Language.Current.FormEventDataItemDataSet_LabelDataItem;
            this.txtDataItem.Title = Language.Current.FormEventDataItemDataSet_LabelDataItem;
            this.ddlDataSourceType.Title = Language.Current.FormEventDataItemDataSet_LabelDataSource;
            this.ddlDataSource.Title = Language.Current.FormEventDataItemDataSet_LabelDataSource;
            this.txtDataItem.Regex = CoreConstant.ENTITY_CODE_REGEX;
            this.txtDataItem.RegexMsg = CommonLanguage.Current.RegexMsg_EntityCode;
        }
        private void ddlDataSourceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ddlDataSource.AllowDataSource = (EnumEventDataSource)ddlDataSourceType.SelectedValue; 
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!this.DoValidate())
            {
                return;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void FormEventDataItemDataSet_Load(object sender, EventArgs e)
        {
            dtDataSourceType = new DataTable();
            dtDataSourceType.Columns.Add("Text");
            dtDataSourceType.Columns.Add("Value", typeof(EnumEventDataSource));
            DataRow dr;
            if ((this.AllowDataSourceType & EnumEventDataSource.FormElement) == EnumEventDataSource.FormElement)
            {
                dr = dtDataSourceType.NewRow();
                dr["Text"] = Language.Current.DataSource_FormElement;
                dr["Value"] = EnumEventDataSource.FormElement;
                dtDataSourceType.Rows.Add(dr);
            }
            if ((this.AllowDataSourceType & EnumEventDataSource.System) == EnumEventDataSource.System)
            {
                dr = dtDataSourceType.NewRow();
                dr["Text"] = Language.Current.DataSource_System;
                dr["Value"] = EnumEventDataSource.System;
                dtDataSourceType.Rows.Add(dr);
            }
            this.ddlDataSource.FormEntity = this.formEntity;
            this.ddlDataSourceType.DataSource = dtDataSourceType;
            if (this.ddlDataSourceType.Items.Count == 0)
            {
                this.ddlDataSource.Enabled = false;
            }
        }
    }
}
