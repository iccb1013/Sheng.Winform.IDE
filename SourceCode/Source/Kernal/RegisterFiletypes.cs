/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Win32;
using Sheng.SailingEase.Win32;
namespace Sheng.SailingEase.Kernal
{
    public static class RegisterFiletypes
    {
        static void NotifyShellAfterChanges()
        {
            Shell32.SHChangeNotify(Shell32.SHCNE_ASSOCCHANGED, Shell32.SHCNF_IDLIST, IntPtr.Zero, IntPtr.Zero);
        }
        const string explorerFileExts = @"Software\Microsoft\Windows\CurrentVersion\Explorer\FileExts";
        public static bool IsRegisteredFileType(string extension)
        {
            try
            {
                using (RegistryKey key = Registry.ClassesRoot.OpenSubKey("." + extension))
                {
                    if (key != null)
                        return true;
                }
            }
            catch (System.Security.SecurityException)
            {
            }
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(explorerFileExts + "\\." + extension))
                {
                    if (key != null)
                        return true;
                }
            }
            catch (System.Security.SecurityException)
            {
            }
            return false;
        }
        public static void RegisterFiletype(string extension, string description, string command, string icon)
        {
            try
            {
                RegisterFiletype(Registry.ClassesRoot, extension, description, command, icon);
            }
            catch (UnauthorizedAccessException)
            {
                try
                {
                    RegisterFiletype(Registry.CurrentUser.CreateSubKey("Software\\Classes"), extension, description, command, icon);
                }
                catch { }
            }
            NotifyShellAfterChanges();
        }
        static void RegisterFiletype(RegistryKey rootKey, string extension, string description, string command, string icon)
        {
            RegistryKey extKey, clsKey, openKey;
            extKey = rootKey.CreateSubKey("." + extension);
            string prev = (string)extKey.GetValue("", "");
            if (prev != "" && prev != ("SE." + extension + "file"))
            {
                extKey.SetValue("PreSD", extKey.GetValue(""));
            }
            extKey.SetValue("", "SE." + extension + "file");
            extKey.Close();
            try
            {
                extKey = Registry.CurrentUser.OpenSubKey(explorerFileExts + "\\." + extension, true);
                if (extKey != null)
                {
                    extKey.DeleteValue("Progid");
                    extKey.Close();
                }
            }
            catch { }
            clsKey = rootKey.CreateSubKey("SE." + extension + "file");
            clsKey.SetValue("", description);
            clsKey.CreateSubKey("DefaultIcon").SetValue("", '"' + icon + '"');
            openKey = clsKey.CreateSubKey("shell\\open\\command");
            openKey.SetValue("", command);
            openKey.Close();
            clsKey.Close();
        }
        public static void UnRegisterFiletype(string extension)
        {
            UnRegisterFiletype(extension, Registry.ClassesRoot);
            try
            {
                UnRegisterFiletype(extension, Registry.CurrentUser.CreateSubKey("Software\\Classes"));
            }
            catch { } 
            NotifyShellAfterChanges();
        }
        static void UnRegisterFiletype(string extension, RegistryKey root)
        {
            try
            {
                root.DeleteSubKeyTree("SE." + extension + "file");
            }
            catch { }
            try
            {
                RegistryKey extKey;
                extKey = root.OpenSubKey("." + extension, true);
                if (extKey == null) return;
                if ((string)extKey.GetValue("", "") != ("SE." + extension + "file")) return;
                string prev = (string)extKey.GetValue("PreSD", "");
                if (prev != "")
                {
                    extKey.SetValue("", prev);
                }
                extKey.Close();
                if (prev != null)
                {
                    root.DeleteSubKeyTree("." + extension);
                }
            }
            catch { }
        }
    }
}
