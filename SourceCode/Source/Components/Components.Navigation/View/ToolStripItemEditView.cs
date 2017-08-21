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
using Sheng.SailingEase.ComponentModel.Design;
using Sheng.SailingEase.Components.NavigationComponent.Localisation;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Core.Development;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Localisation;
namespace Sheng.SailingEase.Components.NavigationComponent.View
{
    partial class ToolStripItemEditView : FormViewBase
    {
        private ToolStripItemEntityTypesFactory _toolStripItemEntityTypesFactory = ToolStripItemEntityTypesFactory.Instance;
        private Label _lblDisableEvent = new Label();
        private BindingList<ToolStripItemEntityProvideAttribute> _itemProvideAttribute =
            new BindingList<ToolStripItemEntityProvideAttribute>();
        private UserControlEvent _userControlEvent;
        private string _groupId;
        private ToolStripItemAbstract _toolStripElement;
        public ToolStripItemAbstract ToolStripElement
        {
            get
            {
                return this._toolStripElement;
            }
            set
            {
                this._toolStripElement = value;
                this.propertyGrid.SelectedObjects = new object[] { this._toolStripElement };
                this._userControlEvent.HostEntity = this._toolStripElement;
            }
        }
        public ToolStripItemEditView(string groupId)
        {
            InitializeComponent();
            Unity.ApplyResource(this);
            ApplyLanguageResource();
            _groupId = groupId;
            PropertyGridTypeWrapper typeWrapper = new PropertyGridTypeWrapper(typeof(ToolStripItemAbstract));
            typeWrapper.ActOnSubClass = true;
            typeWrapper.VisibleAll = true;
            typeWrapper.VisibleException.AddRange(new string[] {
                ToolStripItemAbstract.Property_Name, ToolStripItemAbstract.Property_Code });
            propertyGrid.AddTypeWrapper(typeWrapper);
            this.txtCode.Regex = CoreConstant.ENTITY_CODE_REGEX;
            this.txtCode.CustomValidate = CodeValidateMethod;
            this._userControlEvent = new UserControlEvent(null);
            this._userControlEvent.FormEntity = null;
            this._userControlEvent.Dock = DockStyle.Fill;
        }
        private void ToolStripItemEditView_Load(object sender, EventArgs e)
        {
            this._itemProvideAttribute =
                new BindingList<ToolStripItemEntityProvideAttribute>(_toolStripItemEntityTypesFactory.GetProvideAttributeList());
            ddlControlType.DataSource = this._itemProvideAttribute;
            this._lblDisableEvent.Text = Language.Current.FormFormElementAdd_LabelDisable;
            this._lblDisableEvent.Location = new Point(12, 12);
            if (this.ToolStripElement == null)
            {
                CreateToolStripItem();
            }
            else
            {
                this.txtName.Text = this.ToolStripElement.Name;
                this.txtCode.Text = this.ToolStripElement.Code;
                this.ddlControlType.SelectedItem = _toolStripItemEntityTypesFactory.GetProvideAttribute(this.ToolStripElement);
                this.ddlControlType.Enabled = false;
            }
            this.tabPageEvent.Controls.Clear();
            this.tabPageEvent.Controls.Add(this._userControlEvent);
            this.ddlControlType.SelectedIndexChanged += new EventHandler(ddlControlType_SelectedIndexChanged);
        }
        private void ApplyLanguageResource()
        {
            this.tabPageGeneral.Text = Language.Current.ToolStripItemEditView_TabPageGeneral;
            this.tabPageDetail.Text = Language.Current.ToolStripItemEditView_TabPageDetail;
            this.tabPageEvent.Text = Language.Current.ToolStripItemEditView_TabPageEvent;
            this.txtName.Title = Language.Current.ToolStripItemEditView_LabelName;
            this.txtCode.Title = Language.Current.ToolStripItemEditView_LabelCode;
            this.txtCode.RegexMsg = CommonLanguage.Current.RegexMsg_EntityCode;
        }
        private bool CodeValidateMethod(object sender, out string msg)
        {
            msg = String.Empty;
            TextBox textBox = (TextBox)sender;
            string code = String.Empty;
            if (this.ToolStripElement != null)
                code = this.ToolStripElement.Code;
            if (Keywords.Container(code))
            {
                msg = Language.Current.FormFormElementAdd_MessageUseKeywords;
                return false;
            }
            if (code != textBox.Text)
            {
                if (ToolStripArchive.Instance.CheckExistByCode(textBox.Text))
                {
                    msg = Language.Current.FormFormElementAdd_MessageElementCodeExist;
                    return false;
                }
            }
            return true;
        }
        private void CreateToolStripItem()
        {
            ToolStripItemEntityProvideAttribute attribute = this.ddlControlType.SelectedValue as ToolStripItemEntityProvideAttribute;
            if (attribute != null)
            {
                this.ToolStripElement = _toolStripItemEntityTypesFactory.CreateInstance(attribute);
            }
        }
        private void ddlControlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            CreateToolStripItem();
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.DoValidate() == false)
            {
                return;
            }
            this.ToolStripElement.Name = this.txtName.Text;
            this.ToolStripElement.Code = this.txtCode.Text;
            this.ToolStripElement.GroupId = this._groupId;
            ToolStripArchive.Instance.Commit(this.ToolStripElement);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
