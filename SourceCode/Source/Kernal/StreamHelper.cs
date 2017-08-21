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
using System.IO;
namespace Sheng.SailingEase.Kernal
{
    public static class StreamHelper
    {
        private static Encoding _encoding = System.Text.Encoding.UTF8;
        public static Stream Parse(string str)
        {
            byte[] buffer = _encoding.GetBytes(str);
            MemoryStream stream = new MemoryStream(buffer);
            return stream;
        }
        public static string GetString(Stream stream)
        {
            StreamReader readStream = new StreamReader(stream, _encoding);
            string str = readStream.ReadToEnd();
            return str;
        }
    }
}
