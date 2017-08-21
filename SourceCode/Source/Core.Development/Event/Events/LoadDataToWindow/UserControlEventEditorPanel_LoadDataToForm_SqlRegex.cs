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
using System.Diagnostics;
namespace Sheng.SailingEase.Core.Development
{
    [ToolboxItem(false)]
    partial class UserControlEventEditorPanel_LoadDataToForm_SqlRegex : UserControlEventEditorPanelBase
    {
        private IDataEntityComponentService _dataEntityComponentService
           = ServiceUnity.Container.Resolve<IDataEntityComponentService>();
        public UserControlEventEditorPanel_LoadDataToForm_SqlRegex(EventEditorAdapterAbstract hostAdapter)
            : base(hostAdapter)
        {
            InitializeComponent();
            Unity.ApplyResource(this);
            ApplyLanguageResource();
            this.txtSqlRegex.FormEntity = this.HostAdapter.HostFormEntity;
            this.txtSqlRegex.AllowDataSourceType = LoadDataToFormDev.AllowWhereSetDataSourceType;
            this.txtSqlRegex.AllowFormElementControlType = LoadDataToFormDev.AllowFormElementControlType;
        }
        private void ApplyLanguageResource()
        {
        }
        private void btnGetSqlRegex_Click(object sender, EventArgs e)
        {
            LoadDataToFormDevEditorAdapter adapter = this.HostAdapter as LoadDataToFormDevEditorAdapter;
            this.txtSqlRegex.Text = String.Empty;
            if (adapter.ParameterPanels.DataEntity.DataEntityId == String.Empty)
            {
                return;
            }
            string dataEntityId = adapter.ParameterPanels.DataEntity.DataEntityId;
            DataEntity dataEntity = _dataEntityComponentService.GetDataEntity(dataEntityId);
            List<DataItemEntity> dataItems = dataEntity.Items.ToList();
            string sqlRegex = "SELECT {Field} FROM {Table} WHERE 1=1";
            string tableName = dataEntity.Code;
            sqlRegex = sqlRegex.Replace("{Table}", "[" + tableName + "]");
            string dataItemName;
            foreach (LoadDataToFormDev.LoadItem load in adapter.ParameterPanels.Load.Loads)
            {
                List<DataItemEntity> items =
                    (from c in dataItems where c.Id.Equals(load.DataItem) select c).ToList();
                Debug.Assert(items.Count == 1, "没有找到指定的数据项实体");
                dataItemName = items[0].Code;
                sqlRegex = sqlRegex.Replace("{Field}", "[" + dataItemName + "],{Field}");
            }
            sqlRegex = sqlRegex.Replace(",{Field}", "");
            sqlRegex = sqlRegex.Replace("{Field}", "*");
            if (adapter.ParameterPanels.DataEntity.Wheres.Count == 0)
            {
                this.txtSqlRegex.SetContent(sqlRegex.Replace("WHERE 1=1", String.Empty));
                return;
            }
            sqlRegex += Environment.NewLine;
            string valueName = String.Empty;
            foreach (LoadDataToFormEvent.WhereItem whereItem in adapter.ParameterPanels.DataEntity.Wheres)
            {
                List<DataItemEntity> items =
                    (from c in dataItems where c.Id.Equals(whereItem.DataItem) select c).ToList();
                Debug.Assert(items.Count == 1, "没有找到指定的数据项实体");
                dataItemName = items[0].Code;
                if (whereItem.Source.Type == EnumEventDataSource.FormElement)
                {
                    valueName = "{FormElement." +
                         this.HostAdapter.HostFormEntity.FindFormElementById(whereItem.Source.Source).Code + "}";
                }
                else
                {
                    valueName = "{System." +
                       ((EnumSystemDataSource)Convert.ToInt32(whereItem.Source.Source)).ToString() + "}";
                }
                sqlRegex += Environment.NewLine + " AND " + CommonOperater.CombineFieldAndValue(
                    dataItemName, valueName, whereItem.MatchType);
            }
            this.txtSqlRegex.SetContent(sqlRegex.Replace("1=1" + Environment.NewLine + Environment.NewLine + " AND ",
                Environment.NewLine));
        }
        public override string PanelTitle
        {
            get
            {
                return Language.Current.LoadDataToFormDev_EditorPanel_Sql;
            }
        }
        public override List<XObject> GetXml()
        {
            List<XObject> _xObject = new List<XObject>();
            _xObject.Add(new XElement("SqlRegex", txtSqlRegex.Text));
            return _xObject;
        }
        public override void SetParameter(EventBase even)
        {
            LoadDataToFormDev _event = even as LoadDataToFormDev;
            this.txtSqlRegex.Text = _event.SqlRegex;
        }
    }
}
