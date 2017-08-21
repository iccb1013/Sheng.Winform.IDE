/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Caching.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Caching.Expirations
{
    [Serializable]
    [ComVisible(false)]
    public class FileDependency : ICacheItemExpiration
    {
        private readonly string dependencyFileName;
        private DateTime lastModifiedTime;
        public FileDependency(string fullFileName)
        {
            if (string.IsNullOrEmpty(fullFileName))
            {
                throw new ArgumentException("fullFileName", Resources.ExceptionNullFileName);
            }           
            dependencyFileName = Path.GetFullPath(fullFileName);
            EnsureTargetFileAccessible();
            if (!File.Exists(dependencyFileName))
            {
                throw new ArgumentException(Resources.ExceptionInvalidFileName, "fullFileName");
            }
            this.lastModifiedTime = File.GetLastWriteTime(fullFileName);
        }
		public string FileName
		{
			get { return dependencyFileName; }
		}
		public DateTime LastModifiedTime
		{
			get { return lastModifiedTime; }
		}
        public bool HasExpired()
        {
            EnsureTargetFileAccessible();
            if (File.Exists(this.dependencyFileName) == false)
            {
                return true;
            }
            DateTime currentModifiedTime = File.GetLastWriteTime(dependencyFileName);
            if (DateTime.Compare(lastModifiedTime, currentModifiedTime) != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void Notify()
        {
        }
        public void Initialize(CacheItem owningCacheItem)
        {
        }
        private void EnsureTargetFileAccessible()
        {
			string file = dependencyFileName;
            FileIOPermission permission = new FileIOPermission(FileIOPermissionAccess.Read, file);
            permission.Demand();
        }
    }
}
