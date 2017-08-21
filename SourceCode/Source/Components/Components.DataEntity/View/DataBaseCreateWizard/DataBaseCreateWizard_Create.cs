/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Sheng.SailingEase.Controls;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Data;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Kernal;
namespace Sheng.SailingEase.Components.DataEntityComponent.View
{
    [ToolboxItem(false)]
    partial class DataBaseCreateWizard_Create : WizardPanelBase
    {
        private BackgroundWorker _backgroundWorkerCreateDataBase = new BackgroundWorker();
        private DataBaseCreateOption _option;
        private IDataEntityComponentService _dataEntityComponentService = ServiceUnity.DataEntityComponentService;
        private IDictionaryComponentService _dictionaryComponentService = ServiceUnity.DictionaryComponentService;
        public DataBaseCreateWizard_Create()
        {
            InitializeComponent();
            Unity.ApplyResource(this);
            this.panelCreate.Dock = DockStyle.Fill;
            this.panelError.Dock = DockStyle.Fill;
            _backgroundWorkerCreateDataBase.DoWork +=
                new DoWorkEventHandler(backgroundWorkerCreateDataBase_DoWork);
            _backgroundWorkerCreateDataBase.RunWorkerCompleted +=
                new RunWorkerCompletedEventHandler(backgroundWorkerCreateDataBase_RunWorkerCompleted);
        }
        public override void ProcessButton()
        {
            this.WizardView.BackButtonEnabled = false;
            this.WizardView.NextButtonEnabled = false;
            this.WizardView.FinishButtonEnabled = false;
        }
        public override void Submit()
        {
            this.WizardView.NextPanel();
        }
        public override void Run()
        {
            _option = WizardView.GetOptionInstance<DataBaseCreateOption>();
            this.WizardView.CloseButtonEnabled = false;
            this.panelCreate.Visible = true;
            this.panelError.Visible = false;
            this._backgroundWorkerCreateDataBase.RunWorkerAsync();
        }
        private void backgroundWorkerCreateDataBase_DoWork(object sender,
           DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            CreateDataBase(worker, e);
        }
        private void backgroundWorkerCreateDataBase_RunWorkerCompleted(object sender,
           RunWorkerCompletedEventArgs e)
        {
            this.WizardView.CloseButtonEnabled = true;
            if (e.Error != null)
            {
                this.WizardView.BackButtonEnabled = true;
                this.panelCreate.Visible = false;
                this.panelError.Visible = true;
                this.txtExceptionMsg.Text = e.Error.Message;
            }
            else
            {
                this.WizardView.NextPanel();
            }
        }
        private void CreateDataBase(BackgroundWorker worker, DoWorkEventArgs e)
        {
            SEDataBase dataBase;
            StringBuilder strSql = new StringBuilder();
            if (this._option.CreateDataBase)
            {
                dataBase = new SEDataBase(AppConstant.ConnectionStringName);
                strSql.Append(DataBaseProvide.Current.CreateSql(this._option.DataBaseName));
                dataBase.ExecuteNonQuery(strSql.ToString());
            }
            dataBase = new SEDataBase(AppConstant.ConnectionStringName2);
            strSql = new StringBuilder();
            List<DataEntity> dataEntityList = _dataEntityComponentService.GetDataEntityList();
            foreach (DataEntityDev dataEntity in dataEntityList)
            {
                strSql.Append(dataEntity.GetSql());
                strSql.Append(Environment.NewLine);
            }
            dataBase.ExecuteNonQuery(strSql.ToString());
            strSql = new StringBuilder();
            if (this._option.InsertEnum)
            {
                string strEnumKey = String.Empty;
                int enumItemSort = 0;
                List<EnumEntity> entityList = _dictionaryComponentService.GetEnumEntityList();
                foreach (EnumEntity enumEntity in entityList)
                {
                    enumItemSort = 0;
                    strSql = new StringBuilder();
                    SEDbCommandParameterCollection enumParameters = new SEDbCommandParameterCollection();
                    string itemsSql = String.Empty;
                    foreach (EnumItemEntity item in enumEntity.Items)
                    {
                        string strParamterText = "@text" + enumItemSort.ToString();
                        string strParamterValue = "@value" + enumItemSort.ToString();
                        string strParamterEnumKey = "@enumKey" + enumItemSort.ToString();
                        string strParamterSort = "@sort" + enumItemSort.ToString();
                        itemsSql += "INSERT INTO [Enum] (Text,Value,EnumKey,Sort) VALUES ({0},{1},{2},{3});";
                        itemsSql += Environment.NewLine;
                        strSql.Append(String.Format(itemsSql,
                            strParamterText, strParamterValue, strParamterEnumKey, strParamterSort));
                        enumParameters.Add(new SEDbCommandParameter(strParamterText, item.Text));
                        enumParameters.Add(new SEDbCommandParameter(strParamterValue, item.Value));
                        enumParameters.Add(new SEDbCommandParameter(strParamterEnumKey, enumEntity.Code));
                        enumParameters.Add(new SEDbCommandParameter(strParamterSort, enumItemSort.ToString()));
                        enumItemSort++;
                    }
                    dataBase.ExecuteNonQuery(CommandType.Text, strSql.ToString(), enumParameters);
                }
            }
            string passwordMD5 = MD5Encryption.MD5Generat(_option.Password);
            strSql = new StringBuilder();
            SEDbCommandParameterCollection parametersAccount = new SEDbCommandParameterCollection();
            strSql.Append("INSERT INTO [User] (Name,LoginName,Password) VALUES ({0},{1},{2})");
            string strName = "@name";
            string strLoginNamee = "@loginName";
            string strPassword = "@password";
            strSql = new StringBuilder(String.Format(strSql.ToString(), strName, strLoginNamee, strPassword));
            parametersAccount.Add(new SEDbCommandParameter(strName, _option.LoginName));
            parametersAccount.Add(new SEDbCommandParameter(strLoginNamee, _option.LoginName));
            parametersAccount.Add(new SEDbCommandParameter(strPassword, passwordMD5));
            dataBase.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parametersAccount);
        }
    }
}
