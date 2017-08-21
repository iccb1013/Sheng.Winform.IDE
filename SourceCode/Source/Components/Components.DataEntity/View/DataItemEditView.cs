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
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.IDataBaseProvide;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Components.DataEntityComponent.Localisation;
using Sheng.SailingEase.Localisation;
using Sheng.SailingEase.Kernal;
namespace Sheng.SailingEase.Components.DataEntityComponent.View
{
    partial class DataItemEditView : FormViewBase
    {
        private DataEntityDev _dataEntity;
        private BindingList<FieldProvideAttribute> _itemProvideAttribute = new BindingList<FieldProvideAttribute>();
        private DataItemEntityDev _dataItemEntity;
        public DataItemEntityDev DataItemEntity
        {
            get
            {
                return this._dataItemEntity;
            }
            set
            {
                this._dataItemEntity = value;
            }
        }
        public DataItemEditView(DataEntityDev dataEntity)
        {
            InitializeComponent();
            this.txtCode.Regex = CoreConstant.ENTITY_CODE_REGEX;
            this.txtCode.CustomValidate = CodeValidateMethod;
            this.txtLength.MaxLength = 4; 
            this.txtLength.CustomValidate = LengthValidateMethod;
            Unity.ApplyResource(this);
            this.ApplyLanguageResource();
            this._dataEntity = dataEntity;
        }
        private void DataItemEditView_Load(object sender, EventArgs e)
        {
            this._itemProvideAttribute = new BindingList<FieldProvideAttribute>(DataBaseProvide.Current.FieldFactory.GetProvideAttributeList());
            this.availableDataItems.DisplayMember = FieldProvideAttribute.NameProperty;
            this.availableDataItems.DescriptionMember = FieldProvideAttribute.DescriptionProperty;
            this.availableDataItems.DataBind(_itemProvideAttribute);
            if (DataItemEntity == null)
            {
                DataItemEntity = new DataItemEntityDev(this._dataEntity);
            }
            else
            {
                DataItemEntity.Snapshot();
                this.txtName.Text = _dataItemEntity.Name;
                this.txtCode.Text = _dataItemEntity.Code;
                this.availableDataItems.SetSelectedValue(
                    DataBaseProvide.Current.FieldFactory.GetProvideAttribute(_dataItemEntity.Field));
                this.txtLength.Text = _dataItemEntity.Length.ToString();
                this.txtDecimalDigits.Value = _dataItemEntity.Length.DecimalDigits;
                this.txtDefaultValue.Text = _dataItemEntity.DefaultValue;
                this.cbAllowEmpty.Checked = _dataItemEntity.AllowEmpty;
                this.cbExclusive.Checked = _dataItemEntity.Exclusive;
                this.txtRemark.Text = _dataItemEntity.Remark;
            }
        }
        private void DataItemEditView_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.DialogResult != DialogResult.OK)
            {
                DataItemEntity.Revert();
            }
        }
        private void ApplyLanguageResource()
        {
            this.tabPageGeneral.Text = Language.Current.DataItemEditView_TabPageGeneral;
            this.tabPageRemark.Text = Language.Current.DataItemEditView_TabPageRemark;
            this.txtName.Title = Language.Current.DataItemEditView_LabelName;
            this.txtCode.Title = Language.Current.DataItemEditView_LabelCode;
            this.txtCode.RegexMsg = CommonLanguage.Current.RegexMsg_EntityCode;
            this.availableDataItems.Title = Language.Current.DataItemEditView_LabelDataType;
            this.txtLength.Title = Language.Current.DataItemEditView_LabelLength;
        }
        private bool LengthValidateMethod(object sender, out string msg)
        {
            ComboBox textBox = (ComboBox)sender;
            string lengthStr = textBox.Text;
            if (lengthStr.ToUpper() == "MAX" && this.DataItemEntity.Field.AllowMaxLength)
            {
                msg = String.Empty;
                return true;
            }
            else
            {
                bool validate = true;
                string validateMsg = String.Format(Language.Current.DataItemEditView_MessageLengthOutOfRange,
                    this.DataItemEntity.Field.LengthMin, this.DataItemEntity.Field.LengthMax);
                if (this.DataItemEntity.Field.AllowMaxLength)
                {
                    validateMsg += Language.Current.DataItemEditView_MessageLengthOrMax;
                }
                if (RegexHelper.IsNumeric(lengthStr) == false)
                {
                    validate = false;
                }
                else
                {
                    int length = Convert.ToInt32(lengthStr);
                    if (length > this.DataItemEntity.Field.LengthMax || length < this.DataItemEntity.Field.LengthMin)
                    {
                        validate = false;
                    }
                }
                msg = validateMsg;
                return validate;
            }
        }
        private bool CodeValidateMethod(object sender, out string msg)
        {
            msg = String.Empty;
            TextBox textBox = (TextBox)sender;
            string code = String.Empty;
            if (this.DataItemEntity != null)
                code = this.DataItemEntity.Code;
            if (code != textBox.Text)
            {
                if (DataEntityArchive.Instance.ItemEntityExist(this._dataEntity.Id, textBox.Text))
                {
                    msg = Language.Current.DataItemEditView_DataEntityItemCodeExist;
                    return false;
                }
            }
            return true;
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.DoValidate() == false)
                return;
            DataItemEntity.Name = txtName.Text;
            DataItemEntity.Code = txtCode.Text;
            DataItemEntity.Sys = false;
            DataItemEntity.Remark = txtRemark.Text;
            DataItemEntity.Length = (FieldLength)this.txtLength.Text;
            DataItemEntity.DecimalDigits = (byte)(txtDecimalDigits.Value);
            DataItemEntity.DefaultValue = txtDefaultValue.Text;
            DataItemEntity.AllowEmpty = cbAllowEmpty.Checked;
            DataItemEntity.Exclusive = cbExclusive.Checked;
            DataEntityArchive.Instance.CommitDataItem(DataItemEntity, this._dataEntity.Id);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void availableDataItems_SelectedValueChanged(object sender, Controls.SEComboSelector2.OnSelectedValueChangedEventArgs e)
        {
            FieldProvideAttribute itemAttr = (FieldProvideAttribute)e.Value;
            IField dataItem = DataBaseProvide.Current.FieldFactory.CreateInstance(itemAttr);
            this.DataItemEntity.Field = dataItem;
            if (dataItem.AllowMaxLength)
                this.txtLength.DropDownStyle = ComboBoxStyle.DropDown;
            else
                this.txtLength.DropDownStyle = ComboBoxStyle.Simple;
            this.txtLength.Text = dataItem.Length.ToString();
            this.txtLength.Enabled = dataItem.LengthEnable;
            this.txtDecimalDigits.Enabled = dataItem.DecimalDigitsEnable;
            this.txtDecimalDigits.Maximum = dataItem.DecimalDigitsMax;
            this.txtDecimalDigits.Minimum = dataItem.DecimalDigitsMin;
            this.txtDecimalDigits.Value = dataItem.Length.DecimalDigits;
        }
    }
}
