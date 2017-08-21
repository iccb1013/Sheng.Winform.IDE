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
using Sheng.SailingEase.Infrastructure;
namespace Sheng.SailingEase.Core.Development
{
    [ToolboxItem(false)]
     partial class UserControlEventEditorPanel_SaveFormData_SqlRegex : UserControlEventEditorPanelBase
    {
        private IDataEntityComponentService _dataEntityComponentService
           = ServiceUnity.Container.Resolve<IDataEntityComponentService>();
        public UserControlEventEditorPanel_SaveFormData_SqlRegex(EventEditorAdapterAbstract hostAdapter)
            : base(hostAdapter)
        {
            InitializeComponent();
            Unity.ApplyResource(this);
            ApplyLanguageResource();
            this.txtSqlRegex.FormEntity = this.HostAdapter.HostFormEntity;
            this.txtSqlRegex.AllowDataSourceType = SaveDataDev.AllowDataSourceType;
            this.txtSqlRegex.AllowFormElementControlType = SaveDataDev.AllowFormElementControlType;
        }
        private void ApplyLanguageResource()
        {
        }
        private void btnGetSqlRegex_Click(object sender, EventArgs e)
        {
            SaveDataDevEditorAdapter adapter = this.HostAdapter as SaveDataDevEditorAdapter;
            this.txtSqlRegex.Text = String.Empty;
            if (adapter.ParameterPanels.DataEntity.DataEntityId == String.Empty)
            {
                return;
            }
            if (adapter.ParameterPanels.DataEntity.Saves.Count == 0)
            {
                return;
            }
            string dataEntityId = adapter.ParameterPanels.DataEntity.DataEntityId;
            DataEntity dataEntity = _dataEntityComponentService.GetDataEntity(dataEntityId);
            string sqlRegex = "INSERT INTO {Table} ({Field}) " + Environment.NewLine + "VALUES({Value})";
            string tableName = dataEntity.Code;
            sqlRegex = sqlRegex.Replace("{Table}", "[" + tableName + "]" + Environment.NewLine);
            string dataItemName, valueName;
            foreach (SaveDataEvent.SaveItem save in adapter.ParameterPanels.DataEntity.Saves)
            {
                dataItemName = dataEntity.Items.GetEntityById(save.DataItem).Code;
                if (save.Source.Type == EnumEventDataSource.FormElement)
                {
                    valueName = "{FormElement." +
                        this.HostAdapter.HostFormEntity.FindFormElementById(save.Source.Source).Code + "}";
                }
                else
                {
                    valueName = "{System." +
                        ((EnumSystemDataSource)Convert.ToInt32(save.Source.Source)).ToString() + "}";
                }
                sqlRegex = sqlRegex.Replace("{Field}", "[" + dataItemName + "],{Field}");
                sqlRegex = sqlRegex.Replace("{Value}", valueName + ",{Value}");
            }
            sqlRegex = sqlRegex.Replace(",{Field}", "");
            sqlRegex = sqlRegex.Replace(",{Value}", "");
            this.txtSqlRegex.SetContent(sqlRegex);
        }
        public override string PanelTitle
        {
            get
            {
                return Language.Current.SaveFormDataDev_EditorPanel_Sql;
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
            SaveDataDev _event = even as SaveDataDev;
            this.txtSqlRegex.Text = _event.SqlRegex;
        }
    }
}
