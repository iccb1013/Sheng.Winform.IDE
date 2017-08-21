using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Core.Development;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Windows.Forms.Development.Localisation;
using Sheng.SailingEase.Localisation;
using Sheng.SailingEase.ComponentModel.Design.UndoEngine;
using Sheng.SailingEase.ComponentModel.Design;

namespace Sheng.SailingEase.Windows.Forms.Development
{
    /// <summary>
    /// 添加数据列
    /// 此窗体有两种用法,一是从数据列表表界面过来,添加一个就OK
    /// 另一个是直接从窗体设计器进来,点过一点添加以后,取消按钮变成关闭,可以一直点添加
    /// </summary>
    partial class FormSEPaginationDataGridViewDevColumnAdd : FormViewBase
    {
        #region 私有成员

        private IDataEntityComponentService _dataEntityComponentService = ServiceUnity.DataEntityComponentService;

        private IWindowDesignService _windowDesignService = ServiceUnity.WindowDesignService;

        private FormElementDataListEntityDev _entity;

        /// <summary>
        /// 选择的数据实体
        /// </summary>
        private string _dataEntityId;

        /// <summary>
        /// 当前是否为编辑状态
        /// </summary>
        private bool _isEdit = false;

        #endregion

        #region 公开属性

        /// <summary>
        /// 用于直接从窗体设计器中点添加进来时,每添加一列都能够updateview
        /// </summary>
        public IShellControlDev ShellControlDev
        {
            get;
            set;
        }

        private FormEntityDev _formEntity;
        /// <summary>
        /// 所属的窗口实体
        /// 用于查询代码是否被占用
        /// </summary>
        public FormEntityDev FormEntity
        {
            get
            {
                return this._formEntity;
            }
            set
            {
                this._formEntity = value;
            }
        }

        private FormElementDataListTextBoxColumnEntityDev _formElementDataColumnEntity;
        /// <summary>
        /// 数据列对象
        /// </summary>
        public FormElementDataListTextBoxColumnEntityDev FormElementDataColumnEntity
        {
            get
            {
                return this._formElementDataColumnEntity;
            }
            set
            {
                this._formElementDataColumnEntity = value;
            }
        }

        /// <summary>
        /// 当前正在编辑的列对象所在的列表表,允许为null
        /// 当从集合编辑器中过来时,就不为null,就是集合编辑器中正在编辑的集合
        /// </summary>
        public IList EditCollection
        {
            get;
            set;
        }

        #endregion

        #region 构造及窗体事件

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="entity"></param>
        public FormSEPaginationDataGridViewDevColumnAdd(FormElementDataListEntityDev entity)
        {
            InitializeComponent();

            Unity.ApplyResource(this);
            this.ApplyLanguageResource();

            this._dataEntityId = entity.DataEntityId;
            this._entity = entity;

            //加载数据规则列表
            this.ddlDataRule.DataSource = FormElementDataListColumnDataRuleDevTypes.Instance.GetInstanceList<IFormElementDataListColumnDataRule>();
        }

        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormSEPaginationDataGridViewDevColumnAdd_Load(object sender, EventArgs e)
        {
            #region 根据传来的数据实体Id（或空串）判断是否可绑定，并加载相应数据
            //根据传来的数据实体Id（或空串）判断是否可绑定，并加载相应数据
            if (this._dataEntityId ==null ||  this._dataEntityId == String.Empty)
            {
                radioButtonUnBind.Checked = true;
                radioButtonBind.Enabled = false;
            }
            else
            {
                radioButtonBind.Checked = true;
                //加载数据项下拉列表
                ddlDataItem.DataSource = _dataEntityComponentService.GetDataEntity(this._dataEntityId).Items;
            }
            ChangeBind();
            #endregion

            #region 判断是否编辑状态
            //判断是否编辑状态
            if (this.FormElementDataColumnEntity == null)
            {
                return;
            }

            this._isEdit = true;

            #endregion

            #region 加载要编辑的列的信息

            //加载要编辑的列的信息
            this.txtName.Text = this.FormElementDataColumnEntity.Name;
            this.txtCode.Text = this.FormElementDataColumnEntity.Code;
            this.txtText.Text = this.FormElementDataColumnEntity.Text;

            if (this.FormElementDataColumnEntity.IsBind)
            {
                this.radioButtonBind.Checked = true;
            }
            else
            {
                this.radioButtonUnBind.Checked = true;
            }
            if (this.radioButtonBind.Checked)
            {
                this.ddlDataItem.SelectedValue = this.FormElementDataColumnEntity.DataItemId.Split('.')[1];
            }
            this.txtDataPropertyName.Text = this.FormElementDataColumnEntity.DataPropertyName;
            this.cbVisible.Checked = this.FormElementDataColumnEntity.Visible;


            //数据规则
            foreach (object obj in this.ddlDataRule.Items)
            {
                UIElementDataListColumnDataRuleAbstract dataRuleAbstract = obj as UIElementDataListColumnDataRuleAbstract;
                if (dataRuleAbstract.GetType().Equals(this.FormElementDataColumnEntity.DataRule.GetType()))
                {
                    this.ddlDataRule.SelectedItem = obj;
                    break;
                }
            }

            IFormElementDataListColumnDataRule dataRule = this.ddlDataRule.SelectedItem as IFormElementDataListColumnDataRule;
            dataRule.SetParameter(this.FormElementDataColumnEntity.DataRule);

            #endregion

            #region 显示warning信息

            //显示warning信息
            //if (this.FormElementDataColumnEntity.Warning && this.FormElementDataColumnEntity.SelfWarning)
            //{
            //    MessageBox.Show(
            //        this.FormElementDataColumnEntity.WarningMsg,
            //           Language.Current.MessageBoxCaptiton_Warning,
            //           MessageBoxButtons.OK,
            //           MessageBoxIcon.Warning
            //           );
            //}
            if (this.FormElementDataColumnEntity.Warning.ExistWarning)
            {
                FormWarning formWarning = new FormWarning();
                formWarning.WarningSign = this.FormElementDataColumnEntity.Warning;
                formWarning.ShowDialog();
            }

            #endregion
        }

        #endregion

        #region 应用资源

        private void ApplyLanguageResource()
        {
            this.txtName.Title = Language.Current.FormSEPaginationDataGridViewDevColumnAdd_LabelName;
            this.txtCode.Title = Language.Current.FormSEPaginationDataGridViewDevColumnAdd_LabelCode;
            this.txtDataPropertyName.Title = Language.Current.FormSEPaginationDataGridViewDevColumnAdd_DataPropertyName;
            this.txtText.Title = Language.Current.FormSEPaginationDataGridViewDevColumnAdd_LabelHeaderText;
            this.ddlDataItem.Title = Language.Current.FormSEPaginationDataGridViewDevColumnAdd_DataItem;

            this.txtCode.Regex = CoreConstant.ENTITY_CODE_REGEX;
            this.txtCode.RegexMsg = CommonLanguage.Current.RegexMsg_EntityCode;
            this.txtDataPropertyName.Regex = CoreConstant.ENTITY_CODE_REGEX;
            this.txtDataPropertyName.RegexMsg = CommonLanguage.Current.RegexMsg_EntityCode;
        }

        #endregion

        #region 内部方法

        /// <summary>
        /// 重置控件的值
        /// </summary>
        private void ResetControl()
        {
            this.txtName.Text = String.Empty;
            this.txtCode.Text = String.Empty;
            this.txtText.Text = String.Empty;
            this.radioButtonBind.Checked = true;
            if (this.ddlDataItem.Items.Count > 0)
            {
                ddlDataItem.SelectedIndex = 0;
            }
            this.cbVisible.Checked = true;
        }

        private void ChangeBind()
        {
            lblDataItem.Visible = ddlDataItem.Visible = radioButtonBind.Checked;
            lblDataPropertyName.Visible = txtDataPropertyName.Visible = !radioButtonBind.Checked;

            ddlDataItem.AllowEmpty = !radioButtonBind.Checked;
            txtDataPropertyName.AllowEmpty = radioButtonBind.Checked;
        }

        #endregion

        #region 事件处理

        private void btnOK_Click(object sender, EventArgs e)
        {
            #region 数据验证

            if (!this.DoValidate())
            {
                return;
            }

            #endregion

            #region 检查指定的代码在正在编辑的列表中是否已存在

            //检查指定的代码在正在编辑的列表中是否已存在
            if (this.EditCollection != null)
            {
                FormElementDataListTextBoxColumnEntityDev formElementDataColumnEntityDev;
                foreach (object obj in this.EditCollection)
                {
                    if (obj == this.FormElementDataColumnEntity)
                        continue;

                    formElementDataColumnEntityDev = obj as FormElementDataListTextBoxColumnEntityDev;
                    if (formElementDataColumnEntityDev.Code.Equals(this.txtCode.Text))
                    {
                        MessageBox.Show(
                            Language.Current.FormSEPaginationDataGridViewDevColumnAdd_MessageElementCodeExist,
                            CommonLanguage.Current.MessageCaption_Notice,
                            MessageBoxButtons.OK, MessageBoxIcon.Information
                       );
                        return;
                    }
                }
            }

            #endregion

            #region 如果不是编辑状态，new一个新的FormElementDataListTextBoxColumnEntityDev

            if (_isEdit == false)
            {
                this.FormElementDataColumnEntity = new FormElementDataListTextBoxColumnEntityDev();
                this.FormElementDataColumnEntity.Id = Guid.NewGuid().ToString();
            }

            #endregion

            #region 查检指定的代码是否已被占用

            //查检指定的代码是否已被占用
            if (this.FormElementDataColumnEntity.Code != this.txtCode.Text)
            {
                //检查指定的代码在窗体元素中是否已存在
                //这里要忽略掉Id,因为到FormEntity中去检查时,FormEntity中的数据列与正在编辑的数据列不是同步的
                //一个数据列对象的code如果由a改成b,只要还没保存,那FormEntity中的就还是a,如果由b再改回a
                //这时到FormEntity中去查,就会报a已被占用,所以要忽略掉Id
                if (!this.FormEntity.ValidateCode(this.txtCode.Text, FormElementDataColumnEntity.Id))
                {
                    MessageBox.Show(
                        Language.Current.FormSEPaginationDataGridViewDevColumnAdd_MessageElementCodeExist,
                           CommonLanguage.Current.MessageCaption_Notice,
                           MessageBoxButtons.OK, MessageBoxIcon.Information
                           );
                    return;
                }
            }

            #endregion

            #region 保存数据

            this.FormElementDataColumnEntity.Name = txtName.Text;
            this.FormElementDataColumnEntity.Code = txtCode.Text;
            this.FormElementDataColumnEntity.Text = txtText.Text;

            this.FormElementDataColumnEntity.IsBind = radioButtonBind.Checked;
            this.FormElementDataColumnEntity.Visible = cbVisible.Checked;
            if (ddlDataItem.SelectedValue != null)
            {
                this.FormElementDataColumnEntity.DataItemId = this._dataEntityId + "." + ddlDataItem.SelectedValue.ToString();
            }
            this.FormElementDataColumnEntity.DataPropertyName = txtDataPropertyName.Text;

            #region 数据规则

            //数据规则
            IFormElementDataListColumnDataRule dataRule = this.ddlDataRule.SelectedItem as IFormElementDataListColumnDataRule;
            this.FormElementDataColumnEntity.DataRule = dataRule.GetParameter();

            #endregion

            if (this.ShellControlDev == null)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                this.btnCancel.Text = Language.Current.FormSEPaginationDataGridViewDevColumnAdd_ButtonClose;

                FormElementDataListEntityDev.AddColumnCommand command = new FormElementDataListEntityDev.AddColumnCommand(this._entity)
                {
                    Column = this._formElementDataColumnEntity
                };

                _windowDesignService.ExecuteCommand(command, new Action<SEUndoUnitAbstract, SEUndoEngine.Type>(
                            delegate(SEUndoUnitAbstract undoUnit, SEUndoEngine.Type type)
                            {
                                SEUndoEngineFormDesigner undoEngine = undoUnit.UndoEngine as SEUndoEngineFormDesigner;
                                undoEngine.UpdateView(this._entity);
                                undoEngine.UpdatePropertyGrid(false);
                            }));

                ResetControl();
                this.ShellControlDev.UpdateView();
            }

            #endregion

            _windowDesignService.MakeDirty();
        }

        private void radioButtonBind_CheckedChanged(object sender, EventArgs e)
        {
            ChangeBind();
        }

        private void ddlDataItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlDataItem.SelectedItem == null)
                return;

            DataRowView dr = ddlDataItem.SelectedItem as DataRowView;

            if (dr != null)
            {
                if (this.txtName.Text == String.Empty)
                    this.txtName.Text = dr["Name"].ToString();

                if (this.txtCode.Text == String.Empty)
                    this.txtCode.Text = "Column" + dr["Code"].ToString();

                if (this.txtText.Text == String.Empty)
                    this.txtText.Text = dr["Name"].ToString();
            }
        }

        private void ddlDataRule_SelectedIndexChanged(object sender, EventArgs e)
        {
            IFormElementDataListColumnDataRule dataRule = ddlDataRule.SelectedItem as IFormElementDataListColumnDataRule;
            panelDataRule.Controls.Clear();
            panelDataRule.Controls.Add(dataRule.ParameterSetPanel);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.txtName.Text = String.Empty;
            this.txtCode.Text = String.Empty;
            this.txtText.Text = String.Empty;
        }

        #endregion

    
    }
}
