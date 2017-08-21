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
using Sheng.SailingEase.Core.Development.Localisation;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Infrastructure;
namespace Sheng.SailingEase.Core.Development
{
    [ToolboxItem(false)]
    partial class UserControlEventEditorPanel_OpenWindow_SendData : UserControlEventEditorPanelBase
    {
        private DataTable dtSendData;
        public UserControlEventEditorPanel_OpenWindow_SendData(EventEditorAdapterAbstract hostAdapter)
            : base(hostAdapter)
        {
            InitializeComponent();
            UIHelper.ProcessDataGridView(this.dataGridViewSendData);
            Unity.ApplyResource(this);
            ApplyLanguageResource();
            if (this.HostAdapter.HostFormEntity == null)
            {
                this.btnAdd.Enabled = false;
                this.btnDelete.Enabled = false;
            }
            dtSendData = new DataTable();
            dtSendData.Columns.Add("DataName");
            dtSendData.Columns.Add("DataCode");
            dtSendData.Columns.Add("DataSourceName");
            dtSendData.Columns.Add("DataSource");
            dtSendData.Columns.Add("Warning", typeof(bool));
            this.dataGridViewSendData.AutoGenerateColumns = false;
            this.dataGridViewSendData.DataSource = this.dtSendData;
        }
        private void ApplyLanguageResource()
        {
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            FormEventOpenFormSendDataSet formEventOpenFormSendDataSet =
               new FormEventOpenFormSendDataSet(this.HostAdapter.HostFormEntity);
            formEventOpenFormSendDataSet.SendData = this.dtSendData;
            if (formEventOpenFormSendDataSet.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            DataRow dr = this.dtSendData.NewRow();
            dr["DataName"] = formEventOpenFormSendDataSet.DataName;
            dr["DataCode"] = formEventOpenFormSendDataSet.DataCode;
            dr["DataSource"] = formEventOpenFormSendDataSet.SelectedDataSourceString;
            dr["DataSourceName"] = formEventOpenFormSendDataSet.SelectedDataSourceVisibleString;
            dr["Warning"] = false;
            this.dtSendData.Rows.Add(dr);
            this.dtSendData.AcceptChanges();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewSendData.SelectedRows.Count == 0)
            {
                return;
            }
            DataRow dr = ((DataRowView)this.dataGridViewSendData.SelectedRows[0].DataBoundItem).Row;
            this.dtSendData.Rows.Remove(dr);
            this.dtSendData.AcceptChanges();
        }
        private void dataGridViewSendData_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow dr in this.dataGridViewSendData.Rows)
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
                return Language.Current.OpenFormDev_EditorPanel_SendData;
            }
        }
        public override List<XObject> GetXml()
        {
            List<XObject> _xObject = new List<XObject>();
            XElement sendData = new XElement("SendData");
            foreach (DataRow dr in this.dtSendData.Rows)
            {
                sendData.Add(new XElement("Send",
                    new XAttribute("DataName", dr["DataName"].ToString()),
                    new XAttribute("DataCode", dr["DataCode"].ToString()),
                    new XAttribute("Source", dr["DataSource"].ToString()),
                    new XAttribute("Value", String.Empty)
                    ));
            }
            _xObject.Add(sendData);
            return _xObject;
        }
        public override void SetParameter(EventBase even)
        {
            OpenWindowDev _event = even as OpenWindowDev;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(_event.SendDataXml);
            XmlNodeList xmlNodeList = xmlDoc.SelectNodes("SendData/Send");
            DataRow dr;
            bool warningRow;
            this.dtSendData.Rows.Clear();
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                warningRow = false;
                dr = dtSendData.NewRow();
                dr["DataName"] = xmlNode.Attributes["DataName"].Value;
                dr["DataCode"] = xmlNode.Attributes["DataCode"].Value;
                dr["DataSource"] = xmlNode.Attributes["Source"].Value;
                dr["DataSourceName"] =
                    StringParserLogic.DataSourceVisibleString(this.HostAdapter.HostFormEntity, xmlNode.Attributes["Source"].Value, out warningRow);
                dr["Warning"] = warningRow;
                dtSendData.Rows.Add(dr);
            }
        }
    }
}
