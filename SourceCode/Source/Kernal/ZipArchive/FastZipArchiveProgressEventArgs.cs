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
    public class FastZipArchiveProgressEventArgs : EventArgs
    {
        private ProgressEventArgs _progressEventArgs;
        internal FastZipArchiveProgressEventArgs(ProgressEventArgs e)
        {
            _progressEventArgs = e;
        }
        public bool ContinueRunning
        {
            get { return _progressEventArgs.ContinueRunning; }
            set { _progressEventArgs.ContinueRunning = value; }
        }
        public string Name
        {
            get { return _progressEventArgs.Name; }
        }
        public float PercentComplete
        {
            get { return _progressEventArgs.PercentComplete; }
        }
        public long Processed
        {
            get { return _progressEventArgs.Processed; }
        }
        public long Target
        {
            get { return _progressEventArgs.Target; }
        }
    }
}
