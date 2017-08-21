using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Practices.Composite.Events;
using Sheng.SailingEase.Controls.Extensions;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Kernal;
using Sheng.SailingEase.Localisation;

namespace Sheng.SailingEase.Components.NavigationComponent.View
{
    public partial class MenuStripTreeContainer : UserControl, ITreeContainer
    {
        #region 私有成员

        private IEventAggregator _eventAggregator = ServiceUnity.Container.Resolve<IEventAggregator>();

        #region 菜单

        TreeMenuMenuEntityFolder _treeMenuMenuEntityFolder;
        TreeMenuMenuEntity _treeMenuMenuEntity;

        #endregion

        #region Commands

        private AddMenuEntityCommand _addMenuEntityCommand;
        private EditMenuEntityCommand _editMenuEntityCommand;
        private DeleteMenuEntityCommand _deleteMenuEntityCommand;
        private MoveBeforeMenuEntityCommand _moveBeforeMenuEntityCommand;
        private MoveAfterMenuEntityCommand _moveAfterMenuEntityCommand;

        #endregion

        #endregion

        #region 公开属性

        private TypeBinderTreeViewController _controller;
        public TypeBinderTreeViewController Controller
        {
            get { return _controller; }
        }

        #endregion

        #region 构造和窗体事件

        public MenuStripTreeContainer()
        {
            InitializeComponent();

            //初始化命令
            InitCommands();

            //初始化菜单
            InitContextMenu();

            //初始化 DataGridView 控制器
            InitController();

            SubscribeEvent();
        }

        private void MenuTreeContainer_Load(object sender, EventArgs e)
        {
            LoadTree();
        }

        #endregion

        #region 私有方法

        private void LoadTree()
        {
            MenuEntityFolder menuFolder =
               new MenuEntityFolder(MenuStripArchive.Instance.GetMenuEntityList());

            _controller.DataBind(menuFolder);
        }

        private void InitCommands()
        {
            #region Commands

            _addMenuEntityCommand = new AddMenuEntityCommand()
            {
                GetArgumentHandler = () =>
                {
                    MenuEntity selectedItem = _controller.GetSelectedData<MenuEntity>();
                    if (selectedItem == null)
                        return String.Empty;
                    else
                        return selectedItem.Id;
                }
            };
            _editMenuEntityCommand = new EditMenuEntityCommand()
            {
                CanExcuteHandler = () =>
                {
                    return _controller.GetSelectedData<MenuEntity>() != null;
                },
                GetArgumentHandler = () => { return _controller.GetSelectedData<MenuEntity>(); }
            };
            _deleteMenuEntityCommand = new DeleteMenuEntityCommand()
            {
                CanExcuteHandler = () =>
                {
                    return _controller.GetSelectedData<MenuEntity>() != null;
                },
                GetArgumentHandler = () =>
                {
                    return new List<MenuEntity>() { _controller.GetSelectedData<MenuEntity>() };
                }
            };
            _moveBeforeMenuEntityCommand = new MoveBeforeMenuEntityCommand()
            {
                CanExcuteHandler = () =>
                {
                    return _controller.GetSelectedData<MenuEntity>() != null &&
                        _controller.GetPreSelectedData<MenuEntity>() != null;
                },
                GetMenuId = () => { return _controller.GetSelectedData<MenuEntity>().Id; },
                GetBeforeMenuId = () => { return _controller.GetPreSelectedData<MenuEntity>().Id; }
            };

            _moveAfterMenuEntityCommand = new MoveAfterMenuEntityCommand()
            {
                CanExcuteHandler = () =>
                {
                    return _controller.GetSelectedData<MenuEntity>(true) != null &&
                        _controller.GetNextSelectedData<MenuEntity>() != null;
                },
                GetMenuId = () => { return _controller.GetSelectedData<MenuEntity>().Id; },
                GetAfterMenuId = () => { return _controller.GetNextSelectedData<MenuEntity>().Id; }
            };

            #endregion
        }

        private void InitContextMenu()
        {
            #region TreeMenuDataEntityFolder

            _treeMenuMenuEntityFolder = new TreeMenuMenuEntityFolder();
            _treeMenuMenuEntityFolder.AddCommand = _addMenuEntityCommand;

            #endregion

            #region TreeMenuDataEntity

            _treeMenuMenuEntity = new TreeMenuMenuEntity();
            _treeMenuMenuEntity.AddCommand = _addMenuEntityCommand;
            _treeMenuMenuEntity.EditCommand = _editMenuEntityCommand;
            _treeMenuMenuEntity.DeleteCommand = _deleteMenuEntityCommand;
            _treeMenuMenuEntity.MoveBeforeCommand = _moveBeforeMenuEntityCommand;
            _treeMenuMenuEntity.MoveAfterCommand = _moveAfterMenuEntityCommand;

            #endregion
        }

        private void InitController()
        {
            _controller = new TypeBinderTreeViewController(this.treeView);

            #region ImageList

            //节点的小图标ImageList
            ImageList _imageList = new ImageList();
            _imageList.Images.Add(IconsLibrary.Folder);  //0
            _imageList.Images.Add(IconsLibrary.Menu);  //1
            _controller.ImageList = _imageList;

            #endregion

            #region Codon

            _controller.AddNodeCodon(new TypeBinderTreeViewNodeCodon(
                typeof(MenuEntityFolder), typeof(MenuEntity), 
                MenuEntityFolder.Property_Text, MenuEntityFolder.Property_Items)
            {
                ImageIndex = TreeImages.Folder,
                ContextMenuStrip = _treeMenuMenuEntityFolder.View
            });

            _controller.AddNodeCodon(new TypeBinderTreeViewNodeCodon(
                typeof(MenuEntity), typeof(MenuEntity), MenuEntity.Property_Name, MenuEntity.Property_Menus)
            {
                ActOnSubClass = true,//因为实际运行是Dev结尾的类型
                ImageIndex = TreeImages.MenuEntity,
                ContextMenuStrip = _treeMenuMenuEntity.View
            });

            #endregion

            _controller.AfterSelect += new TypeBinderTreeViewController.OnAfterSelectHandler(_controller_AfterSelect);
        }

        private void SubscribeEvent()
        {
            //订阅数据实体的增删改事件，以更新呈现

            #region MainMenuAddedEvent

            SubscriptionToken menuEntityAddedEventToken =
                _eventAggregator.GetEvent<MenuStripItemAddedEvent>().Subscribe((e) =>
                {
                    _controller.Add(e.Entity,
                        (node) =>
                        {
                            if (String.IsNullOrEmpty(e.Entity.ParentId) && node.Codon.DataBoundType == typeof(MenuEntityFolder))
                                return true;
                            else
                            {
                                if (node.Codon.DataBoundType == typeof(MenuEntity))
                                {
                                    MenuEntity entity = (MenuEntity)node.DataBoundItem;
                                    return e.Entity.ParentId == entity.Id;
                                }
                                else
                                    return false;
                            }
                        });
                });

            #endregion

            #region MainMenuRemovedEvent

            SubscriptionToken menuEntityRemovedEvent =
                _eventAggregator.GetEvent<MenuStripItemRemovedEvent>().Subscribe((e) => { _controller.Remove(e.Entity); });

            #endregion

            #region MainMenuUpdatedEvent

            SubscriptionToken menuEntityUpdatedEvent =
                _eventAggregator.GetEvent<MenuStripItemUpdatedEvent>().Subscribe((e) => { _controller.Update(e.Entity); });

            #endregion

            #region MainMenuMoveBeforeEvent

            SubscriptionToken menuEntityMoveBeforeEvent =
                _eventAggregator.GetEvent<MenuStripItemMoveBeforeEvent>().Subscribe((e) =>
                {
                    _controller.MoveBefore(
                        (source) =>
                        {
                            if (source.Codon.DataBoundType == typeof(MenuEntity))
                            {
                                MenuEntity menuEntity = (MenuEntity)source.DataBoundItem;
                                return menuEntity.Id == e.Id;
                            }
                            else
                                return false;
                        },
                        (target) =>
                        {
                            if (target.Codon.DataBoundType == typeof(MenuEntity))
                            {
                                MenuEntity menuEntity = (MenuEntity)target.DataBoundItem;
                                return menuEntity.Id == e.BeforeId;
                            }
                            else
                                return false;
                        }
                        );
                    //_navigationTreeController.Select((node) =>
                    //{
                    //    if (node.Codon.DataBoundType == typeof(MenuEntity))
                    //    {
                    //        MenuEntity menuEntity = (MenuEntity)node.DataBoundItem;
                    //        return menuEntity.Id == e.Id;
                    //    }
                    //    else
                    //        return false;
                    //});
                });

            #endregion

            #region MainMenuMoveAfterEvent

            SubscriptionToken menuEntityMoveAfterEvent =
                _eventAggregator.GetEvent<MenuStripItemMoveAfterEvent>().Subscribe((e) =>
                {
                    _controller.MoveAfter(
                        (source) =>
                        {
                            if (source.Codon.DataBoundType == typeof(MenuEntity))
                            {
                                MenuEntity menuEntity = (MenuEntity)source.DataBoundItem;
                                return menuEntity.Id == e.Id;
                            }
                            else
                                return false;
                        },
                        (target) =>
                        {
                            if (target.Codon.DataBoundType == typeof(MenuEntity))
                            {
                                MenuEntity menuEntity = (MenuEntity)target.DataBoundItem;
                                return menuEntity.Id == e.AfterId;
                            }
                            else
                                return false;
                        }
                        );
                    //_navigationTreeController.Select((node) =>
                    //{
                    //    if (node.Codon.DataBoundType == typeof(MenuEntity))
                    //    {
                    //        MenuEntity menuEntity = (MenuEntity)node.DataBoundItem;
                    //        return menuEntity.Id == e.Id;
                    //    }
                    //    else
                    //        return false;
                    //});
                });

            #endregion

            #region * Unsubscribe *

            this.Disposed += (sender, e) =>
            {
                _eventAggregator.GetEvent<MenuStripItemAddedEvent>().Unsubscribe(menuEntityAddedEventToken);
                _eventAggregator.GetEvent<MenuStripItemRemovedEvent>().Unsubscribe(menuEntityRemovedEvent);
                _eventAggregator.GetEvent<MenuStripItemUpdatedEvent>().Unsubscribe(menuEntityUpdatedEvent);
                _eventAggregator.GetEvent<MenuStripItemMoveBeforeEvent>().Unsubscribe(menuEntityMoveBeforeEvent);
                _eventAggregator.GetEvent<MenuStripItemMoveAfterEvent>().Unsubscribe(menuEntityMoveAfterEvent);
            };

            #endregion
        }

        #endregion

        #region 事件处理

        void _controller_AfterSelect(object sender, TypeBinderTreeViewController.AfterSelectEventArgs e)
        {
            if (AfterSelect != null)
            {
                TreeContainerEventArgs args = new TreeContainerEventArgs(e.Node);
                AfterSelect(this, args);
            }
        }

        #endregion

        #region TreeImages

        struct TreeImages
        {
            public const int Folder = 0;
            public const int MenuEntity = 1;
        }

        #endregion

        #region ITreeContainer 成员

        public ITypeBinderTreeViewNode SelectedNode
        {
            get { return _controller.SelectedNode; }
        }

        public void Select(object obj)
        {
            _controller.Select(obj);
        }

        public void Expand()
        {
            _controller.Expand();
        }

        public event OnTreeContainerAfterSelectHandler AfterSelect;

        #endregion
    }
}
