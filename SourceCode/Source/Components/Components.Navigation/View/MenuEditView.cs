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
using System.Text;
using System.Windows.Forms;
using Sheng.SailingEase.Components.NavigationComponent.Localisation;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Core.Development;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Localisation;
namespace Sheng.SailingEase.Components.NavigationComponent.View
{
    partial class MenuEditView : FormViewBase
    {
        string _parentId;
        UserControlEvent _userControlEvent = new UserControlEvent(null);
        MenuEntity _mainMenuEntity;
        public MenuEntity MainMenuEntity
        {
            get
            {
                return this._mainMenuEntity;
            }
            set
            {
                this._mainMenuEntity = value;
            }
        }
        public MenuEditView(string parentId)
        {
            InitializeComponent();
            Unity.ApplyResource(this);
            ApplyLanguageResource();
            this._parentId = parentId;
            this.txtCode.Regex = CoreConstant.ENTITY_CODE_REGEX;
            this.txtCode.CustomValidate = CodeValidateMethod;
            _userControlEvent.Dock = DockStyle.Fill;
            this.panelEvent.Controls.Add(_userControlEvent);
        }
        private void MenuEditView_Load(object sender, EventArgs e)
        {
            if (this.MainMenuEntity != null)
            {
                this.txtName.Text = _mainMenuEntity.Name;
                this.txtCode.Text = _mainMenuEntity.Code;
                this.txtText.Text = _mainMenuEntity.Text;
                this.txtRemark.Text = _mainMenuEntity.Remark;
            }
            else
            {
                this.MainMenuEntity = new MainMenuEntityDev();
            }
            this._userControlEvent.FormEntity = null;
            this._userControlEvent.HostEntity = this.MainMenuEntity;
        }
        private void ApplyLanguageResource()
        {
            this.txtName.Title = Language.Current.MenuEditView_LabelName;
            this.txtCode.Title = Language.Current.MenuEditView_LabelCode;
            this.txtCode.RegexMsg = CommonLanguage.Current.RegexMsg_EntityCode;
            this.tabPageGeneral.Text = Language.Current.MenuEditView_TabPageGeneral;
            this.tabPageEvent.Text = Language.Current.MenuEditView_TabPageEvent;
            this.tabPageRemark.Text = Language.Current.MenuEditView_TabPageRemark;
        }
        private bool CodeValidateMethod(object sender, out string msg)
        {
            msg = String.Empty;
            TextBox textBox = (TextBox)sender;
            string code = String.Empty;
            if (this.MainMenuEntity != null)
                code = this.MainMenuEntity.Code;
            if (Keywords.Container(code))
            {
                msg = Language.Current.MenuEditView_MessageUseKeywords;
                return false;
            }
            if (code != textBox.Text)
            {
                if (MenuStripArchive.Instance.CheckExistByCode(textBox.Text))
                {
                    msg = Language.Current.MenuEditView_MessageMainMenuCodeExist;
                    return false;
                }
            }
            return true;
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.DoValidate() == false)
            {
                return;
            }
            _mainMenuEntity.Name = txtName.Text;
            _mainMenuEntity.Code = txtCode.Text;
            _mainMenuEntity.Text = txtText.Text;
            _mainMenuEntity.Remark = txtRemark.Text;
            _mainMenuEntity.ParentId = this._parentId;
            _mainMenuEntity.Layer = (byte)(MenuStripArchive.Instance.GetLayer(this._parentId) + 1);
            MenuStripArchive.Instance.Commit(this.MainMenuEntity);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
