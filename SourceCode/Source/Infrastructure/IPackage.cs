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
namespace Sheng.SailingEase.Infrastructure
{
    public interface IPackage
    {
        string PackageFile { get; }
        bool Container(string name);
        void DeleteFile(string name);
        void AddFile(string file);
        void AddFile(string file, string entryName);
        void AddFile(Stream stream, string entryName);
        void AddFileContent(Stream stream, string entryName);
        void AddFileContent(string content, string entryName);
        Stream GetFileStream(string file);
        string GetFileContent(string file);
        List<string> GetFileList(string directory);
        List<string> GetFileList(string directory, string filter);
        void Close();
    }
}
