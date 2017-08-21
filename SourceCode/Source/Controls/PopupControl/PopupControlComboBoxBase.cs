/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
namespace Sheng.SailingEase.Controls.PopupControl
{
    [ToolboxBitmap(typeof(System.Windows.Forms.ComboBox)), ToolboxItem(true), ToolboxItemFilter("System.Windows.Forms"), Description("Displays an editable text box with a drop-down list of permitted values.")]
    public partial class PopupControlComboBoxBase : System.Windows.Forms.ComboBox
    {
        public PopupControlComboBoxBase()
        {
            InitializeComponent();
        }
        private static Type _modalMenuFilter;
        private static Type modalMenuFilter
        {
            get
            {
                if (_modalMenuFilter == null)
                {
                    _modalMenuFilter = Type.GetType("System.Windows.Forms.ToolStripManager+ModalMenuFilter");
                }
                if (_modalMenuFilter == null)
                {
                    _modalMenuFilter = new List<Type>(typeof(ToolStripManager).Assembly.GetTypes()).Find(
                    delegate(Type type)
                    {
                        return type.FullName == "System.Windows.Forms.ToolStripManager+ModalMenuFilter";
                    });
                }
                return _modalMenuFilter;
            }
        }
        private static MethodInfo _suspendMenuMode;
        private static MethodInfo suspendMenuMode
        {
            get
            {
                if (_suspendMenuMode == null)
                {
                    Type t = modalMenuFilter;
                    if (t != null)
                    {
                        _suspendMenuMode = t.GetMethod("SuspendMenuMode", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
                    }
                }
                return _suspendMenuMode;
            }
        }
        private static void SuspendMenuMode()
        {
            MethodInfo suspendMenuMode = PopupControlComboBoxBase.suspendMenuMode;
            if (suspendMenuMode != null)
            {
                suspendMenuMode.Invoke(null, null);
            }
        }
        private static MethodInfo _resumeMenuMode;
        private static MethodInfo resumeMenuMode
        {
            get
            {
                if (_resumeMenuMode == null)
                {
                    Type t = modalMenuFilter;
                    if (t != null)
                    {
                        _resumeMenuMode = t.GetMethod("ResumeMenuMode", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
                    }
                }
                return _resumeMenuMode;
            }
        }
        private static void ResumeMenuMode()
        {
            MethodInfo resumeMenuMode = PopupControlComboBoxBase.resumeMenuMode;
            if (resumeMenuMode != null)
            {
                resumeMenuMode.Invoke(null, null);
            }
        }
        protected override void OnDropDown(EventArgs e)
        {
            base.OnDropDown(e);
            SuspendMenuMode();
        }
        protected override void OnDropDownClosed(EventArgs e)
        {
            ResumeMenuMode();
            base.OnDropDownClosed(e);
        }
    }
}
