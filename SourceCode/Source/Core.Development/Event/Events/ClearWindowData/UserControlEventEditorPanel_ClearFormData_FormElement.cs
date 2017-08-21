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
using Sheng.SailingEase.Core.Development;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Core.Development.Localisation;
using Sheng.SailingEase.Core;
namespace Sheng.SailingEase.Core.Development
{
    [ToolboxItem(false)]
    partial class UserControlEventEditorPanel_ClearFormData_FormElement : UserControlEventEditorPanelBase
    {
        private DataTable dtFormElement;
        public DataTable FormElement
        {
            get
            {
                return this.dtFormElement;
            }
            set
            {
                this.dtFormElement = value;
            }
        }
        public UserControlEventEditorPanel_ClearFormData_FormElement(EventEditorAdapterAbstract hostAdapter)
            : base(hostAdapter)
        {
            InitializeComponent();
            UIHelper.ProcessDataGridView(this.dataGridViewFormElement);
            Unity.ApplyResource(this);
            ApplyLanguageResource();
            this.dtFormElement = new DataTable();
            this.dtFormElement.Columns.Add("FormElement");
            this.dtFormElement.Columns.Add("FormElementName");
            this.dtFormElement.Columns.Add("Warning", typeof(bool));
            this.dataGridViewFormElement.AutoGenerateColumns = false;
            this.dataGridViewFormElement.DataSource = this.dtFormElement;
        }
        private void ApplyLanguageResource()
        {
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            FormFormElementChoose formDataEntityChoose =
               new FormFormElementChoose(this.HostAdapter.HostFormEntity);
            if (formDataEntityChoose.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            DataRow dr = this.dtFormElement.NewRow();
            dr["FormElement"] = formDataEntityChoose.SelectedDataSourceString;
            dr["FormElementName"] = formDataEntityChoose.SelectedDataSourceVisibleString;
            dr["Warning"] = false;
            this.dtFormElement.Rows.Add(dr);
            this.dtFormElement.AcceptChanges();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewFormElement.SelectedRows.Count == 0)
            {
                return;
            }
            DataRow dr = ((DataRowView)this.dataGridViewFormElement.SelectedRows[0].DataBoundItem).Row;
            this.dtFormElement.Rows.Remove(dr);
            this.dtFormElement.AcceptChanges();
        }
        private void dataGridViewFormElement_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow dr in this.dataGridViewFormElement.Rows)
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
                return Language.Current.ClearFormDataDev_EditorPanel_FormElement;
            }
        }
        public override List<XObject> GetXml()
        {
            List<XObject> _xObject = new List<XObject>();
            XElement clears = new XElement("Clears");
            foreach (DataRow dr in this.FormElement.Rows)
            {
                clears.Add(new XElement("Clear", new XAttribute("FormElement", dr["FormElement"].ToString())));
            }
            _xObject.Add(clears);
            return _xObject;
        }
        public override void SetParameter(EventBase even)
        {
            ClearFormDataDev _event = even as ClearFormDataDev;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(_event.ClearsSetXml);
            XmlNodeList xmlNodeList = xmlDoc.SelectNodes("Clears/Clear");
            DataRow dr;
            bool warningRow;
            this.FormElement.Rows.Clear();
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                warningRow = false;
                dr = this.FormElement.NewRow();
                dr["FormElement"] = xmlNode.Attributes["FormElement"].Value;
                dr["FormElementName"] =
                    StringParserLogic.DataSourceVisibleString(this.HostAdapter.HostFormEntity, xmlNode.Attributes["FormElement"].Value, out warningRow);
                dr["Warning"] = warningRow;
                this.FormElement.Rows.Add(dr);
            }
            this.FormElement.AcceptChanges();
        }
    }
}
