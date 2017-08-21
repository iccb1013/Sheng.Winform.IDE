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
using Sheng.SailingEase.Components.NavigationComponent.Localisation;
using Sheng.SailingEase.Controls;
using Sheng.SailingEase.Controls.Extensions;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Kernal;
using Sheng.SailingEase.Localisation;

namespace Sheng.SailingEase.Components.NavigationComponent.View
{
    [ToolboxItem(false)]
    partial class ExplorerGridContainer : UserControl
    {
        #region 私有成员

        private IEventAggregator _eventAggregator = ServiceUnity.EventAggregator;

        /// <summary>
        /// 当前上下文对象
        /// 就是当前列表中的数据的所属对象
        /// </summary>
        private object _contextData;

        /// <summary>
        /// 当前列表中显示的数据的数据类型
        /// （就是Tab页对应的类型，如MenuEntity或ToolStripItemAbstract）
        /// </summary>
        private Type _currentDataType;

        private TypeBinderDataGridViewController _controller;

        private Columns _gridColumns = new Columns();

        #region 右键菜单

        private GridMenuMenuEntity _gridViewMenuMenuEntity;

        private GridMenuToolStripPage _gridMenuToolStripPage;
        private GridMenuToolStripGroup _gridMenuToolStripGroup;
        private GridMenuToolStripItem _gridMenuToolStripItem;

        #endregion

        #region Commands

        #region MenuStrip

        private AddMenuEntityCommand _addMenuEntityCommand;
        private EditMenuEntityCommand _editMenuEntityCommand;
        private DeleteMenuEntityCommand _deleteMenuEntityCommand;
        private MoveBeforeMenuEntityCommand _moveBeforeMenuEntityCommand;
        private MoveAfterMenuEntityCommand _moveAfterMenuEntityCommand;

        #endregion

        #region ToolStrip

        private AddToolStripPageCommand _addToolStripPageCommand;
        private EditToolStripPageCommand _editToolStripPageCommand;
        private RemoveToolStripPageCommand _removeToolStripPageCommand;
        private MoveBeforeToolStripPageCommand _moveBeforeToolStripPageCommand;
        private MoveAfterToolStripPageCommand _moveAfterToolStripPageCommand;

        private AddToolStripGroupCommand _addToolStripGroupCommand;
        private EditToolStripGroupCommand _editToolStripGroupCommand;
        private RemoveToolStripGroupCommand _removeToolStripGroupCommand;
        private MoveBeforeToolStripGroupCommand _moveBeforeToolStripGroupCommand;
        private MoveAfterToolStripGroupCommand _moveAfterToolStripGroupCommand;

        private AddToolStripItemCommand _addToolStripItemCommand;
        private EditToolStripItemCommand _editToolStripItemCommand;
        private DeleteToolStripItemCommand _deleteToolStripItemCommand;
        private MoveBeforeToolStripItemCommand _moveBeforeToolStripItemCommand;
        private MoveAfterToolStripItemCommand _moveAfterToolStripItemCommand;

        #endregion

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

                    //_currentDataType 指定的类型是通过树（TreeView）的控制器的对应的 Codon 指定的
                    //所以此处不需要考虑 IsSubClassOf 和 Interface，这里的类型判断是明确的

                    _toolStrip.AddAction =
                       (sender, codon) =>
                       {
                           if (_currentDataType == typeof(MenuEntity) && _addMenuEntityCommand.CanExcute())
                           {
                               _addMenuEntityCommand.Excute();
                           }
                           else if (_currentDataType == typeof(ToolStripPageEntity) && _addToolStripPageCommand.CanExcute())
                           {
                               _addToolStripPageCommand.Excute();
                           }
                           else if (_currentDataType == typeof(ToolStripGroupEntity) && _addToolStripGroupCommand.CanExcute())
                           {
                               _addToolStripGroupCommand.Excute();
                           }
                           else if (_currentDataType == typeof(ToolStripItemAbstract) && _addToolStripItemCommand.CanExcute())
                           {
                               _addToolStripItemCommand.Excute();
                           }
                       };
                    _toolStrip.AddActionIsEnabled =
                        (codon) =>
                        {
                            if (
                                (_currentDataType == typeof(MenuEntity) && _addMenuEntityCommand.CanExcute()) ||
                                (_currentDataType == typeof(ToolStripPageEntity) && _addToolStripPageCommand.CanExcute()) ||
                                 (_currentDataType == typeof(ToolStripGroupEntity) && _addToolStripGroupCommand.CanExcute()) ||
                                (_currentDataType == typeof(ToolStripItemAbstract) && _addToolStripItemCommand.CanExcute())
                                )
                                return true;
                            else
                                return false;
                        };

                    #endregion

                    #region EditAction

                    _toolStrip.EditAction =
                         (sender, codon) =>
                         {
                             if (_currentDataType == typeof(MenuEntity) && _editMenuEntityCommand.CanExcute())
                             {
                                 _editMenuEntityCommand.Excute();
                             }
                             else if (_currentDataType == typeof(ToolStripPageEntity) && _editToolStripPageCommand.CanExcute())
                             {
                                 _editToolStripPageCommand.Excute();
                             }
                             else if (_currentDataType == typeof(ToolStripGroupEntity) && _editToolStripGroupCommand.CanExcute())
                             {
                                 _editToolStripGroupCommand.Excute();
                             }
                             else if (_currentDataType == typeof(ToolStripItemAbstract) && _editToolStripItemCommand.CanExcute())
                             {
                                 _editToolStripItemCommand.Excute();
                             }
                         };
                    _toolStrip.EditActionIsEnabled =
                       (codon) =>
                       {
                           if (
                                (_currentDataType == typeof(MenuEntity) && _editMenuEntityCommand.CanExcute()) ||
                                (_currentDataType == typeof(ToolStripPageEntity) && _editToolStripPageCommand.CanExcute()) ||
                                 (_currentDataType == typeof(ToolStripGroupEntity) && _editToolStripGroupCommand.CanExcute()) ||
                                (_currentDataType == typeof(ToolStripItemAbstract) && _editToolStripItemCommand.CanExcute())
                                )
                               return true;
                           else
                               return false;
                       };

                    #endregion

                    #region DeleteAction

                    _toolStrip.DeleteAction =
                         (sender, codon) =>
                         {
                             if (_currentDataType == typeof(MenuEntity) && _deleteMenuEntityCommand.CanExcute())
                             {
                                 _deleteMenuEntityCommand.Excute();
                             }
                             else if (_currentDataType == typeof(ToolStripPageEntity) && _removeToolStripPageCommand.CanExcute())
                             {
                                 _removeToolStripPageCommand.Excute();
                             }
                             else if (_currentDataType == typeof(ToolStripGroupEntity) && _removeToolStripGroupCommand.CanExcute())
                             {
                                 _removeToolStripGroupCommand.Excute();
                             }
                             else if (_currentDataType == typeof(ToolStripItemAbstract) && _deleteToolStripItemCommand.CanExcute())
                             {
                                 _deleteToolStripItemCommand.Excute();
                             }
                         };
                    _toolStrip.DeleteActionIsEnabled =
                       (codon) =>
                       {
                           if (
                               (_currentDataType == typeof(MenuEntity) && _deleteMenuEntityCommand.CanExcute()) ||
                               (_currentDataType == typeof(ToolStripPageEntity) && _removeToolStripPageCommand.CanExcute()) ||
                                (_currentDataType == typeof(ToolStripGroupEntity) && _removeToolStripGroupCommand.CanExcute()) ||
                               (_currentDataType == typeof(ToolStripItemAbstract) && _deleteToolStripItemCommand.CanExcute())
                               )
                               return true;
                           else
                               return false;
                       };

                    #endregion

                    #region MoveBeforeAction

                    _toolStrip.MoveBeforeAction =
                         (sender, codon) =>
                         {
                             if (_currentDataType == typeof(MenuEntity) && _moveBeforeMenuEntityCommand.CanExcute())
                             {
                                 _moveBeforeMenuEntityCommand.Excute();
                             }
                             else if (_currentDataType == typeof(ToolStripPageEntity) &&_moveBeforeToolStripPageCommand.CanExcute())
                             {
                                 _moveBeforeToolStripPageCommand.Excute();
                             }
                             else if (_currentDataType == typeof(ToolStripGroupEntity) && _moveBeforeToolStripGroupCommand.CanExcute())
                             {
                                 _moveBeforeToolStripGroupCommand.Excute();
                             }
                             else if (_currentDataType == typeof(ToolStripItemAbstract) && _moveBeforeToolStripItemCommand.CanExcute())
                             {
                                 _moveBeforeToolStripItemCommand.Excute();
                             }
                         };
                    _toolStrip.MoveBeforeActionIsEnabled =
                       (codon) =>
                       {
                           if (
                              (_currentDataType == typeof(MenuEntity) && _moveBeforeMenuEntityCommand.CanExcute()) ||
                              (_currentDataType == typeof(ToolStripPageEntity) && _moveBeforeToolStripPageCommand.CanExcute()) ||
                               (_currentDataType == typeof(ToolStripGroupEntity) && _moveBeforeToolStripGroupCommand.CanExcute()) ||
                              (_currentDataType == typeof(ToolStripItemAbstract) && _moveBeforeToolStripItemCommand.CanExcute())
                              )
                               return true;
                           else
                               return false;
                       };

                    #endregion

                    #region MoveAfterAction

                    _toolStrip.MoveAfterAction =
                         (sender, codon) =>
                         {
                             if (_currentDataType == typeof(MenuEntity) && _moveAfterMenuEntityCommand.CanExcute())
                             {
                                 _moveAfterMenuEntityCommand.Excute();
                             }
                             else if (_currentDataType == typeof(ToolStripPageEntity) && _moveAfterToolStripPageCommand.CanExcute())
                             {
                                 _moveAfterToolStripPageCommand.Excute();
                             }
                             else if (_currentDataType == typeof(ToolStripGroupEntity) && _moveAfterToolStripGroupCommand.CanExcute())
                             {
                                 _moveAfterToolStripGroupCommand.Excute();
                             }
                             else if (_currentDataType == typeof(ToolStripItemAbstract) && _moveAfterToolStripItemCommand.CanExcute())
                             {
                                 _moveAfterToolStripItemCommand.Excute();
                             }
                         };
                    _toolStrip.MoveAfterActionIsEnabled =
                       (codon) =>
                       {
                           if (
                             (_currentDataType == typeof(MenuEntity) && _moveAfterMenuEntityCommand.CanExcute()) ||
                             (_currentDataType == typeof(ToolStripPageEntity) && _moveAfterToolStripPageCommand.CanExcute()) ||
                              (_currentDataType == typeof(ToolStripGroupEntity) && _moveAfterToolStripGroupCommand.CanExcute()) ||
                             (_currentDataType == typeof(ToolStripItemAbstract) && _moveAfterToolStripItemCommand.CanExcute())
                             )
                               return true;
                           else
                               return false;
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
            this.pictureBox1.Image = ImagesLibrary.Navigate_48;

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

        /// <summary>
        /// 载入主菜单列表
        /// </summary>
        private void LoadMenuStrip()
        {
            _controller.DataBind<MenuEntity>(MenuStripArchive.Instance.GetMenuEntityList());
        }

        /// <summary>
        /// 载入主工具栏列表
        /// </summary>
        private void LoadToolStrip()
        {
            _controller.DataBind<ToolStripItemAbstract>(ToolStripArchive.Instance.GetEntityList());
        }

        private void InitController()
        {
            _controller = new TypeBinderDataGridViewController(this.dataGridView);

            #region Codon

            #region MenuStrip

            TypeBinderDataGridViewTypeCodon menuStripCodon =
                new TypeBinderDataGridViewTypeCodon(typeof(MenuEntity))
                {
                    ActOnSubClass = true,
                    Columns = _gridColumns.MenuColumns,
                    ContextMenuStrip = _gridViewMenuMenuEntity.View
                };
            _controller.AddCodon(menuStripCodon);

            #endregion

            #region ToolStrip

            TypeBinderDataGridViewTypeCodon toolStripPageCodon =
                new TypeBinderDataGridViewTypeCodon(typeof(ToolStripPageEntity))
                {
                    ActOnSubClass = true,
                    Columns = _gridColumns.ToolStripPageColumns,
                    ContextMenuStrip = _gridMenuToolStripPage.View
                };
            _controller.AddCodon(toolStripPageCodon);

            TypeBinderDataGridViewTypeCodon toolStripGroupCodon =
               new TypeBinderDataGridViewTypeCodon(typeof(ToolStripGroupEntity))
               {
                   ActOnSubClass = true,
                   Columns = _gridColumns.ToolStripGroupColumns,
                   ContextMenuStrip = _gridMenuToolStripGroup.View
               };
            _controller.AddCodon(toolStripGroupCodon);

            TypeBinderDataGridViewTypeCodon toolStripItemCodon =
                new TypeBinderDataGridViewTypeCodon(typeof(ToolStripItemAbstract))
                {
                    ActOnSubClass = true,
                    Columns = _gridColumns.ToolStripItemColumns,
                    ContextMenuStrip = _gridMenuToolStripItem.View
                };
            _controller.AddCodon(toolStripItemCodon);

            #endregion

            #endregion

            _controller.DoubleClick += new TypeBinderDataGridViewController.OnDoubleClickHandler(_controller_DoubleClick);
            _controller.SelectedItemChanged += new TypeBinderDataGridViewController.OnSelectedItemChangedHandler(_controller_SelectedItemChanged);
        }

        private void InitCommands()
        {
            #region MenuStrip

            #region AddMenuEntityCommand

            _addMenuEntityCommand = new AddMenuEntityCommand()
            {
                CanExcuteHandler = () =>
                {
                    return _currentDataType != null && _currentDataType.Equals(typeof(MenuEntity));
                },
                //此项所属的项的Id
                GetArgumentHandler = () =>
                {
                    if (_contextData is MenuEntity)
                    {
                        MenuEntity entity = (MenuEntity)_contextData;
                        return entity.Id;
                    }
                    else
                    {
                        //正在添加的项为顶层
                        return null;
                    }
                }
            };

            #endregion

            #region EditMenuEntityCommand

            _editMenuEntityCommand = new EditMenuEntityCommand()
            {
                CanExcuteHandler = () =>
                {
                    bool currentType = _currentDataType != null && _currentDataType.Equals(typeof(MenuEntity));

                    return currentType && (_controller.SelectedItemsCount == 1 &&
                        _controller.GetSelectedItem<MenuEntity>() != null);
                },
                GetArgumentHandler = () => { return _controller.GetSelectedItem<MenuEntity>(); }
            };

            #endregion

            #region DeleteMenuEntityCommand

            _deleteMenuEntityCommand = new DeleteMenuEntityCommand()
            {
                CanExcuteHandler = () =>
                {
                    bool currentType = _currentDataType != null && _currentDataType.Equals(typeof(MenuEntity));

                    return currentType && (_controller.GetSelectedItems<MenuEntity>().Count > 0);
                },
                GetArgumentHandler = () => { return _controller.GetSelectedItems<MenuEntity>(); }
            };

            #endregion

            #region MoveUpMenuEntityCommand

            _moveBeforeMenuEntityCommand = new MoveBeforeMenuEntityCommand()
            {
                CanExcuteHandler = () =>
                {
                    bool currentType = _currentDataType != null && _currentDataType.Equals(typeof(MenuEntity));

                    return currentType && (_controller.SelectedItemsCount == 1 &&
                        _controller.SelectedItemIndex > 0 &&
                        _controller.GetSelectedItem<MenuEntity>() != null);
                },
                GetMenuId = () => { return _controller.GetSelectedItem<MenuEntity>().Id; },
                GetBeforeMenuId = () =>
                {
                    return _controller.GetItem<MenuEntity>(
                        _controller.SelectedItemIndex - 1).Id;
                }
            };

            #endregion

            #region MoveDownMenuEntityCommand

            _moveAfterMenuEntityCommand = new MoveAfterMenuEntityCommand()
            {
                CanExcuteHandler = () =>
                {
                    bool currentType = _currentDataType != null && _currentDataType.Equals(typeof(MenuEntity));

                    return currentType && (_controller.SelectedItemsCount == 1 &&
                        _controller.SelectedItemIndex < _controller.ItemsCount - 1 &&
                        _controller.GetSelectedItem<MenuEntity>() != null);
                },
                GetMenuId = () => { return _controller.GetSelectedItem<MenuEntity>().Id; },
                GetAfterMenuId = () =>
                {
                    return _controller.GetItem<MenuEntity>(
                        _controller.SelectedItemIndex + 1).Id;
                }
            };

            #endregion

            #endregion

            #region ToolStrip

            #region Page

            _addToolStripPageCommand = new AddToolStripPageCommand();

            _editToolStripPageCommand = new EditToolStripPageCommand()
            {
                CanExcuteHandler = () =>
                {
                    bool currentType = _currentDataType != null && _currentDataType.Equals(typeof(ToolStripPageEntity));

                    return currentType && (_controller.SelectedItemsCount == 1 &&
                        _controller.GetSelectedItem<ToolStripPageEntity>() != null);
                },
                GetArgumentHandler = () => { return _controller.GetSelectedItem<ToolStripPageEntity>(); }
            };

            _removeToolStripPageCommand = new RemoveToolStripPageCommand()
            {
                CanExcuteHandler = () =>
                {
                    bool currentType = _currentDataType != null && _currentDataType.Equals(typeof(ToolStripPageEntity));

                    return currentType && (_controller.GetSelectedItems<ToolStripPageEntity>().Count > 0);
                },
                GetArgumentHandler = () => { return _controller.GetSelectedItems<ToolStripPageEntity>(); }
            };

            _moveBeforeToolStripPageCommand = new MoveBeforeToolStripPageCommand()
            {
                CanExcuteHandler = () =>
                {
                    bool currentType = _currentDataType != null && _currentDataType.Equals(typeof(ToolStripPageEntity));

                    return currentType && (_controller.SelectedItemsCount == 1 &&
                        _controller.SelectedItemIndex > 0 &&
                        _controller.GetSelectedItem<ToolStripPageEntity>() != null);
                },
                GetId = () => { return _controller.GetSelectedItem<ToolStripPageEntity>().Id; },
                GetBeforeId = () =>
                {
                    return _controller.GetItem<ToolStripPageEntity>(
                        _controller.SelectedItemIndex - 1).Id;
                }
            };

            _moveAfterToolStripPageCommand = new MoveAfterToolStripPageCommand()
            {
                CanExcuteHandler = () =>
                {
                    bool currentType = _currentDataType != null && _currentDataType.Equals(typeof(ToolStripPageEntity));

                    return currentType && (_controller.SelectedItemsCount == 1 &&
                        _controller.SelectedItemIndex < _controller.ItemsCount - 1 &&
                        _controller.GetSelectedItem<ToolStripPageEntity>() != null);
                },
                GetId = () => { return _controller.GetSelectedItem<ToolStripPageEntity>().Id; },
                GetAfterId = () =>
                {
                    return _controller.GetItem<ToolStripPageEntity>(
                        _controller.SelectedItemIndex + 1).Id;
                }
            };

            #endregion

            #region Group

            _addToolStripGroupCommand = new AddToolStripGroupCommand()
            {
                CanExcuteHandler = () =>
                {
                    return _currentDataType != null && _currentDataType.Equals(typeof(ToolStripGroupEntity)) &&
                        _contextData != null && _contextData is ToolStripPageEntity;
                },
                //此项所属的项的Id
                GetArgumentHandler = () =>
                {
                    if (_contextData is ToolStripPageEntity)
                    {
                        ToolStripPageEntity entity = (ToolStripPageEntity)_contextData;
                        return entity.Id;
                    }
                    else
                    {
                        return null;
                    }
                }
            };

            _editToolStripGroupCommand = new EditToolStripGroupCommand()
            {
                CanExcuteHandler = () =>
                {
                    bool currentType = _currentDataType != null && _currentDataType.Equals(typeof(ToolStripGroupEntity));

                    return currentType && (_controller.SelectedItemsCount == 1 &&
                        _controller.GetSelectedItem<ToolStripGroupEntity>() != null);
                },
                GetArgumentHandler = () => { return _controller.GetSelectedItem<ToolStripGroupEntity>(); }
            };

            _removeToolStripGroupCommand = new RemoveToolStripGroupCommand()
            {
                CanExcuteHandler = () =>
                {
                    bool currentType = _currentDataType != null && _currentDataType.Equals(typeof(ToolStripGroupEntity));

                    return currentType && (_controller.GetSelectedItems<ToolStripGroupEntity>().Count > 0);
                },
                GetArgumentHandler = () => { return _controller.GetSelectedItems<ToolStripGroupEntity>(); }
            };

            _moveBeforeToolStripGroupCommand = new MoveBeforeToolStripGroupCommand()
            {
                CanExcuteHandler = () =>
                {
                    bool currentType = _currentDataType != null && _currentDataType.Equals(typeof(ToolStripGroupEntity));

                    return currentType && (_controller.SelectedItemsCount == 1 &&
                        _controller.SelectedItemIndex > 0 &&
                        _controller.GetSelectedItem<ToolStripGroupEntity>() != null);
                },
                GetId = () => { return _controller.GetSelectedItem<ToolStripGroupEntity>().Id; },
                GetBeforeId = () =>
                {
                    return _controller.GetItem<ToolStripGroupEntity>(
                        _controller.SelectedItemIndex - 1).Id;
                }
            };

            _moveAfterToolStripGroupCommand = new MoveAfterToolStripGroupCommand()
            {
                CanExcuteHandler = () =>
                {
                    bool currentType = _currentDataType != null && _currentDataType.Equals(typeof(ToolStripGroupEntity));

                    return currentType && (_controller.SelectedItemsCount == 1 &&
                        _controller.SelectedItemIndex < _controller.ItemsCount - 1 &&
                        _controller.GetSelectedItem<ToolStripGroupEntity>() != null);
                },
                GetId = () => { return _controller.GetSelectedItem<ToolStripGroupEntity>().Id; },
                GetAfterId = () =>
                {
                    return _controller.GetItem<ToolStripGroupEntity>(
                        _controller.SelectedItemIndex + 1).Id;
                }
            };

            #endregion

            #region Items

            #region AddToolStripItemCommand

            _addToolStripItemCommand = new AddToolStripItemCommand()
            {
                CanExcuteHandler = () =>
                {
                    return _currentDataType != null && _currentDataType.Equals(typeof(ToolStripItemAbstract))
                        && _contextData != null && _contextData is ToolStripGroupEntity;
                },
                //此项所属的项的Id
                GetArgumentHandler = () =>
                {
                    if (_contextData is ToolStripGroupEntity)
                    {
                        ToolStripGroupEntity entity = (ToolStripGroupEntity)_contextData;
                        return entity.Id;
                    }
                    else
                    {
                        return null;
                    }
                }
            };

            #endregion

            #region EditToolStripItemCommand

            _editToolStripItemCommand = new EditToolStripItemCommand()
            {
                CanExcuteHandler = () =>
                {
                    bool currentType = _currentDataType != null && _currentDataType.Equals(typeof(ToolStripItemAbstract));

                    return currentType && (_controller.SelectedItemsCount == 1 &&
                        _controller.GetSelectedItem<ToolStripItemAbstract>() != null);
                },
                GetArgumentHandler = () => { return _controller.GetSelectedItem<ToolStripItemAbstract>(); }
            };

            #endregion

            #region DeleteToolStripItemCommand

            _deleteToolStripItemCommand = new DeleteToolStripItemCommand()
            {
                CanExcuteHandler = () =>
                {
                    bool currentType = _currentDataType != null && _currentDataType.Equals(typeof(ToolStripItemAbstract));

                    return currentType && (_controller.GetSelectedItems<ToolStripItemAbstract>().Count > 0);
                },
                GetArgumentHandler = () => { return _controller.GetSelectedItems<ToolStripItemAbstract>(); }
            };

            #endregion

            #region MoveBeforeToolStripItemCommand

            _moveBeforeToolStripItemCommand = new MoveBeforeToolStripItemCommand()
            {
                CanExcuteHandler = () =>
                {
                    bool currentType = _currentDataType != null && _currentDataType.Equals(typeof(ToolStripItemAbstract));

                    return currentType && (_controller.SelectedItemsCount == 1 &&
                        _controller.SelectedItemIndex > 0 &&
                        _controller.GetSelectedItem<ToolStripItemAbstract>() != null);
                },
                GetMenuId = () => { return _controller.GetSelectedItem<ToolStripItemAbstract>().Id; },
                GetBeforeMenuId = () =>
                {
                    return _controller.GetItem<ToolStripItemAbstract>(
                        _controller.SelectedItemIndex - 1).Id;
                }
            };

            #endregion

            #region MoveAfterToolStripItemCommand

            _moveAfterToolStripItemCommand = new MoveAfterToolStripItemCommand()
            {
                CanExcuteHandler = () =>
                {
                    bool currentType = _currentDataType != null && _currentDataType.Equals(typeof(ToolStripItemAbstract));

                    return currentType && (_controller.SelectedItemsCount == 1 &&
                        _controller.SelectedItemIndex < _controller.ItemsCount - 1 &&
                        _controller.GetSelectedItem<ToolStripItemAbstract>() != null);
                },
                GetMenuId = () => { return _controller.GetSelectedItem<ToolStripItemAbstract>().Id; },
                GetAfterMenuId = () =>
                {
                    return _controller.GetItem<ToolStripItemAbstract>(
                        _controller.SelectedItemIndex + 1).Id;
                }
            };

            #endregion

            #endregion

            #endregion
        }

        private void InitContextMenu()
        {
            #region MenuStrip

            _gridViewMenuMenuEntity = new GridMenuMenuEntity();
            _gridViewMenuMenuEntity.AddCommand = _addMenuEntityCommand;
            _gridViewMenuMenuEntity.EditCommand = _editMenuEntityCommand;
            _gridViewMenuMenuEntity.DeleteCommand = _deleteMenuEntityCommand;
            _gridViewMenuMenuEntity.MooveUpCommand = _moveBeforeMenuEntityCommand;
            _gridViewMenuMenuEntity.MoveDownCommand = _moveAfterMenuEntityCommand;

            #endregion

            #region ToolStrip

            _gridMenuToolStripPage = new GridMenuToolStripPage();
            _gridMenuToolStripPage.AddCommand = _addToolStripPageCommand;
            _gridMenuToolStripPage.EditCommand = _editToolStripPageCommand;
            _gridMenuToolStripPage.DeleteCommand = _removeToolStripPageCommand;
            _gridMenuToolStripPage.MoveBeforeCommand = _moveBeforeToolStripPageCommand;
            _gridMenuToolStripPage.MoveAfterCommand = _moveAfterToolStripPageCommand;

            _gridMenuToolStripGroup = new GridMenuToolStripGroup();
            _gridMenuToolStripGroup.AddCommand = _addToolStripGroupCommand;
            _gridMenuToolStripGroup.EditCommand = _editToolStripGroupCommand;
            _gridMenuToolStripGroup.DeleteCommand = _removeToolStripGroupCommand;
            _gridMenuToolStripGroup.MoveBeforeCommand = _moveBeforeToolStripGroupCommand;
            _gridMenuToolStripGroup.MoveAfterCommand = _moveAfterToolStripGroupCommand;

            _gridMenuToolStripItem = new GridMenuToolStripItem();
            _gridMenuToolStripItem.AddCommand = _addToolStripItemCommand;
            _gridMenuToolStripItem.EditCommand = _editToolStripItemCommand;
            _gridMenuToolStripItem.DeleteCommand = _deleteToolStripItemCommand;
            _gridMenuToolStripItem.MoveBeforeCommand = _moveBeforeToolStripItemCommand;
            _gridMenuToolStripItem.MoveAfterCommand = _moveAfterToolStripItemCommand;

            #endregion
        }

        private void SubscribeEvent()
        {
            //订阅增删改事件，以更新呈现

            #region MenuStrip

            #region Add,Remove,Update

            SubscriptionToken menuItemAddedEvent =
                _eventAggregator.GetEvent<MenuStripItemAddedEvent>().Subscribe((e) => { _controller.Add(e.Entity); });
            SubscriptionToken menuItemRemovedEvent =
                _eventAggregator.GetEvent<MenuStripItemRemovedEvent>().Subscribe((e) => { _controller.Remove(e.Entity); });
            SubscriptionToken menuItemUpdatedEvent =
                _eventAggregator.GetEvent<MenuStripItemUpdatedEvent>().Subscribe((e) => { _controller.Update(e.Entity); });

            #endregion

            #region MainMenuMoveBeforeEvent

            SubscriptionToken menuItemMoveUpEvent =
               _eventAggregator.GetEvent<MenuStripItemMoveBeforeEvent>().Subscribe((e) =>
               {
                   _controller.MoveBefore<MenuEntity>(
                       (moveSource) => { return moveSource.Id == e.Id; },
                       (moveTarget) => { return moveTarget.Id == e.BeforeId; }
                       );
                   _controller.Select<MenuEntity>(
                       (obj) => { return obj.Id == e.Id; }
                       );
               });

            #endregion

            #region MainMenuMoveAfterEvent

            SubscriptionToken menuItemMoveDownEvent =
              _eventAggregator.GetEvent<MenuStripItemMoveAfterEvent>().Subscribe((e) =>
              {
                  _controller.MoveAfter<MenuEntity>(
                      (moveSource) => { return moveSource.Id == e.Id; },
                      (moveTarget) => { return moveTarget.Id == e.AfterId; }
                      );
                  _controller.Select<MenuEntity>(
                       (obj) => { return obj.Id == e.Id; }
                       );
              });

            #endregion

            #endregion

            #region ToolStrip

            #region Page

            #region Add,Remove,Update

            SubscriptionToken toolStripPageAddedEventToken =
                _eventAggregator.GetEvent<ToolStripPageAddedEvent>().Subscribe((e) => { _controller.Add(e.Entity); });
            SubscriptionToken toolStripPageRemovedEventToken =
                _eventAggregator.GetEvent<ToolStripPageRemovedEvent>().Subscribe((e) => { _controller.Remove(e.Entity); });
            SubscriptionToken toolStripPageUpdatedEventToken =
                _eventAggregator.GetEvent<ToolStripPageUpdatedEvent>().Subscribe((e) => { _controller.Update(e.Entity); });

            #endregion

            #region ToolStripItemMoveBeforeEvent

            SubscriptionToken toolStripPageMoveBeforeEventToken =
               _eventAggregator.GetEvent<ToolStripPageMoveBeforeEvent>().Subscribe((e) =>
               {
                   _controller.MoveBefore<ToolStripPageEntity>(
                       (moveSource) => { return moveSource.Id == e.Id; },
                       (moveTarget) => { return moveTarget.Id == e.BeforeId; }
                       );
                   _controller.Select<ToolStripPageEntity>(
                       (obj) => { return obj.Id == e.Id; }
                       );
               });

            #endregion

            #region ToolStripItemMoveAfterEvent

            SubscriptionToken toolStripPageMoveAfterEventToken =
              _eventAggregator.GetEvent<ToolStripPageMoveAfterEvent>().Subscribe((e) =>
              {
                  _controller.MoveAfter<ToolStripPageEntity>(
                      (moveSource) => { return moveSource.Id == e.Id; },
                      (moveTarget) => { return moveTarget.Id == e.AfterId; }
                      );
                  _controller.Select<ToolStripPageEntity>(
                       (obj) => { return obj.Id == e.Id; }
                       );
              });

            #endregion

            #endregion

            #region Group

            #region Add,Remove,Update

            SubscriptionToken toolStripGroupAddedEventToken =
                _eventAggregator.GetEvent<ToolStripGroupAddedEvent>().Subscribe((e) => { _controller.Add(e.Entity); });
            SubscriptionToken toolStripGroupRemovedEventToken =
                _eventAggregator.GetEvent<ToolStripGroupRemovedEvent>().Subscribe((e) => { _controller.Remove(e.Entity); });
            SubscriptionToken toolStripGroupUpdatedEventToken =
                _eventAggregator.GetEvent<ToolStripGroupUpdatedEvent>().Subscribe((e) => { _controller.Update(e.Entity); });

            #endregion

            #region ToolStripItemMoveBeforeEvent

            SubscriptionToken toolStripGroupMoveBeforeEventToken =
               _eventAggregator.GetEvent<ToolStripGroupMoveBeforeEvent>().Subscribe((e) =>
               {
                   _controller.MoveBefore<ToolStripGroupEntity>(
                       (moveSource) => { return moveSource.Id == e.Id; },
                       (moveTarget) => { return moveTarget.Id == e.BeforeId; }
                       );
                   _controller.Select<ToolStripGroupEntity>(
                       (obj) => { return obj.Id == e.Id; }
                       );
               });

            #endregion

            #region ToolStripItemMoveAfterEvent

            SubscriptionToken toolStripGroupMoveAfterEventToken =
              _eventAggregator.GetEvent<ToolStripGroupMoveAfterEvent>().Subscribe((e) =>
              {
                  _controller.MoveAfter<ToolStripGroupEntity>(
                      (moveSource) => { return moveSource.Id == e.Id; },
                      (moveTarget) => { return moveTarget.Id == e.AfterId; }
                      );
                  _controller.Select<ToolStripGroupEntity>(
                       (obj) => { return obj.Id == e.Id; }
                       );
              });

            #endregion

            #endregion

            #region Items

            #region Add,Remove,Update

            SubscriptionToken toolStripItemAddedEvent =
                _eventAggregator.GetEvent<ToolStripItemAddedEvent>().Subscribe((e) => { _controller.Add(e.Entity); });
            SubscriptionToken toolStripItemRemovedEvent =
                _eventAggregator.GetEvent<ToolStripItemRemovedEvent>().Subscribe((e) => { _controller.Remove(e.Entity); });
            SubscriptionToken toolStripItemUpdatedEvent =
                _eventAggregator.GetEvent<ToolStripItemUpdatedEvent>().Subscribe((e) => { _controller.Update(e.Entity); });

            #endregion

            #region ToolStripItemMoveBeforeEvent

            SubscriptionToken toolStripItemMoveBeforeEvent =
               _eventAggregator.GetEvent<ToolStripItemMoveBeforeEvent>().Subscribe((e) =>
               {
                   _controller.MoveBefore<ToolStripItemAbstract>(
                       (moveSource) => { return moveSource.Id == e.Id; },
                       (moveTarget) => { return moveTarget.Id == e.BeforeId; }
                       );
                   _controller.Select<ToolStripItemAbstract>(
                       (obj) => { return obj.Id == e.Id; }
                       );
               });

            #endregion

            #region ToolStripItemMoveAfterEvent

            SubscriptionToken toolStripItemMoveAfterEvent =
              _eventAggregator.GetEvent<ToolStripItemMoveAfterEvent>().Subscribe((e) =>
              {
                  _controller.MoveAfter<ToolStripItemAbstract>(
                      (moveSource) => { return moveSource.Id == e.Id; },
                      (moveTarget) => { return moveTarget.Id == e.AfterId; }
                      );
                  _controller.Select<ToolStripItemAbstract>(
                       (obj) => { return obj.Id == e.Id; }
                       );
              });

            #endregion

            #endregion

            #endregion

            #region * Unsubscribe *

            this.Disposed += (sender, e) =>
            {
                _eventAggregator.GetEvent<MenuStripItemAddedEvent>().Unsubscribe(menuItemAddedEvent);
                _eventAggregator.GetEvent<MenuStripItemRemovedEvent>().Unsubscribe(menuItemRemovedEvent);
                _eventAggregator.GetEvent<MenuStripItemUpdatedEvent>().Unsubscribe(menuItemUpdatedEvent);
                _eventAggregator.GetEvent<MenuStripItemMoveBeforeEvent>().Unsubscribe(menuItemMoveUpEvent);
                _eventAggregator.GetEvent<MenuStripItemMoveAfterEvent>().Unsubscribe(menuItemMoveDownEvent);

                _eventAggregator.GetEvent<ToolStripPageAddedEvent>().Unsubscribe(toolStripPageAddedEventToken);
                _eventAggregator.GetEvent<ToolStripPageUpdatedEvent>().Unsubscribe(toolStripPageUpdatedEventToken);
                _eventAggregator.GetEvent<ToolStripPageRemovedEvent>().Unsubscribe(toolStripPageRemovedEventToken);
                _eventAggregator.GetEvent<ToolStripPageMoveBeforeEvent>().Unsubscribe(toolStripPageMoveBeforeEventToken);
                _eventAggregator.GetEvent<ToolStripPageMoveAfterEvent>().Unsubscribe(toolStripPageMoveAfterEventToken);

                _eventAggregator.GetEvent<ToolStripGroupAddedEvent>().Unsubscribe(toolStripGroupAddedEventToken);
                _eventAggregator.GetEvent<ToolStripGroupUpdatedEvent>().Unsubscribe(toolStripGroupUpdatedEventToken);
                _eventAggregator.GetEvent<ToolStripGroupRemovedEvent>().Unsubscribe(toolStripGroupRemovedEventToken);
                _eventAggregator.GetEvent<ToolStripGroupMoveBeforeEvent>().Unsubscribe(toolStripGroupMoveBeforeEventToken);
                _eventAggregator.GetEvent<ToolStripGroupMoveAfterEvent>().Unsubscribe(toolStripGroupMoveAfterEventToken);

                _eventAggregator.GetEvent<ToolStripItemAddedEvent>().Unsubscribe(toolStripItemAddedEvent);
                _eventAggregator.GetEvent<ToolStripItemRemovedEvent>().Unsubscribe(toolStripItemRemovedEvent);
                _eventAggregator.GetEvent<ToolStripItemUpdatedEvent>().Unsubscribe(toolStripItemUpdatedEvent);
                _eventAggregator.GetEvent<ToolStripItemMoveBeforeEvent>().Unsubscribe(toolStripItemMoveBeforeEvent);
                _eventAggregator.GetEvent<ToolStripItemMoveAfterEvent>().Unsubscribe(toolStripItemMoveAfterEvent);

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
            _contextData = null;
            _currentDataType = null;
        }

        /// <summary>
        /// 绑定数据
        /// currentType 表示当前列表中显示的数据的类型，这个不能从contextData上取，因为contextData可能是个ToolStripItemFolder之类
        /// listType是当前显示的list中的数据的类型
        /// contextData是list所属对象
        /// </summary>
        /// <param name="list"></param>
        /// <param name="type"></param>
        /// <param name="contextData"></param>
        public void DataBind(IList list, Type listDataType, object contextData)
        {
            _currentDataType = listDataType;
            _contextData = contextData;
            _controller.DataBind(list, listDataType, contextData);
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
