using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Practices.Composite.Events;
using Sheng.SailingEase.ComponentModel;
using Sheng.SailingEase.Components.WindowComponent.Localisation;
using Sheng.SailingEase.Controls.Extensions;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Kernal;
using Sheng.SailingEase.Localisation;

namespace Sheng.SailingEase.Components.WindowComponent.View
{
    partial class ExplorerTreeView : PadViewBase
    {
        #region 私有成员

        private IEventAggregator _eventAggregator = ServiceUnity.EventAggregator;

        #region 菜单

        TreeMenuFolder _treeMenuFolder;
        TreeMenuWindowEntity _treeMenuWindowEntity;

        #endregion

        #region Commands

        AddFolderCommand _addFolderCommand;
        EditFolderCommand _editFolderCommand;
        DeleteFolderCommand _deleteFolderCommand;

        AddWindowCommand _addWindowCommand;
        EditWindowCommand _editWindowCommand;
        DeleteWindowCommand _deleteWindowCommand;

        #endregion

        private WindowArchive _windowArchive = WindowArchive.Instance;

        #endregion

        #region 公开属性

        private TypeBinderTreeViewController _navigationTreeController;
        public TypeBinderTreeViewController NavigationTreeController
        {
            get { return _navigationTreeController; }
        }

        #endregion

        #region 构造和窗体事件

        public ExplorerTreeView()
        {
            InitializeComponent();

            this.TabText = Language.Current.NavigationTreeView;

            //初始化命令
            InitCommands();

            //初始化菜单
            InitContextMenu();

            //初始化 TreeView 控制器
            InitController();

            SubscribeEvent();
        }

        private void FormNavigationTree_Load(object sender, EventArgs e)
        {
            LoadTree();
        }

        #endregion

        #region 私有方法

        private void InitCommands()
        {
            #region Commands

            _addFolderCommand = new AddFolderCommand()
            {
                GetArgumentHandler = () =>
                {
                    FolderEntityIndex folderEntityIndex = 
                        _navigationTreeController.GetSelectedData<FolderEntityIndex>(true);
                    return folderEntityIndex.Folder;
                }
            };
            _editFolderCommand = new EditFolderCommand()
            {
                CanExcuteHandler = () =>
                {
                    return _navigationTreeController.GetSelectedData<FolderEntityIndex>() != null;
                },
                GetArgumentHandler = () =>
                {
                    FolderEntityIndex folderEntityIndex =
                         _navigationTreeController.GetSelectedData<FolderEntityIndex>(true);
                    return folderEntityIndex.Folder;
                }
            };
            _deleteFolderCommand = new DeleteFolderCommand()
            {
                CanExcuteHandler = () =>
                {
                    return _navigationTreeController.GetSelectedData<FolderEntityIndex>() != null;
                },
                GetArgumentHandler = () =>
                {
                    FolderEntityIndex folderEntityIndex =
                        _navigationTreeController.GetSelectedData<FolderEntityIndex>(true);
                    return new List<WindowFolderEntity>() { folderEntityIndex.Folder };
                }
            };
          
            _addWindowCommand = new  AddWindowCommand()
            {
                CanExcuteHandler = () =>
                {
                    return _navigationTreeController.GetSelectedData<FolderEntityIndex>(true) != null;
                },
                GetArgumentHandler = () =>
                {
                    FolderEntityIndex folderEntityIndex =
                        _navigationTreeController.GetSelectedData<FolderEntityIndex>(true);
                    return folderEntityIndex.Folder;
                }
            };
            _editWindowCommand = new EditWindowCommand()
            {
                CanExcuteHandler = () =>
                {
                    return _navigationTreeController.GetSelectedData<WindowEntityIndex>() != null;
                },
                GetArgumentHandler = () =>
                {
                    WindowEntityIndex windowEntityIndex =
                       _navigationTreeController.GetSelectedData<WindowEntityIndex>();
                    return windowEntityIndex.Window;
                }
            };
            _deleteWindowCommand = new DeleteWindowCommand()
            {
                CanExcuteHandler = () =>
                {
                    return _navigationTreeController.GetSelectedData<WindowEntityIndex>() != null;
                },
                GetArgumentHandler = () =>
                {
                    WindowEntityIndex windowEntityIndex =
                       _navigationTreeController.GetSelectedData<WindowEntityIndex>();
                    return new List<WindowEntity>() { windowEntityIndex.Window };
                }
            };

            #endregion
        }

        private void InitContextMenu()
        {
            #region TreeMenuDataEntityFolder

            _treeMenuFolder = new TreeMenuFolder();
            _treeMenuFolder.AddCommand = _addFolderCommand;
            _treeMenuFolder.EditCommand = _editFolderCommand;
            _treeMenuFolder.DeleteCommand = _deleteFolderCommand;
            _treeMenuFolder.AddWindowCommand = _addWindowCommand;

            #endregion

            #region TreeMenuDataEntity

            _treeMenuWindowEntity = new TreeMenuWindowEntity();
            _treeMenuWindowEntity.AddCommand = _addWindowCommand;
            _treeMenuWindowEntity.EditCommand = _editWindowCommand ; 
            _treeMenuWindowEntity.DeleteCommand = _deleteWindowCommand;

            #endregion
            
        }

        private void InitController()
        {
            _navigationTreeController = new TypeBinderTreeViewController(this.navigationTree);

            #region ImageList

            //节点的小图标ImageList
            ImageList _imageList = new ImageList();
            _imageList.Images.Add(IconsLibrary.Folder);  //0
            _imageList.Images.Add(IconsLibrary.Form);  //1
            _navigationTreeController.ImageList = _imageList;

            #endregion

            #region Codon

           

            _navigationTreeController.AddNodeCodon(new TypeBinderTreeViewNodeCodon(
               typeof(FolderEntityIndex), typeof(WindowEntityIndex),
               FolderEntityIndex.Property_Name, FolderEntityIndex.Property_Items)
                {
                    ImageIndex = TreeImages.Folder,
                    ContextMenuStrip = _treeMenuFolder.View
                    //GetItemsFunc = (folderEntity) =>
                    //{
                    //    return WindowArchive.Instance.GetIndexList(((FolderEntityIndex)folderEntity).Id);
                    //}
                });

            _navigationTreeController.AddNodeCodon(new TypeBinderTreeViewNodeCodon(
                typeof(WindowEntityIndex), WindowEntityIndex.Property_Name)
                {
                    ImageIndex = TreeImages.Form,
                    ContextMenuStrip = _treeMenuWindowEntity.View
                });

            #endregion

            _navigationTreeController.AfterSelect +=
                new TypeBinderTreeViewController.OnAfterSelectHandler(_navigationTreeController_AfterSelect);

            #region 处理拖放

            navigationTree.AllowDrop = true;

            navigationTree.CanDropFunc = (dragNode, dropNode) =>
                {
                    //只有在放置节点为目录时，才允许放置
                    ITypeBinderTreeViewNode binderDropNode = (ITypeBinderTreeViewNode)dropNode;
                    if (binderDropNode.Codon.DataBoundType == typeof(FolderEntityIndex))
                        return true;
                    else
                        return false;
                };

            navigationTree.DragDropAction = (dragNode, dropNode) =>
                {
                    ITypeBinderTreeViewNode binderDragNode = (ITypeBinderTreeViewNode)dragNode;
                    ITypeBinderTreeViewNode binderDropNode = (ITypeBinderTreeViewNode)dropNode;
                    FolderEntityIndex dropFolder = (FolderEntityIndex)binderDropNode.DataBoundItem;

                    if (binderDragNode.Codon.DataBoundType == typeof(FolderEntityIndex))
                    {
                        FolderEntityIndex dragFolder = (FolderEntityIndex)binderDragNode.DataBoundItem;
                        _windowArchive.MoveFolder(dragFolder.Id, dropFolder.Id);
                    }
                    else if (binderDragNode.Codon.DataBoundType == typeof(WindowEntityIndex))
                    {
                        WindowEntityIndex dragWindow = (WindowEntityIndex)binderDragNode.DataBoundItem;
                        _windowArchive.MoveWindow(dragWindow.Id, dropFolder.Id);
                    }
                    else
                    {
                        Debug.Assert(false, "未处理的节点类型");
                    }

                    binderDragNode.Parent.Items.Remove(binderDragNode.DataBoundItem);
                    binderDropNode.Items.Add(binderDragNode.DataBoundItem);

                    navigationTree.MoveNode(dragNode, dropNode);
                };

            #endregion
        }

        private void LoadTree()
        {
            FolderEntityIndex rootFolder = new FolderEntityIndex(new WindowFolderEntity()
                {
                    Id = String.Empty,
                    Name = Language.Current.ExplorerView_RootFolder
                });

            rootFolder.Items = _windowArchive.GetIndexList();

            _navigationTreeController.DataBind(rootFolder);
        }

        private void SubscribeEvent()
        {
            //订阅数据实体的增删改事件，以更新呈现

            SubscriptionToken windowFolderAddedEventToken =
                _eventAggregator.GetEvent<WindowFolderAddedEvent>().Subscribe((e) =>
                {
                    _navigationTreeController.Add(new FolderEntityIndex(e.Folder), (node) =>
                    {
                        if (node.Codon.DataBoundType == typeof(FolderEntityIndex))
                        {
                            FolderEntityIndex folderEntityIndex = (FolderEntityIndex)node.DataBoundItem;
                            return folderEntityIndex.Id == e.Folder.Parent;
                        }
                        else return false;
                    });

                    //发出添加了文件夹或窗体的事件通知
                    OnNodeAdded();
                });
            SubscriptionToken windowFolderRemovedEventToken =
                _eventAggregator.GetEvent<WindowFolderRemovedEvent>().Subscribe((e) =>
                {
                    _navigationTreeController.Remove(
                        (node) =>
                        {
                            if (node.Codon.DataBoundType == typeof(FolderEntityIndex))
                            {
                                FolderEntityIndex folderEntityIndex = (FolderEntityIndex)node.DataBoundItem;
                                return folderEntityIndex.Id == e.Folder.Id;
                            }
                            else return false;
                        });
                });
            SubscriptionToken windowFolderUpdatedEventToken =
                _eventAggregator.GetEvent<WindowFolderUpdatedEvent>().Subscribe((e) =>
                {
                    _navigationTreeController.Update((node) =>
                    {
                        if (node.Codon.DataBoundType == typeof(FolderEntityIndex))
                        {
                            FolderEntityIndex folderEntityIndex = (FolderEntityIndex)node.DataBoundItem;
                            return folderEntityIndex.Id == e.Folder.Id;
                        }
                        else return false;
                    });
                });

            SubscriptionToken windowAddedEventToken =
                _eventAggregator.GetEvent<WindowAddedEvent>().Subscribe((e) =>
                {
                    _navigationTreeController.Add(new WindowEntityIndex(e.Entity),
                        (node) =>
                        {
                            if (node.Codon.DataBoundType == typeof(FolderEntityIndex))
                            {
                                FolderEntityIndex folderEntityIndex = (FolderEntityIndex)node.DataBoundItem;
                                return folderEntityIndex.Id == e.Entity.FolderId;
                            }
                            else return false;
                        });

                    //发出添加了文件夹或窗体的事件通知
                    OnNodeAdded();
                });
            SubscriptionToken windowRemovedEventToken =
                _eventAggregator.GetEvent<WindowRemovedEvent>().Subscribe((e) =>
                {
                    _navigationTreeController.Remove(
                        (node) =>
                        {
                            if (node.Codon.DataBoundType == typeof(WindowEntityIndex))
                            {
                                WindowEntityIndex windowEntityIndex = (WindowEntityIndex)node.DataBoundItem;
                                return windowEntityIndex.Id == e.Id;
                            }
                            else return false;
                        });
                });
            SubscriptionToken windowUpdatedEventToken =
                _eventAggregator.GetEvent<WindowUpdatedEvent>().Subscribe((e) =>
                {
                    _navigationTreeController.Update((node) =>
                    {
                        if (node.Codon.DataBoundType == typeof(WindowEntityIndex))
                        {
                            WindowEntityIndex windowEntityIndex = (WindowEntityIndex)node.DataBoundItem;
                            return windowEntityIndex.Id == e.Entity.Id;
                        }
                        else return false;
                    });
                });

            this.Disposed += (sender, e) =>
            {
                _eventAggregator.GetEvent<WindowFolderAddedEvent>().Unsubscribe(windowFolderAddedEventToken);
                _eventAggregator.GetEvent<WindowFolderRemovedEvent>().Unsubscribe(windowFolderRemovedEventToken);
                _eventAggregator.GetEvent<WindowFolderUpdatedEvent>().Unsubscribe(windowFolderUpdatedEventToken);

                _eventAggregator.GetEvent<WindowAddedEvent>().Unsubscribe(windowAddedEventToken);
                _eventAggregator.GetEvent<WindowRemovedEvent>().Unsubscribe(windowRemovedEventToken);
                _eventAggregator.GetEvent<WindowUpdatedEvent>().Unsubscribe(windowUpdatedEventToken);
            };
        }


        #endregion

        #region 公开方法

        public void Select(object obj)
        {
            _navigationTreeController.Select(obj);
        }

        public void Expand()
        {
            _navigationTreeController.Expand();
        }

        #endregion

        #region 事件处理

        private void _navigationTreeController_AfterSelect(object sender, 
            TypeBinderTreeViewController.AfterSelectEventArgs e)
        {
            if (AfterSelect != null)
            {
                AfterSelectEventArgs args = new AfterSelectEventArgs()
                {
                    DataBoundItem = e.Node.DataBoundItem,
                    DataBoundType = e.Node.Codon.DataBoundType,
                    ItemType = e.Node.ItemType,
                    Items = e.Node.Items
                };
                AfterSelect(this, args);
            }
        }

        private void OnNodeAdded()
        {
            if (NodeAdded != null)
            {
                ITypeBinderTreeViewNode node = _navigationTreeController.SelectedNode;

                NodeAddedEventArgs args = new NodeAddedEventArgs()
                {
                    DataBoundItem = node.DataBoundItem,
                    DataBoundType = node.Codon.DataBoundType,
                    ItemType = node.ItemType,
                    Items = node.Items
                };

                NodeAdded(this, args);
            }
        }

        #endregion

        #region 事件

        public delegate void OnAfterSelectHandler(object sender, AfterSelectEventArgs e);
        /// <summary>
        /// 切换树中选择的项时发生
        /// 根据AfterSelectEventArgs.TabPageType 来判断选的是哪颗树
        /// </summary>
        public event OnAfterSelectHandler AfterSelect;

        public delegate void OnNodeAddedHandler(object sender, NodeAddedEventArgs e);
        /// <summary>
        /// 在添加文件夹或窗体后发生，供 ExplorerGridContainer 刷新 DataGrid 中的数据
        /// 参见 ExplorerGridContainer 中说明
        /// </summary>
        public event OnNodeAddedHandler NodeAdded;

        #endregion

        #region AfterSelectEventArgs

        public class AfterSelectEventArgs : EventArgs
        {
            /// <summary>
            /// 所绑定的对象
            /// </summary>
            public object DataBoundItem
            {
                get;
                internal set;
            }

            /// <summary>
            /// 所对应的绑定数据类型
            /// </summary>
            public Type DataBoundType
            {
                get;
                internal set;
            }

            /// <summary>
            /// Items中的对象的类型，不是DataBoundItem 的 Type！
            /// </summary>
            public Type ItemType
            {
                get;
                internal set;
            }

            /// <summary>
            /// 此属性用于获取此节点下的数据集，用于呈现在右侧列表中
            /// </summary>
            public IList Items
            {
                get;
                internal set;
            }

            public AfterSelectEventArgs()
            {
            }
        }

        #endregion

        #region NodeAddedEventArgs

        public class NodeAddedEventArgs : EventArgs
        {
            /// <summary>
            /// 所绑定的对象
            /// </summary>
            public object DataBoundItem
            {
                get;
                internal set;
            }

            /// <summary>
            /// 所对应的绑定数据类型
            /// </summary>
            public Type DataBoundType
            {
                get;
                internal set;
            }

            /// <summary>
            /// Items中的对象的类型，不是DataBoundItem 的 Type！
            /// </summary>
            public Type ItemType
            {
                get;
                internal set;
            }

            /// <summary>
            /// 此属性用于获取此节点下的数据集，用于呈现在右侧列表中
            /// </summary>
            public IList Items
            {
                get;
                internal set;
            }

            public NodeAddedEventArgs()
            {
            }
        }

        #endregion

        #region TreeImages

        struct TreeImages
        {
            public const int Folder = 0;
            public const int Form = 1;
        }

        #endregion
    }
}
