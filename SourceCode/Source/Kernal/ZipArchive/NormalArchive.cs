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
using Sheng.SailingEase.Kernal;
namespace Sheng.SailingEase.Kernal
{
    public class NormalArchive : IArchive
    {
        private ZipArchive _fileStorager;
        private string _packageName;
        private string _key;
        public NormalArchive(string name, string key)
        {
            _packageName = name;
            _key = key;
            _fileStorager = new ZipArchive(_packageName);
        }
        public void Close()
        {
            _fileStorager.Close();
        }
        public void Create()
        {
            _fileStorager.CreatePackage();
        }
        public void CreateDirectory(string directory)
        {
            _fileStorager.CreateDirectory(directory);
        }
        public bool Container(string name)
        {
            return _fileStorager.Container(name);
        }
        public void DeleteFile(string name)
        {
            _fileStorager.DeleteFile(name);
        }
        public void AddFile(string file)
        {
            AddFile(file, null);
        }
        public void AddFile(string file, string entryName)
        {
            _fileStorager.AddFile(file, entryName);
        }
        public void AddFile(Stream stream, string entryName)
        {
            string tempFile = Path.GetTempFileName();
            StreamReader streamReader = new StreamReader(stream);
            StreamWriter streamWriter = new StreamWriter(tempFile);
            streamWriter.Write(streamReader.ReadToEnd());
            streamReader.Close();
            streamReader.Dispose();
            streamWriter.Close();
            streamWriter.Dispose();
            _fileStorager.AddFile( tempFile, entryName);
            File.Delete(tempFile);
        }
        public string GetFile(string file)
        {
            return _fileStorager.GetFile( file);
        }
        public void GetFile(string file, string outputFile)
        {
            _fileStorager.GetFile(file, outputFile);
        }
        public Stream GetFileStream(string file)
        {
            Stream stream = _fileStorager.GetFileStream(file);
            return stream;
        }
        public List<string> GetFileList(string directory, string filter)
        {
            return _fileStorager.GetFileList(directory, filter);
        }
    }
}
