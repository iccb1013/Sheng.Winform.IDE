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
using ICSharpCode.SharpZipLib.Core;
namespace Sheng.SailingEase.Kernal
{
    public class FastZipArchiveScanEventArgs : EventArgs
    {
        private ScanEventArgs _scanEventArgs;
        internal FastZipArchiveScanEventArgs(ScanEventArgs e)
        {
            _scanEventArgs = e;
        }
        public bool ContinueRunning
        {
            get { return _scanEventArgs.ContinueRunning; }
            set { _scanEventArgs.ContinueRunning = value; }
        }
        public string Name
        {
            get { return _scanEventArgs.Name; }
        }
    }
}
