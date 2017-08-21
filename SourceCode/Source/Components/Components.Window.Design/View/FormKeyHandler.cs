/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
namespace Sheng.SailingEase.Components.Window.DesignComponent
{
    class FormKeyHandler : IMessageFilter
    {
        const int keyPressedMessage = 0x100;
        const int leftMouseButtonDownMessage = 0x0202;
        Hashtable keyTable = new Hashtable();
        private static bool inserted = false;
        public static bool Inserted
        {
            get { return inserted; }
        }
        public static void Insert()
        {
            inserted = true;
            Application.AddMessageFilter(new FormKeyHandler());
        }
        public FormKeyHandler()
        {
            keyTable[Keys.Left] = new CommandWrapper(MenuCommands.KeyMoveLeft);
            keyTable[Keys.Right] = new CommandWrapper(MenuCommands.KeyMoveRight);
            keyTable[Keys.Up] = new CommandWrapper(MenuCommands.KeyMoveUp);
            keyTable[Keys.Down] = new CommandWrapper(MenuCommands.KeyMoveDown);
            keyTable[Keys.Tab] = new CommandWrapper(MenuCommands.KeySelectNext);
            keyTable[Keys.Delete] = new CommandWrapper(MenuCommands.Delete);
            keyTable[Keys.Back] = new CommandWrapper(MenuCommands.Delete);
            keyTable[Keys.Left | Keys.Shift] = new CommandWrapper(MenuCommands.KeySizeWidthDecrease);
            keyTable[Keys.Right | Keys.Shift] = new CommandWrapper(MenuCommands.KeySizeWidthIncrease);
            keyTable[Keys.Up | Keys.Shift] = new CommandWrapper(MenuCommands.KeySizeHeightDecrease);
            keyTable[Keys.Down | Keys.Shift] = new CommandWrapper(MenuCommands.KeySizeHeightIncrease);
            keyTable[Keys.Tab | Keys.Shift] = new CommandWrapper(MenuCommands.KeySelectPrevious);
            keyTable[Keys.Delete | Keys.Shift] = new CommandWrapper(MenuCommands.Delete);
            keyTable[Keys.Back | Keys.Shift] = new CommandWrapper(MenuCommands.Delete);
            keyTable[Keys.Left | Keys.Control] = new CommandWrapper(MenuCommands.KeyNudgeLeft);
            keyTable[Keys.Right | Keys.Control] = new CommandWrapper(MenuCommands.KeyNudgeRight);
            keyTable[Keys.Up | Keys.Control] = new CommandWrapper(MenuCommands.KeyNudgeUp);
            keyTable[Keys.Down | Keys.Control] = new CommandWrapper(MenuCommands.KeyNudgeDown);
            keyTable[Keys.Left | Keys.Control | Keys.Shift] = new CommandWrapper(MenuCommands.KeyNudgeWidthDecrease);
            keyTable[Keys.Right | Keys.Control | Keys.Shift] = new CommandWrapper(MenuCommands.KeyNudgeWidthIncrease);
            keyTable[Keys.Up | Keys.Control | Keys.Shift] = new CommandWrapper(MenuCommands.KeyNudgeHeightDecrease);
            keyTable[Keys.Down | Keys.Control | Keys.Shift] = new CommandWrapper(MenuCommands.KeyNudgeHeightIncrease);
            keyTable[Keys.X | Keys.Control] = new CommandWrapper(MenuCommands.Cut);
            keyTable[Keys.C | Keys.Control] = new CommandWrapper(MenuCommands.Copy);
            keyTable[Keys.V | Keys.Control] = new CommandWrapper(MenuCommands.Paste);
            keyTable[Keys.A | Keys.Control] = new CommandWrapper(MenuCommands.SelectAll);
        }
        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg != keyPressedMessage /*&& m.Msg != leftMouseButtonDownMessage*/)
            {
                return false;
            }
            if (FormHostingContainer.Instance.ActiveHosting == null
              || ((Control)FormHostingContainer.Instance.ActiveHosting.DesignSurface.View).Focused == false)
            {
                return false;
            }
            Keys keyPressed = (Keys)m.WParam.ToInt32() | Control.ModifierKeys;
            if (keyPressed == Keys.Escape)
            {
                FormHostingContainer.Instance.ActiveHosting.SelectionService.SetSelectedComponents(
                    new object[] { FormHostingContainer.Instance.ActiveHosting.DesignerHost.RootComponent });
                return true;
            }
            if (keyPressed == (Keys.Control|Keys.S))
            {
                FormHostingContainer.Instance.ActiveHosting.Save();
                return true;
            }
            CommandWrapper commandWrapper = (CommandWrapper)keyTable[keyPressed];
            if (commandWrapper != null)
            {
                if (commandWrapper.CommandID == MenuCommands.Delete)
                {
                    if (FormHostingContainer.Instance.ActiveHosting.EnableDelete == false)
                        return false;
                }
                Debug.WriteLine("Run menu command: " + commandWrapper.CommandID);
                FormHostingContainer.Instance.ActiveHosting.DesignSurface.DoAction(commandWrapper.CommandID);
                return true;
            }
            return false;
        }
    }
    class CommandWrapper
    {
        CommandID commandID;
        bool restoreSelection;
        public CommandID CommandID
        {
            get
            {
                return commandID;
            }
        }
        public bool RestoreSelection
        {
            get
            {
                return restoreSelection;
            }
        }
        public CommandWrapper(CommandID commandID)
            : this(commandID, false)
        {
        }
        public CommandWrapper(CommandID commandID, bool restoreSelection)
        {
            this.commandID = commandID;
            this.restoreSelection = restoreSelection;
        }
    }
}
