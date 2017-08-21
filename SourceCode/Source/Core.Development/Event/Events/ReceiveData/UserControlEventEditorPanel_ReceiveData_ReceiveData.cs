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
using System.Xml;
using System.Xml.Linq;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Core.Development.Localisation;
using Sheng.SailingEase.Core;
namespace Sheng.SailingEase.Core.Development
{
    [ToolboxItem(false)]
    partial class UserControlEventEditorPanel_ReceiveData_ReceiveData : UserControlEventEditorPanelBase
    {
        private DataTable dtReceiveData;
        public UserControlEventEditorPanel_ReceiveData_ReceiveData(EventEditorAdapterAbstract hostAdapter)
            : base(hostAdapter)
        {
            InitializeComponent();
            UIHelper.ProcessDataGridView(this.dataGridViewReceiveData);
            Unity.ApplyResource(this);
            ApplyLanguageResource();
            dtReceiveData = new DataTable();
            dtReceiveData.Columns.Add("DataCode");
            dtReceiveData.Columns.Add("ReceiveToName");
            dtReceiveData.Columns.Add("ReceiveTo");
            dtReceiveData.Columns.Add("Warning", typeof(bool));
            this.dataGridViewReceiveData.AutoGenerateColumns = false;
            this.dataGridViewReceiveData.DataSource = this.dtReceiveData;
        }
        private void ApplyLanguageResource()
        {
            this.ColumnDataCode.HeaderText = Language.Current.UserControlEventEditorPanel_ReceiveData_ReceiveData_ColumnDataCode;
            this.ColumnReceiveToName.HeaderText = Language.Current.UserControlEventEditorPanel_ReceiveData_ReceiveData_ColumnReceiveToName;
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            FormEventReceiveDataSet formEventReceiveDataSet =
                new FormEventReceiveDataSet(this.HostAdapter.HostFormEntity);
            if (formEventReceiveDataSet.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            DataRow dr = this.dtReceiveData.NewRow();
            dr["DataCode"] = formEventReceiveDataSet.DataCode;
            dr["ReceiveTo"] = formEventReceiveDataSet.SelectedDataSourceString;
            dr["ReceiveToName"] = formEventReceiveDataSet.SelectedDataSourceVisibleString;
            dr["Warning"] = false;
            this.dtReceiveData.Rows.Add(dr);
            this.dtReceiveData.AcceptChanges();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewReceiveData.SelectedRows.Count == 0)
            {
                return;
            }
            DataRow dr = ((DataRowView)dataGridViewReceiveData.SelectedRows[0].DataBoundItem).Row;
            this.dtReceiveData.Rows.Remove(dr);
            this.dtReceiveData.AcceptChanges();
        }
        private void dataGridViewReceiveData_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow dr in this.dataGridViewReceiveData.Rows)
            {
                if (dr != null)
                {
                    if ((bool)((DataRowView)dr.DataBoundItem).Row["Warning"])
                    {
                        dr.DefaultCellStyle.BackColor = UIHelper.DataGridViewRowBackColorWarning;
                        dr.DefaultCellStyle.SelectionBackColor = UIHelper.DataGridViewSelectedRowBackColorWarning;
                        dr.DefaultCellStyle.ForeColor = UIHelper.DataGridViewRowForeColorWarning;
                        dr.DefaultCellStyle.SelectionForeColor = UIHelper.DataGridViewSelectedRowForeColorWarning;
                    }
                    else
                    {
                        dr.DefaultCellStyle.BackColor = UIHelper.DataGridViewRowBackColorNormal;
                        dr.DefaultCellStyle.SelectionBackColor = UIHelper.DataGridViewSelectedRowBackColorNormal;
                        dr.DefaultCellStyle.ForeColor = UIHelper.DataGridViewRowForeColorNormal;
                        dr.DefaultCellStyle.SelectionForeColor = UIHelper.DataGridViewSelectedRowForeColorNormal;
                    }
                }
            }
        }
        public override string PanelTitle
        {
            get
            {
                return Language.Current.ReceiveDataDev_EditorPanel_ReceiveData;
            }
        }
        public override List<XObject> GetXml()
        {
            List<XObject> _xObject = new List<XObject>();
            XElement receiveData = new XElement("ReceiveData");
            foreach (DataRow dr in this.dtReceiveData.Rows)
            {
                receiveData.Add(new XElement("Receive",
                    new XAttribute("DataCode", dr["DataCode"].ToString()),
                    new XAttribute("ReceiveTo", dr["ReceiveTo"].ToString())
                    ));
            }
            _xObject.Add(receiveData);
            return _xObject;
        }
        public override void SetParameter(EventBase even)
        {
            ReceiveDataDev _event = even as ReceiveDataDev;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(_event.ReceiveDataXml);
            XmlNodeList xmlNodeList = xmlDoc.SelectNodes("ReceiveData/Receive");
            DataRow dr;
            bool warningRow;
            dtReceiveData.Rows.Clear();
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                warningRow = false;
                dr = dtReceiveData.NewRow();
                dr["DataCode"] = xmlNode.Attributes["DataCode"].Value;
                dr["ReceiveTo"] = xmlNode.Attributes["ReceiveTo"].Value;
                dr["ReceiveToName"] =
                    StringParserLogic.DataSourceVisibleString(this.HostAdapter.HostFormEntity, xmlNode.Attributes["ReceiveTo"].Value, out warningRow);
                dr["Warning"] = warningRow;
                dtReceiveData.Rows.Add(dr);
            }
        }
    }
}
