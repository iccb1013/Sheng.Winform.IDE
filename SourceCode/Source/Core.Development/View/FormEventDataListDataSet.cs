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
using Sheng.SailingEase.Localisation;
using Sheng.SailingEase.Core.Development.Localisation;
namespace Sheng.SailingEase.Core.Development
{
    partial class FormEventDataListDataSet : FormViewBase
    {
        private FormGlobalFormElementChoose formGlobalFormElementChoose;
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
                return this.ddlFormElement.AllowFormElementControlType;
            }
            set
            {
                this.ddlFormElement.AllowFormElementControlType = value;
            }
        }
        private WindowEntity formEntity;
        private string dataListId;
        public string DataListId
        {
            get
            {
                return this.dataListId;
            }
            set
            {
                this.dataListId = value;
            }
        }
        private DataTable dtDataColumn;
        private DataTable dtDataSourceType;
        public string DataColumn
        {
            get
            {
                if (this.DataListId == null || this.DataListId == String.Empty)
                {
                    return txtDataColumn.Text;
                }
                else
                {
                    return ddlDataColumn.SelectedValue.ToString();
                }
            }
        }
        public string DataColumnName
        {
            get
            {
                if (this.DataListId == null || this.DataListId == String.Empty)
                {
                    return txtDataColumn.Text;
                }
                else
                {
                    return ddlDataColumn.Text;
                }
            }
        }
        public string SelectedDataSourceVisibleString
        {
            get
            {
                return StringParserLogic.DataSourceVisibleString(this.ddlFormElement.SelectedItem);
            }
        }
        public string SelectedDataSourceString
        {
            get
            {
                return StringParserLogic.DataSourceString(this.ddlFormElement.SelectedItem);
            }
        }
        public FormEventDataListDataSet(WindowEntity formEntity)
        {
            InitializeComponent();
            Unity.ApplyResource(this);
            ApplyLanguageResource();
            this.formEntity = formEntity;
            this.txtDataColumn.Location = this.ddlDataColumn.Location;
            this.txtDataColumn.Size = this.ddlDataColumn.Size;
        }
        private void ApplyLanguageResource()
        {
            this.txtDataColumn.Regex = CoreConstant.ENTITY_CODE_REGEX;
            this.txtDataColumn.RegexMsg = CommonLanguage.Current.RegexMsg_EntityCode;
            this.txtDataColumn.Title = Language.Current.FormEventDataListDataSet_LabelDataColumn;
            this.ddlDataColumn.Title = Language.Current.FormEventDataListDataSet_LabelDataColumn;
            this.ddlDataSourceType.Title = Language.Current.FormEventDataListDataSet_LabelDataSource;
            this.ddlFormElement.Title = Language.Current.FormEventDataListDataSet_LabelFormElement;
        }
        private void FormEventDataListDataSet_Load(object sender, EventArgs e)
        {
            InitForm();
            dtDataSourceType = new DataTable();
            dtDataSourceType.Columns.Add("Text");
            dtDataSourceType.Columns.Add("Value",typeof(EnumEventDataSource));
            DataRow dr;
            if ((this.AllowDataSourceType & EnumEventDataSource.FormElement) == EnumEventDataSource.FormElement)
            {
                dr = dtDataSourceType.NewRow();
                dr["Text"] = "窗体元素";
                dr["Value"] = EnumEventDataSource.FormElement;
                dtDataSourceType.Rows.Add(dr);
            }
            if ((this.AllowDataSourceType & EnumEventDataSource.System) == EnumEventDataSource.System)
            {
                dr = dtDataSourceType.NewRow();
                dr["Text"] = "系统";
                dr["Value"] = EnumEventDataSource.System;
                dtDataSourceType.Rows.Add(dr);
            }            
            this.ddlFormElement.FormEntity = this.formEntity;
            this.ddlDataSourceType.DataSource = dtDataSourceType;
            if (this.ddlDataSourceType.Items.Count == 0)
            {
                this.ddlFormElement.Enabled = false;
            }
        }
        private void ddlDataSourceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ddlFormElement.AllowDataSource = (EnumEventDataSource)ddlDataSourceType.SelectedValue; 
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
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (formGlobalFormElementChoose == null || formGlobalFormElementChoose.IsDisposed)
            {
                formGlobalFormElementChoose = new FormGlobalFormElementChoose();
                formGlobalFormElementChoose.AllowFormElementControlType.Add(typeof(UIElementDataListTextBoxColumnEntity));
                formGlobalFormElementChoose.AllowSelectFormElementControlType.Add(typeof(UIElementDataListTextBoxColumnEntity));
                formGlobalFormElementChoose.InitFormElementTree();
            }
            if (formGlobalFormElementChoose.ShowDialog() == DialogResult.OK)
            {
                this.txtDataColumn.Text = formGlobalFormElementChoose.FormElementCode;
            }
        }
        private void InitForm()
        {
            if (this.DataListId != null && this.DataListId != String.Empty)
            {
                dtDataColumn = new DataTable();
                dtDataColumn.Columns.Add("Text");
                dtDataColumn.Columns.Add("Value");
                UIElementDataListEntity dataListEntity =
                    (UIElementDataListEntity)this.formEntity.FindFormElementById(this.dataListId);
                DataRow drDataColumn;
                foreach (UIElementDataListTextBoxColumnEntity dc in dataListEntity.DataColumns)
                {
                    drDataColumn = dtDataColumn.NewRow();
                    drDataColumn["Text"] = dc.Name;
                    drDataColumn["Value"] = dc.Id;
                    dtDataColumn.Rows.Add(drDataColumn);
                }
                this.btnBrowse.Visible = false;
                ddlDataColumn.Visible = true;
                txtDataColumn.Visible = false;
                ddlDataColumn.AllowEmpty = false;
                txtDataColumn.AllowEmpty = true;
                ddlDataColumn.DataSource = dtDataColumn;
            }
            else
            {
                this.btnBrowse.Visible = true;
                ddlDataColumn.Visible = false;
                txtDataColumn.Visible = true;
                ddlDataColumn.AllowEmpty = true;
                txtDataColumn.AllowEmpty = false;
            }
        }
    }
}
