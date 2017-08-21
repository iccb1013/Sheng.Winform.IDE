using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sheng.SailingEase.Controls;
using Sheng.SailingEase.Core.Development;
using System.ComponentModel;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Core;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;
using Sheng.SailingEase.Windows.Forms.Development.Localisation;
using Sheng.SailingEase.ComponentModel.Design.UndoEngine;
using Sheng.SailingEase.ComponentModel.Design;

namespace Sheng.SailingEase.Windows.Forms.Development
{
    [ToolboxItem(false)]
    [ToolboxBitmap(typeof(DataGridView))]
    [Description(ConstantLanguage.SEPaginationDataGridViewDev_Description)]
    [Designer(typeof(SEPaginationDataGridViewDevDesigner))]
    [RuntimeControlToolboxItem(ConstantLanguage.SEPaginationDataGridViewDev_Name,
        ConstantLanguage.SEPaginationDataGridViewDev_Catalog, typeof(SEDataGridViewDev))]
    [RuntimeControlDesignSupportAttribute(typeof(FormElementDataListEntityDev), "DataGridView")]
    partial class SEDataGridViewDev : SEDataGridView, IShellControlDev
    {
        private EntityBase _entity;

        [NonSerialized]
        private UIElement _formElement;

        public SEDataGridViewDev()
        {
            this.AutoGenerateColumns = false;
            this.AllowUserToAddRows = false;
            this.AllowUserToDeleteRows = false;
            this.MultiSelect = false;
            this.ReadOnly = true;
            this.RowHeadersVisible = false;
            this.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.RowTemplate.Height = 23;
            this.BackgroundColor = Color.White;
            this.BorderStyle = BorderStyle.Fixed3D;
            this.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            this.AllowUserToResizeRows = false;
        }

        #region IShellControlDev 成员

        public EntityBase Entity
        {
            get
            {
                return this._entity;
            }
            set
            {
                ShellControlHelper.ReplaceControlEntity(_formElement, (UIElement)value);

                this._entity = value;
                this._formElement = (UIElement)value;

                #region 检查InnerElement的code

                //对于包含InnerElement的对象，如DataList，还必须为InnerElement分配新的Code(如果需要)
                //DataList是需要的，为数据列
                //这段代码必须放在把newElement加到FormEntity的Elements之后
                //否则无法为所有的数据列获取正确的Code，比如有三个列，现在为第一个列取到了Column1,但如果数据列Entity不在FormEntity的Element中
                //在取第二个列的Code时，还会拿一个Column1出来
                foreach (UIElement column in _formElement.GetInnerElement())
                {
                    //如果列的code不可用（已被占用）
                    if (this._formElement.HostFormEntity.ValidateCode(column.Code) == false)
                    {
                        column.Code = _formElement.HostFormEntity.CreationElementCode("Column");
                    }
                }

                #endregion

                //为实体对象设置其所关联的实际组件
                IFormElementEntityDev entityDev = value as IFormElementEntityDev;
                if (entityDev != null)
                {
                    entityDev.Component = this;
                }

                //复制粘贴或剪切粘贴后需要更新呈现
                UpdateView();
            }
        }

        private bool _viewUpdating = false;
        public bool ViewUpdating
        {
            get { return this._viewUpdating; }
            set { this._viewUpdating = value; }
        }

        public void UpdateView()
        {
            this.ViewUpdating = true;

            //同步特有属性
            FormElementDataListEntityDev formElement = (FormElementDataListEntityDev)_formElement;

            PropertyDescriptor pds;

            #region 主体部分

            AnchorStyles anchorStyles = new AnchorStyles();

            //处理边缘锚定
            //先清除所有方向描定，.net默认左上角锚定
            anchorStyles = anchorStyles & (AnchorStyles)AnchorStyles.Top;
            anchorStyles = anchorStyles & (AnchorStyles)AnchorStyles.Right;
            anchorStyles = anchorStyles & (AnchorStyles)AnchorStyles.Bottom;
            anchorStyles = anchorStyles & (AnchorStyles)AnchorStyles.Left;

            if (formElement.Anchor.Top)
                anchorStyles = anchorStyles | (AnchorStyles)AnchorStyles.Top;
            if (formElement.Anchor.Right)
                anchorStyles = anchorStyles | (AnchorStyles)AnchorStyles.Right;
            if (formElement.Anchor.Bottom)
                anchorStyles = anchorStyles | (AnchorStyles)AnchorStyles.Bottom;
            if (formElement.Anchor.Left)
                anchorStyles = anchorStyles | (AnchorStyles)AnchorStyles.Left;

            ShellControlHelper.SetProperty(this, "Anchor", anchorStyles);

            ShellControlHelper.SetProperty(this, "Text", formElement.Text);
            ShellControlHelper.SetProperty(this, "Size", formElement.Size);
            ShellControlHelper.SetProperty(this, "Location", formElement.Location);
            ShellControlHelper.SetProperty(this, "Text", formElement.Text);
            ShellControlHelper.SetProperty(this, "ForeColor", formElement.ForeColor);
            ShellControlHelper.SetProperty(this, "Visible", formElement.Visible);
            ShellControlHelper.SetProperty(this, "Enabled", formElement.Enabled);
            //ShellControlHelper.SetProperty(this, "NavigationLocation", formElement.NavigationLocation);
            //ShellControlHelper.SetProperty(this, "Pagination", formElement.Pagination);
            //ShellControlHelper.SetProperty(this, "ShowItemCount", formElement.ShowItemCount);
            //ShellControlHelper.SetProperty(this, "ShowPageCount", formElement.ShowPageCount);
            //ShellControlHelper.SetProperty(this, "ShowPageHomeEnd", formElement.ShowPageHomeEnd);
            ShellControlHelper.SetProperty(this, "TabIndex", formElement.TabIndex);

            #endregion

            #region DataGridView部分

            PropertyDescriptorCollection pdcDataGridView = TypeDescriptor.GetProperties(this);

            pds = pdcDataGridView.Find("BackgroundColor", false);
            if (pds != null)
            {
                //DataGridView.BackgroundColor不支持Color.Empty
                if (formElement.BackColor == Color.Empty)
                    formElement.BackColorValue = "2.Window";

                if (!pds.GetValue(this).Equals(formElement.BackColor))
                {
                    pds.SetValue(this, formElement.BackColor);
                }
            }

            #region 列同步不能,原因不明

            //列
            /*
            DataGridViewColumnCollection dataGridViewColumnCollection = new DataGridViewColumnCollection(this.DataGridView);
            foreach (FormElementDataColumnEntityDev column in formElement.DataColumns)
            {
                dataGridViewColumnCollection.Add(new DataGridViewTextBoxColumnDev(column));
            }

            pds = pdcDataGridView.Find("Columns", false);
            if (pds != null)
            {
                pds.SetValue(this.DataGridView, dataGridViewColumnCollection);
            }
             */

            #endregion
            this.Columns.Clear();
            foreach (FormElementDataListTextBoxColumnEntityDev column in formElement.DataColumns)
            {
                //编辑数据列之后，列对象的DataList属性会为空
                //在这里将DataList属性再次绑定上，放在这里也是为了避免其它类似潜在的问题
                column.DataList = (FormElementDataListEntityDev)this._entity;

                // Debug.Assert(column.DataList != null, "FormElementDataColumnEntityDev 对象的 DataList 属性为 null");

                this.Columns.Add(DataGridViewColumnFactory.GetDataGridViewColumn(column));
            }

            #endregion

            this.ViewUpdating = false;
        }

        public void UpdateEntity()
        {
            ShellControlHelper.UpdateEntity(this._formElement, this);
        }

        public string GetCode()
        {
            return this._entity.Code;
        }

        public string GetName()
        {
            return this._entity.Name;
        }

        public string GetControlTypeName()
        {
            return FormElementEntityDevTypes.Instance.GetName(this._formElement);
        }

        public void ClearEntity()
        {
            this._entity = null;
            this._formElement = null;
        }

        public string GetText()
        {
            return this._formElement.Text;
        }

        public EventCollection GetEvents()
        {
            return this._formElement.Events;
        }

        public void InitializationEntity(EntityBase entity)
        {
        }

        #endregion
    }


    class SEPaginationDataGridViewDevDesigner : ControlDesigner
    {
        private IWindowDesignService _windowDesignService = ServiceUnity.WindowDesignService;

        DesignerVerbCollection m_Verbs;

        // DesignerVerbCollection is overridden from ComponentDesigner
        public override DesignerVerbCollection Verbs
        {
            get
            {
                if (m_Verbs == null)
                {
                    // Create and initialize the collection of verbs
                    m_Verbs = new DesignerVerbCollection();

                    m_Verbs.Add(new DesignerVerb(Language.Current.SEPaginationDataGridViewDev_Verb_AddColumn, new EventHandler(AddColumn)));
                    m_Verbs.Add(new DesignerVerb(Language.Current.SEPaginationDataGridViewDev_Verb_EditColumns, new EventHandler(EditColumns)));
                }
                return m_Verbs;
            }
        }

        SEPaginationDataGridViewDevDesigner()
        {
        }

        private void EditColumns(object sender, EventArgs args)
        {
            IShellControlDev shellControlDev = this.Control as IShellControlDev;
            FormElementDataListEntityDev formElementDataListEntityDev = shellControlDev.Entity as FormElementDataListEntityDev;

            using (FormCollectionEditor formCollectionEditor = new FormCollectionEditor(formElementDataListEntityDev.DataColumns, true))
            {
                formCollectionEditor.Title = Language.Current.SEPaginationDataGridViewDev_Verb_FormCollectionEditorTitle;
                formCollectionEditor.OnAdd += new FormCollectionEditor.CollectionEditorAddEventHandler(formCollectionEditor_OnAdd);
                formCollectionEditor.OnEdit += new FormCollectionEditor.CollectionEditorEditEventHandler(formCollectionEditor_OnEdit);

                if (formCollectionEditor.ShowDialog() == DialogResult.OK)
                {
                    #region 处理可撤销的工作单元

                    if (formCollectionEditor.SupportCancel)
                    {
                        formCollectionEditor.UndoTransaction.SetName(Language.Current.SEPaginationDataGridViewDev_EditColumns_UndoTransaction_Name);

                        //在可撤销单元被撤销或重做时，刷新设计器中呈现的列
                        formCollectionEditor.UndoTransaction.Action = new Action<SEUndoUnitAbstract, SEUndoEngine.Type>(
                            delegate(SEUndoUnitAbstract undoUnit, SEUndoEngine.Type type)
                            {
                                SEUndoEngineFormDesigner undoEngine = undoUnit.UndoEngine as SEUndoEngineFormDesigner;
                                undoEngine.UpdateView(shellControlDev.Entity);
                                undoEngine.UpdatePropertyGrid(false);
                            });

                        _windowDesignService.AddUndoUnit(formCollectionEditor.UndoTransaction);
                    }

                    #endregion

                    shellControlDev.UpdateView();
                    _windowDesignService.MakeDirty();
                }
            }
        }

        private void AddColumn(object sender, EventArgs args)
        {
            IShellControlDev shellControlDev = this.Control as IShellControlDev;
            FormElementDataListEntityDev formElementDataListEntityDev = (FormElementDataListEntityDev)shellControlDev.Entity;

            using (FormSEPaginationDataGridViewDevColumnAdd formSEPaginationDataGridViewDevColumnAdd =
                new FormSEPaginationDataGridViewDevColumnAdd(formElementDataListEntityDev))
            {
                formSEPaginationDataGridViewDevColumnAdd.FormEntity = (FormEntityDev)formElementDataListEntityDev.HostFormEntity;
                formSEPaginationDataGridViewDevColumnAdd.ShellControlDev = shellControlDev;

                formSEPaginationDataGridViewDevColumnAdd.ShowDialog();
            }
        }

        private bool formCollectionEditor_OnAdd(FormCollectionEditor.CollectionEditorEventArgs e, out object addValue)
        {
            IShellControlDev shellControlDev = this.Control as IShellControlDev;
            FormElementDataListEntityDev formElementDataListEntityDev = shellControlDev.Entity as FormElementDataListEntityDev;

            using (FormSEPaginationDataGridViewDevColumnAdd formFormElementAdd_DataColumn =
               new FormSEPaginationDataGridViewDevColumnAdd(formElementDataListEntityDev))
            {
                formFormElementAdd_DataColumn.FormEntity = (FormEntityDev)formElementDataListEntityDev.HostFormEntity;
                formFormElementAdd_DataColumn.EditCollection = e.EditCollection;

                if (formFormElementAdd_DataColumn.ShowDialog() == DialogResult.OK)
                {
                    addValue = formFormElementAdd_DataColumn.FormElementDataColumnEntity;
                    return true;
                }
                else
                {
                    addValue = null;
                    return false;
                }
            }
        }

        private bool formCollectionEditor_OnEdit(object obj, FormCollectionEditor.CollectionEditorEventArgs e, out object oldValue)
        {
            ICloneable clone = obj as ICloneable;
            oldValue = obj;
            if (clone != null)
            {
                oldValue = clone.Clone();
            }

            IShellControlDev shellControlDev = this.Control as IShellControlDev;
            FormElementDataListEntityDev formElementDataListEntityDev = shellControlDev.Entity as FormElementDataListEntityDev;

            using (FormSEPaginationDataGridViewDevColumnAdd formFormElementAdd_DataColumn =
               new FormSEPaginationDataGridViewDevColumnAdd(formElementDataListEntityDev))
            {
                formFormElementAdd_DataColumn.FormElementDataColumnEntity = (FormElementDataListTextBoxColumnEntityDev)obj;
                formFormElementAdd_DataColumn.FormEntity = (FormEntityDev)formElementDataListEntityDev.HostFormEntity;
                formFormElementAdd_DataColumn.EditCollection = e.EditCollection;

                if (formFormElementAdd_DataColumn.ShowDialog() == DialogResult.OK)
                    return true;
                else
                    return false;
            }
        }
    }
}
