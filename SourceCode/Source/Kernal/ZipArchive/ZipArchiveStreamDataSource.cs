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
using ICSharpCode.SharpZipLib.Zip;
using System.IO;
namespace Sheng.SailingEase.Kernal
{
    class ZipArchiveStreamDataSource : IStaticDataSource
    {
        private Stream _stream;
        public ZipArchiveStreamDataSource(Stream stream)
        {
            _stream = stream;
        }
        public Stream GetSource()
        {
            return _stream;
        }
    }
}
