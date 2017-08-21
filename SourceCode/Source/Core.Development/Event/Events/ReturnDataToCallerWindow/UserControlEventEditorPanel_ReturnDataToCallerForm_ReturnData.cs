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
namespace Sheng.SailingEase.Core.Development
{
    [ToolboxItem(false)]
    partial class UserControlEventEditorPanel_ReturnDataToCallerForm_ReturnData : UserControlEventEditorPanelBase
    {
        private DataTable dtReturnData;
        private IWindowComponentService _windowComponentService = ServiceUnity.WindowComponentService;
        public UserControlEventEditorPanel_ReturnDataToCallerForm_ReturnData(EventEditorAdapterAbstract hostAdapter)
            : base(hostAdapter)
        {
            InitializeComponent();
            UIHelper.ProcessDataGridView(this.dataGridViewReturnData);
            Unity.ApplyResource(this);
            ApplyLanguageResource();
            dtReturnData = new DataTable();
            dtReturnData.Columns.Add("FormElementCode");
            dtReturnData.Columns.Add("Source");
            dtReturnData.Columns.Add("SourceName");
            dtReturnData.Columns.Add("Warning", typeof(bool));
            this.dataGridViewReturnData.AutoGenerateColumns = false;
            this.dataGridViewReturnData.DataSource = this.dtReturnData;
        }
        private void ApplyLanguageResource()
        {
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            FormEventReturnDataToCallerFormSet formEventReturnDataToCallerFormSet =
                new FormEventReturnDataToCallerFormSet(this.HostAdapter.HostFormEntity);
            if (formEventReturnDataToCallerFormSet.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            DataRow dr = this.dtReturnData.NewRow();
            dr["FormElementCode"] = formEventReturnDataToCallerFormSet.FormElementCode;
            dr["Source"] = formEventReturnDataToCallerFormSet.SelectedDataSourceString;
            dr["SourceName"] = formEventReturnDataToCallerFormSet.SelectedDataSourceVisibleString;
            dr["Warning"] = false;
            this.dtReturnData.Rows.Add(dr);
            this.dtReturnData.AcceptChanges();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewReturnData.SelectedRows.Count == 0)
            {
                return;
            }
            DataRow dr = ((DataRowView)dataGridViewReturnData.SelectedRows[0].DataBoundItem).Row;
            this.dtReturnData.Rows.Remove(dr);
            this.dtReturnData.AcceptChanges();
        }
        private void dataGridViewReturnData_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow dr in this.dataGridViewReturnData.Rows)
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
                return Language.Current.ReturnDataToCallerFormDev_EditorPanel_ReturnData;
            }
        }
        public override List<XObject> GetXml()
        {
            List<XObject> _xObject = new List<XObject>();
            XElement returnSet = new XElement("ReturnSet");
            foreach (DataRow dr in this.dtReturnData.Rows)
            {
                returnSet.Add(new XElement("Return",
                    new XAttribute("FormElementCode", dr["FormElementCode"].ToString()),
                    new XAttribute("Source", dr["Source"].ToString())
                    ) );
            }
            _xObject.Add(returnSet);
            return _xObject;
        }
        public override void SetParameter(EventBase even)
        {
            ReturnDataToCallerFormDev _event = even as ReturnDataToCallerFormDev;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(_event.ReturnSetXml);
            XmlNodeList xmlNodeList = xmlDoc.SelectNodes("ReturnSet/Return");
            DataRow dr;
            bool warningRow;
            this.dtReturnData.Rows.Clear();
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                warningRow = false;
                dr = dtReturnData.NewRow();
                if (_windowComponentService.CheckElementExistByCode
                    (xmlNode.Attributes["FormElementCode"].Value) == false)
                {
                    warningRow = true;
                }
                dr["FormElementCode"] = xmlNode.Attributes["FormElementCode"].Value;
                dr["Source"] = xmlNode.Attributes["Source"].Value;
                dr["SourceName"] =
                    StringParserLogic.DataSourceVisibleString(this.HostAdapter.HostFormEntity, xmlNode.Attributes["Source"].Value, out warningRow);
                dr["Warning"] = warningRow;
                dtReturnData.Rows.Add(dr);
            }
        }
    }
}
