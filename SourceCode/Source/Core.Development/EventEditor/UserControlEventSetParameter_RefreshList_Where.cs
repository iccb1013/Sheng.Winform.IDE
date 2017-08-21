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
using Sheng.SIMBE.IDE.EntityDev;
using Sheng.SIMBE.LanguageResource.IDELanguageResource;
using Sheng.SIMBE.IDE.Sys;
using Sheng.SIMBE.Core.Event;
using Sheng.SIMBE.IDE.EventDev;
using Sheng.SIMBE.Core.Entity;
using Sheng.SIMBE.IDE.Operator;
namespace Sheng.SIMBE.IDE.UI.EventSet
{
    [ToolboxItem(false)]
    internal partial class UserControlEventSetParameter_RefreshList_Where : UserControlEventSetParameter
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
        private DataTable dtWhere;
        public DataTable Where
        {
            get
            {
                return this.dtWhere;
            }
            set
            {
                this.dtWhere = value;
                this.dataGridViewWhere.DataSource = this.dtWhere;
            }
        }
        public UserControlEventSetParameter_RefreshList_Where(FormEntityDev formEntity)
            : base(formEntity)
        {
            InitializeComponent();
            ApplyLanguageResource();
            dtWhere = new DataTable();
            dtWhere.Columns.Add("DataItemName");
            dtWhere.Columns.Add("DataSourceName");
            dtWhere.Columns.Add("DataItem");
            dtWhere.Columns.Add("DataSource");
            dtWhere.Columns.Add("MatchType"); 
            dtWhere.Columns.Add("Warning", typeof(bool));
            this.dataGridViewWhere.AutoGenerateColumns = false;
            this.dataGridViewWhere.DataSource = this.dtWhere;
            this.ColumnMatchType.DataSource = EnumMember.MatchType;
            this.ColumnMatchType.DisplayMember = "Text";
            this.ColumnMatchType.ValueMember = "Value";
        }
        private void ApplyLanguageResource()
        {
            this.lblTitle.Text = " " + Language.GetString("UserControlEventSetParameter_RefreshList_Where_LabelTitle");
            this.ColumnDataItemName.HeaderText = Language.GetString("UserControlEventSetParameter_RefreshList_Where_ColumnDataItemName");
            this.ColumnDataSource.HeaderText = Language.GetString("UserControlEventSetParameter_RefreshList_Where_ColumnDataSource");
            this.ColumnMatchType.HeaderText = Language.GetString("UserControlEventSetParameter_RefreshList_Where_ColumnMatchType");
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            FormEventDataItemDataSourceChoose formEventSaveFormDataSet =
               new FormEventDataItemDataSourceChoose(
                   this.ParameterSetPlanCollection.General.DataEntityId, 
                   this.FormEntity);
            formEventSaveFormDataSet.AllowDataList = false;
            if (formEventSaveFormDataSet.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            DataRow dr = this.dtWhere.NewRow();
            dr["DataItem"] = formEventSaveFormDataSet.SelectedDataItemId;
            dr["DataSource"] = formEventSaveFormDataSet.DataSource;
            dr["DataItemName"] = formEventSaveFormDataSet.SelectedDataItemName;
            dr["DataSourceName"] = formEventSaveFormDataSet.DataSourceName;
            dr["MatchType"] = "1";
            dr["Warning"] = false;
            this.dtWhere.Rows.Add(dr);
            this.dtWhere.AcceptChanges();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewWhere.SelectedRows.Count == 0)
            {
                return;
            }
            DataRow dr = ((DataRowView)dataGridViewWhere.SelectedRows[0].DataBoundItem).Row;
            this.dtWhere.Rows.Remove(dr);
            this.dtWhere.AcceptChanges();
        }
        private void dataGridViewWhere_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow dr in this.dataGridViewWhere.Rows)
            {
                if (dr != null)
                {
                    if ((bool)((DataRowView)dr.DataBoundItem).Row["Warning"])
                    {
                        dr.DefaultCellStyle.BackColor = UIConfigure.DataGridViewRowBackColorWarning;
                        dr.DefaultCellStyle.SelectionBackColor = UIConfigure.DataGridViewSelectedRowBackColorWarning;
                    }
                    else
                    {
                        dr.DefaultCellStyle.BackColor = UIConfigure.DataGridViewRowBackColorNormal;
                        dr.DefaultCellStyle.SelectionBackColor = UIConfigure.DataGridViewSelectedRowBackColorNormal;
                    }
                }
            }
        }
        public override string GetXml()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement xmlEle;
            xmlEle = xmlDoc.CreateElement("Paramter");
            xmlDoc.AppendChild(xmlEle);
            xmlEle = xmlDoc.CreateElement("WhereSet");
            XmlElement xmlEleWhere;
            XmlAttribute xmlAttribute;
            foreach (DataRow dr in this.Where.Rows)
            {
                xmlEleWhere = xmlDoc.CreateElement("Where");
                xmlAttribute = xmlDoc.CreateAttribute("DataItemEntityId");
                xmlAttribute.Value = dr["DataItem"].ToString();
                xmlEleWhere.Attributes.Append(xmlAttribute);
                xmlAttribute = xmlDoc.CreateAttribute("Source");
                xmlAttribute.Value = dr["DataSource"].ToString();
                xmlEleWhere.Attributes.Append(xmlAttribute);
                xmlAttribute = xmlDoc.CreateAttribute("MatchType");
                xmlAttribute.Value = dr["MatchType"].ToString();
                xmlEleWhere.Attributes.Append(xmlAttribute);
                xmlEle.AppendChild(xmlEleWhere);
            }
            xmlDoc.FirstChild.AppendChild(xmlEle);
            return xmlDoc.FirstChild.InnerXml;
        }
        public override void SetParameter(EventBase even)
        {
            RefreshListDev _event = even as RefreshListDev;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(_event.WhereSetXml);
            XmlNodeList xmlNodeList = xmlDoc.SelectNodes("WhereSet/Where");
            DataRow dr;
            bool warninigRow;
            string [] strSource;
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                warninigRow = false;
                strSource = xmlNode.Attributes["Source"].Value.Split('.');
                dr = dtWhere.NewRow();
                dr["DataItem"] = xmlNode.Attributes["DataItemEntityId"].Value;
                dr["DataSource"] = xmlNode.Attributes["Source"].Value;
                DataItemEntityDev dataItemEntityDev = DataEntityOperator.GetDataItemEntity(dr["DataItem"].ToString());
                if (dataItemEntityDev == null)
                {
                    dr["DataItemName"] = String.Empty;
                    warninigRow = true;
                }
                else
                {
                    dr["DataItemName"] = dataItemEntityDev.Name;
                }
                dr["DataSourceName"] = "";
                dr["MatchType"] = xmlNode.Attributes["MatchType"].Value;
                if (strSource[0] == "FormElement")
                {
                    FormElement formElement = this.FormEntity.GetFormElement(strSource[1]);
                    if (formElement == null)
                    {
                        dr["DataSourceName"] = String.Empty;
                        warninigRow = true;
                    }
                    else
                    {
                        dr["DataSourceName"] += Language.GetString("DataSource_FormElement") +
                            "." + formElement.ControlTypeName + "." + formElement.Name;
                    }
                }
                else
                {
                    dr["DataSourceName"] += Language.GetString("DataSource_System") + ".";
                    dr["DataSourceName"] += EnumMember.SystemDataSource.Select("Value = '" +
                        strSource[1] + "'")[0]["Text"].ToString();
                }
                dr["Warning"] = warninigRow;
                dtWhere.Rows.Add(dr);
            }
        }
    }
}
