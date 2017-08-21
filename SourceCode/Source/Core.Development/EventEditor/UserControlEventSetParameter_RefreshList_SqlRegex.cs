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
using Sheng.SIMBE.SEControl.TextEditor.Document;
using Sheng.SIMBE.LanguageResource.IDELanguageResource;
using Sheng.SIMBE.IDE.EntityDev;
using Sheng.SIMBE.IDE.Operator;
using Sheng.SIMBE.Core;
using Sheng.SIMBE.Core.Event;
using Sheng.SIMBE.IDE.EventDev;
namespace Sheng.SIMBE.IDE.UI.EventSet
{
    [ToolboxItem(false)]
    internal partial class UserControlEventSetParameter_RefreshList_SqlRegex : UserControlEventSetParameter
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
        public UserControlEventSetParameter_RefreshList_SqlRegex(FormEntityDev formEntity)
            : base(formEntity)
        {
            InitializeComponent();
            ApplyLanguageResource();
            this.txtSqlRegex.Document.HighlightingStrategy =
                HighlightingStrategyFactory.CreateHighlightingStrategy("SQL");
        }
        private void ApplyLanguageResource()
        {
            this.lblNotice.Text = Language.GetString("UserControlEventSetParameter_RefreshList_SqlRegex_LabelNotice");
            this.btnGetSqlRegex.Text = Language.GetString("UserControlEventSetParameter_RefreshList_SqlRegex_ButtonGetSqlRegex");
            this.lblTitle.Text = " " + Language.GetString("UserControlEventSetParameter_RefreshList_SqlRegex_LabelTitle");
        }
        private void btnGetSqlRegex_Click(object sender, EventArgs e)
        {
            this.txtSqlRegex.Text = String.Empty;
            string sqlRegex = "SELECT * FROM {Table} WHERE 1=1";
            string dataEntityId = this.ParameterSetPlanCollection.General.DataEntityId;
            if (dataEntityId == String.Empty)
            {
                return;
            }
            string tableName = DataEntityOperator.GetDataEntity(dataEntityId).Code;
            sqlRegex = sqlRegex.Replace("{Table}", "[" + tableName + "]");
            if (this.ParameterSetPlanCollection.Where.Where == null || this.ParameterSetPlanCollection.Where.Where.Rows.Count == 0)
            {
                this.txtSqlRegex.Text = sqlRegex.Replace("WHERE 1=1", String.Empty);
                return;
            }
            sqlRegex += Environment.NewLine;
            string dataItemName = String.Empty;
            string valueName = String.Empty;
            foreach (DataRow dr in this.ParameterSetPlanCollection.Where.Where.Rows)
            {
                dataItemName = DataEntityOperator.GetDataItemEntity(dr["DataItem"].ToString()).Code;
                if (dr["DataSource"].ToString().Split('.')[0] == "FormElement")
                {
                    valueName = "{FormElement." +
                         this.FormEntity.GetFormElement(dr["DataSource"].ToString().Split('.')[1]).Code + "}";
                }
                else
                {
                    valueName = "{System." +
                       ((EnumSystemDataSource)Convert.ToInt32(dr["DataSource"].ToString().Split('.')[1])).ToString() + "}";
                }
                sqlRegex += Environment.NewLine + " AND " + EventOperator.CombineFieldAndValue(
                    dataItemName, valueName, (EnumMatchType)Convert.ToInt32(dr["MatchType"].ToString()));
            }
            this.txtSqlRegex.Text = sqlRegex.Replace("1=1" + Environment.NewLine + Environment.NewLine + " AND ",
                Environment.NewLine); 
        }
        public override string GetXml()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement xmlEle;
            xmlEle = xmlDoc.CreateElement("Paramter");
            xmlDoc.AppendChild(xmlEle);
            xmlEle = xmlDoc.CreateElement("SqlRegex");
            xmlEle.InnerText = this.txtSqlRegex.Text;
            xmlDoc.FirstChild.AppendChild(xmlEle);
            return xmlDoc.FirstChild.InnerXml;
        }
        public override void SetParameter(EventBase even)
        {
            RefreshListDev _event = even as RefreshListDev;
            this.txtSqlRegex.Text = _event.SqlRegex;
        }
    }
}
