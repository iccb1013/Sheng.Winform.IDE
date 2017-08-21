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
namespace Sheng.SailingEase.Kernal
{
    public class FastZipArchive
    {
        private FastZip fastZip;
        private FastZipEvents events = new FastZipEvents();
        public FastZipArchive()
        {
            events.ProcessDirectory = new ProcessDirectoryHandler(ReportProcessDirectory);
            events.ProcessFile = new ProcessFileHandler(ReportProcessFile);
            events.Progress = new ProgressHandler(ReportProgress);
            events.ProgressInterval = TimeSpan.FromSeconds(1);
            fastZip = new FastZip(events);
            fastZip.CreateEmptyDirectories = true;
            fastZip.RestoreAttributesOnExtract = false;
            fastZip.RestoreDateTimeOnExtract = false;
        }
        public void PackageDirectory(string sourceDirectory, string targetFile)
        {
            if (Directory.Exists(sourceDirectory) == false)
                throw new DirectoryNotFoundException(sourceDirectory);
            fastZip.CreateZip(targetFile, sourceDirectory, true, String.Empty);
        }
        public string ExtractPackage(string file)
        {
            string tempPath = Path.Combine(Path.GetTempPath(), Path.GetFileName(file));
            ExtractPackage(file, tempPath, String.Empty);
            return tempPath;
        }
        public void ExtractPackage(string file, string targetDirectory)
        {
            ExtractPackage(file, targetDirectory, String.Empty);
        }
        public void ExtractPackage(string file, string targetDirectory, string fileFilter)
        {
            if (File.Exists(file) == false)
                throw new FileNotFoundException(file);
            if (Directory.Exists(targetDirectory) == false)
                Directory.CreateDirectory(targetDirectory);
            fastZip.ExtractZip(file, targetDirectory, fileFilter);
        }
        private void ReportProgress(object sender, ProgressEventArgs e)
        {
            if (OnProcess != null)
            {
                FastZipArchiveProgressEventArgs args = new FastZipArchiveProgressEventArgs(e);
                OnProcess(this, args);
            }
        }
        private void ReportProcessFile(object sender, ScanEventArgs e)
        {
            if (OnProcessFile != null)
            {
                FastZipArchiveScanEventArgs args = new FastZipArchiveScanEventArgs(e);
                OnProcessFile(this, args);
            }
        }
        private void ReportProcessDirectory(object sender, DirectoryEventArgs e)
        {
            if (OnProcessDirectory != null)
            {
                FastZipArchiveDirectoryEventArgs args = new FastZipArchiveDirectoryEventArgs(e);
                OnProcessDirectory(this, args);
            }
        }
        public delegate void OnProgressHandler(object sender, FastZipArchiveProgressEventArgs e);
        public event OnProgressHandler OnProcess;
        public delegate void OnProcessFileHandler(object sender, FastZipArchiveScanEventArgs e);
        public event OnProcessFileHandler OnProcessFile;
        public delegate void OnProcessDirectoryHandler(object sender, FastZipArchiveDirectoryEventArgs e);
        public event OnProcessDirectoryHandler OnProcessDirectory;
    }
}
