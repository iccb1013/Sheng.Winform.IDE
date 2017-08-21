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
using Sheng.SailingEase.Components.DataEntityComponent.Localisation;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Kernal;
using Sheng.SailingEase.Localisation;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Controls.Extensions;
using System.Collections;

namespace Sheng.SailingEase.Components.DataEntityComponent.View
{
    partial class ExplorerTreeView : PadViewBase
    {
        #region 私有成员

        private IEventAggregator _eventAggregator = ServiceUnity.EventAggregator;

        private TypeBinderTreeViewController _navigationTreeController;

        #region 菜单

        TreeMenuDataEntityFolder _treeMenuDataEntityFolder;
        TreeMenuDataEntity _treeMenuDataEntity;
        TreeMenuDataItemEntity _treeMenuDataItemEntity;

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

            //初始化 DataGridView 控制器
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

            _addDataEntityCommand = new AddDataEntityCommand();
            _editDataEntityCommand = new EditDataEntityCommand()
            {
                CanExcuteHandler = () =>
                {
                    return _navigationTreeController.GetSelectedData<DataEntityDev>() != null;
                },
                GetArgumentHandler = () => { return _navigationTreeController.GetSelectedData<DataEntityDev>(); }
            };
            _deleteDataEntityCommand = new DeleteDataEntityCommand()
            {
                CanExcuteHandler = () =>
                {
                    return _navigationTreeController.GetSelectedData<DataEntityDev>() != null;
                },
                GetArgumentHandler = () =>
                {
                    return new List<DataEntityDev>() { _navigationTreeController.GetSelectedData<DataEntityDev>() };
                }
            };
            _createSqlCommand = new CreateSqlCommand()
            {
                CanExcuteHandler = () =>
                {
                    return _navigationTreeController.GetSelectedData<DataEntityDev>() != null;
                },
                GetArgumentHandler = () =>
                {
                    return new List<DataEntity>() { _navigationTreeController.GetSelectedData<DataEntityDev>() };
                }
            };

            _addDataItemEntityCommand = new AddDataItemEntityCommand()
            {
                CanExcuteHandler = () =>
                {
                    return _navigationTreeController.GetSelectedData<DataEntityDev>(true) != null;
                },
                GetArgumentHandler = () =>
                {
                    return _navigationTreeController.GetSelectedData<DataEntityDev>(true) ;
                }
            };
            _editDataItemEntityCommand = new EditDataItemEntityCommand()
            {
                CanExcuteHandler = () =>
                {
                    return _navigationTreeController.GetSelectedData<DataItemEntityDev>() != null;
                },
                GetArgumentHandler = () => { return _navigationTreeController.GetSelectedData<DataItemEntityDev>(); }
            };
            _deleteDataItemEntityCommand = new DeleteDataItemEntityCommand()
            {
                CanExcuteHandler = () =>
                {
                    return _navigationTreeController.GetSelectedData<DataItemEntityDev>() != null;
                },
                GetArgumentHandler = () =>
                {
                    return new List<DataItemEntityDev>() { _navigationTreeController.GetSelectedData<DataItemEntityDev>() };
                }
            };

            #endregion
        }

        private void InitContextMenu()
        {
            #region TreeMenuDataEntityFolder

            _treeMenuDataEntityFolder = new TreeMenuDataEntityFolder();
            _treeMenuDataEntityFolder.AddCommand = _addDataEntityCommand;

            #endregion

            #region TreeMenuDataEntity

            _treeMenuDataEntity = new TreeMenuDataEntity();
            _treeMenuDataEntity.AddCommand = _addDataEntityCommand;
            _treeMenuDataEntity.EditCommand = _editDataEntityCommand; 
            _treeMenuDataEntity.DeleteCommand = _deleteDataEntityCommand;
            _treeMenuDataEntity.AddItemCommand = _addDataItemEntityCommand;
            _treeMenuDataEntity.CreateSqlCommand = _createSqlCommand;

            #endregion

            #region TreeMenuDataItemEntity

            _treeMenuDataItemEntity = new TreeMenuDataItemEntity();
            _treeMenuDataItemEntity.AddCommand = _addDataItemEntityCommand;
            _treeMenuDataItemEntity.EditCommand = _editDataItemEntityCommand;
            _treeMenuDataItemEntity.DeleteCommand = _deleteDataItemEntityCommand;

            #endregion
        }

        private void InitController()
        {
            _navigationTreeController = new TypeBinderTreeViewController(this.dataEntityTree);

            #region ImageList

            //节点的小图标ImageList
            ImageList _imageList = new ImageList();
            _imageList.Images.Add(IconsLibrary.Folder);  //0
            _imageList.Images.Add(IconsLibrary.DataEntity3);  //1
            _imageList.Images.Add(IconsLibrary.Cube2);  //2
            _navigationTreeController.ImageList = _imageList;

            #endregion

            #region Codon

            _navigationTreeController.AddNodeCodon(new TypeBinderTreeViewNodeCodon(
                typeof(DataEntityFolder), typeof(DataEntityDev),
                DataEntityFolder.Property_Text, DataEntityFolder.Property_Items)
            {
                ImageIndex = TreeImages.Folder,
                ContextMenuStrip = _treeMenuDataEntityFolder.View
            });

            _navigationTreeController.AddNodeCodon(new TypeBinderTreeViewNodeCodon(
                typeof(DataEntityDev), typeof(DataItemEntityDev),
                DataEntityDev.Property_Name, DataEntityDev.Property_Items)
            {
                ImageIndex = TreeImages.DataEntity,
                ContextMenuStrip = _treeMenuDataEntity.View
            });

            _navigationTreeController.AddNodeCodon(new TypeBinderTreeViewNodeCodon(
                typeof(DataItemEntityDev), DataItemEntityDev.Property_Name)
            {
                ImageIndex = TreeImages.DataItemEntity,
                ContextMenuStrip = _treeMenuDataItemEntity.View
            });

            #endregion

            _navigationTreeController.AfterSelect +=
                new TypeBinderTreeViewController.OnAfterSelectHandler(_navigationTreeController_AfterSelect);

        }

        private void LoadTree()
        {
            DataEntityFolder dataEntityFolder =
                new DataEntityFolder(DataEntityArchive.Instance.GetDataEntityList());

            _navigationTreeController.DataBind(dataEntityFolder);
        }

        private void SubscribeEvent()
        {
            //订阅数据实体的增删改事件，以更新呈现

            SubscriptionToken dataEntityAddedEventToken =
                _eventAggregator.GetEvent<DataEntityAddedEvent>().Subscribe((e) =>
                {
                    _navigationTreeController.Add(e.DataEntity,
                        (node) =>
                        {
                            if (node.Codon.DataBoundType == typeof(DataEntityFolder)) return true;
                            else return false;
                        });
                });
            SubscriptionToken dataEntityRemovedEvent =
                _eventAggregator.GetEvent<DataEntityRemovedEvent>().Subscribe((e) => { _navigationTreeController.Remove(e.DataEntity); });
            SubscriptionToken dataEntityUpdatedEvent =
                _eventAggregator.GetEvent<DataEntityUpdatedEvent>().Subscribe((e) => { _navigationTreeController.Update(e.DataEntity); });

            SubscriptionToken dataItemEntityAddedEvent =
                _eventAggregator.GetEvent<DataItemEntityAddedEvent>().Subscribe((e) =>
                {
                    _navigationTreeController.Add(e.Entity,
                        (node) =>
                        {
                            if (node.Codon.DataBoundType == typeof(DataEntityDev))
                            {
                                DataEntityDev dataEntity = (DataEntityDev)node.DataBoundItem;
                                return dataEntity.Id == e.Entity.Owner.Id;
                            }
                            else return false;
                        });
                });
            SubscriptionToken dataItemEntityRemovedEvent =
                _eventAggregator.GetEvent<DataItemEntityRemovedEvent>().Subscribe((e) => { _navigationTreeController.Remove(e.Entity); });
            SubscriptionToken dataItemEntityUpdatedEvent =
                _eventAggregator.GetEvent<DataItemEntityUpdatedEvent>().Subscribe((e) => { _navigationTreeController.Update(e.Entity); });

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

        #endregion

        #region 事件

        public delegate void OnAfterSelectHandler(object sender, AfterSelectEventArgs e);
        /// <summary>
        /// 切换树中选择的项时发生
        /// 根据AfterSelectEventArgs.TabPageType 来判断选的是哪颗树
        /// </summary>
        public event OnAfterSelectHandler AfterSelect;

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

        #region TreeImages

        struct TreeImages
        {
            public const int Folder = 0;
            public const int DataEntity = 1;
            public const int DataItemEntity = 2;
        }

        #endregion
    }
}
