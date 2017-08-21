

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sheng.SailingEase.Controls;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Kernal;

namespace Sheng.SailingEase.Core.Development
{
    /// <summary>
    /// 事件中的数据源选择控件
    /// 此对象的SelectItem属性所获取的对象,可能是一个FormElement对象,也可以是一个表示可用系统枚举的枚举
    /// </summary>
    class FormElementComboBox : SEComboBox
    {
        //private DataTable dtFormElement;

        private EnumEventDataSource _allowDataSource = EnumEventDataSource.FormElement;
        /// <summary>
        /// 允许选择的数据源类型
        /// </summary>
        public EnumEventDataSource AllowDataSource
        {
            get
            {
                return this._allowDataSource;
            }
            set
            {
                this._allowDataSource = value;

                InitItem();
            }
        }

        private UIElementEntityTypeCollection _allowFormElementControlType = new UIElementEntityTypeCollection();
        /// <summary>
        /// 具体允许列出的控件,默认为全部
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

                InitItem();
            }
        }

        private WindowEntity _formEntity;
        /// <summary>
        /// 原 FormEntityDev
        /// </summary>
        public WindowEntity FormEntity
        {
            get
            {
                return this._formEntity;
            }
            set
            {
                this._formEntity = value;
                InitItem();
            }
        }

        /// <summary>
        /// 选定的窗体元素的完整Id路径
        /// 例如如果是数据列,则结果为数据列表Id.数据列Id
        /// </summary>
        public string SelectedFormElementId
        {
            get
            {
                if (this.SelectedItem == null)
                    return String.Empty;

                UIElement element = this.SelectedItem as UIElement;
                if (element != null)
                    return element.Id;
                else
                    return String.Empty;
            }
            set
            {
                foreach (object obj in this.Items)
                {
                    if (obj == null)
                        continue;
                    UIElement element = obj as UIElement;
                    if (element == null)
                        continue;

                    if (element.Id == value)
                    {
                        this.SelectedItem = element;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 选定的窗体元素的完整Code路径
        /// 例如如果是数据列,则结果为数据列表Code.数据列Code
        /// </summary>
        public string SelectedFormElementFullCode
        {
            get
            {
                if (this.SelectedItem == null)
                    return String.Empty;

                UIElement element = this.SelectedItem as UIElement;
                if (element != null)
                    return element.FullCode;
                else
                    return String.Empty;
            }
        }

        /// <summary>
        /// 选定的窗体元素的名称
        /// </summary>
        public string SelectedFormElementName
        {
            get
            {
                if (this.SelectedItem == null)
                    return String.Empty;

                UIElement element = this.SelectedItem as UIElement;

                if (element != null)
                    return element.Name;
                else
                    return String.Empty;
            }
        }

        public string SelectedFormElementFullName
        {
            get
            {
                if (this.SelectedItem == null)
                    return String.Empty;

                UIElement element = this.SelectedItem as UIElement;

                if (element != null)
                    return element.FullName;
                else
                    return String.Empty;
            }
        }

        public UIElement SelectedFormElement
        {
            get
            {
                return this.SelectedItem as UIElement;
            }
        }

        /// <summary>
        /// 选择的数据源的用于呈现的字符串表达形式
        /// </summary>
        public string SelectedDataSourceVisibleString
        {
            get
            {
                if (this.SelectedItem == null)
                    return String.Empty;
                else
                    return StringParserLogic.DataSourceVisibleString(this.SelectedItem);
            }
        }

        /// <summary>
        /// 选择的数据源的用于xml存储的字符串表达形式
        /// </summary>
        public string SelectedDataSourceString
        {
            get
            {
                if (this.SelectedItem == null)
                    return String.Empty;
                else
                    return StringParserLogic.DataSourceString(this.SelectedItem);
            }
        }

        public FormElementComboBox()
        {
            this.MaxDropDownItems = 16;

            this.DropDownStyle = ComboBoxStyle.DropDownList;
            this.DrawMode = DrawMode.OwnerDrawFixed;
            this.Sorted = false;

            this.DrawItem += new DrawItemEventHandler(ComboBoxDrawItem);
            this.MeasureItem += new MeasureItemEventHandler(ComboBoxMeasureItem);
        }

        private void ComboBoxDrawItem(object sender, DrawItemEventArgs dea)
        {
            if (dea.Index < 0 || dea.Index >= this.Items.Count)
            {
                return;
            }
            Graphics g = dea.Graphics;
            Brush stringColor = SystemBrushes.ControlText;

            if ((dea.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                if ((dea.State & DrawItemState.Focus) == DrawItemState.Focus)
                {
                    g.FillRectangle(SystemBrushes.Highlight, dea.Bounds);
                    stringColor = SystemBrushes.HighlightText;
                }
                else
                {
                    g.FillRectangle(SystemBrushes.Window, dea.Bounds);
                }
            }
            else
            {
                g.FillRectangle(SystemBrushes.Window, dea.Bounds);
            }

            object item = this.Items[dea.Index];
            int xPos = dea.Bounds.X;


            //绘制下拉项目
            string typeString = String.Empty;
            if (item is UIElement)
            {
                UIElement site = item as UIElement;
                if (site != null)
                {
                    string name = site.FullCode;

                    using (Font f = new Font(this.Font, FontStyle.Bold))
                    {
                        g.DrawString(name, f, stringColor, xPos, dea.Bounds.Y);
                        xPos += (int)g.MeasureString(name + "-", f).Width;
                    }

                    //TODO:因为数据列不在FormElementEntityTypes中，所 以数据列的类型名子获取不能
                    //见FormElementEntityTypeCollection 182行
                    typeString = "(" + site.Name + ") " + FormElementEntityDevTypes.Instance.GetName(site);
                }
            }
            else if(item is EnumSystemDataSource)
            {
                EnumSystemDataSource systemDataSourrce = (EnumSystemDataSource)item;

                string name = systemDataSourrce.ToString();

                using (Font f = new Font(this.Font, FontStyle.Bold))
                {
                    g.DrawString(name, f, stringColor, xPos, dea.Bounds.Y);
                    xPos += (int)g.MeasureString(name + "-", f).Width;
                }

                typeString = EnumDescConverter.Get(typeof(EnumSystemDataSource)).Select(
                    "Value = '" + Convert.ToInt32(systemDataSourrce) + "'")[0]["Text"].ToString();

            }

            //  string typeString = item.GetType().ToString();
            g.DrawString(typeString, this.Font, stringColor, xPos, dea.Bounds.Y);
        }

        private void ComboBoxMeasureItem(object sender, MeasureItemEventArgs mea)
        {
            if (mea.Index < 0 || mea.Index >= this.Items.Count)
            {
                mea.ItemHeight = this.Font.Height;
                return;
            }
            object item = this.Items[mea.Index];
            SizeF size = mea.Graphics.MeasureString(item.GetType().ToString(), 
                this.Font);

            mea.ItemHeight = (int)size.Height;
            mea.ItemWidth = (int)size.Width;

            if (item is UIElement)
            {
                UIElement site = (UIElement)item;
                if (site != null)
                {
                    string name = site.Name;
                    using (Font f = new Font(this.Font, FontStyle.Bold))
                    {
                        mea.ItemWidth += (int)mea.Graphics.MeasureString(name + "-", f).Width;
                    }
                }
            }
        }

        /// <summary>
        /// 初始化下拉框中允许选择的元素
        /// 窗体元素或系统枚举
        /// </summary>
        private void InitItem()
        {
            if (this.AllowDataSource == EnumEventDataSource.FormElement)
                InitFormElement();
            else if(this.AllowDataSource == EnumEventDataSource.System)
                InitSystem();
        }

        private void InitFormElement()
        {
            //加载窗体元素下拉框
            this.Items.Clear();

            if (this.FormEntity == null)
                return;

            UIElementCollection formElemtns =
                this.FormEntity.GetFormElement(this.AllowFormElementControlType);

            foreach (UIElement formElement in formElemtns)
            {
                this.Items.Add(formElement);
            }
        }

        private void InitSystem()
        {
            this.Items.Clear();

            foreach (DataRow dr in EnumDescConverter.Get(typeof(EnumSystemDataSource)).Rows)
            {
                this.Items.Add((EnumSystemDataSource)dr["Object"]);
            }
        }
    }
}
