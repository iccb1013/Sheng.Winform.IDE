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
     partial class UserControlEventEditorPanel_UpdateFormData_SqlRegex : UserControlEventEditorPanelBase
    {
        private IDataEntityComponentService _dataEntityComponentService
           = ServiceUnity.Container.Resolve<IDataEntityComponentService>();
        public UserControlEventEditorPanel_UpdateFormData_SqlRegex(EventEditorAdapterAbstract hostAdapter)
            : base(hostAdapter)
        {
            InitializeComponent();
            Unity.ApplyResource(this);
            ApplyLanguageResource();
            this.txtSqlRegex.FormEntity = this.HostAdapter.HostFormEntity;
            this.txtSqlRegex.AllowDataSourceType = UpdateFormDataDev.AllowDataSourceType;
            this.txtSqlRegex.AllowFormElementControlType = UpdateFormDataDev.AllowFormElementControlType;
        }
        private void ApplyLanguageResource()
        {
        }
        private void btnGetSqlRegex_Click(object sender, EventArgs e)
        {
            UpdateFormDataDevEditorAdapter adapter = this.HostAdapter as UpdateFormDataDevEditorAdapter;
            this.txtSqlRegex.Text = String.Empty;
            if (adapter.ParameterPanels.Update.Updates.Count == 0 &&
                adapter.ParameterPanels.Where.Wheres.Count==  0)
            {
                return;
            }
            string dataEntityId = adapter.ParameterPanels.Update.DataEntityId;
            DataEntity dataEntity = _dataEntityComponentService.GetDataEntity(dataEntityId);
            string sqlRegex = "UPDATE {Table} SET {Field} WHERE 1=1";
            string tableName = dataEntity.Code;
            sqlRegex = sqlRegex.Replace("{Table}", "[" + tableName + "]");
            string dataItemName = String.Empty;
            string valueName = String.Empty;
            if (adapter.ParameterPanels.Update.Updates.Count > 0)
            {
                sqlRegex += Environment.NewLine;
                foreach (UpdateFormDataDev.UpdateItem update in adapter.ParameterPanels.Update.Updates)
                {
                    dataItemName = dataEntity.Items.GetEntityById(update.DataItem).Code;
                    if (update.Source.Type == EnumEventDataSource.FormElement)
                    {
                        valueName = "{FormElement." +
                            this.HostAdapter.HostFormEntity.FindFormElementById(update.Source.Source).Code + "}";
                    }
                    else
                    {
                        valueName = "{System." +
                            ((EnumSystemDataSource)Convert.ToInt32(update.Source.Source)).ToString() + "}";
                    }
                    sqlRegex = sqlRegex.Replace("{Field}", "[" + dataItemName + "] = " + valueName + ",{Field}");
                }
                sqlRegex = sqlRegex.Replace(",{Field}", "");
            }
            else
            {
                sqlRegex = sqlRegex.Replace("{Field}", "");
            }
            if (adapter.ParameterPanels.Where.Wheres.Count == 0)
            {
                this.txtSqlRegex.SetContent(sqlRegex.Replace("WHERE 1=1", String.Empty));
                return;
            }
            sqlRegex += Environment.NewLine;
            foreach (UpdateFormDataEvent.WhereItem where in adapter.ParameterPanels.Where.Wheres)
            {
                dataItemName = dataEntity.Items.GetEntityById(where.DataItem).Code;
                if (where.Source.Type == EnumEventDataSource.FormElement)
                {
                    valueName = "{FormElement." +
                        this.HostAdapter.HostFormEntity.FindFormElementById(where.Source.Source).Code + "}";
                }
                else
                {
                    valueName = "{System." +
                        ((EnumSystemDataSource)Convert.ToInt32(where.Source.Source)).ToString() + "}";
                }
                sqlRegex += Environment.NewLine + " AND " + CommonOperater.CombineFieldAndValue(
                    dataItemName, valueName, where.MatchType);
            }
            this.txtSqlRegex.SetContent(sqlRegex.Replace("1=1" + Environment.NewLine + Environment.NewLine + " AND ",
                 Environment.NewLine));
        }
        public override string PanelTitle
        {
            get
            {
                return Language.Current.UpdateFormDataDev_EditorPanel_Sql;
            }
        }
        public override List<XObject> GetXml()
        {
            List<XObject> _xObject = new List<XObject>();
            _xObject.Add(new XElement("SqlRegex", this.txtSqlRegex.Text));
            return _xObject;
        }
        public override void SetParameter(EventBase even)
        {
            UpdateFormDataDev _event = even as UpdateFormDataDev;
            this.txtSqlRegex.Text = _event.SqlRegex;
        }
    }
}
