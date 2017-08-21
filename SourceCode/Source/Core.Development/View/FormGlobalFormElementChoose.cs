using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Core.Development.Localisation;
using Sheng.SailingEase.ComponentModel;
using Sheng.SailingEase.Localisation;
using Sheng.SailingEase.Controls;

namespace Sheng.SailingEase.Core.Development
{
    //本想移到Window模块里，然后通过服务提供这个功能
    //但是想想放在这里也可以，通过Window模块的服务获取所有窗体列表即可
    //因为不管怎么实现Window模块，不管是winform版还是将来silverlight版等，窗体元素选择功能都是一样的

    /// <summary>
    /// 全局窗体元素选择
    /// 注意:不支持在此选窗体,选窗体使用专门的窗体选择窗口
    /// </summary>
    partial class FormGlobalFormElementChoose : FormViewBase
    {
        #region 私有成员

        private UIElement _selectedFormElement;

        /// <summary>
        /// 当前是否列出了所有允许列出的窗体元素
        /// </summary>
        private bool _showAll = true;

        #endregion

        #region 公开属性

        private UIElementEntityTypeCollection _allowFormElementControlType = new UIElementEntityTypeCollection();
        /// <summary>
        /// 允许列出的窗体元素类型
        /// 默认为全部
        /// </summary>
        public UIElementEntityTypeCollection AllowFormElementControlType
        {
            get
            {
                return this._allowFormElementControlType;
            }
            set
            {
                this._allowFormElementControlType = value;
            }
        }

        private UIElementEntityTypeCollection _allowSelectFormElementControlType = new UIElementEntityTypeCollection();
        /// <summary>
        /// 允许选择的窗体元素类型
        /// 默认为全部
        /// </summary>
        public UIElementEntityTypeCollection AllowSelectFormElementControlType
        {
            get
            {
                return this._allowSelectFormElementControlType;
            }
            set
            {
                this._allowSelectFormElementControlType = value;
            }
        }

        /// <summary>
        /// 选择的窗体元素完整Id路径
        /// </summary>
        public string FormElementId
        {
            get
            {
                if (this._selectedFormElement == null)
                    return String.Empty;
                else
                    return this._selectedFormElement.Id;
            }
        }

        /// <summary>
        /// 选择的窗体元素名
        /// </summary>
        public string FormElementName
        {
            get
            {
                if (this._selectedFormElement == null)
                    return String.Empty;
                else
                    return this._selectedFormElement.Name;
            }
        }

        public string FormElementCode
        {
            get
            {
                if (this._selectedFormElement == null)
                    return String.Empty;
                else
                    return this._selectedFormElement.Code;
            }
        }

        public string FormElementFullCode
        {
            get
            {
                if (this._selectedFormElement == null)
                    return String.Empty;
                else
                    return this._selectedFormElement.FullCode;
            }
        }

        /// <summary>
        /// 选择的窗体元素对象
        /// </summary>
        public UIElement SelectedFormElement
        {
            get
            {
                if (this._selectedFormElement == null)
                    return null;
                else
                    return this._selectedFormElement;
            }
        }

        /// <summary>
        /// 选择的数据源的用于呈现的字符串表达形式
        /// </summary>
        public string SelectedDataSourceVisibleString
        {
            get
            {
                if (this._selectedFormElement == null)
                    return String.Empty;
                else
                    return StringParserLogic.DataSourceVisibleString(this._selectedFormElement);
            }
        }

        /// <summary>
        /// 选择的数据源的用于xml存储的字符串表达形式
        /// </summary>
        public string SelectedDataSourceString
        {
            get
            {
                if (this._selectedFormElement == null)
                    return String.Empty;
                else
                    return StringParserLogic.DataSourceString(this._selectedFormElement);
            }
        }

        #endregion

        #region 构造和窗体事件

        /// <summary>
        /// 构造，需手动调用 InitFormElementTree 初始化树
        /// </summary>
        public FormGlobalFormElementChoose()
        {
            InitializeComponent();

            this.contextMenuStrip.Renderer = ToolStripRenders.Default;

            Unity.ApplyResource(this);
        }

        private void FormGlobalFormElementChoose_Load(object sender, EventArgs e)
        {
           
        }

        #endregion

        #region 事件处理

        //点击确定按钮
        private void btnOK_Click(object sender, EventArgs e)
        {
            SelectFinish();
        }

        private void linkLabelFormElement_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Type type = e.Link.LinkData as Type;
            if (type != null)
            {
                this.treeViewGridFormElement.Model = new GlobalFormElementTreeModel(new UIElementEntityTypeCollection(new Type[] { type }));
                _showAll = false;
            }
        }

        private void toolStripMenuItemSelect_Click(object sender, EventArgs e)
        {
            SelectFinish();
        }

        private void toolStripMenuItemShowAll_Click(object sender, EventArgs e)
        {
            if (_showAll == false)
            {
                this.treeViewGridFormElement.Model = new GlobalFormElementTreeModel(AllowFormElementControlType);
                _showAll = true;
            }
        }

        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
             //判断是否做出了选择
            if (this.treeViewGridFormElement.SelectedNode == null)
            {
                toolStripMenuItemSelect.Enabled = false;
            }
            else
            {
                toolStripMenuItemSelect.Enabled = true;
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 初始化窗体元素超连接
        /// </summary>
        private void InitFormElementLink()
        {
            StringBuilder sbLink = new StringBuilder();
            this.linkLabelFormElement.Links.Clear();
            this.linkLabelFormElement.Text = String.Empty;

            sbLink.Append(Language.Current.FormGlobalFormElementChoose_CurrentAllowFormElementControlType);
            sbLink.Append(":");

            if (this.AllowSelectFormElementControlType.Any)
            {
                sbLink.Append(Language.Current.FormGlobalFormElementChoose_AllowFormElementControlType_All);
                this.linkLabelFormElement.Links.Add(sbLink.Length -
                    Language.Current.FormGlobalFormElementChoose_AllowFormElementControlType_All.Length,
                    Language.Current.FormGlobalFormElementChoose_AllowFormElementControlType_All.Length, null);
            }
            else
            {
                foreach (Type type in this.AllowSelectFormElementControlType)
                {
                    UIElementEntityProvideAttribute attribute = FormElementEntityDevTypes.Instance.GetProvideAttribute(type);
                    if (attribute != null)
                    {
                        sbLink.Append(attribute.Name);
                        this.linkLabelFormElement.Links.Add(sbLink.Length - attribute.Name.Length, attribute.Name.Length, type);
                        sbLink.Append(",");
                    }
                }

                if (sbLink.Length >= 1)
                    sbLink.Remove(sbLink.Length - 1, 1);
            }

            this.linkLabelFormElement.Text = sbLink.ToString();
        }

        private void SelectFinish()
        {
            //判断是否做出了选择
            if (this.treeViewGridFormElement.SelectedNode == null)
            {
                MessageBox.Show(
                   Language.Current.FormGlobalFormElementChoose_MessageChooseFormElement,
                    CommonLanguage.Current.MessageCaption_Notice,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                    );
                return;
            }

            GlobalFormElementTreeItemBase treeItem =
                this.treeViewGridFormElement.SelectedNode.Tag as GlobalFormElementTreeItemBase;
            UIElement formElement = treeItem.Entity as UIElement;
            if (formElement == null)
            {
                MessageBox.Show(
                   Language.Current.FormGlobalFormElementChoose_MessageChooseFormElement,
                   CommonLanguage.Current.MessageCaption_Notice,
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Information
                   );
                return;
            }

            //判断选择元素类型是否在允许的范围内
            if (!this.AllowSelectFormElementControlType.Allowable(formElement))
            {
                MessageBox.Show(
                   Language.Current.FormGlobalFormElementChoose_MessageChooseFormElementNotSupport,
                   CommonLanguage.Current.MessageCaption_Notice,
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Information
                   );
                return;
            }


            this._selectedFormElement = formElement;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        #endregion

        #region 公开方法

        //根据AllowFormElementControlType和AllowSelectFormElementControlType的设置初始化窗体元素树
        //放在构造里，那么修改窗体的这两个属性时，要么只能完全重新赋值，set里做这个工作，重新初始化，那么构造里的初始化完全没用，浪费
        //放在Load里，那在关闭窗体再打开后，又会重新初始化一次，而且会丢失之前的操作状态，如打开了哪些节点，选中了哪个元素
        //所以干脆单独列个方法，需要初始化树的地方调用一次
        /// <summary>
        /// 初始化窗体元素树
        /// </summary>
        public void InitFormElementTree()
        {
            //在树中列出当前所有窗体,在load事件中作,保证读取的数据都是最新的
            this.treeViewGridFormElement.Model = new GlobalFormElementTreeModel(AllowFormElementControlType);
            InitFormElementLink();
        }

        #endregion

      
      
    }
}
