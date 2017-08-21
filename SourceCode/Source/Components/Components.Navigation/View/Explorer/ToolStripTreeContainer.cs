using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Practices.Composite.Events;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Kernal;
using Sheng.SailingEase.Localisation;
using Sheng.SailingEase.Controls.Extensions;
using System.Diagnostics;

namespace Sheng.SailingEase.Components.NavigationComponent.View
{
    public partial class ToolStripTreeContainer : UserControl, ITreeContainer
    {
        #region 私有成员

        private IEventAggregator _eventAggregator = ServiceUnity.Container.Resolve<IEventAggregator>();

        private ToolStripArchive _toolStripArchive = ToolStripArchive.Instance;

        #region 菜单

        TreeMenuToolStripItemFolder _treeMenuToolStripItemFolder;
        TreeMenuToolStripPage _treeMenuToolStripPage;
        TreeMenuToolStripGroup _treeMenuToolStripGroup;
        TreeMenuToolStripItem _treeMenuToolStripItem;

        #endregion

        #region Commands

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

        #region 公开属性

        private TypeBinderTreeViewController _controller;
        public TypeBinderTreeViewController Controller
        {
            get { return _controller; }
        }

        #endregion

        #region 构造和窗体事件

        public ToolStripTreeContainer()
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

        private void ToolStripTreeContainer_Load(object sender, EventArgs e)
        {
            LoadTree();
        }

        #endregion

        #region 私有方法

        private void LoadTree()
        {
            ToolStripItemFolder menuFolder =
               new ToolStripItemFolder(ToolStripArchive.Instance.GetPageCollection());

            _controller.DataBind(menuFolder);
        }

        private void InitCommands()
        {
            #region Commands

            #region Page

            _addToolStripPageCommand = new AddToolStripPageCommand();

            _editToolStripPageCommand = new EditToolStripPageCommand()
            {
                GetArgumentHandler = () =>
                {
                    ToolStripPageEntity selectedItem = _controller.GetSelectedData<ToolStripPageEntity>();
                    if (selectedItem == null)
                        return null;
                    else
                        return selectedItem;
                }
            };

            _removeToolStripPageCommand = new RemoveToolStripPageCommand()
            {
                CanExcuteHandler = () =>
                {
                    return _controller.GetSelectedData<ToolStripPageEntity>() != null;
                },
                GetArgumentHandler = () =>
                {
                    return new List<ToolStripPageEntity>() { _controller.GetSelectedData<ToolStripPageEntity>() };
                }
            };

            _moveBeforeToolStripPageCommand = new MoveBeforeToolStripPageCommand()
            {
                CanExcuteHandler = () =>
                {
                    return _controller.GetSelectedData<ToolStripPageEntity>() != null &&
                        _controller.GetPreSelectedData<ToolStripPageEntity>() != null;
                },
                GetId = () => { return _controller.GetSelectedData<ToolStripPageEntity>().Id; },
                GetBeforeId = () => { return _controller.GetPreSelectedData<ToolStripPageEntity>().Id; }
            };

            _moveAfterToolStripPageCommand = new MoveAfterToolStripPageCommand()
            {
                CanExcuteHandler = () =>
                {
                    return _controller.GetSelectedData<ToolStripPageEntity>(true) != null &&
                        _controller.GetNextSelectedData<ToolStripPageEntity>() != null;
                },
                GetId = () => { return _controller.GetSelectedData<ToolStripPageEntity>().Id; },
                GetAfterId = () => { return _controller.GetNextSelectedData<ToolStripPageEntity>().Id; }
            };

            #endregion

            #region Group

            _addToolStripGroupCommand = new AddToolStripGroupCommand()
             {
                GetArgumentHandler = () =>
                {
                    ToolStripPageEntity selectedItem = _controller.GetSelectedData<ToolStripPageEntity>();
                    if (selectedItem == null)
                        return String.Empty;
                    else
                        return selectedItem.Id;
                }
            };

            _editToolStripGroupCommand = new EditToolStripGroupCommand()
            {
                CanExcuteHandler = () =>
                {
                    return _controller.GetSelectedData<ToolStripGroupEntity>() != null;
                },
                GetArgumentHandler = () => { return _controller.GetSelectedData<ToolStripGroupEntity>(); }
            };

            _removeToolStripGroupCommand = new RemoveToolStripGroupCommand()
            {
                CanExcuteHandler = () =>
                {
                    return _controller.GetSelectedData<ToolStripGroupEntity>() != null;
                },
                GetArgumentHandler = () =>
                {
                    return new List<ToolStripGroupEntity>() { _controller.GetSelectedData<ToolStripGroupEntity>() };
                }
            };

            _moveBeforeToolStripGroupCommand = new MoveBeforeToolStripGroupCommand()
            {
                CanExcuteHandler = () =>
                {
                    return _controller.GetSelectedData<ToolStripGroupEntity>() != null &&
                        _controller.GetPreSelectedData<ToolStripGroupEntity>() != null;
                },
                GetId = () => { return _controller.GetSelectedData<ToolStripGroupEntity>().Id; },
                GetBeforeId = () => { return _controller.GetPreSelectedData<ToolStripGroupEntity>().Id; }
            };

            _moveAfterToolStripGroupCommand = new MoveAfterToolStripGroupCommand()
            {
                CanExcuteHandler = () =>
                {
                    return _controller.GetSelectedData<ToolStripGroupEntity>(true) != null &&
                        _controller.GetNextSelectedData<ToolStripGroupEntity>() != null;
                },
                GetId = () => { return _controller.GetSelectedData<ToolStripGroupEntity>().Id; },
                GetAfterId = () => { return _controller.GetNextSelectedData<ToolStripGroupEntity>().Id; }
            };

            #endregion

            #region Items

            _addToolStripItemCommand = new AddToolStripItemCommand()
            {
                GetArgumentHandler = () =>
                {
                    ToolStripGroupEntity selectedItem = _controller.GetSelectedData<ToolStripGroupEntity>();
                    if (selectedItem == null)
                        return String.Empty;
                    else
                        return selectedItem.Id;
                }
            };

            _editToolStripItemCommand = new EditToolStripItemCommand()
            {
                CanExcuteHandler = () =>
                {
                    return _controller.GetSelectedData<ToolStripItemAbstract>() != null;
                },
                GetArgumentHandler = () => { return _controller.GetSelectedData<ToolStripItemAbstract>(); }
            };

            _deleteToolStripItemCommand = new DeleteToolStripItemCommand()
            {
                CanExcuteHandler = () =>
                {
                    return _controller.GetSelectedData<ToolStripItemAbstract>() != null;
                },
                GetArgumentHandler = () =>
                {
                    return new List<ToolStripItemAbstract>() { _controller.GetSelectedData<ToolStripItemAbstract>() };
                }
            };

            _moveBeforeToolStripItemCommand = new MoveBeforeToolStripItemCommand()
            {
                CanExcuteHandler = () =>
                {
                    return _controller.GetSelectedData<ToolStripItemAbstract>() != null &&
                        _controller.GetPreSelectedData<ToolStripItemAbstract>() != null;
                },
                GetMenuId = () => { return _controller.GetSelectedData<ToolStripItemAbstract>().Id; },
                GetBeforeMenuId = () => { return _controller.GetPreSelectedData<ToolStripItemAbstract>().Id; }
            };

            _moveAfterToolStripItemCommand = new MoveAfterToolStripItemCommand()
            {
                CanExcuteHandler = () =>
                {
                    return _controller.GetSelectedData<ToolStripItemAbstract>(true) != null &&
                        _controller.GetNextSelectedData<ToolStripItemAbstract>() != null;
                },
                GetMenuId = () => { return _controller.GetSelectedData<ToolStripItemAbstract>().Id; },
                GetAfterMenuId = () => { return _controller.GetNextSelectedData<ToolStripItemAbstract>().Id; }
            };

            #endregion

            #endregion
        }

        private void InitContextMenu()
        {
            #region TreeMenuToolStripItemFolder

            _treeMenuToolStripItemFolder = new TreeMenuToolStripItemFolder();
            _treeMenuToolStripItemFolder.AddCommand = _addToolStripPageCommand;

            #endregion

            #region TreeMenuToolStripPage

            _treeMenuToolStripPage = new TreeMenuToolStripPage();
            _treeMenuToolStripPage.AddCommand = _addToolStripGroupCommand; //添加分组
            _treeMenuToolStripPage.EditCommand = _editToolStripPageCommand;
            _treeMenuToolStripPage.DeleteCommand = _removeToolStripPageCommand;
            _treeMenuToolStripPage.MoveBeforeCommand = _moveBeforeToolStripPageCommand;
            _treeMenuToolStripPage.MoveAfterCommand = _moveAfterToolStripPageCommand;

            #endregion

            #region TreeMenuToolStripGroup

            _treeMenuToolStripGroup = new TreeMenuToolStripGroup();
            _treeMenuToolStripGroup.AddCommand = _addToolStripItemCommand; //添加项
            _treeMenuToolStripGroup.EditCommand = _editToolStripGroupCommand;
            _treeMenuToolStripGroup.DeleteCommand = _removeToolStripGroupCommand;
            _treeMenuToolStripGroup.MoveBeforeCommand = _moveBeforeToolStripGroupCommand;
            _treeMenuToolStripGroup.MoveAfterCommand = _moveAfterToolStripGroupCommand;

            #endregion

            #region TreeMenuToolStripItem

            _treeMenuToolStripItem = new TreeMenuToolStripItem();
            _treeMenuToolStripItem.AddCommand = _addToolStripItemCommand;
            _treeMenuToolStripItem.EditCommand = _editToolStripItemCommand;
            _treeMenuToolStripItem.DeleteCommand = _deleteToolStripItemCommand;
            _treeMenuToolStripItem.MoveBeforeCommand = _moveBeforeToolStripItemCommand;
            _treeMenuToolStripItem.MoveAfterCommand = _moveAfterToolStripItemCommand;

            #endregion
        }

        private void InitController()
        {
            _controller = new TypeBinderTreeViewController(this.treeView);

            #region ImageList

            //节点的小图标ImageList
            ImageList _imageList = new ImageList();
            _imageList.Images.Add(IconsLibrary.Folder);  //0
            _imageList.Images.Add(IconsLibrary.ActiveTabItem);  //1
            _imageList.Images.Add(IconsLibrary.GroupItem); //2
            _imageList.Images.Add(IconsLibrary.Menu);  //3
            _controller.ImageList = _imageList;

            #endregion

            #region Codon

            _controller.AddNodeCodon(new TypeBinderTreeViewNodeCodon(
               typeof(ToolStripItemFolder), typeof(ToolStripPageEntity),
               ToolStripItemFolder.Property_Text, ToolStripItemFolder.Property_Items)
            {
                ImageIndex = TreeImages.Folder,
                ContextMenuStrip = _treeMenuToolStripItemFolder.View
            });

            TypeBinderTreeViewNodeCodon pageCodon = new TypeBinderTreeViewNodeCodon(
                typeof(ToolStripPageEntity), typeof(ToolStripGroupEntity),
                ToolStripItemAbstract.Property_Name)
                {
                    ImageIndex = TreeImages.ToolStripPageEntity,
                    ActOnSubClass = true,
                    ContextMenuStrip = _treeMenuToolStripPage.View,
                    RelationChildMember = ToolStripPageEntity.Property_Id
                };
            pageCodon.GetItemsFunc = (pageEntity) =>
            {
                return ToolStripArchive.Instance.GetGroupCollection(((ToolStripPageEntity)pageEntity).Id);
            };
            _controller.AddNodeCodon(pageCodon);

            TypeBinderTreeViewNodeCodon groupCodon = new TypeBinderTreeViewNodeCodon(
                typeof(ToolStripGroupEntity), typeof(ToolStripItemAbstract),
                ToolStripItemAbstract.Property_Name)
                {
                    ImageIndex = TreeImages.ToolStripGroupEntity,
                    ActOnSubClass = true,
                    ContextMenuStrip = _treeMenuToolStripGroup.View,
                    RelationParentMember = ToolStripGroupEntity.Property_PageId,
                    RelationChildMember = ToolStripGroupEntity.Property_Id
                };
            groupCodon.GetItemsFunc = (groupEntity) =>
            {
                return ToolStripArchive.Instance.GetEntityList(((ToolStripGroupEntity)groupEntity).Id);
            };
            _controller.AddNodeCodon(groupCodon);

            _controller.AddNodeCodon(new TypeBinderTreeViewNodeCodon(
                typeof(ToolStripItemAbstract), ToolStripItemAbstract.Property_Name)
            {
                ActOnSubClass = true,//因为实际运行是Dev结尾的类型
                ImageIndex = TreeImages.ToolStripItemAbstract,
                ContextMenuStrip = _treeMenuToolStripItem.View,
                RelationParentMember = ToolStripItemAbstract.Property_GroupId
            });

            #endregion

            _controller.AfterSelect += new TypeBinderTreeViewController.OnAfterSelectHandler(_controller_AfterSelect);

            #region 处理拖放

            treeView.AllowDrop = true;

            #region CanDragFunc

            treeView.CanDragFunc = (dragNode) =>
            {
                //总文件夹不允许拖动
                ITypeBinderTreeViewNode binderDragNode = (ITypeBinderTreeViewNode)dragNode;
                if (binderDragNode.Codon.DataBoundType == typeof(ToolStripItemFolder))
                    return false;
                else
                    return true;
            };

            #endregion

            #region CanDropFunc

            treeView.CanDropFunc = (dragNode, dropNode) =>
            {
                ITypeBinderTreeViewNode binderDragNode = (ITypeBinderTreeViewNode)dragNode;
                ITypeBinderTreeViewNode binderDropNode = (ITypeBinderTreeViewNode)dropNode;

                //如果拖的是页，那只允许放在页上
                if (binderDragNode.Codon.DataBoundType == typeof(ToolStripPageEntity))
                {
                    if (binderDropNode.Codon.DataBoundType == typeof(ToolStripPageEntity))
                        return true;
                }

                //如果拖的是分组，允许放在页和分组上
                if (binderDragNode.Codon.DataBoundType == typeof(ToolStripGroupEntity))
                {
                    if (binderDropNode.Codon.DataBoundType == typeof(ToolStripPageEntity) ||
                        binderDropNode.Codon.DataBoundType == typeof(ToolStripGroupEntity))
                        return true;
                }

                //如果拖的是项，允许放在分组和项上
                if (binderDragNode.Codon.DataBoundType == typeof(ToolStripItemAbstract))
                {
                    if (binderDropNode.Codon.DataBoundType == typeof(ToolStripGroupEntity) ||
                        binderDropNode.Codon.DataBoundType == typeof(ToolStripItemAbstract))
                        return true;
                }

                return false;
            };

            #endregion

            #region DragDropAction

            treeView.DragDropAction = (dragNode, dropNode) =>
                {
                    ITypeBinderTreeViewNode binderDragNode = (ITypeBinderTreeViewNode)dragNode;
                    ITypeBinderTreeViewNode binderDropNode = (ITypeBinderTreeViewNode)dropNode;

                    #region 如果拖的是页

                    //如果拖的是页，那只允许放在页上
                    //移动拖动的页到放置的页后面
                    if (binderDragNode.Codon.DataBoundType == typeof(ToolStripPageEntity))
                    {
                        if (binderDropNode.Codon.DataBoundType == typeof(ToolStripPageEntity))
                        {
                            //只要调用 _toolStripArchive.MovePageAfter 就行了
                            //树节点的移动会由相关事件引发
                            ToolStripPageEntity dragPage = (ToolStripPageEntity)binderDragNode.DataBoundItem;
                            ToolStripPageEntity dropPage = (ToolStripPageEntity)binderDropNode.DataBoundItem;
                            _toolStripArchive.MovePageAfter(dragPage.Id, dropPage.Id);
                        }
                    }

                    #endregion

                    #region 如果拖的是分组

                    //如果拖的是分组，允许放在页和分组上
                    if (binderDragNode.Codon.DataBoundType == typeof(ToolStripGroupEntity))
                    {
                        ToolStripGroupEntity dragGroup = (ToolStripGroupEntity)binderDragNode.DataBoundItem;

                        //放在放置页的最后面
                        if (binderDropNode.Codon.DataBoundType == typeof(ToolStripPageEntity))
                        {
                            ToolStripPageEntity dropPage = (ToolStripPageEntity)binderDropNode.DataBoundItem;
                            dragGroup.PageId = dropPage.Id;

                            //确保拖的分组在放的页的最后一项
                            if (binderDropNode.Items.Count > 0)
                            {
                                ToolStripGroupEntity lastGroup = (ToolStripGroupEntity)binderDropNode.Items[binderDropNode.Items.Count - 1];
                                _toolStripArchive.MoveGroupAfter(dragGroup.Id, lastGroup.Id);
                            }

                            _toolStripArchive.UpdateGroup(dragGroup);

                        }
                        //放在放置分组的后面，同时注意修改拖动分组所属页到放置分组所属的页上
                        else if (binderDropNode.Codon.DataBoundType == typeof(ToolStripGroupEntity))
                        {
                            ToolStripGroupEntity dropGroup = (ToolStripGroupEntity)binderDropNode.DataBoundItem;
                            _toolStripArchive.MoveGroupAfter(dragGroup.Id, dropGroup.Id);

                            if (dragGroup.PageId != dropGroup.PageId)
                            {
                                dragGroup.PageId = dropGroup.PageId;
                                _toolStripArchive.UpdateGroup(dragGroup);
                            }
                        }
                    }

                    #endregion

                    #region 如果拖的是项

                    //如果拖的是项，允许放在分组和项上
                    if (binderDragNode.Codon.DataBoundType == typeof(ToolStripItemAbstract))
                    {
                        ToolStripItemAbstract dragItem = (ToolStripItemAbstract)binderDragNode.DataBoundItem;

                        //放在放置分组的最后面，同时修改拖动项所属分组
                        //放置分组肯定不是项原来的分组，因为如果把树子节点拖到自己的父节点上什么也不会发生，SETreeView里不允许这种操作
                        if (binderDropNode.Codon.DataBoundType == typeof(ToolStripGroupEntity))
                        {
                            ToolStripGroupEntity dropGroup = (ToolStripGroupEntity)binderDropNode.DataBoundItem;
                            dragItem.GroupId = dropGroup.Id;

                            //确保拖的分组在放的页的最后一项
                            if (binderDropNode.Items.Count > 0)
                            {
                                ToolStripItemAbstract lastItem = (ToolStripItemAbstract)binderDropNode.Items[binderDropNode.Items.Count - 1];
                                _toolStripArchive.MoveAfter(dragItem.Id, lastItem.Id);
                            }

                            _toolStripArchive.Update(dragItem);
                        }
                        //放在放置项的后面，同时修改拖动项所属分组为放置项所属的分组，如果他们不在一个分组
                        if (binderDropNode.Codon.DataBoundType == typeof(ToolStripItemAbstract))
                        {
                            ToolStripItemAbstract dropItem = (ToolStripItemAbstract)binderDropNode.DataBoundItem;
                            _toolStripArchive.MoveAfter(dragItem.Id, dropItem.Id);

                            if (dragItem.GroupId != dropItem.GroupId)
                            {
                                dragItem.GroupId = dropItem.GroupId;
                                _toolStripArchive.Update(dragItem);
                            }
                        }
                    }

                    #endregion
                };

            #endregion

            #endregion
        }

        private void SubscribeEvent()
        {
            //订阅数据实体的增删改事件，以更新呈现

            #region Page

            #region ToolStripPageAddedEvent

            SubscriptionToken toolStripPageAddedEventToken =
               _eventAggregator.GetEvent<ToolStripPageAddedEvent>().Subscribe((e) =>
               {
                   _controller.Add(e.Entity,
                       (node) =>
                       {
                           if (node.Codon.DataBoundType == typeof(ToolStripItemFolder))
                               return true;
                           else
                               return false;
                       });
               });

            #endregion

            #region ToolStripPageUpdatedEvent

            SubscriptionToken toolStripPageUpdatedEventToken =
                _eventAggregator.GetEvent<ToolStripPageUpdatedEvent>().Subscribe((e) => { _controller.Update(e.Entity); });

            #endregion

            #region ToolStripPageRemovedEvent

            SubscriptionToken toolStripPageRemovedEventToken =
                _eventAggregator.GetEvent<ToolStripPageRemovedEvent>().Subscribe((e) => { _controller.Remove(e.Entity); });

            #endregion

            #region ToolStripPageMoveBeforeEvent

            SubscriptionToken toolStripPageMoveBeforeEventToken =
                _eventAggregator.GetEvent<ToolStripPageMoveBeforeEvent>().Subscribe((e) =>
                {
                    _controller.MoveBefore(
                        (source) =>
                        {
                            if (source.Codon.DataBoundType == typeof(ToolStripPageEntity))
                            {
                                ToolStripPageEntity toolStripPageEntity = (ToolStripPageEntity)source.DataBoundItem;
                                return toolStripPageEntity.Id == e.Id;
                            }
                            else
                                return false;
                        },
                        (target) =>
                        {
                            if (target.Codon.DataBoundType == typeof(ToolStripPageEntity))
                            {
                                ToolStripPageEntity toolStripPageEntity = (ToolStripPageEntity)target.DataBoundItem;
                                return toolStripPageEntity.Id == e.BeforeId;
                            }
                            else
                                return false;
                        }
                        );
                });

            #endregion

            #region ToolStripPageMoveAfterEvent

            SubscriptionToken toolStripPageMoveAfterEventToken =
                _eventAggregator.GetEvent<ToolStripPageMoveAfterEvent>().Subscribe((e) =>
                {
                    _controller.MoveAfter(
                        (source) =>
                        {
                            if (source.Codon.DataBoundType == typeof(ToolStripPageEntity))
                            {
                                ToolStripPageEntity toolStripPageEntity = (ToolStripPageEntity)source.DataBoundItem;
                                return toolStripPageEntity.Id == e.Id;
                            }
                            else
                                return false;
                        },
                        (target) =>
                        {
                            if (target.Codon.DataBoundType == typeof(ToolStripPageEntity))
                            {
                                ToolStripPageEntity toolStripPageEntity = (ToolStripPageEntity)target.DataBoundItem;
                                return toolStripPageEntity.Id == e.AfterId;
                            }
                            else
                                return false;
                        }
                        );
                });

            #endregion

            #endregion

            #region Group

            #region ToolStripGroupAddedEvent

            SubscriptionToken toolStripGroupAddedEventToken =
               _eventAggregator.GetEvent<ToolStripGroupAddedEvent>().Subscribe((e) =>
               {
                   _controller.Add(e.Entity,
                       (node) =>
                       {
                           if (node.Codon.DataBoundType == typeof(ToolStripPageEntity))
                           {
                               ToolStripPageEntity entity = (ToolStripPageEntity)node.DataBoundItem;
                               return e.Entity.PageId == entity.Id;
                           }
                           else
                               return false;
                       });
               });

            #endregion

            #region ToolStripGroupUpdatedEvent

            SubscriptionToken toolStripGroupUpdatedEventToken =
                _eventAggregator.GetEvent<ToolStripGroupUpdatedEvent>().Subscribe((e) => { _controller.Update(e.Entity); });

            #endregion

            #region ToolStripGroupRemovedEvent

            SubscriptionToken toolStripGroupRemovedEventToken =
                _eventAggregator.GetEvent<ToolStripGroupRemovedEvent>().Subscribe((e) => { _controller.Remove(e.Entity); });

            #endregion

            #region ToolStripGroupMoveBeforeEvent

            SubscriptionToken toolStripGroupMoveBeforeEventToken =
                _eventAggregator.GetEvent<ToolStripGroupMoveBeforeEvent>().Subscribe((e) =>
                {
                    _controller.MoveBefore(
                        (source) =>
                        {
                            if (source.Codon.DataBoundType == typeof(ToolStripGroupEntity))
                            {
                                ToolStripGroupEntity toolStripGroupEntity = (ToolStripGroupEntity)source.DataBoundItem;
                                return toolStripGroupEntity.Id == e.Id;
                            }
                            else
                                return false;
                        },
                        (target) =>
                        {
                            if (target.Codon.DataBoundType == typeof(ToolStripGroupEntity))
                            {
                                ToolStripGroupEntity toolStripGroupEntity = (ToolStripGroupEntity)target.DataBoundItem;
                                return toolStripGroupEntity.Id == e.BeforeId;
                            }
                            else
                                return false;
                        }
                        );
                });

            #endregion

            #region ToolStripGroupMoveAfterEvent

            SubscriptionToken toolStripGroupMoveAfterEventToken =
                _eventAggregator.GetEvent<ToolStripGroupMoveAfterEvent>().Subscribe((e) =>
                {
                    _controller.MoveAfter(
                        (source) =>
                        {
                            if (source.Codon.DataBoundType == typeof(ToolStripGroupEntity))
                            {
                                ToolStripGroupEntity toolStripGroupEntity = (ToolStripGroupEntity)source.DataBoundItem;
                                return toolStripGroupEntity.Id == e.Id;
                            }
                            else
                                return false;
                        },
                        (target) =>
                        {
                            if (target.Codon.DataBoundType == typeof(ToolStripGroupEntity))
                            {
                                ToolStripGroupEntity toolStripGroupEntity = (ToolStripGroupEntity)target.DataBoundItem;
                                return toolStripGroupEntity.Id == e.AfterId;
                            }
                            else
                                return false;
                        }
                        );
                });

            #endregion

            #endregion

            #region Items

            #region ToolStripItemAddedEvent

            SubscriptionToken toolStripItemAddedEventToken =
                _eventAggregator.GetEvent<ToolStripItemAddedEvent>().Subscribe((e) =>
                {
                    _controller.Add(e.Entity,
                        (node) =>
                        {
                            if (node.Codon.DataBoundType == typeof(ToolStripGroupEntity))
                            {
                                ToolStripGroupEntity entity = (ToolStripGroupEntity)node.DataBoundItem;
                                return e.Entity.GroupId == entity.Id;
                            }
                            else
                                return false;
                        });
                });

            #endregion

            #region ToolStripItemRemovedEvent

            SubscriptionToken toolStripItemRemovedEventToken =
                _eventAggregator.GetEvent<ToolStripItemRemovedEvent>().Subscribe((e) => { _controller.Remove(e.Entity); });

            #endregion

            #region ToolStripItemUpdatedEvent

            SubscriptionToken toolStripItemUpdatedEventToken =
                _eventAggregator.GetEvent<ToolStripItemUpdatedEvent>().Subscribe((e) => { _controller.Update(e.Entity); });

            #endregion

            #region ToolStripItemMoveBeforeEvent

            SubscriptionToken toolStripItemMoveBeforeEventToken =
                _eventAggregator.GetEvent<ToolStripItemMoveBeforeEvent>().Subscribe((e) =>
                {
                    _controller.MoveBefore(
                        (source) =>
                        {
                            if (source.Codon.DataBoundType == typeof(ToolStripItemAbstract))
                            {
                                ToolStripItemAbstract toolStripEntity = (ToolStripItemAbstract)source.DataBoundItem;
                                return toolStripEntity.Id == e.Id;
                            }
                            else
                                return false;
                        },
                        (target) =>
                        {
                            if (target.Codon.DataBoundType == typeof(ToolStripItemAbstract))
                            {
                                ToolStripItemAbstract toolStripEntity = (ToolStripItemAbstract)target.DataBoundItem;
                                return toolStripEntity.Id == e.BeforeId;
                            }
                            else
                                return false;
                        }
                        );
                    //_navigationTreeController.Select((node) =>
                    //{
                    //    if (node.Codon.DataBoundType == typeof(ToolStripItemAbstract))
                    //    {
                    //        ToolStripItemAbstract menuEntity = (ToolStripItemAbstract)node.DataBoundItem;
                    //        return menuEntity.Id == e.Id;
                    //    }
                    //    else
                    //        return false;
                    //});
                });

            #endregion

            #region ToolStripItemMoveAfterEvent

            SubscriptionToken toolStripItemMoveAfterEventToken =
                _eventAggregator.GetEvent<ToolStripItemMoveAfterEvent>().Subscribe((e) =>
                {
                    _controller.MoveAfter(
                        (source) =>
                        {
                            if (source.Codon.DataBoundType == typeof(ToolStripItemAbstract))
                            {
                                ToolStripItemAbstract toolStripEntity = (ToolStripItemAbstract)source.DataBoundItem;
                                return toolStripEntity.Id == e.Id;
                            }
                            else
                                return false;
                        },
                        (target) =>
                        {
                            if (target.Codon.DataBoundType == typeof(ToolStripItemAbstract))
                            {
                                ToolStripItemAbstract toolStripEntity = (ToolStripItemAbstract)target.DataBoundItem;
                                return toolStripEntity.Id == e.AfterId;
                            }
                            else
                                return false;
                        }
                        );
                    //_navigationTreeController.Select((node) =>
                    //{
                    //    if (node.Codon.DataBoundType == typeof(ToolStripItemAbstract))
                    //    {
                    //        ToolStripItemAbstract menuEntity = (ToolStripItemAbstract)node.DataBoundItem;
                    //        return menuEntity.Id == e.Id;
                    //    }
                    //    else
                    //        return false;
                    //});
                });

            #endregion

            #endregion

            #region * Unsubscribe *

            this.Disposed += (sender, e) =>
            {
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

                _eventAggregator.GetEvent<ToolStripItemAddedEvent>().Unsubscribe(toolStripItemAddedEventToken);
                _eventAggregator.GetEvent<ToolStripItemRemovedEvent>().Unsubscribe(toolStripItemRemovedEventToken);
                _eventAggregator.GetEvent<ToolStripItemUpdatedEvent>().Unsubscribe(toolStripItemUpdatedEventToken);
                _eventAggregator.GetEvent<ToolStripItemMoveBeforeEvent>().Unsubscribe(toolStripItemMoveBeforeEventToken);
                _eventAggregator.GetEvent<ToolStripItemMoveAfterEvent>().Unsubscribe(toolStripItemMoveAfterEventToken);
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
            public const int ToolStripPageEntity = 1;
            public const int ToolStripGroupEntity = 2;
            public const int ToolStripItemAbstract = 3;
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
