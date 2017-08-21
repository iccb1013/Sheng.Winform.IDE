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
using ICSharpCode.SharpZipLib.Core;
using System.Collections;
namespace Sheng.SailingEase.Kernal
{
    public class ZipArchiveEx
    {
        public ZipArchiveEx()
        {
        }
        public void CreatePackage(string name)
        {
            ZipFile zipFile = ZipFile.Create(name);
            zipFile.BeginUpdate();
            ZipEntry entry = new ZipEntry("~package");
            entry.Size = 0;
            entry.CompressedSize = 0;
            zipFile.Add(entry);
            zipFile.CommitUpdate();
            zipFile.Close();
        }
        public void CreateDirectory(string file, string directory)
        {
            if (File.Exists(file) == false)
                throw new FileNotFoundException(file);
            ZipFile zipFile = new ZipFile(file);
            zipFile.BeginUpdate();
            zipFile.AddDirectory(directory);
            zipFile.CommitUpdate();
            zipFile.Close();
        }
        public bool Container(string file,string name)
        {
            if (File.Exists(file) == false)
                throw new FileNotFoundException(file);
            name = name.Replace("\\", "/");
            ZipFile zipFile = new ZipFile(file);
            bool result = zipFile.FindEntry(name, true) > 0;
            zipFile.Close();
            return result;
        }
        public void DeleteFile(string file, string name)
        {
            if (File.Exists(file) == false)
                throw new FileNotFoundException();
            ZipFile zipFile = new ZipFile(file);
            zipFile.BeginUpdate();
            zipFile.Delete(name);
            zipFile.CommitUpdate();
            zipFile.Close();
        }
        public void AddFile(string targetFile, string file)
        {
            AddFile(targetFile, file, null);
        }
        public void AddFile(string targetFile, string file, string entryName)
        {
           
            if (File.Exists(file) == false)
                throw new FileNotFoundException(file);
            if (File.Exists(targetFile) == false)
                throw new FileNotFoundException(targetFile);
            if (String.IsNullOrEmpty(entryName))
                entryName = Path.GetFileName(file);
            ZipFile zipFile = new ZipFile(targetFile);
            zipFile.BeginUpdate();
            zipFile.Add(file, entryName);
            zipFile.CommitUpdate();
            zipFile.Close();
        }
        public void AddFile(string targetFile, Stream stream, string entryName)
        {
            if (File.Exists(targetFile) == false)
                throw new FileNotFoundException(targetFile);
            if (stream == null)
                throw new ArgumentNullException();
            if (String.IsNullOrEmpty(entryName))
                throw new ArgumentNullException(entryName);
            stream.Seek(0, SeekOrigin.Begin);
            ZipArchiveStreamDataSource dataSource = new ZipArchiveStreamDataSource(stream);
            ZipFile zipFile = new ZipFile(targetFile);
            zipFile.BeginUpdate();
            zipFile.Add(dataSource, entryName);
            zipFile.CommitUpdate();
            zipFile.Close();
        }
        public string GetFile(string targetFile, string file)
        {
            string tempFile = Path.GetTempFileName();
            GetFile(targetFile, file, tempFile);
            return tempFile;
        }
        public void GetFile(string targetFile, string file, string outputFile)
        {
            if (File.Exists(targetFile) == false)
                throw new FileNotFoundException(targetFile);
            string entryFile = file.Replace('\\', '/');
            ZipFile zipFile = new ZipFile(targetFile);
            ZipEntry entry = zipFile.GetEntry(entryFile);
            if (entry == null)
                throw new FileNotFoundException(file);
            int size = 2048;
            byte[] data = new byte[2048];
            using (Stream stream = zipFile.GetInputStream(entry))
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
            zipFile.Close();
            return;
        }
        public FileStream GetFileStream(string targetFile, string file)
        {
            string filePath = GetFile(targetFile, file);
            FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return stream;
        }
        public List<string> GetFileList(string targetFile, string directory, string filter)
        {
            return GetFileList(targetFile, directory, filter, false);
        }
        public List<string> GetFileList(string targetFile, string directory, string filter, bool recursive)
        {
            if (String.IsNullOrEmpty(targetFile) || String.IsNullOrEmpty(targetFile))
            {
                throw new ArgumentException();
            }
            if (File.Exists(targetFile) == false)
                throw new FileNotFoundException(targetFile);
            List<string> fileList = new List<string>();
            string entryFile = directory.Replace('\\', '/');
            ZipFile zipFile = new ZipFile(targetFile);
            if (String.IsNullOrEmpty(filter))
                filter = "*.*";
            string regexFilter = PathHelper.GetFileNameRegexFilter(filter);
            NameFilter nameFilter = new NameFilter(regexFilter);
            IEnumerator enumerator = zipFile.GetEnumerator();
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
            zipFile.Close();
            return fileList;
        }
    }
}
