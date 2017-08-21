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
    public class EncryptionArchive : IArchive
    {
        private ZipArchive _fileStorager;
        private string _packageName;
        private string _key;
        public EncryptionArchive(string name, string key)
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
            string tempFile = Path.Combine(Path.GetTempPath(), Path.GetFileName(file));
            Encryption.EncryptFile(file, tempFile, _key);
            _fileStorager.AddFile(tempFile, entryName);
            File.Delete(tempFile);
        }
        public void AddFile(Stream stream, string entryName)
        {
            string tempFile = Path.GetTempFileName();
            FileStream outStream = new FileStream(tempFile, FileMode.OpenOrCreate, FileAccess.Write);
            Encryption.EncryptStream(stream, outStream, _key);
            outStream.Close();
            _fileStorager.AddFile(tempFile, entryName);
            File.Delete(tempFile);
            outStream.Close();
            outStream.Dispose();
        }
        public string GetFile(string file)
        {
            return _fileStorager.GetFile(file);
        }
        public void GetFile(string file, string outputFile)
        {
            _fileStorager.GetFile(file, outputFile);
        }
        public Stream GetFileStream(string file)
        {
            Stream stream = _fileStorager.GetFileStream(file);
            Stream outStream = new MemoryStream();
            Encryption.DecryptStream(stream, outStream, _key);
            return outStream;
        }
        public List<string> GetFileList(string directory, string filter)
        {
            return _fileStorager.GetFileList(directory, filter);
        }
    }
}
