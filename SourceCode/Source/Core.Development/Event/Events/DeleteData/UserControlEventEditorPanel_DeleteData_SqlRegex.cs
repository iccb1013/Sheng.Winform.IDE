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
using Sheng.SailingEase.Core.Development.Localisation;
using Sheng.SailingEase.Infrastructure;
using System.Diagnostics;
namespace Sheng.SailingEase.Core.Development
{
    [ToolboxItem(false)]
    partial class UserControlEventEditorPanel_DeleteData_SqlRegex : UserControlEventEditorPanelBase
    {
        private IDataEntityComponentService _dataEntityComponentService 
            = ServiceUnity.Container.Resolve<IDataEntityComponentService>();
        public UserControlEventEditorPanel_DeleteData_SqlRegex(EventEditorAdapterAbstract hostAdapter)
            : base(hostAdapter)
        {
            InitializeComponent();
            Unity.ApplyResource(this);
            ApplyLanguageResource();
            this.txtSqlRegex.FormEntity = this.HostAdapter.HostFormEntity;
            this.txtSqlRegex.AllowDataSourceType = DeleteDataDev.AllowDataSourceType;
            this.txtSqlRegex.AllowFormElementControlType = DeleteDataDev.AllowFormElementControlType;
        }
        private void ApplyLanguageResource()
        {
        }
        private void btnGetSqlRegex_Click(object sender, EventArgs e)
        {
            DeleteDataDevEditorAdapter adapter = this.HostAdapter as DeleteDataDevEditorAdapter;
            this.txtSqlRegex.Text = String.Empty;
            if (adapter.ParameterPanels.DataEntity.DataEntityId == String.Empty)
            {
                return;
            }
            string dataEntityId = adapter.ParameterPanels.DataEntity.DataEntityId;
            DataEntity dataEntity = _dataEntityComponentService.GetDataEntity(dataEntityId);
            string sqlRegex = "DELETE FROM {Table} WHERE 1=1";
            string tableName = dataEntity.Code;
            sqlRegex = sqlRegex.Replace("{Table}", "[" + tableName + "]");
            if (adapter.ParameterPanels.DataEntity.Where.Count == 0)
            {
                this.txtSqlRegex.SetContent(sqlRegex.Replace("WHERE 1=1", String.Empty));
                return;
            }
            sqlRegex += Environment.NewLine;
            string dataItemName = String.Empty;
            string valueName = String.Empty;
            foreach (DeleteDataEvent.WhereItem whereItem in adapter.ParameterPanels.DataEntity.Where)
            {
                UIElement formElement;
                DataItemEntity item = dataEntity.Items.GetEntityById(whereItem.DataItem);
                Debug.Assert(item != null, "没有找到指定的数据项实体");
                dataItemName = item.Code;
                if (whereItem.Source.Type == EnumEventDataSource.FormElement)
                {
                    formElement = this.HostAdapter.HostFormEntity.FindFormElementById(whereItem.Source.Source);
                    valueName = "{FormElement." + formElement.FullCode + "}";
                }
                else
                {
                    valueName = "{System." + ((EnumSystemDataSource)Convert.ToInt32(whereItem.Source.Source)).ToString() + "}";
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
                return Language.Current.DeleteDataDev_EditorPanel_Sql;
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
            DeleteDataDev _event = even as DeleteDataDev;
            this.txtSqlRegex.Text = _event.SqlRegex;
        }
    }
}
