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
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
namespace Sheng.SailingEase.Controls
{
    interface ISEMessageBoxForm
    {
        string Caption {  set; }
        string Message { get; set; }
        Icon CustomIcon { set; }
        MessageBoxIcon StandardIcon { set; }
        Font Font { get; set; }
        bool AllowSaveResponse { get; set; }
        string SaveResponseText { get; set; }
        bool PlayAlertSound { get; set; }
        int Timeout { get; set; }
        SEMessageBoxTimeoutResult TimeoutResult { get; set; }
        bool SaveResponse { get; }
        SEMessageBoxButton Result { get; }
        SEMessageBoxButton CustomCancelButton { set; }
        ArrayList Buttons { get; }
        DialogResult ShowDialog();
        DialogResult ShowDialog(IWin32Window owner);
        void Dispose();
    }
}
