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
using System.Xml;
using System.Windows.Forms;
using Sheng.SIMBE.LanguageResource.IDELanguageResource;
using Sheng.SIMBE.IDE.EntityDev;
using Sheng.SIMBE.Core;
using Sheng.SIMBE.Core.Event;
using Sheng.SIMBE.IDE.EventDev;
using Sheng.SIMBE.IDE.Sys;
namespace Sheng.SIMBE.IDE.UI.EventSet
{
    [ToolboxItem(false)]
    internal partial class UserControlEventSetParameter_RefreshList_General : UserControlEventSetParameter
    {
        private RefreshListParameterSetPlanCollection parameterSetPlanCollection;
        public RefreshListParameterSetPlanCollection ParameterSetPlanCollection
        {
            get
            {
                return this.parameterSetPlanCollection;
            }
            set
            {
                this.parameterSetPlanCollection = value;
            }
        }
        DataTable dtList;
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
            }
        }
        public string EnteredName
        {
            get
            {
                return this.txtName.Text;
            }
        }
        public string EnteredCode
        {
            get
            {
                return this.txtCode.Text;
            }
        }
        public UserControlEventSetParameter_RefreshList_General(FormEntityDev formEntity)
            : base(formEntity)
        {
            InitializeComponent();
            this.txtCode.Regex = Constant.ENTITY_CODE_REGEX;
            ApplyLanguageResource();
            radioButtonDataEntity.Checked = true;
            dtList = new DataTable();
            dtList.Columns.Add("Name");
            dtList.Columns.Add("Value");
            dtList.Columns.Add("DataEntityId");
            DataRow dr;
            foreach (Sheng.SIMBE.Core.Entity.FormElement ele in this.FormEntity.Elements)
            {
                if (ele.ControlType == EnumFormElementControlType.DataList)
                {
                    dr = dtList.NewRow();
                    dr["Name"] = ele.Name;
                    dr["Value"] = ele.Id;
                    dr["DataEntityId"] = ((FormElementDataListEntityDev)ele).DataEntityId;
                    dtList.Rows.Add(dr);
                }
            }
            ddlDataList.DataSource = dtList;
        }
        private void ApplyLanguageResource()
        {
            this.lblTitle.Text = " " + Language.GetString("UserControlEventSetParameter_RefreshList_General_LabelTitle");
            this.lblName.Text = Language.GetString("UserControlEventSetParameter_RefreshList_General_LabelName") + ":";
            this.lblCode.Text = Language.GetString("UserControlEventSetParameter_RefreshList_General_LabelCode") + ":";
            this.lblDataList.Text = Language.GetString("UserControlEventSetParameter_RefreshList_General__LabelDataList") + ":";
            this.txtName.Title = Language.GetString("UserControlEventSetParameter_RefreshList_General_LabelName");
            this.txtCode.Title = Language.GetString("UserControlEventSetParameter_RefreshList_General_LabelCode");
            this.txtCode.RegexMsg = Language.GetString("RegexMsg_OnlyAllowMonogram");
            this.radioButtonDataEntity.Text = Language.GetString("UserControlEventSetParameter_RefreshList_General_RadioButtonDataEntity");
            this.radioButtonSql.Text = Language.GetString("UserControlEventSetParameter_RefreshList_General_RadioButtonSql");
        }
        private void ddlDataList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlDataList.SelectedValue == null)
            {
                return;
            }
            if (dtList.Select("Value='" + ddlDataList.SelectedValue.ToString() +
                "'")[0]["DataEntityId"].ToString() == String.Empty)
            {
                this.DataEntityId = String.Empty;
            }
            else
            {
                this.DataEntityId = dtList.Select("Value='" + ddlDataList.SelectedValue.ToString() +
                   "'")[0]["DataEntityId"].ToString();
            }
            if (ddlDataList.SelectedValue != null)
            {
            }
            else
            {
            }
        }
        public override string GetXml()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement xmlEle;
            xmlEle = xmlDoc.CreateElement("Paramter");
            xmlDoc.AppendChild(xmlEle);
            xmlEle = xmlDoc.CreateElement("Mode");
            if (radioButtonDataEntity.Checked)
            {
                xmlEle.InnerText = ((int)EnumSaveFormDataMode.DataEntity).ToString();
            }
            else
            {
                xmlEle.InnerText = ((int)EnumSaveFormDataMode.SQL).ToString();
            }
            xmlDoc.FirstChild.AppendChild(xmlEle);
            xmlEle = xmlDoc.CreateElement("DataListId");
            xmlEle.InnerText = this.ddlDataList.SelectedValue.ToString();
            xmlDoc.FirstChild.AppendChild(xmlEle);
            return xmlDoc.FirstChild.InnerXml;
        }
        public override void SetParameter(EventBase even)
        {
            RefreshListDev _event = even as RefreshListDev;
            this.txtName.Text = _event.Name;
            this.txtCode.Text = _event.Code;
            if (_event.RefreshMode == EnumRefreshListMode.DataEntity)
            {
                this.radioButtonDataEntity.Checked = true;
            }
            else
            {
                this.radioButtonSql.Checked = true;
            }
            this.ddlDataList.SelectedValue = _event.DataListId;
            if (ddlDataList.SelectedValue != null)
            {
                this.DataEntityId =
                       dtList.Select("Value='" + ddlDataList.SelectedValue.ToString() +
                       "'")[0]["DataEntityId"].ToString();
            }
        }
    }
}
