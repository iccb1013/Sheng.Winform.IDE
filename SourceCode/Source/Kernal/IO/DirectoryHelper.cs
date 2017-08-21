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
using System.Collections;
namespace Sheng.SailingEase.Kernal
{
    public static class DirectoryHelper
    {
        public static string GetRelativePath(string strPath1, string strPath2)
        {
            if (!strPath1.EndsWith("\\")) strPath1 += "\\";    
            int intIndex = -1, intPos = strPath1.IndexOf('\\');
            while (intPos >= 0)
            {
                intPos++;
                if (string.Compare(strPath1, 0, strPath2, 0, intPos, true) != 0) break;
                intIndex = intPos;
                intPos = strPath1.IndexOf('\\', intPos);
            }
            if (intIndex >= 0)
            {
                strPath2 = strPath2.Substring(intIndex);
                intPos = strPath1.IndexOf("\\", intIndex);
                while (intPos >= 0)
                {
                    strPath2 = "..\\" + strPath2;
                    intPos = strPath1.IndexOf("\\", intPos + 1);
                }
            }
            return strPath2;
        }
        public static string[] GetFiles(string path, string filter, bool recursive)
        {
            if (Directory.Exists(path) == false)
            {
                return new string[] { };
            }
            List<string> fileList = new List<string>();
            string[] arExtensions = filter.Split('|');
            foreach (string f in arExtensions)
            {
                string[] strFiles = Directory.GetFiles(path, f);
                fileList.AddRange(strFiles);
            }
            if (recursive)
            {
                foreach (string f in Directory.GetDirectories(path))
                {
                    fileList.AddRange(GetFiles(f, filter, true));
                }
            }
            return fileList.ToArray();
        }
    }
}
