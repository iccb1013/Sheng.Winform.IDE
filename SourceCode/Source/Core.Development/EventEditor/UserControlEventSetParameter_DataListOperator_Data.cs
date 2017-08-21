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
using Sheng.SIMBE.IDE.EntityDev;
using Sheng.SIMBE.Core.Event;
using Sheng.SIMBE.IDE.Sys;
using Sheng.SIMBE.Core;
using Sheng.SIMBE.IDE.Operator;
using Sheng.SIMBE.Core.Entity;
using Sheng.SIMBE.IDE.Services;
namespace Sheng.SIMBE.IDE.UI.EventSet
{
    [ToolboxItem(false)]
    internal partial class UserControlEventSetParameter_DataListOperator_Data : UserControlEventSetParameter
    {
        private DataTable dtDataSet;
        public DataTable DataSet
        {
            get
            {
                return this.dtDataSet;
            }
            set
            {
                this.dtDataSet = value;
            }
        }
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
        public UserControlEventSetParameter_DataListOperator_Data(FormEntityDev formEntity)
            : base(formEntity)
        {
            InitializeComponent();
            UIService.ProcessDataGridView(this.dataGridViewDataSet);
            ApplyDefLanguageResource();
            ApplyLanguageResource();
            dtDataSet = new DataTable();
            dtDataSet.Columns.Add("DataColumn");
            dtDataSet.Columns.Add("DataColumnName");
            dtDataSet.Columns.Add("Source");
            dtDataSet.Columns.Add("SourceName");
            dtDataSet.Columns.Add("Where");
            dtDataSet.Columns.Add("Warning", typeof(bool));
            this.dataGridViewDataSet.AutoGenerateColumns = false;
            this.dataGridViewDataSet.DataSource = this.dtDataSet;           
            this.ColumnWhere.DataSource = EnumMember.TrueFalse;
            this.ColumnWhere.DisplayMember = "Text";
            this.ColumnWhere.ValueMember = "Value";
        }
        private void ApplyLanguageResource()
        {
            this.Title = " " + Language.GetString("UserControlEventSetParameter_DataListOperator_Data_LabelTitle");
        }
        public override string GetXml()
        {
            SEXmlDocument xmlDoc = new SEXmlDocument();
            xmlDoc.AppendChild("Paramter");
            DataColumnDataSourceRelationCollection dataSet =
                new DataColumnDataSourceRelationCollection();
            foreach (DataRow dr in this.dtDataSet.Rows)
            {
                DataColumnDataSourceRelation where = new DataColumnDataSourceRelation();
                where.DataColumn = dr["DataColumn"].ToString();
                where.Source = new DataSource(dr["Source"].ToString());
                where.Where = Convert.ToBoolean(dr["Where"].ToString());
                dataSet.Add(where);
            }
            xmlDoc.SelectSingleNode("Paramter").InnerXml += dataSet.ToXml("DataSet");
            return xmlDoc.SelectSingleNode("Paramter").InnerXml;
        }
        public override void SetParameter(EventBase even)
        {
            DataListOperator _event = even as DataListOperator;
            DataRow dr;
            bool warningRow;
            foreach(DataColumnDataSourceRelation relation in _event.DataSet)
            {
                warningRow = false;
                dr = this.dtDataSet.NewRow();
                dr["DataColumn"] = relation.DataColumn;
                dr["DataColumnName"] = "";
                dr["Source"] = relation.Source.ToString();
                dr["Where"] = relation.Where.ToString();
                dr["SourceName"] =
                    StringParserService.DataSourceVisibleString(this.FormEntity, relation.Source.ToString(), out warningRow);
                if (_event.ObjectForm == EnumDataListOperatorObjectForm.Caller)
                {
                    dr["DataColumnName"] = relation.DataColumn;
                    if (!FormOperator.CheckDataColumnExistByCode(_event.DataList, relation.DataColumn))
                    {
                        warningRow = true;
                    }
                }
                else
                {
                    if (!this.FormEntity.Elements.Contains(_event.DataList))
                    {
                        warningRow = true;
                    }
                    else 
                    {
                        if (this.FormEntity.Elements.Contains(relation.DataColumn))
                            dr["DataColumnName"] = this.FormEntity.Elements.GetFormElementById(relation.DataColumn).Name;
                        else
                            warningRow = true;
                    }
                }
                dr["Warning"] = warningRow;
                this.dtDataSet.Rows.Add(dr);
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            FormEventDataListDataSet formEventDataListOperatorDataSetSet =
               new FormEventDataListDataSet(this.FormEntity);
            formEventDataListOperatorDataSetSet.DataListId = this.ParameterSetPlanCollection.General.DataListId;
            if (formEventDataListOperatorDataSetSet.ShowDialog() == DialogResult.OK)
            {
                DataRow dr = this.dtDataSet.NewRow();
                dr["DataColumn"] = formEventDataListOperatorDataSetSet.DataColumn;
                dr["DataColumnName"] = formEventDataListOperatorDataSetSet.DataColumnName;
                dr["Source"] = formEventDataListOperatorDataSetSet.SelectedDataSourceString;
                dr["SourceName"] = formEventDataListOperatorDataSetSet.SelectedDataSourceVisibleString;
                dr["Where"] = false.ToString();
                dr["Warning"] = false;
                this.dtDataSet.Rows.Add(dr);
                this.dtDataSet.AcceptChanges();
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewDataSet.SelectedRows.Count == 0)
            {
                return;
            }
            DataRow dr = ((DataRowView)this.dataGridViewDataSet.SelectedRows[0].DataBoundItem).Row;
            this.dtDataSet.Rows.Remove(dr);
            this.dtDataSet.AcceptChanges();
        }
    }
}
