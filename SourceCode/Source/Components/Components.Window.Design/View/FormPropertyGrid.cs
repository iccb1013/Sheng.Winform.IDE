using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.ComponentModel.Design;
using Sheng.SailingEase.Drawing;
using Sheng.SailingEase.Localisation;
using Sheng.SailingEase.Components.Window.DesignComponent.Localisation;
using Sheng.SailingEase.Core.Development;
using Sheng.SailingEase.Core;

namespace Sheng.SailingEase.Components.Window.DesignComponent
{
    partial class FormPropertyGrid : PadViewBase
    {
        #region 私有成员

        private IWindowComponentService _windowComponentService = ServiceUnity.WindowComponentService;

        private FormHostingContainer _container;

        #endregion

        #region 公开属性

        /// <summary>
        /// 属性网格
        /// </summary>
        public PropertyGridPad PropertyGrid
        {
            get
            {
                return this.propertyGrid;
            }
        }

        private ICollection selectableObjects;
        /// <summary>
        /// 可供选择的所有组件
        /// 显示在下拉框中
        /// </summary>
        public ICollection SelectableObjects
        {
            get
            {
                return selectableObjects;
            }
            set
            {
                selectableObjects = value;
                SetSelectableObjects(value);
            }
        }

        private object selectedObject;
        /// <summary>
        /// 窗体控件
        /// </summary>
        public object SelectedObject
        {
            get
            {
                return selectedObject;
            }
            set
            {
                selectedObject = value;
                selectedObjects = null;
                UpdateSelectedObjectIfActive();
            }
        }

        private object[] selectedObjects;
        /// <summary>
        /// 窗体控件
        /// </summary>
        public object[] SelectedObjects
        {
            get
            {
                return selectedObjects;
            }
            set
            {
                selectedObject = null;
                selectedObjects = value;
                UpdateSelectedObjectIfActive();
            }
        }

        private IDesignerHost host;
        public IDesignerHost Host
        {
            get
            {
                return host;
            }
            set
            {
                host = value;
             //   PropertyPad.UpdateHostIfActive(this);
            }
        }

        #endregion

        #region 构造及窗体事件

        public FormPropertyGrid(FormHostingContainer container)
        {
            InitializeComponent();

            _container = container;
            _container.ActiveHostingChanged += new FormHostingContainer.OnActiveHostingChangedHandler(_container_ActiveHostingChanged);
            _container.HostingSelectionChanged += new FormHostingContainer.OnHostingSelectionChangedHandler(_container_HostingSelectionChanged);
            _container.HostingInitFormElementsComponentComplete += 
                new FormHostingContainer.OnHostingInitFormElementsComponentCompleteHandler(_container_HostingInitFormElementsComponentComplete);

            #region PropertyGridValidator

            PropertyGridValidator validator = new PropertyGridValidator(typeof(EntityBase))
            {
                ActOnSub = true,
            };
            validator.Validator = (e) =>
            {
                bool success = true;
                string message = null;

                //因为C#是区分大小写的，所以这里不转为全小写或全大写，避免Code和CODE两个不同Property
                //理论上我不会这么做
                if (e.Property == EntityBase.Property_Code)
                {
                    //判断是否使用了系统保留字
                    if (Keywords.Container(e.Value.ToString()))
                    {
                        success = false;
                        message = CommonLanguage.Current.ValueInefficacyUseKeywords;
                    }
                    else
                    {
                        Debug.Assert(e.Objects.Length == 1, "验证 Code 时对象数目只能是一个");
                        object obj = e.Objects[0];

                        string value = e.Value.ToString();

                        //判断在此窗体范围内,代码是否已被占用
                        if (FormHostingContainer.Instance.ActiveFormEntity.ValidateCode(value) == false)
                        {
                            success = false;
                            message = Language.Current.FormDesigner_FormPropertyGrid_MessageFormElementCodeExist;
                        }
                        else
                        {
                            //如果当前选择的目标对象是一个窗体,则还需判断指定的代码是否与其它窗体的代码冲突
                            //此处不用考虑多选的情况,因为多选状态下,代码属性不显示
                            WindowEntity windowEntity = e.Objects[0] as WindowEntity;
                            if (windowEntity != null)
                            {
                                //判断code是否已被占用
                                if (_windowComponentService.CheckExistByCode(value))
                                {
                                    success = false;
                                    message = Language.Current.FormDesigner_FormPropertyGrid_MessageFormEntityExist;
                                }
                            }
                        }
                    }
                }
                return new PropertyGridValidateResult(success, message);
            };
            propertyGrid.AddValidator(validator);

            #endregion

            //注意,ComboBox里绑定的对象是实际控件,不是Entity
            this.comboBoxFormElementList.DrawMode = DrawMode.OwnerDrawFixed;
            this.comboBoxFormElementList.Sorted = false;
            this.comboBoxFormElementList.DrawItem += new DrawItemEventHandler(ComboBoxDrawItem);
            this.comboBoxFormElementList.MeasureItem += new MeasureItemEventHandler(ComboBoxMeasureItem);
            this.comboBoxFormElementList.SelectedIndexChanged += new EventHandler(ComboBoxSelectedIndexChanged);

            this.VisibleChanged+=new EventHandler(FormPropertyGrid_VisibleChanged);

            this.Icon = DrawingTool.ImageToIcon(IconsLibrary.Property);
            this.TabText = Language.Current.FormPropertyGrid_TabText;
        }

        private void FormPropertyGrid_VisibleChanged(object sender, EventArgs e)
        {
            //如果窗体由不可见变为可见,更新选属性网格
            //因为在窗体不可见时,属性网格是停止更新的
            if (this.Visible)
            {
                UpdateSelectedObjectIfActive();
            }
        }

        #endregion

        #region 事件处理

        void _container_HostingSelectionChanged(FormDesignSurfaceHosting hosting)
        {
            UpdateSelection(hosting);
        }

        void _container_ActiveHostingChanged(FormDesignSurfaceHosting hosting)
        {
            UpdateStatus(hosting);
        }

        void _container_HostingInitFormElementsComponentComplete(FormDesignSurfaceHosting hosting)
        {
            if (hosting == _container.ActiveHosting)
                UpdateStatus(hosting);
        }

        #endregion

        #region ComboBox事件

        private void ComboBoxDrawItem(object sender, DrawItemEventArgs dea)
        {
            if (dea.Index < 0 || dea.Index >= this.comboBoxFormElementList.Items.Count)
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

            object item = this.comboBoxFormElementList.Items[dea.Index];
            int xPos = dea.Bounds.X;

            /*
            if (item is IComponent)
            {
                ISite site = ((IComponent)item).Site;
                if (site != null)
                {
                    string name = site.Name;
                    using (Font f = new Font(this.comboBoxFormElementList.Font, FontStyle.Bold))
                    {
                        g.DrawString(name, f, stringColor, xPos, dea.Bounds.Y);
                        xPos += (int)g.MeasureString(name + "-", f).Width;
                    }
                }
            }
             */

            string typeString = String.Empty;
            if (item is IShellControlDev)
            {
                IShellControlDev site = (IShellControlDev)item;
                if (site != null)
                {
                    string name = site.GetCode();
                    using (Font f = new Font(this.comboBoxFormElementList.Font, FontStyle.Bold))
                    {
                        g.DrawString(name, f, stringColor, xPos, dea.Bounds.Y);
                        xPos += (int)g.MeasureString(name + "-", f).Width;
                    }


                    typeString = site.GetControlTypeName();
                }
            }

          //  string typeString = item.GetType().ToString();
            g.DrawString(typeString, this.comboBoxFormElementList.Font, stringColor, xPos, dea.Bounds.Y);
        }

        private void ComboBoxMeasureItem(object sender, MeasureItemEventArgs mea)
        {
            if (mea.Index < 0 || mea.Index >= this.comboBoxFormElementList.Items.Count)
            {
                mea.ItemHeight = this.comboBoxFormElementList.Font.Height;
                return;
            }
            object item = this.comboBoxFormElementList.Items[mea.Index];
            SizeF size = mea.Graphics.MeasureString(item.GetType().ToString(), this.comboBoxFormElementList.Font);

            mea.ItemHeight = (int)size.Height;
            mea.ItemWidth = (int)size.Width;

            if (item is IComponent)
            {
                ISite site = ((IComponent)item).Site;
                if (site != null)
                {
                    string name = site.Name;
                    using (Font f = new Font(this.comboBoxFormElementList.Font, FontStyle.Bold))
                    {
                        mea.ItemWidth += (int)mea.Graphics.MeasureString(name + "-", f).Width;
                    }
                }
            }
        }

        private bool inUpdate = false;
        private void ComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            if (!inUpdate)
            {
                if (host != null)
                {
                    ISelectionService selectionService = (ISelectionService)host.GetService(typeof(ISelectionService));
                    if (this.comboBoxFormElementList.SelectedIndex >= 0)
                    {
                        selectionService.SetSelectedComponents(
                            new object[] { this.comboBoxFormElementList.Items[this.comboBoxFormElementList.SelectedIndex] });
                    }
                    else
                    {
                        SetDesignableObjects(null);
                        selectionService.SetSelectedComponents(new object[] { });
                    }
                }
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 对可供选择的组件排序
        /// </summary>
        /// <param name="objects"></param>
        /// <returns></returns>
        private ICollection SortObjectsBySiteName(ICollection objects)
        {
            List<object> unsortedObjects = new List<object>();
            foreach (object o in objects)
            {
                unsortedObjects.Add(o);
            }
            unsortedObjects.Sort(CompareObjectsBySiteName);
            return unsortedObjects.ToArray();
        }

        private int CompareObjectsBySiteName(object x, object y)
        {
            return String.Compare(GetObjectSiteName(x), GetObjectSiteName(y));
        }

        //private string GetObjectSiteName(object o)
        //{
        //    if (o != null)
        //    {
        //        IComponent component = o as IComponent;
        //        if (component != null)
        //        {
        //            ISite site = component.Site;
        //            if (site != null)
        //            {
        //                return site.Name;
        //            }
        //        }
        //        return o.GetType().ToString();
        //    }
        //    return String.Empty;
        //}

        /// <summary>
        /// 获取控件的显示名(所关联对象的Code)
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private string GetObjectSiteName(object o)
        {
            if (o != null)
            {
                IShellControlDev component = o as IShellControlDev;
                if (component != null)
                {
                    return component.GetCode();
                }
                return o.GetType().ToString();
            }
            return String.Empty;
        }

        /// <summary>
        /// 如果当前属性面板窗体有效(可见状态)
        /// 将实体对象提取出来,然后设置到属性网格中
        /// </summary>
        private void UpdateSelectedObjectIfActive()
        {
            //如果窗体不可见,不更新属性网格
            if (!this.Visible)
            {
                return;
            }

            //加载当前可用的设计器谓词
            //先加载设计器谓词，避免先显示了属性行再显示谓词面板造成属性行部分滚动条闪动
            if (this.Host != null)
            {
                IMenuCommandService menuCommandService =
                    this.Host.GetService(typeof(IMenuCommandService)) as IMenuCommandService;
                Debug.Assert(menuCommandService != null, "menuCommandService 为 null");
                if (menuCommandService != null)
                    this.PropertyGrid.Verbs = menuCommandService.Verbs;
            }

            if (SelectedObjects != null)
            {
                EntityBase[] entity = new EntityBase[SelectedObjects.Length];
                object[] selArray = new object[SelectedObjects.Length];
                SelectedObjects.CopyTo(selArray, 0);

                for (int i = 0; i < SelectedObjects.Length; i++)
                {
                    IShellControlDev shellControlDev = selArray[i] as IShellControlDev;
                    //entity[i] = shellControlDev.GetEntity();
                    entity[i] = shellControlDev.Entity;
                }

                SetDesignableObjects(entity);
            }
            else
            {
                IShellControlDev shellControlDev = SelectedObject as IShellControlDev;

                if (shellControlDev != null)
                {
                    //SetDesignableObject(shellControlDev.GetEntity());
                    SetDesignableObjects(new object[]{ shellControlDev.Entity});
                }
                else
                {
                    SetDesignableObjects(null);
                }
            }
        }

        /// <summary>
        /// 设置当前所有可供选择的组件
        /// </summary>
        /// <param name="coll"></param>
        private void SetSelectableObjects(ICollection coll)
        {
            inUpdate = true;
            this.comboBoxFormElementList.Items.Clear();
            if (coll != null)
            {
                foreach (object obj in SortObjectsBySiteName(coll))
                {
                    comboBoxFormElementList.Items.Add(obj);
                }
            }
            SelectedObjectsChanged();
            inUpdate = false;
        }

        private void SetDesignableObjects(object[] obj)
        {
            inUpdate = true;
            this.PropertyGrid.SelectedObjects = obj;
            SelectedObjectsChanged();
            inUpdate = false;
        }

        /// <summary>
        /// 设计器中选择的对象改变
        /// </summary>
        private void SelectedObjectsChanged()
        {
            if (this.PropertyGrid.SelectedObjects != null && this.PropertyGrid.SelectedObjects.Length == 1)
            {
                for (int i = 0; i < this.comboBoxFormElementList.Items.Count; ++i)
                {
                    //if (this.PropertyGrid.SelectedObjects[0] == 
                    //    ((IShellControlDev)this.comboBoxFormElementList.Items[i]).GetEntity())
                    if (this.PropertyGrid.SelectedObjects[0] ==
                        ((IShellControlDev)this.comboBoxFormElementList.Items[i]).Entity)
                    {
                        this.comboBoxFormElementList.SelectedIndex = i;
                    }
                }
            }
            else
            {
                this.comboBoxFormElementList.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// 更新/设置属性面板窗体
        /// </summary>
        private void UpdateStatus(FormDesignSurfaceHosting hosting)
        {
            if (hosting != null && hosting.DesignerHost != null)
            {
                this.Host = hosting.DesignerHost;
                this.SelectableObjects = hosting.DesignerHost.Container.Components;
            }
            else
            {
                this.Host = null;
                this.SelectableObjects = null;
            }

            UpdateSelection(hosting);
        }

        /// <summary>
        /// 更新属性网格选定的对象
        /// </summary>
        private void UpdateSelection(FormDesignSurfaceHosting hosting)
        {
            //这里不能直接这样写==null，省下面两个else
            //这样写的结果是切换选择的对象时，PropertyGrid中间都会经历一次没有选择的状态
            //使行共享功能完全失效！
            //this.SelectedObject = null;

            if (hosting != null && hosting.SelectionService != null)
            {
                ICollection selection = hosting.SelectionService.GetSelectedComponents();
                if (selection != null)
                {
                    object[] selArray = new object[selection.Count];
                    selection.CopyTo(selArray, 0);
                    this.SelectedObjects = selArray;
                }
                else
                {
                    this.SelectedObject = null;
                }
            }
            else
            {
                this.SelectedObject = null;
            }
        }

        #endregion
    }
}
