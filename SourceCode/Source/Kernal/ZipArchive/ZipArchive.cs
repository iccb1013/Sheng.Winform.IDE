/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
namespace Sheng.SailingEase.Kernal
{
    public class ZipArchive : IDisposable
    {
        private string _file;
        private ZipFile _zipFile;
        public ZipArchive(string file)
        {
            _file = file;
            if (File.Exists(file))
            {
                _zipFile = new ZipFile(file);
            }
        }
        private bool FileInitialized()
        {
            return _zipFile != null;
        }
        public void Close()
        {
            if (FileInitialized() == false)
                return;
            _zipFile.Close();
        }
        public void CreatePackage()
        {
            if (FileInitialized())
            {
                throw new InvalidOperationException();
            }
            _zipFile = ZipFile.Create(_file);
            _zipFile.BeginUpdate();
            ZipEntry entry = new ZipEntry("~package");
            entry.Size = 0;
            entry.CompressedSize = 0;
            _zipFile.Add(entry);
            _zipFile.CommitUpdate();
        }
        public void CreateDirectory(string directory)
        {
            if (FileInitialized() == false)
                CreatePackage();
            _zipFile.BeginUpdate();
            _zipFile.AddDirectory(directory);
            _zipFile.CommitUpdate();
        }
        public bool Container(string name)
        {
            if (FileInitialized() == false)
                CreatePackage();
            name = name.Replace("\\", "/");
            bool result = _zipFile.FindEntry(name, true) > 0;
            return result;
        }
        public void DeleteFile(string name)
        {
            if (FileInitialized() == false)
                CreatePackage();
            _zipFile.BeginUpdate();
            _zipFile.Delete(name);
            _zipFile.CommitUpdate();
        }
        public void AddFile(string file)
        {
            AddFile(file, null);
        }
        public void AddFile(string file, string entryName)
        {
           
            if (File.Exists(file) == false)
                throw new FileNotFoundException(file);
            if (FileInitialized() == false)
                CreatePackage();
            if (String.IsNullOrEmpty(entryName))
                entryName = Path.GetFileName(file);
            _zipFile.BeginUpdate();
            _zipFile.Add(file, entryName);
            _zipFile.CommitUpdate();
        }
        public void AddFile(Stream stream, string entryName)
        {
            if (stream == null)
                throw new ArgumentNullException();
            if (FileInitialized() == false)
                CreatePackage();
            if (String.IsNullOrEmpty(entryName))
                throw new ArgumentNullException(entryName);
            stream.Seek(0, SeekOrigin.Begin);
            ZipArchiveStreamDataSource dataSource = new ZipArchiveStreamDataSource(stream);
            _zipFile.BeginUpdate();
            _zipFile.Add(dataSource, entryName);
            _zipFile.CommitUpdate();
        }
        public string GetFile(string file)
        {
            string tempFile = Path.GetTempFileName();
            GetFile(file, tempFile);
            return tempFile;
        }
        public void GetFile(string file, string outputFile)
        {
            if (FileInitialized() == false)
                CreatePackage();
            string entryFile = file.Replace('\\', '/');
            ZipEntry entry = _zipFile.GetEntry(entryFile);
            if (entry == null)
                throw new FileNotFoundException(file);
            int size = 2048;
            byte[] data = new byte[2048];
            using (Stream stream = _zipFile.GetInputStream(entry))
            {
                FileStream fs = File.Create(outputFile);
                while (true)
                {
                    size = stream.Read(data, 0, data.Length);
                    if (size > 0)
                    {
                        fs.Write(data, 0, size);
                    }
                    else
                    {
                        break;
                    }
                }
                fs.Close();
            }
            return;
        }
        public FileStream GetFileStream(string file)
        {
            string filePath = GetFile(file);
            FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return stream;
        }
        public List<string> GetFileList(string directory, string filter)
        {
            return GetFileList(directory, filter, false);
        }
        public List<string> GetFileList(string directory, string filter, bool recursive)
        {
            if (String.IsNullOrEmpty(directory))
            {
                throw new ArgumentException();
            }
            if (FileInitialized() == false)
                CreatePackage();
            List<string> fileList = new List<string>();
            string entryFile = directory.Replace('\\', '/');
            if (String.IsNullOrEmpty(filter))
                filter = "*.*";
            string regexFilter = PathHelper.GetFileNameRegexFilter(filter);
            NameFilter nameFilter = new NameFilter(regexFilter);
            IEnumerator enumerator = _zipFile.GetEnumerator();
            while (enumerator.MoveNext())
            {
                ZipEntry zipEntry = (ZipEntry)enumerator.Current;
                string entryPath = Path.GetDirectoryName(zipEntry.Name);
                if (recursive && entryPath.StartsWith(directory) == false)
                    continue;
                if (recursive == false && entryPath != directory)
                    continue;
                if (zipEntry.IsDirectory == false && nameFilter.IsMatch(Path.GetFileName(zipEntry.Name)))
                {
                    string file = zipEntry.Name.Remove(0, directory.Length);
                    file = file.TrimStart('/');
                    fileList.Add(file);
                }
            }
            return fileList;
        }
        public void Dispose()
        {
            Close();
        }
    }
}
