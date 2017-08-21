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
using System.Runtime.InteropServices;
namespace Sheng.SailingEase.Controls
{
    public class DragHelper
    {
        [DllImport("comctl32.dll")]
        public static extern bool InitCommonControls();
        [DllImport("comctl32.dll", CharSet = CharSet.Auto)]
        public static extern bool ImageList_BeginDrag(IntPtr himlTrack, int
            iTrack, int dxHotspot, int dyHotspot);
        [DllImport("comctl32.dll", CharSet = CharSet.Auto)]
        public static extern bool ImageList_DragMove(int x, int y);
        [DllImport("comctl32.dll", CharSet = CharSet.Auto)]
        public static extern void ImageList_EndDrag();
        [DllImport("comctl32.dll", CharSet = CharSet.Auto)]
        public static extern bool ImageList_DragEnter(IntPtr hwndLock, int x, int y);
        [DllImport("comctl32.dll", CharSet = CharSet.Auto)]
        public static extern bool ImageList_DragLeave(IntPtr hwndLock);
        [DllImport("comctl32.dll", CharSet = CharSet.Auto)]
        public static extern bool ImageList_DragShowNolock(bool fShow);
        static DragHelper()
        {
            InitCommonControls();
        }
    }
}
