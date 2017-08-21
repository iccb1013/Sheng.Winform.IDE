using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sheng.SailingEase.ComponentModel;
using Sheng.SailingEase.Controls.Extensions;
using Sheng.SailingEase.Drawing;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Components.DictionaryComponent.Localisation;
using Microsoft.Practices.Composite.Events;
using Sheng.SailingEase.Kernal;

namespace Sheng.SailingEase.Components.DictionaryComponent.View
{
    public partial class EnumView : WorkbenchViewBase
    {
        public const string SINGLEKEY = "EnumView";

        #region 私有成员

        private IEventAggregator _eventAggregator = ServiceUnity.Container.Resolve<IEventAggregator>();

        private TypeBinderDataGridViewController _enumGridViewController;
        private TypeBinderDataGridViewController _enumItemGridViewController;

        private GridColumns _gridColumns = new GridColumns();
        private GridViewMenuEnumEntity _gridViewMenuEnumEntity;
        private GridViewMenuEnumItemEntity _gridViewMenuEnumItemEntity;

        #region Commands

        private AddEnumEntityCommand _addEnumEntityCommand;
        private EditEnumEntityCommand _editEnumEntityCommand;
        private DeleteEnumEntityCommand _deleteEnumEntityCommand;

        private AddEnumItemEntityCommand _addEnumItemEntityCommand;
        private EditEnumItemEntityCommand _editEnumItemEntityCommand;
        private DeleteEnumItemEntityCommand _deleteEnumItemEntityCommand;

        #endregion

        #endregion

        #region 构造及窗体事件

        public EnumView()
        {
            InitializeComponent();

            UIHelper.ProcessDataGridView(this.dataGridViewEnum);
            UIHelper.ProcessDataGridView(this.dataGridViewEnumItem);

            this.Single = true;
            this.SingleKey = SINGLEKEY;

            this.HideOnClose = true;
            this.Icon = DrawingTool.ImageToIcon(Resources.Enum);
            this.TabText = Language.Current.DictionaryView_TabText;

            //DesignMode 拿不到 ServiceUnity.Container，报错
            if (DesignMode)
                return;

            //初始化命令
            InitCommands();

            //初始化菜单
            InitContextMenu();

            //初始化 DataGridView 控制器
            InitController();

            SubscribeEvent();
        }

        private void DictionaryView_Load(object sender, EventArgs e)
        {
            LoadEnumList();
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 载入枚举列表
        /// </summary>
        private void LoadEnumList()
        {
            _enumGridViewController.DataBind<EnumEntityDev>(DictionaryArchive.Instance.GetEnumEntityList());
        }

        private void InitCommands()
        {
            #region Commands

            _addEnumEntityCommand = new AddEnumEntityCommand();
            _editEnumEntityCommand = new EditEnumEntityCommand()
            {
                CanExcuteHandler = () =>
                {
                    return _enumGridViewController.SelectedItemsCount == 1 &&
                        _enumGridViewController.GetSelectedItem<EnumEntityDev>() != null;
                },
                GetArgumentHandler = () => { return _enumGridViewController.GetSelectedItem<EnumEntityDev>(); }
            };
            _deleteEnumEntityCommand = new DeleteEnumEntityCommand()
            {
                CanExcuteHandler = () => { return _enumGridViewController.GetSelectedItems<EnumEntityDev>().Count > 0; },
                GetArgumentHandler = () => { return _enumGridViewController.GetSelectedItems<EnumEntityDev>(); }
            };
            _addEnumItemEntityCommand = new AddEnumItemEntityCommand()
            {
                CanExcuteHandler = () =>
                {
                    return _enumGridViewController.SelectedItemsCount == 1 &&
                        _enumGridViewController.GetSelectedItem<EnumEntityDev>() != null;
                },
                GetArgumentHandler = () => { return _enumGridViewController.GetSelectedItem<EnumEntityDev>(); }
            };
            _editEnumItemEntityCommand = new EditEnumItemEntityCommand()
            {
                CanExcuteHandler = () =>
                {
                    return _enumItemGridViewController.SelectedItemsCount == 1 &&
                        _enumItemGridViewController.GetSelectedItem<EnumItemEntityDev>() != null;
                },
                GetArgumentHandler = () => { return _enumItemGridViewController.GetSelectedItem<EnumItemEntityDev>(); }
            };
            _deleteEnumItemEntityCommand = new DeleteEnumItemEntityCommand()
            {
                CanExcuteHandler = () => { return _enumItemGridViewController.GetSelectedItems<EnumItemEntityDev>().Count > 0; },
                GetArgumentHandler = () => { return _enumItemGridViewController.GetSelectedItems<EnumItemEntityDev>(); }

            };

            #endregion
        }

        private void InitContextMenu()
        {
            #region GridViewMenuDataEntity

            _gridViewMenuEnumEntity = new GridViewMenuEnumEntity();
            _gridViewMenuEnumEntity.AddCommand = _addEnumEntityCommand;
            _gridViewMenuEnumEntity.EditCommand = _editEnumEntityCommand;
            _gridViewMenuEnumEntity.DeleteCommand = _deleteEnumEntityCommand;
            _gridViewMenuEnumEntity.AddItemCommand = _addEnumItemEntityCommand;

            #endregion

            #region GridViewMenuEnumItemEntity

            _gridViewMenuEnumItemEntity = new GridViewMenuEnumItemEntity();
            _gridViewMenuEnumItemEntity.AddCommand = _addEnumItemEntityCommand;
            _gridViewMenuEnumItemEntity.EditCommand = _editEnumItemEntityCommand;
            _gridViewMenuEnumItemEntity.DeleteCommand = _deleteEnumItemEntityCommand;

            #endregion
        }

        private void InitController()
        {
            #region _enumGridViewController

            _enumGridViewController = new TypeBinderDataGridViewController(this.dataGridViewEnum);

            TypeBinderDataGridViewTypeCodon dataEntityCodon =
                new TypeBinderDataGridViewTypeCodon(typeof(EnumEntityDev))
                {
                    Columns = _gridColumns.EnumColumns,
                    ContextMenuStrip = _gridViewMenuEnumEntity.View
                };

            _enumGridViewController.AddCodon(dataEntityCodon);

            _enumGridViewController.SelectedItemChanged +=
                new TypeBinderDataGridViewController.OnSelectedItemChangedHandler(_enumGridViewController_SelectedItemChanged);

            #endregion

            #region _enumItemGridViewController

            _enumItemGridViewController = new TypeBinderDataGridViewController(this.dataGridViewEnumItem);

            TypeBinderDataGridViewTypeCodon enumItemCodon =
               new TypeBinderDataGridViewTypeCodon(typeof(EnumItemEntityDev))
               {
                   Columns = _gridColumns.EnumItemColumns,
                   ContextMenuStrip = _gridViewMenuEnumItemEntity.View
               };

            _enumItemGridViewController.AddCodon(enumItemCodon);

            #endregion
        }

        private void SubscribeEvent()
        {
            //订阅数据实体的增删改事件，以更新呈现

            SubscriptionToken enumAddedEvent =
                _eventAggregator.GetEvent<EnumAddedEvent>().Subscribe((e) => { _enumGridViewController.Add(e.EnumEntity); });
            SubscriptionToken enumRemovedEvent =
                _eventAggregator.GetEvent<EnumRemovedEvent>().Subscribe((e) => { _enumGridViewController.Remove(e.EnumEntity); });
            SubscriptionToken enumUpdatedEvent =
                _eventAggregator.GetEvent<EnumUpdatedEvent>().Subscribe((e) => { _enumGridViewController.Update(e.EnumEntity); });

            SubscriptionToken enumItemEntityAddedEvent =
                _eventAggregator.GetEvent<EnumItemEntityAddedEvent>().Subscribe((e) => { _enumItemGridViewController.Add(e.Entity); });
            SubscriptionToken enumItemEntityRemovedEvent =
                _eventAggregator.GetEvent<EnumItemEntityRemovedEvent>().Subscribe((e) => { _enumItemGridViewController.Remove(e.Entity); });
            SubscriptionToken enumItemEntityUpdatedEvent =
                _eventAggregator.GetEvent<EnumItemEntityUpdatedEvent>().Subscribe((e) => { _enumItemGridViewController.Update(e.Entity); });

            this.Disposed += (sender, e) =>
            {
                _eventAggregator.GetEvent<EnumAddedEvent>().Unsubscribe(enumAddedEvent);
                _eventAggregator.GetEvent<EnumRemovedEvent>().Unsubscribe(enumRemovedEvent);
                _eventAggregator.GetEvent<EnumUpdatedEvent>().Unsubscribe(enumUpdatedEvent);

                _eventAggregator.GetEvent<EnumItemEntityAddedEvent>().Unsubscribe(enumItemEntityAddedEvent);
                _eventAggregator.GetEvent<EnumItemEntityRemovedEvent>().Unsubscribe(enumItemEntityRemovedEvent);
                _eventAggregator.GetEvent<EnumItemEntityUpdatedEvent>().Unsubscribe(enumItemEntityUpdatedEvent);
            };
        }

        #endregion

        #region 事件处理

        void _enumGridViewController_SelectedItemChanged(object sender, EventArgs e)
        {
            if (_enumGridViewController.SelectedItemsCount != 1)
            {
                _enumItemGridViewController.ClearData();
            }
            else
            {
                EnumEntityDev enumEntity = _enumGridViewController.GetSelectedItem<EnumEntityDev>();
                _enumItemGridViewController.DataBind<EnumItemEntityDev>(enumEntity.Items, enumEntity);
            }
        }

        #endregion

        #region IWorkbenchWindow 成员

        private List<ToolStripCodon> _toolStripList;
        public override List<ToolStripCodon> ToolStripList
        {
            get
            {
                if (_toolStripList == null)
                {
                    _toolStripList = new List<ToolStripCodon>();

                    //_toolStripList.Add(_dataEntityGridView.ToolStrip);
                }

                return _toolStripList;
            }
        }

        #endregion

    }
}
