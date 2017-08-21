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
    partial class UserControlEventEditorPanel_DataListRefresh_SqlRegex : UserControlEventEditorPanelBase
    {
        private IDataEntityComponentService _dataEntityComponentService;
        public UserControlEventEditorPanel_DataListRefresh_SqlRegex(EventEditorAdapterAbstract hostAdapter)
            : base(hostAdapter)
        {
            InitializeComponent();
            _dataEntityComponentService = ServiceUnity.Container.Resolve<IDataEntityComponentService>();
            Unity.ApplyResource(this);
            ApplyLanguageResource();
            this.txtSqlRegex.FormEntity = this.HostAdapter.HostFormEntity;
            this.txtSqlRegex.AllowDataSourceType = DataListRefreshDev.AllowWhereSetDataSourceType;
            this.txtSqlRegex.AllowFormElementControlType = DataListRefreshDev.AllowWhereSetFormElementControlType;
        }
        private void ApplyLanguageResource()
        {
        }
        private void btnGetSqlRegex_Click(object sender, EventArgs e)
        {
            DataListRefreshDevEditorAdapter adapter = this.HostAdapter as DataListRefreshDevEditorAdapter;
            this.txtSqlRegex.Text = String.Empty;
            string dataEntityId = adapter.ParameterPanels.General.DataEntityId;
            if (dataEntityId == String.Empty || dataEntityId == null)
                return;
            UIElementDataListEntity dataListEntity = 
                this.HostAdapter.HostFormEntity.Elements.GetFormElementById(
                adapter.ParameterPanels.General.DataListId) as UIElementDataListEntity;
            if (dataListEntity == null)
                return;
            string sqlRegex = String.Empty;
            sqlRegex += 
@"SELECT Count(Id) FROM {Table};
SELECT {Field} FROM {Table} WHERE 1=1 {Where};
WITH tableOrder AS
(
    SELECT Id,{Field},
    ROW_NUMBER() OVER (order by Id) AS RowNumber
    FROM {Table} WHERE 1=1 {Where}
) 
SELECT *
FROM tableOrder
WHERE RowNumber BETWEEN @rowNumberStart AND @rowNumberEnd
ORDER BY Id;
";
            DataEntity dataEntity = _dataEntityComponentService.GetDataEntity(dataEntityId);
            string tableName = dataEntity.Code;
            sqlRegex = sqlRegex.Replace("{Table}", "[" + tableName + "]");
            foreach (UIElementDataListTextBoxColumnEntity column in dataListEntity.DataColumns)
            {
                sqlRegex = sqlRegex.Replace("{Field}", "[" + column.GetDataItemCode() + "],{Field}");
            }
            sqlRegex = sqlRegex.Replace(",{Field}", "");
            if (adapter.ParameterPanels.Where.Wheres == null || adapter.ParameterPanels.Where.Wheres.Count == 0)
            {
                sqlRegex = sqlRegex.Replace("{Where}", String.Empty);
                this.txtSqlRegex.SetContent(sqlRegex.Replace("WHERE 1=1", String.Empty));
                return;
            }
            sqlRegex += Environment.NewLine;
            string strWhere = String.Empty;
            string dataItemName = String.Empty;
            string valueName = String.Empty;
            foreach(DataListRefreshEvent.WhereItem whereItem in adapter.ParameterPanels.Where.Wheres)
            {
                DataItemEntity item = dataEntity.Items.GetEntityById(whereItem.DataItem);
                Debug.Assert(item!=null, "没有找到指定的数据项实体");
                dataItemName = item.Code;
                if (whereItem.Source.ToString().Split('.')[0] == "FormElement")
                {
                    valueName = "{FormElement." +
                         this.HostAdapter.HostFormEntity.FindFormElementById(whereItem.Source.ToString().Split('.')[1]).Code + "}";
                }
                else
                {
                    valueName = "{System." +
                       ((EnumSystemDataSource)Convert.ToInt32(whereItem.Source.ToString().Split('.')[1])).ToString() + "}";
                }
                strWhere += " AND " +
                        CommonOperater.CombineFieldAndValue(dataItemName, valueName,whereItem.MatchType);
            }
            sqlRegex = sqlRegex.Replace("{Where}", strWhere);
            this.txtSqlRegex.SetContent(sqlRegex.Replace("1=1" + Environment.NewLine + Environment.NewLine + " AND ",
                Environment.NewLine));
        }
        public override string PanelTitle
        {
            get
            {
                return Language.Current.DataListRefreshDev_EditorPanel_Sql;
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
            DataListRefreshDev _event = even as DataListRefreshDev;
            this.txtSqlRegex.Text = _event.SqlRegex;
        }
    }
}
