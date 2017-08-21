/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.IO;
using System.Collections.Generic;
namespace Sheng.SailingEase.Kernal
{
    public interface IArchive
    {
        void Close();
        void AddFile(Stream stream, string entryName);
        void AddFile(string file);
        void AddFile(string file, string entryName);
        bool Container(string name);
        void Create();
        void CreateDirectory(string directory);
        void DeleteFile(string name);
        string GetFile(string file);
        void GetFile(string file, string outputFile);
        Stream GetFileStream(string file);
        List<string> GetFileList(string directory, string filter);
    }
}
