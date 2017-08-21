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
    public class FastZipArchiveDirectoryEventArgs
    {
        private DirectoryEventArgs _directoryEventArgs;
        public FastZipArchiveDirectoryEventArgs(DirectoryEventArgs e)
        {
            _directoryEventArgs = e;
        }
        public bool HasMatchingFiles
        {
            get { return _directoryEventArgs.HasMatchingFiles; }
        }
    }
}
