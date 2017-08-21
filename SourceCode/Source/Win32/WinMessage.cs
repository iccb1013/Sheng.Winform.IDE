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
using System.Diagnostics;
namespace Sheng.SailingEase.Win32
{
    public static class WinMessage
    {
        public static void SendMessage(string destProcessName, int msgID, string strMsg)
        {
            if (strMsg == null)
                return;
            Process[] foundProcess = Process.GetProcessesByName(destProcessName);
            foreach (Process p in foundProcess)
            {
                int toWindowHandler = p.MainWindowHandle.ToInt32();
                if (toWindowHandler != 0)
                {
                    User32.CopyDataStruct cds;
                    cds.dwData = (IntPtr)msgID;   
                    cds.lpData = strMsg;            
                    cds.cbData = System.Text.Encoding.Default.GetBytes(strMsg).Length + 1;  
                    int fromWindowHandler = 0;
                    User32.SendMessage(toWindowHandler, User32.WM_COPYDATA, fromWindowHandler, ref  cds);
                }
            }
        }
        public static string ReceiveMessage(ref  System.Windows.Forms.Message m)
        {
            if (m.Msg == User32.WM_COPYDATA)
            {
                User32.CopyDataStruct cds = (User32.CopyDataStruct)m.GetLParam(typeof(User32.CopyDataStruct));
                return cds.lpData;
            }
            else
            {
                return String.Empty;
            }
        }
    }
}
