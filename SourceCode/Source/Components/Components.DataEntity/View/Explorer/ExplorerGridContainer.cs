using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Practices.Composite.Events;
using Sheng.SailingEase.ComponentModel;
using Sheng.SailingEase.Controls.Extensions;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Kernal;
using System.Collections;
using Sheng.SailingEase.Localisation;

namespace Sheng.SailingEase.Components.DataEntityComponent.View
{
    partial class ExplorerGridContainer : UserControl
    {
        #region 私有成员

        private IEventAggregator _eventAggregator;

        private TypeBinderDataGridViewController _controller;

        private GridColumns _gridColumns = new GridColumns();

        #region 右键菜单

        /// <summary>
        /// 列表中右击数据实体时的菜单
        /// </summary>
        private GridViewMenuDataEntity _gridViewMenuDataEntity = null;
        /// <summary>
        /// 列表中右击数据项时的菜单
        /// </summary>
        private GridViewMenuDataItemEntity _gridViewMenuDataItemEntity = null;

        #endregion

        #region Commands

        AddDataEntityCommand _addDataEntityCommand;
        EditDataEntityCommand _editDataEntityCommand;
        DeleteDataEntityCommand _deleteDataEntityCommand;
        CreateSqlCommand _createSqlCommand;

        AddDataItemEntityCommand _addDataItemEntityCommand;
        EditDataItemEntityCommand _editDataItemEntityCommand;
        DeleteDataItemEntityCommand _deleteDataItemEntityCommand;

        #endregion

        #endregion

        #region 公开属性

        private ToolStripGeneral _toolStrip;
        public ToolStripCodon ToolStrip
        {
            get
            {
                if (_toolStrip == null)
                {
                    #region ToolStripGeneral

                    _toolStrip = new ToolStripGeneral();

                    #region AddAction

                    _toolStrip.AddAction =
                       (sender, codon) =>
                       {
                           if (_addDataEntityCommand.CanExcute())
                           {
                               _addDataEntityCommand.Excute();
                           }
                           else if (_addDataItemEntityCommand.CanExcute())
                           {
                               _addDataItemEntityCommand.Excute();
                           }
                       };
                    _toolStrip.AddActionIsEnabled =
                        (codon) =>
                        {
                            if (_addDataEntityCommand.CanExcute() || _addDataItemEntityCommand.CanExcute())
                                return true;
                            else
                                return false;
                        };

                    #endregion

                    #region EditAction

                    _toolStrip.EditAction =
                         (sender, codon) =>
                         {
                             if (_editDataEntityCommand.CanExcute())
                             {
                                 _editDataEntityCommand.Excute();
                             }
                             else if (_editDataItemEntityCommand.CanExcute())
                             {
                                 _editDataItemEntityCommand.Excute();
                             }
                         };
                    _toolStrip.EditActionIsEnabled =
                       (codon) =>
                       {
                           if (_editDataEntityCommand.CanExcute() || _editDataItemEntityCommand.CanExcute())
                               return true;
                           else
                               return false;
                       };

                    #endregion

                    #region DeleteAction

                    _toolStrip.DeleteAction =
                         (sender, codon) =>
                         {
                             if (_deleteDataEntityCommand.CanExcute())
                             {
                                 _deleteDataEntityCommand.Excute();
                             }
                             else if (_deleteDataItemEntityCommand.CanExcute())
                             {
                                 _deleteDataItemEntityCommand.Excute();
                             }
                         };
                    _toolStrip.DeleteActionIsEnabled =
                       (codon) =>
                       {
                           if (_deleteDataEntityCommand.CanExcute() || _deleteDataItemEntityCommand.CanExcute())
                               return true;
                           else
                               return false;
                       };

                    #endregion

                    #region CreateSqlAction

                    _toolStrip.CreateSqlAction =
                         (sender, codon) =>
                         {
                             if (_createSqlCommand.CanExcute())
                             {
                                 _createSqlCommand.Excute();
                             }
                         };
                    _toolStrip.CreateSqlActionIsEnabled =
                       (codon) =>
                       {
                           return _createSqlCommand.CanExcute();
                       };

                    #endregion

                    #endregion
                }

                return _toolStrip;
            }
        }

        #endregion

        #region 构造

        public ExplorerGridContainer()
        {
            InitializeComponent();

            UIHelper.ProcessDataGridView(this.dataGridView);
            this.pictureBox1.Image = ImagesLibrary.Database_add_48;

            _eventAggregator = ServiceUnity.Container.Resolve<IEventAggregator>();

            //初始化命令
            InitCommands();

            //初始化菜单
            InitContextMenu();

            //初始化 DataGridView 控制器
            InitController();

            SubscribeEvent();
        }

        #endregion

        #region 公开方法

        public void Clear()
        {
            _controller.Clear();
        }

        public void DataBind(Type currentType, IList list, Type listType, object contextData)
        {
            _controller.DataBind(list, listType, contextData);
        }

        #endregion

        #region 私有方法

        private void InitCommands()
        {
            #region Commands

            _addDataEntityCommand = new AddDataEntityCommand()
            {
                //这里设置CanExcute是因为工具栏也要用这个Command，而工具栏上的添加按钮是共用于添加实体和项两种情况的
                CanExcuteHandler = () =>
                {
                    return _controller.CurrentType == typeof(DataEntityDev);
                }
            };
            _editDataEntityCommand = new EditDataEntityCommand()
            {
                CanExcuteHandler = () =>
                {
                    return _controller.SelectedItemsCount == 1 &&
                        _controller.GetSelectedItem<DataEntityDev>() != null;
                },
                GetArgumentHandler = () => { return _controller.GetSelectedItem<DataEntityDev>(); }
            };
            _deleteDataEntityCommand = new DeleteDataEntityCommand()
            {
                CanExcuteHandler = () => { return _controller.GetSelectedItems<DataEntityDev>().Count > 0; },
                GetArgumentHandler = () => { return _controller.GetSelectedItems<DataEntityDev>(); }
            };
            _createSqlCommand = new CreateSqlCommand()
            {
                CanExcuteHandler = () => { return _controller.GetSelectedItems<DataEntityDev>().Count > 0; },
                GetArgumentHandler = () => { return _controller.GetSelectedItems<DataEntity>(); }
            };

            _addDataItemEntityCommand = new AddDataItemEntityCommand()
            {
                //这里拿数据实体DataEntityDev对象， 通过_gridViewController的上下文
                CanExcuteHandler = () => { return _controller.GetContextData<DataEntityDev>() != null; },
                GetArgumentHandler = () => { return _controller.GetContextData<DataEntityDev>(); }
            };
            _editDataItemEntityCommand = new EditDataItemEntityCommand()
            {
                CanExcuteHandler = () =>
                {
                    return _controller.SelectedItemsCount == 1 &&
                        _controller.GetSelectedItem<DataItemEntityDev>() != null;
                },
                GetArgumentHandler = () => { return _controller.GetSelectedItem<DataItemEntityDev>(); }
            };
            _deleteDataItemEntityCommand = new DeleteDataItemEntityCommand()
            {
                CanExcuteHandler = () => { return _controller.GetSelectedItems<DataItemEntityDev>().Count > 0; },
                GetArgumentHandler = () => { return _controller.GetSelectedItems<DataItemEntityDev>(); }
            };

            #endregion
        }

        private void InitContextMenu()
        {
            #region GridViewMenuDataEntity

            _gridViewMenuDataEntity = new GridViewMenuDataEntity();
            _gridViewMenuDataEntity.AddCommand = _addDataEntityCommand;
            _gridViewMenuDataEntity.EditCommand = _editDataEntityCommand;
            _gridViewMenuDataEntity.DeleteCommand = _deleteDataEntityCommand;
            _gridViewMenuDataEntity.CreateSqlCommand = _createSqlCommand;

            #endregion

            #region GridViewMenuDataItemEntity

            _gridViewMenuDataItemEntity = new GridViewMenuDataItemEntity();
            _gridViewMenuDataItemEntity.AddCommand = _addDataItemEntityCommand;
            _gridViewMenuDataItemEntity.EditCommand = _editDataItemEntityCommand;
            _gridViewMenuDataItemEntity.DeleteCommand = _deleteDataItemEntityCommand;

            #endregion
        }

        private void InitController()
        {
            _controller = new TypeBinderDataGridViewController(this.dataGridView);

            TypeBinderDataGridViewTypeCodon dataEntityCodon =
                new TypeBinderDataGridViewTypeCodon(typeof(DataEntityDev))
                {
                    Columns = _gridColumns.DataEntityColumns,
                    ContextMenuStrip = _gridViewMenuDataEntity.View
                };

            TypeBinderDataGridViewTypeCodon dataItemEntityCodon =
                new TypeBinderDataGridViewTypeCodon(typeof(DataItemEntityDev))
                {
                    Columns = _gridColumns.DataItemEntityColumns,
                    ContextMenuStrip = _gridViewMenuDataItemEntity.View
                };

            _controller.AddCodon(dataEntityCodon);
            _controller.AddCodon(dataItemEntityCodon);

            _controller.DoubleClick += new TypeBinderDataGridViewController.OnDoubleClickHandler(_controller_DoubleClick);
            _controller.SelectedItemChanged += new TypeBinderDataGridViewController.OnSelectedItemChangedHandler(_controller_SelectedItemChanged);
        }

        private void SubscribeEvent()
        {
            //订阅数据实体的增删改事件，以更新呈现

            SubscriptionToken dataEntityAddedEventToken =
                _eventAggregator.GetEvent<DataEntityAddedEvent>().Subscribe((e) =>
                {
                    if (_controller.Compatible(e.DataEntity))
                        _controller.Add(e.DataEntity);
                });
            SubscriptionToken dataEntityRemovedEvent =
                _eventAggregator.GetEvent<DataEntityRemovedEvent>().Subscribe((e) =>
                {
                    if (_controller.Compatible(e.DataEntity))
                        _controller.Remove(e.DataEntity);
                });
            SubscriptionToken dataEntityUpdatedEvent =
                _eventAggregator.GetEvent<DataEntityUpdatedEvent>().Subscribe((e) =>
                {
                    if (_controller.Compatible(e.DataEntity))
                        _controller.Update(e.DataEntity);
                });

            SubscriptionToken dataItemEntityAddedEvent =
                _eventAggregator.GetEvent<DataItemEntityAddedEvent>().Subscribe((e) =>
                {
                    if (_controller.Compatible(e.Entity))
                        _controller.Add(e.Entity);
                });
            SubscriptionToken dataItemEntityRemovedEvent =
                _eventAggregator.GetEvent<DataItemEntityRemovedEvent>().Subscribe((e) =>
                {
                    if (_controller.Compatible(e.Entity))
                        _controller.Remove(e.Entity);
                });
            SubscriptionToken dataItemEntityUpdatedEvent =
                _eventAggregator.GetEvent<DataItemEntityUpdatedEvent>().Subscribe((e) =>
                {
                    if (_controller.Compatible(e.Entity))
                        _controller.Update(e.Entity);
                });

            this.Disposed += (sender, e) =>
            {
                _eventAggregator.GetEvent<DataEntityAddedEvent>().Unsubscribe(dataEntityAddedEventToken);
                _eventAggregator.GetEvent<DataEntityRemovedEvent>().Unsubscribe(dataEntityRemovedEvent);
                _eventAggregator.GetEvent<DataEntityUpdatedEvent>().Unsubscribe(dataEntityUpdatedEvent);

                _eventAggregator.GetEvent<DataItemEntityAddedEvent>().Unsubscribe(dataItemEntityAddedEvent);
                _eventAggregator.GetEvent<DataItemEntityRemovedEvent>().Unsubscribe(dataItemEntityRemovedEvent);
                _eventAggregator.GetEvent<DataItemEntityUpdatedEvent>().Unsubscribe(dataItemEntityUpdatedEvent);
            };
        }

        #endregion

        #region 事件处理

        void _controller_DoubleClick(object sender, GridViewControllerEventArgs e)
        {
            if (e.Data == null)
                return;

            if (GridDoubleClick != null)
            {
                GridDoubleClickEventArgs args = new GridDoubleClickEventArgs(e.Data, e.DataBoundType);
                GridDoubleClick(this, args);
            }

            if (e.Data.GetType().Equals(typeof(DataItemEntityDev)))
            {
                _editDataItemEntityCommand.Excute();
            }
        }

        void _controller_SelectedItemChanged(object sender, EventArgs e)
        {
            if (GridSelectedItemChanged != null)
            {
                GridSelectedItemChangedEventArgs args = new GridSelectedItemChangedEventArgs(
                    _controller.GetSelectedItems<object>());
                GridSelectedItemChanged(this, args);
            }
        }

        #endregion

        #region 事件

        public delegate void OnGridDoubleClickHandler(object sender, GridDoubleClickEventArgs e);
        public event OnGridDoubleClickHandler GridDoubleClick;

        public delegate void OnGridSelectedItemChangedHandler(object sender, GridSelectedItemChangedEventArgs e);
        public event OnGridSelectedItemChangedHandler GridSelectedItemChanged;

        #endregion

        #region GridDoubleClickEventArgs

        public class GridDoubleClickEventArgs : EventArgs
        {
            /// <summary>
            /// 目标行所绑定的对象
            /// </summary>
            public object Data
            {
                get;
                private set;
            }

            /// <summary>
            /// 所对应的绑定数据类型
            /// </summary>
            public Type DataBoundType
            {
                get;
                private set;
            }

            public GridDoubleClickEventArgs(object data, Type dataBoundType)
            {
                Data = data;
                DataBoundType = dataBoundType;
            }
        }

        #endregion

        #region GridSelectedItemChangedEventArgs

        public class GridSelectedItemChangedEventArgs : EventArgs
        {
            /// <summary>
            /// 选定的对象
            /// </summary>
            public List<object> Data
            {
                get;
                private set;
            }

            public GridSelectedItemChangedEventArgs(List<object> data)
            {
                Data = data;
            }
        }

        #endregion
    }
}
