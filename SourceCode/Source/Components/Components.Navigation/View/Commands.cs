using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sheng.SailingEase.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using Sheng.SailingEase.Localisation;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Components.NavigationComponent.Localisation;

namespace Sheng.SailingEase.Components.NavigationComponent.View
{
    #region Menu

    class AddMenuEntityCommand : AbstactCommand<string>
    {
        public AddMenuEntityCommand()
        {
            ExcuteHandler = () =>
            {
                string parentId = GetArgument();

                using (MenuEditView view = new MenuEditView(parentId))
                {
                    view.ShowDialog();
                }
            };
        }
    }

    class EditMenuEntityCommand : AbstactCommand<MenuEntity>
    {
        public EditMenuEntityCommand()
        {
            ExcuteHandler = () =>
            {
                MenuEntity entity = GetArgument();

                if (entity == null)
                    return;

                if (entity.Sys)
                {
                    //TODO:系统项不允许编辑
                }

                using (MenuEditView view = new MenuEditView(entity.ParentId))
                {
                    view.MainMenuEntity = entity;
                    view.ShowDialog();
                }
            };
        }
    }

    class DeleteMenuEntityCommand : AbstactCommand<List<MenuEntity>>
    {
        public DeleteMenuEntityCommand()
        {
            ExcuteHandler = () =>
            {
                List<MenuEntity> entityList = GetArgument();

                if (entityList == null || entityList.Count == 0)
                    return;

                StringBuilder entityName = new StringBuilder();
                StringBuilder entityNameSys = new StringBuilder();

                foreach (MenuEntity entity in entityList)
                {
                    if (entity.Sys)
                    {
                        entityNameSys.Append(String.Format("[ {0} ],", entity.Name));
                        continue;
                    }
                    entityName.Append(String.Format("[ {0} ],", entity.Name));
                }

                if (entityNameSys.Length > 0)
                {
                    entityNameSys = entityNameSys.Remove(entityNameSys.Length - 1, 1);
                    MessageBox.Show(
                        CommonLanguage.Current.SystemItemNoDeleteAllowed
                        + Environment.NewLine + Environment.NewLine + entityNameSys,
                        CommonLanguage.Current.MessageCaption_Notice,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                entityName = entityName.Remove(entityName.Length - 1, 1);

                string strConfirmDelete = String.Format(CommonLanguage.Current.ConfirmDelete, entityName);
                if (MessageBox.Show(strConfirmDelete,
                     CommonLanguage.Current.MessageCaption_Notice,
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.OK)
                {
                    return;
                }

                foreach (MenuEntity entity in entityList)
                {
                    if (entity.Sys) continue;

                    MenuStripArchive.Instance.Delete(entity);
                }
            };
        }
    }

    class MoveBeforeMenuEntityCommand : ICommand
    {
        private Func<bool> _canExcuteHandler;
        /// <summary>
        /// 用于判断此命令是否可执行的方法委托
        /// </summary>
        public Func<bool> CanExcuteHandler { set { _canExcuteHandler = value; } }

        private Func<string> _getMenuId;
        /// <summary>
        /// 获取在执行命令时所需的参数
        /// </summary>
        public Func<string> GetMenuId { set { _getMenuId = value; } }

        private Func<string> _getBeforeMenuId;
        public Func<string> GetBeforeMenuId { set { _getBeforeMenuId = value; } }

        public bool CanExcute()
        {
            if (_canExcuteHandler == null)
                return true;
            else
                return _canExcuteHandler();
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        public void Excute()
        {
            if (_getMenuId == null || _getBeforeMenuId == null)
            {
                Debug.Assert(false, "GetArgument 为 null");
                return;
            }

            string id = _getMenuId();
            string beforeId = _getBeforeMenuId();

            if (String.IsNullOrEmpty(id) || String.IsNullOrEmpty(beforeId))
            {
                Debug.Assert(false, "id 或 beforeId 为空");
                return;
            }

            MenuStripArchive.Instance.MoveBefore(id, beforeId);
        }
    }

    class MoveAfterMenuEntityCommand : ICommand
    {
        private Func<bool> _canExcuteHandler;
        /// <summary>
        /// 用于判断此命令是否可执行的方法委托
        /// </summary>
        public Func<bool> CanExcuteHandler { set { _canExcuteHandler = value; } }

        private Func<string> _getMenuId;
        /// <summary>
        /// 获取在执行命令时所需的参数
        /// </summary>
        public Func<string> GetMenuId { set { _getMenuId = value; } }

        private Func<string> _getAfterMenuId;
        public Func<string> GetAfterMenuId { set { _getAfterMenuId = value; } }

        public bool CanExcute()
        {
            if (_canExcuteHandler == null)
                return true;
            else
                return _canExcuteHandler();
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        public void Excute()
        {
            if (_getMenuId == null || _getAfterMenuId == null)
            {
                Debug.Assert(false, "GetArgument 为 null");
                return;
            }

            string id = _getMenuId();
            string afterId = _getAfterMenuId();

            if (String.IsNullOrEmpty(id) || String.IsNullOrEmpty(afterId))
            {
                Debug.Assert(false, "id 或 beforeId 为空");
                return;
            }

            MenuStripArchive.Instance.MoveAfter(id, afterId);
        }
    }

    #endregion

    #region ToolStrip

    #region Page

    #region AddToolStripPageCommand

    /// <summary>
    /// 添加工具栏项目
    /// （暂不支持多级）
    /// </summary>
    class AddToolStripPageCommand : AbstactCommand
    {
        public AddToolStripPageCommand()
        {
            ExcuteHandler = () =>
            {
                using (ToolStripPageEditView view = new ToolStripPageEditView())
                {
                    view.ShowDialog();
                }
            };
        }
    }

    #endregion

    #region EditToolStripPageCommand

    class EditToolStripPageCommand : AbstactCommand<ToolStripPageEntity>
    {
        public EditToolStripPageCommand()
        {
            ExcuteHandler = () =>
            {
                ToolStripPageEntity entity = GetArgument();

                if (entity == null)
                    return;

                using (ToolStripPageEditView view = new ToolStripPageEditView())
                {
                    view.ToolStripPage = entity;
                    view.ShowDialog();
                }
            };
        }
    }

    #endregion

    #region RemoveToolStripPageCommand

    class RemoveToolStripPageCommand : AbstactCommand<List<ToolStripPageEntity>>
    {
        public RemoveToolStripPageCommand()
        {
            ExcuteHandler = () =>
            {
                List<ToolStripPageEntity> entityList = GetArgument();

                if (entityList == null || entityList.Count == 0)
                    return;

                StringBuilder entityName = new StringBuilder();

                foreach (ToolStripPageEntity entity in entityList)
                {
                    entityName.Append(String.Format("[ {0} ],", entity.Name));
                }
                
                entityName = entityName.Remove(entityName.Length - 1, 1);

                string strConfirmDelete = String.Format(CommonLanguage.Current.ConfirmDelete, entityName);
                if (MessageBox.Show(strConfirmDelete,
                     CommonLanguage.Current.MessageCaption_Notice,
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.OK)
                {
                    return;
                }

                foreach (ToolStripPageEntity entity in entityList)
                {
                    ToolStripArchive.Instance.RemovePage(entity);
                }
            };
        }
    }

    #endregion

    #region MoveBeforeToolStripPageCommand

    class MoveBeforeToolStripPageCommand : ICommand
    {
        private Func<bool> _canExcuteHandler;
        /// <summary>
        /// 用于判断此命令是否可执行的方法委托
        /// </summary>
        public Func<bool> CanExcuteHandler { set { _canExcuteHandler = value; } }

        private Func<string> _getId;
        /// <summary>
        /// 获取在执行命令时所需的参数
        /// </summary>
        public Func<string> GetId { set { _getId = value; } }

        private Func<string> _getBeforeId;
        public Func<string> GetBeforeId { set { _getBeforeId = value; } }

        public bool CanExcute()
        {
            if (_canExcuteHandler == null)
                return true;
            else
                return _canExcuteHandler();
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        public void Excute()
        {
            if (_getId == null || _getBeforeId == null)
            {
                Debug.Assert(false, "GetArgument 为 null");
                return;
            }

            string id = _getId();
            string beforeId = _getBeforeId();

            if (String.IsNullOrEmpty(id) || String.IsNullOrEmpty(beforeId))
            {
                Debug.Assert(false, "id 或 beforeId 为空");
                return;
            }

            ToolStripArchive.Instance.MovePageBefore(id, beforeId);
        }
    }

    #endregion

    #region MoveAfterToolStripPageCommand

    class MoveAfterToolStripPageCommand : ICommand
    {
        private Func<bool> _canExcuteHandler;
        /// <summary>
        /// 用于判断此命令是否可执行的方法委托
        /// </summary>
        public Func<bool> CanExcuteHandler { set { _canExcuteHandler = value; } }

        private Func<string> _getId;
        /// <summary>
        /// 获取在执行命令时所需的参数
        /// </summary>
        public Func<string> GetId { set { _getId = value; } }

        private Func<string> _getAfterId;
        public Func<string> GetAfterId { set { _getAfterId = value; } }

        public bool CanExcute()
        {
            if (_canExcuteHandler == null)
                return true;
            else
                return _canExcuteHandler();
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        public void Excute()
        {
            if (_getId == null || _getAfterId == null)
            {
                Debug.Assert(false, "GetArgument 为 null");
                return;
            }

            string id = _getId();
            string afterId = _getAfterId();

            if (String.IsNullOrEmpty(id) || String.IsNullOrEmpty(afterId))
            {
                Debug.Assert(false, "id 或 beforeId 为空");
                return;
            }

            ToolStripArchive.Instance.MovePageAfter(id, afterId);
        }
    }

    #endregion

    #endregion

    #region Group

    #region AddToolStripGroupCommand

    /// <summary>
    /// 添加工具栏项目
    /// （暂不支持多级）
    /// </summary>
    class AddToolStripGroupCommand : AbstactCommand<string>
    {
        public AddToolStripGroupCommand()
        {
            ExcuteHandler = () =>
            {
                string pageId = GetArgument();

                Debug.Assert(String.IsNullOrEmpty(pageId) == false, "pageId 为空");

                if (String.IsNullOrEmpty(pageId))
                    return;

                using (ToolStripGroupEditView view = new ToolStripGroupEditView(pageId))
                {
                    view.ShowDialog();
                }
            };
        }
    }

    #endregion

    #region EditToolStripGroupCommand

    class EditToolStripGroupCommand : AbstactCommand<ToolStripGroupEntity>
    {
        public EditToolStripGroupCommand()
        {
            ExcuteHandler = () =>
            {
                ToolStripGroupEntity entity = GetArgument();

                if (entity == null)
                    return;

                using (ToolStripGroupEditView view = new ToolStripGroupEditView(entity.PageId))
                {
                    view.ToolStripGroup = entity;
                    view.ShowDialog();
                }
            };
        }
    }

    #endregion

    #region RemoveToolStripGroupCommand

    class RemoveToolStripGroupCommand : AbstactCommand<List<ToolStripGroupEntity>>
    {
        public RemoveToolStripGroupCommand()
        {
            ExcuteHandler = () =>
            {
                List<ToolStripGroupEntity> entityList = GetArgument();

                if (entityList == null || entityList.Count == 0)
                    return;

                StringBuilder entityName = new StringBuilder();

                foreach (ToolStripGroupEntity entity in entityList)
                {
                    entityName.Append(String.Format("[ {0} ],", entity.Name));
                }

                entityName = entityName.Remove(entityName.Length - 1, 1);

                string strConfirmDelete = String.Format(CommonLanguage.Current.ConfirmDelete, entityName);
                if (MessageBox.Show(strConfirmDelete,
                     CommonLanguage.Current.MessageCaption_Notice,
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.OK)
                {
                    return;
                }

                foreach (ToolStripGroupEntity entity in entityList)
                {
                    ToolStripArchive.Instance.RemoveGroup(entity);
                }
            };
        }
    }

    #endregion

    #region MoveBeforeToolStripGroupCommand

    class MoveBeforeToolStripGroupCommand : ICommand
    {
        private Func<bool> _canExcuteHandler;
        /// <summary>
        /// 用于判断此命令是否可执行的方法委托
        /// </summary>
        public Func<bool> CanExcuteHandler { set { _canExcuteHandler = value; } }

        private Func<string> _getId;
        /// <summary>
        /// 获取在执行命令时所需的参数
        /// </summary>
        public Func<string> GetId { set { _getId = value; } }

        private Func<string> _getBeforeId;
        public Func<string> GetBeforeId { set { _getBeforeId = value; } }

        public bool CanExcute()
        {
            if (_canExcuteHandler == null)
                return true;
            else
                return _canExcuteHandler();
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        public void Excute()
        {
            if (_getId == null || _getBeforeId == null)
            {
                Debug.Assert(false, "GetArgument 为 null");
                return;
            }

            string id = _getId();
            string beforeId = _getBeforeId();

            if (String.IsNullOrEmpty(id) || String.IsNullOrEmpty(beforeId))
            {
                Debug.Assert(false, "id 或 beforeId 为空");
                return;
            }

            ToolStripArchive.Instance.MoveGroupBefore(id, beforeId);
        }
    }

    #endregion

    #region MoveAfterToolStripGroupCommand

    class MoveAfterToolStripGroupCommand : ICommand
    {
        private Func<bool> _canExcuteHandler;
        /// <summary>
        /// 用于判断此命令是否可执行的方法委托
        /// </summary>
        public Func<bool> CanExcuteHandler { set { _canExcuteHandler = value; } }

        private Func<string> _getId;
        /// <summary>
        /// 获取在执行命令时所需的参数
        /// </summary>
        public Func<string> GetId { set { _getId = value; } }

        private Func<string> _getAfterId;
        public Func<string> GetAfterId { set { _getAfterId = value; } }

        public bool CanExcute()
        {
            if (_canExcuteHandler == null)
                return true;
            else
                return _canExcuteHandler();
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        public void Excute()
        {
            if (_getId == null || _getAfterId == null)
            {
                Debug.Assert(false, "GetArgument 为 null");
                return;
            }

            string id = _getId();
            string afterId = _getAfterId();

            if (String.IsNullOrEmpty(id) || String.IsNullOrEmpty(afterId))
            {
                Debug.Assert(false, "id 或 beforeId 为空");
                return;
            }

            ToolStripArchive.Instance.MoveGroupAfter(id, afterId);
        }
    }

    #endregion

    #endregion

    #region Items

    #region AddToolStripItemCommand

    /// <summary>
    /// 添加工具栏项目
    /// （暂不支持多级）
    /// string 是 GroupId
    /// </summary>
    class AddToolStripItemCommand : AbstactCommand<string>
    {
        public AddToolStripItemCommand()
        {
            ExcuteHandler = () =>
            {
                string groupId = GetArgument();

                if (String.IsNullOrEmpty(groupId))
                {
                    Debug.Assert(false, "groupId 为空");
                    return;
                }

                //if (String.IsNullOrEmpty(groupId) == false)
                //{
                //    //工具栏项目暂不支持多级
                //    MessageBox.Show(Language.Current.Explorer_Commands_AddToolStripItemCommand_NotSupportSubItem,
                //        CommonLanguage.Current.MessageCaption_Notice, MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}

                using (ToolStripItemEditView view = new ToolStripItemEditView(groupId))
                {
                    view.ShowDialog();
                }
            };
        }
    }

    #endregion

    #region EditToolStripItemCommand

    class EditToolStripItemCommand : AbstactCommand<ToolStripItemAbstract>
    {
        public EditToolStripItemCommand()
        {
            ExcuteHandler = () =>
            {
                ToolStripItemAbstract entity = GetArgument();

                if (entity == null)
                    return;

                using (ToolStripItemEditView view = new ToolStripItemEditView(entity.GroupId))
                {
                    view.ToolStripElement = entity;
                    view.ShowDialog();
                }
            };
        }
    }

    #endregion

    #region DeleteToolStripItemCommand

    class DeleteToolStripItemCommand : AbstactCommand<List<ToolStripItemAbstract>>
    {
        public DeleteToolStripItemCommand()
        {
            ExcuteHandler = () =>
            {
                List<ToolStripItemAbstract> entityList = GetArgument();

                if (entityList == null || entityList.Count == 0)
                    return;

                StringBuilder entityName = new StringBuilder();
                StringBuilder entityNameSys = new StringBuilder();

                foreach (ToolStripItemAbstract entity in entityList)
                {
                    if (entity.Sys)
                    {
                        entityNameSys.Append(String.Format("[ {0} ],", entity.Name));
                        continue;
                    }
                    entityName.Append(String.Format("[ {0} ],", entity.Name));
                }

                if (entityNameSys.Length > 0)
                {
                    entityNameSys = entityNameSys.Remove(entityNameSys.Length - 1, 1);
                    MessageBox.Show(
                        CommonLanguage.Current.SystemItemNoDeleteAllowed
                        + Environment.NewLine + Environment.NewLine + entityNameSys,
                        CommonLanguage.Current.MessageCaption_Notice,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                entityName = entityName.Remove(entityName.Length - 1, 1);

                string strConfirmDelete = String.Format(CommonLanguage.Current.ConfirmDelete, entityName);
                if (MessageBox.Show(strConfirmDelete,
                     CommonLanguage.Current.MessageCaption_Notice,
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.OK)
                {
                    return;
                }

                foreach (ToolStripItemAbstract entity in entityList)
                {
                    if (entity.Sys) continue;

                    ToolStripArchive.Instance.Delete(entity);
                }
            };
        }
    }

    #endregion

    #region MoveBeforeToolStripItemCommand

    class MoveBeforeToolStripItemCommand : ICommand
    {
        private Func<bool> _canExcuteHandler;
        /// <summary>
        /// 用于判断此命令是否可执行的方法委托
        /// </summary>
        public Func<bool> CanExcuteHandler { set { _canExcuteHandler = value; } }

        private Func<string> _getMenuId;
        /// <summary>
        /// 获取在执行命令时所需的参数
        /// </summary>
        public Func<string> GetMenuId { set { _getMenuId = value; } }

        private Func<string> _getBeforeMenuId;
        public Func<string> GetBeforeMenuId { set { _getBeforeMenuId = value; } }

        public bool CanExcute()
        {
            if (_canExcuteHandler == null)
                return true;
            else
                return _canExcuteHandler();
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        public void Excute()
        {
            if (_getMenuId == null || _getBeforeMenuId == null)
            {
                Debug.Assert(false, "GetArgument 为 null");
                return;
            }

            string id = _getMenuId();
            string beforeId = _getBeforeMenuId();

            if (String.IsNullOrEmpty(id) || String.IsNullOrEmpty(beforeId))
            {
                Debug.Assert(false, "id 或 beforeId 为空");
                return;
            }

            ToolStripArchive.Instance.MoveBefore(id, beforeId);
        }
    }

    #endregion

    #region MoveAfterToolStripItemCommand

    class MoveAfterToolStripItemCommand : ICommand
    {
        private Func<bool> _canExcuteHandler;
        /// <summary>
        /// 用于判断此命令是否可执行的方法委托
        /// </summary>
        public Func<bool> CanExcuteHandler { set { _canExcuteHandler = value; } }

        private Func<string> _getMenuId;
        /// <summary>
        /// 获取在执行命令时所需的参数
        /// </summary>
        public Func<string> GetMenuId { set { _getMenuId = value; } }

        private Func<string> _getAfterMenuId;
        public Func<string> GetAfterMenuId { set { _getAfterMenuId = value; } }

        public bool CanExcute()
        {
            if (_canExcuteHandler == null)
                return true;
            else
                return _canExcuteHandler();
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        public void Excute()
        {
            if (_getMenuId == null || _getAfterMenuId == null)
            {
                Debug.Assert(false, "GetArgument 为 null");
                return;
            }

            string id = _getMenuId();
            string afterId = _getAfterMenuId();

            if (String.IsNullOrEmpty(id) || String.IsNullOrEmpty(afterId))
            {
                Debug.Assert(false, "id 或 beforeId 为空");
                return;
            }

            ToolStripArchive.Instance.MoveAfter(id, afterId);
        }
    }

    #endregion

    #endregion

    #endregion
}
