/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
namespace Sheng.SailingEase.Controls
{
    public static class SEValidateHelper
    {
        public static bool ValidateContainerControl(Control control, out string validateMsg)
        {
            bool validateResult = true;
            string ctrlValidateMsg;
            validateMsg = String.Empty;
            foreach (Control ctrl in control.Controls)
            {
                if (ValidateControl(ctrl, out ctrlValidateMsg) == false)
                {
                    validateMsg += ctrlValidateMsg + Environment.NewLine;
                    validateResult = false;
                }
            }
            if (validateResult == false)
            {
                WipeSpilthSpace(ref validateMsg);
            }
            return validateResult;
        }
        public static bool ValidateControl(Control ctrl, out string validateMsg)
        {
            validateMsg = String.Empty;
            ISEValidate seValidate = ctrl as ISEValidate;
            if (seValidate == null)
                return true;
            bool result = true;
            string msg = String.Empty;
            if (seValidate.SEValidate(out msg) == false)
            {
                validateMsg += msg;
                if (seValidate.HighLight)
                {
                    ctrl.BackColor = Color.Pink;
                }
                result = false;
            }
            else
            {
                if (seValidate.HighLight)
                {
                    ctrl.BackColor = SystemColors.Window;
                }
                result = true;
            }
            if (result == false)
            {
                WipeSpilthSpace(ref validateMsg);
            }
            return result;
        }
        public static void WipeSpilthSpace(ref string validateMsg)
        {
            while (true)
            {
                if (validateMsg.IndexOf("\r\n\r\n") > 0)
                {
                    validateMsg = validateMsg.Replace("\r\n\r\n", "\r\n");
                }
                else
                {
                    break;
                }
            }
        }
    }
}
