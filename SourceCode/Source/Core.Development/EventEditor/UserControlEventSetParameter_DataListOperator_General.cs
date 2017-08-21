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
using System.Xml;
using Sheng.SIMBE.LanguageResource.IDELanguageResource;
using Sheng.SIMBE.Core.Event;
using Sheng.SIMBE.IDE.EventDev;
using Sheng.SIMBE.Core;
using Sheng.SIMBE.IDE.EntityDev;
using Sheng.SIMBE.IDE.Sys;
using Sheng.SIMBE.IDE.UI.FormGui;
namespace Sheng.SIMBE.IDE.UI.EventSet
{
    [ToolboxItem(false)]
    internal partial class UserControlEventSetParameter_DataListOperator_General : UserControlEventSetParameter
    {
        private DataTable dtDataList;
        private DataListOperatorParameterSetPlanCollection parameterSetPlanCollection;
        public DataListOperatorParameterSetPlanCollection ParameterSetPlanCollection
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
        public string DataListId
        {
            get
            {
                if ((EnumDataListOperatorObjectForm)Convert.ToInt16(ddlObjectForm.SelectedValue)
                    == EnumDataListOperatorObjectForm.Current)
                {
                    if (this.ddlDataList.SelectedValue == null)
                    {
                        return String.Empty;
                    }
                    return ddlDataList.SelectedValue.ToString();
                }
                else
                {
                    return String.Empty;
                }
            }
        }
        public UserControlEventSetParameter_DataListOperator_General(FormEntityDev formEntity)
            : base(formEntity)
        {
            InitializeComponent();
            this.txtCode.Regex = Constant.ENTITY_CODE_REGEX;
            this.txtDataList.Regex = Constant.ENTITY_CODE_REGEX;
            ApplyDefLanguageResource();
            ApplyLanguageResource();
            this.txtDataList.Location = this.ddlDataList.Location;
            this.txtDataList.Size = this.ddlDataList.Size;
            this.ddlObjectForm.DataSource = EnumMember.DataListOperatorObjectForm;
            this.ddlOperatorType.DataSource = EnumMember.DataListOperatorOperatorType;
            dtDataList = new DataTable();
            dtDataList.Columns.Add("Name");
            dtDataList.Columns.Add("Value");
            dtDataList.Columns.Add("DataEntityId");
            DataRow dr;
            foreach (Sheng.SIMBE.Core.Entity.FormElement ele in this.FormEntity.Elements)
            {
                if (ele.ControlType == EnumFormElementControlType.DataList)
                {
                    dr = dtDataList.NewRow();
                    dr["Name"] = ele.Name;
                    dr["Value"] = ele.Id;
                    dr["DataEntityId"] = ((FormElementDataListEntityDev)ele).DataEntityId;
                    dtDataList.Rows.Add(dr);
                }
            }
            ddlDataList.DataSource = dtDataList;
        }
        private void ApplyLanguageResource()
        {
            this.Title = " " + Language.GetString("UserControlEventSetParameter_DataListOperator_General_LabelTitle");
            this.txtName.Title = Language.GetString("UserControlEventSetParameter_DataListOperator_General_LabelName");
            this.txtCode.Title = Language.GetString("UserControlEventSetParameter_DataListOperator_General_LabelCode");
            this.txtCode.RegexMsg = Language.GetString("RegexMsg_OnlyAllowMonogram");
            this.ddlDataList.Title = Language.GetString("UserControlEventSetParameter_DataListOperator_General_LabelDataList");
            this.txtDataList.Title = Language.GetString("UserControlEventSetParameter_DataListOperator_General_TextBoxDataSetTitle");
            this.txtDataList.RegexMsg = Language.GetString("RegexMsg_OnlyAllowMonogram");
        }
        public override string GetXml()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement xmlEle;
            xmlEle = xmlDoc.CreateElement("Paramter");
            xmlDoc.AppendChild(xmlEle);
            xmlEle = xmlDoc.CreateElement("ObjectForm");
            xmlEle.InnerText = this.ddlObjectForm.SelectedValue.ToString();
            xmlDoc.FirstChild.AppendChild(xmlEle);
            xmlEle = xmlDoc.CreateElement("OperatorType");
            xmlEle.InnerText = this.ddlOperatorType.SelectedValue.ToString();
            xmlDoc.FirstChild.AppendChild(xmlEle);
            xmlEle = xmlDoc.CreateElement("DataList");
            if ((EnumDataListOperatorObjectForm)Convert.ToInt32(this.ddlObjectForm.SelectedValue)
                == EnumDataListOperatorObjectForm.Caller)
            {
                xmlEle.InnerText = this.txtDataList.Text;
            }
            else
            {
                xmlEle.InnerText = this.ddlDataList.SelectedValue.ToString();
            }
            xmlDoc.FirstChild.AppendChild(xmlEle);
            return xmlDoc.FirstChild.InnerXml;
        }
        public override void SetParameter(EventBase even)
        {
            DataListOperator _event = even as DataListOperator;
            this.txtName.Text = _event.Name;
            this.txtCode.Text = _event.Code;
            this.ddlObjectForm.SelectedValue = ((int)_event.ObjectForm).ToString();
            this.ddlOperatorType.SelectedValue = ((int)_event.OperatorType).ToString();
            if (_event.ObjectForm == EnumDataListOperatorObjectForm.Current)
            {
                this.ddlDataList.SelectedValue = _event.DataList;
            }
            else
            {
                this.txtDataList.Text = _event.DataList;
            }
        }
        private void ddlObjectForm_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((EnumDataListOperatorObjectForm)Convert.ToInt16(ddlObjectForm.SelectedValue)
                == EnumDataListOperatorObjectForm.Caller)
            {
                this.ddlDataList.Visible = false;
                this.ddlDataList.AllowEmpty = true;
                this.txtDataList.Visible = true;
                this.txtDataList.AllowEmpty = false;
                this.btnBrowse.Visible = true;
            }
            else
            {
                this.ddlDataList.Visible = true;
                this.ddlDataList.AllowEmpty = false;
                this.txtDataList.Visible = false;
                this.txtDataList.AllowEmpty = true;
                this.btnBrowse.Visible = false;
            }
            if (this.ParameterSetPlanCollection != null)
            {
                this.ParameterSetPlanCollection.Data.DataSet.Rows.Clear();
                this.ParameterSetPlanCollection.Data.DataSet.AcceptChanges();
            }
        }
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FormGlobalFormElementChoose formGlobalFormElementChoose = new FormGlobalFormElementChoose();
            formGlobalFormElementChoose.AllowFormElementControlType.Add(EnumFormElementControlType.DataList);
            formGlobalFormElementChoose.AllowSelectFormElementControlType.Add(EnumFormElementControlType.DataList);
            if (formGlobalFormElementChoose.ShowDialog() == DialogResult.OK)
            {
                this.txtDataList.Text = formGlobalFormElementChoose.FormElementCode;
            }
        }
    }
}
