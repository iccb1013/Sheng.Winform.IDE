using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Practices.Composite.Events;
using Sheng.SailingEase.Components.WindowComponent.Localisation;
using Sheng.SailingEase.Controls;
using Sheng.SailingEase.Controls.Extensions;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Kernal;

namespace Sheng.SailingEase.Components.WindowComponent.View
{
    [ToolboxItem(false)]
    partial class ExplorerGridContainer : UserControl
    {
        #region 私有成员

        private IEventAggregator _eventAggregator = ServiceUnity.EventAggregator;

        /// <summary>
        /// 当前上下文对象
        /// 当前列表中的文件夹，窗体的父级文件夹
        /// </summary>
        private FolderEntityIndex _currentFolder;

        private TypeBinderDataGridViewController _controller;

        private Columns _gridColumns = new Columns();

        #region 右键菜单

        private GridViewContextMenu _gridViewContextMenu;

        #endregion

        #region Commands

        AddFolderCommand _addFolderCommand;
        AddWindowCommand _addWindowCommand;
        EditCommand _editCommand;
        DeleteCommand _deleteCommand;

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
                           _addWindowCommand.Excute();
                       };
                    _toolStrip.AddActionIsEnabled =
                        (codon) =>
                        {
                            return _addWindowCommand.CanExcute();
                        };

                    #endregion

                    #region AddFolderAction

                    _toolStrip.AddFolderAction =
                       (sender, codon) =>
                       {
                           _addFolderCommand.Excute();
                       };
                    _toolStrip.AddFolderActionIsEnabled =
                        (codon) =>
                        {
                            return _addFolderCommand.CanExcute();
                        };

                    #endregion

                    #region EditAction

                    _toolStrip.EditAction =
                         (sender, codon) =>
                         {
                             _editCommand.Excute();
                         };
                    _toolStrip.EditActionIsEnabled =
                       (codon) =>
                       {
                           return _editCommand.CanExcute();
                       };

                    #endregion

                    #region DeleteAction

                    _toolStrip.DeleteAction =
                         (sender, codon) =>
                         {
                             _deleteCommand.Excute();
                         };
                    _toolStrip.DeleteActionIsEnabled =
                       (codon) =>
                       {
                           return _deleteCommand.CanExcute();
                       };

                    #endregion

                    #endregion
                }

                return _toolStrip;
            }
        }

        #endregion

        #region 构造及窗体事件

        public ExplorerGridContainer()
        {
            InitializeComponent();

            Unity.ApplyResource(this);

            UIHelper.ProcessDataGridView(this.dataGridView);

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

        private void UserControlMainMenu_Load(object sender, EventArgs e)
        {
            lblMenuPath.Text = String.Empty;

            //LoadMenuList();
        }

        #endregion

        #region 事件处理

        void _controller_DoubleClick(object sender, GridViewControllerEventArgs e)
        {
            if (GridDoubleClick != null)
            {
                GridDoubleClickEventArgs args = new GridDoubleClickEventArgs(e.Data, e.DataBoundType);
                GridDoubleClick(this, args);
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

        #region 私有方法

        private void InitController()
        {
           

            _controller = new TypeBinderDataGridViewController(this.dataGridView);

            TypeBinderDataGridViewTypeCodon folderCodon =
                new TypeBinderDataGridViewTypeCodon(typeof(FolderEntityIndex));

            TypeBinderDataGridViewTypeCodon windowCodon =
                new TypeBinderDataGridViewTypeCodon(typeof(WindowEntityIndex));

            TypeBinderDataGridViewComboTypeCodon<IEntityIndex> comboCodon = 
                new TypeBinderDataGridViewComboTypeCodon<IEntityIndex>()
                {
                    ActOnSubClass = true,
                    Columns = _gridColumns.GridColumns,
                    ContextMenuStrip = _gridViewContextMenu.View
                };

            comboCodon.AddCodon(folderCodon);
            comboCodon.AddCodon(windowCodon);

            _controller.AddCodon(comboCodon);

            _controller.DoubleClick += new TypeBinderDataGridViewController.OnDoubleClickHandler(_controller_DoubleClick);
            _controller.SelectedItemChanged += new TypeBinderDataGridViewController.OnSelectedItemChangedHandler(_controller_SelectedItemChanged);
        }

        private void InitCommands()
        {
            #region Commands

            _addFolderCommand = new AddFolderCommand()
            {
                GetArgumentHandler = () =>
                {
                    FolderEntityIndex folderEntityIndex = _currentFolder;
                    return folderEntityIndex.Folder;
                }
            };

            _addWindowCommand = new AddWindowCommand()
            {
                CanExcuteHandler = () =>
                {
                    return _currentFolder != null;
                },
                GetArgumentHandler = () =>
                {
                    FolderEntityIndex folderEntityIndex = _currentFolder;
                    return folderEntityIndex.Folder;
                }
            };

            _editCommand = new EditCommand()
            {
                CanExcuteHandler = () =>
                {
                    return _controller.SelectedItemsCount == 1;
                },
                GetArgumentHandler = () =>
                {
                    return _controller.GetSelectedItem<IEntityIndex>();
                }
            };

            _deleteCommand = new DeleteCommand()
            {
                CanExcuteHandler = () =>
                {
                    return _controller.SelectedItemsCount > 0;
                },
                GetArgumentHandler = () =>
                {
                    return _controller.GetSelectedItems<IEntityIndex>();
                }
            };

            #endregion
        }

        private void InitContextMenu()
        {
            _gridViewContextMenu = new GridViewContextMenu();
            _gridViewContextMenu.AddCommand = _addWindowCommand;
            _gridViewContextMenu.AddFolderCommand = _addFolderCommand;
            _gridViewContextMenu.EditCommand = _editCommand;
            _gridViewContextMenu.DeleteCommand = _deleteCommand;
        }

        private void SubscribeEvent()
        {
            //订阅增删改事件，以更新呈现

           

            #region Folder

            #region Add,Remove,Update

            //SubscriptionToken folderAddedEvent =
            //    _eventAggregator.GetEvent<WindowFolderAddedEvent>().Subscribe((e) =>
            //    {
            //        FolderEntityIndex index = new FolderEntityIndex(e.Folder);
            //        _controller.Add(index);
            //    });
            SubscriptionToken folderRemovedEvent =
                _eventAggregator.GetEvent<WindowFolderRemovedEvent>().Subscribe((e) =>
                {
                    _controller.Remove<FolderEntityIndex>((index) =>
                    {
                        return index.Id == e.Folder.Id;
                    });
                });
            SubscriptionToken folderUpdatedEvent =
                _eventAggregator.GetEvent<WindowFolderUpdatedEvent>().Subscribe((e) =>
                {
                    _controller.Update<FolderEntityIndex>((index) =>
                    {
                        return index.Id == e.Folder.Id;
                    });
                });

            #endregion

            #endregion

            #region Window

            #region Add,Remove,Update

            //SubscriptionToken windowAddedEvent =
            //    _eventAggregator.GetEvent<WindowAddedEvent>().Subscribe((e) =>
            //    {
            //        WindowEntityIndex index = new WindowEntityIndex(e.Entity);
            //        _controller.Add(index);
            //    });
            SubscriptionToken windowRemovedEvent =
                _eventAggregator.GetEvent<WindowRemovedEvent>().Subscribe((e) =>
                {
                    _controller.Remove<WindowEntityIndex>((index) =>
                    {
                        return index.Id == e.Id;
                    });
                });
            SubscriptionToken windowUpdatedEvent =
                _eventAggregator.GetEvent<WindowUpdatedEvent>().Subscribe((e) =>
                {
                    _controller.Update<WindowEntityIndex>((index) =>
                    {
                        return index.Id == e.Entity.Id;
                    });
                });

            #endregion

            #endregion

            #region Disposed

            this.Disposed += (sender, e) =>
            {
                //_eventAggregator.GetEvent<WindowFolderAddedEvent>().Unsubscribe(folderAddedEvent);
                _eventAggregator.GetEvent<WindowFolderRemovedEvent>().Unsubscribe(folderRemovedEvent);
                _eventAggregator.GetEvent<WindowFolderUpdatedEvent>().Unsubscribe(folderUpdatedEvent);

                //_eventAggregator.GetEvent<WindowAddedEvent>().Unsubscribe(windowAddedEvent);
                _eventAggregator.GetEvent<WindowRemovedEvent>().Unsubscribe(windowRemovedEvent);
                _eventAggregator.GetEvent<WindowUpdatedEvent>().Unsubscribe(windowUpdatedEvent);
            };

            #endregion
        }

        #endregion

        #region 公开方法

        /// <summary>
        /// 清除列，数据源，上下文相关数据
        /// </summary>
        public void Clear()
        {
            _controller.Clear();
            _currentFolder = null;
        }

        /// <summary>
        /// 绑定数据
        /// currentType 表示当前列表中显示的数据的类型，这个不能从contextData上取，因为contextData可能是个ToolStripItemFolder之类
        /// listType是当前显示的list中的数据的类型
        /// contextData是list所属对象（当前列表中的文件夹，窗体的父级文件夹）
        /// </summary>
        /// <param name="list"></param>
        /// <param name="type"></param>
        /// <param name="contextData"></param>
        public void DataBind(Type dataType, IList list, Type listType, object contextData)
        {
            //目前，还有可能是 WindowEntityIndex
            if (contextData is FolderEntityIndex)
                _currentFolder = (FolderEntityIndex)contextData;
            else
                _currentFolder = null;

            _controller.DataBind(list, listType, contextData);
        }

        #endregion

        #region 事件

        public delegate void OnGridDoubleClickHandler(object sender, GridDoubleClickEventArgs e);
        public event OnGridDoubleClickHandler GridDoubleClick;

        public delegate void OnGridSelectedItemChangedHandler(object sender,GridSelectedItemChangedEventArgs e);
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
