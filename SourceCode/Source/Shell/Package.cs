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
using Sheng.SailingEase.Infrastructure;
using System.IO;
using Sheng.SailingEase.Kernal;
using System.Diagnostics;
namespace Sheng.SailingEase.Shell
{
    class Package : IPackage
    {
        private IArchive _archive;
        public Package(string name)
        {
            _packageFile = name;
            _archive = new NormalArchive(_packageFile, "Dwzs4JOMaJk=");
        }
        internal void Create()
        {
            _archive.Create();
        }
        private string _packageFile;
        public string PackageFile
        {
            get { return _packageFile; }
        }
        public bool Container(string name)
        {
            return _archive.Container(name);
        }
        public void DeleteFile(string name)
        {
            _archive.DeleteFile(name);
        }
        public void AddFile(string file)
        {
            _archive.AddFile(file);
        }
        public void AddFile(string file, string entryName)
        {
            _archive.AddFile(file, entryName);
        }
        public void AddFile(Stream stream, string entryName)
        {
            _archive.AddFile(stream, entryName);
        }
        public void AddFileContent(Stream stream, string entryName)
        {
            _archive.AddFile(stream, entryName);
        }
        public void AddFileContent(string content, string entryName)
        {
            AddFileContent(StreamHelper.Parse(content), entryName);
        }
        public Stream GetFileStream(string file)
        {
            if (String.IsNullOrEmpty(file))
            {
                Debug.Assert(false, file + " 为空");
                throw new ArgumentNullException();
            }
            if (_archive.Container(file) == false)
            {
                Debug.Assert(false, file + " 不存在");
                throw new FileNotFoundException();
            }
            return _archive.GetFileStream(file);
        }
        public string GetFileContent(string file)
        {
            return StreamHelper.GetString(_archive.GetFileStream(file));
        }
        public List<string> GetFileList(string directory)
        {
            return GetFileList(directory, null);
        }
        public List<string> GetFileList(string directory, string filter)
        {
            return _archive.GetFileList(directory, filter);
        }
        public void Close()
        {
            _archive.Close();
        }
    }
}
