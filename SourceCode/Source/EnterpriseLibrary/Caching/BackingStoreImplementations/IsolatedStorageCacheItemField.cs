/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.IO;
using System.IO.IsolatedStorage;
namespace Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations
{
    internal class IsolatedStorageCacheItemField
    {
        private string fieldName;
        private string fileSystemLocation;
        private IsolatedStorageFile storage;
        private IStorageEncryptionProvider encryptionProvider;
        public IsolatedStorageCacheItemField(IsolatedStorageFile storage, string fieldName,
                                             string fileSystemLocation, IStorageEncryptionProvider encryptionProvider)
        {
            this.fieldName = fieldName;
            this.fileSystemLocation = fileSystemLocation;
            this.storage = storage;
            this.encryptionProvider = encryptionProvider;
        }
        public void Write(object itemToWrite, bool encrypted)
        {
            using (IsolatedStorageFileStream fileStream =
                new IsolatedStorageFileStream(fileSystemLocation + @"\" + fieldName, FileMode.CreateNew, FileAccess.Write, FileShare.None, storage))
            {
                WriteField(itemToWrite, fileStream, encrypted);
            }
        }
        public void Overwrite(object itemToWrite)
        {
            using (IsolatedStorageFileStream fileStream =
                new IsolatedStorageFileStream(fileSystemLocation + @"\" + fieldName, FileMode.Truncate, FileAccess.Write, FileShare.None, storage))
            {
                WriteField(itemToWrite, fileStream, false);
            }
        }
        public object Read(bool encrypted)
        {
            using (IsolatedStorageFileStream fileStream =
                new IsolatedStorageFileStream(fileSystemLocation + @"\" + fieldName, FileMode.Open, FileAccess.Read, FileShare.None, storage))
            {
                return ReadField(fileStream, encrypted);
            }
        }
        protected virtual void WriteField(object itemToWrite, IsolatedStorageFileStream fileStream, bool encrypted)
        {
            byte[] serializedKey = SerializationUtility.ToBytes(itemToWrite);
            if (encrypted)
            {
                serializedKey = EncryptValue(serializedKey);
            }
            if (serializedKey != null)
            {
                fileStream.Write(serializedKey, 0, serializedKey.Length);
            }
        }
        protected virtual object ReadField(IsolatedStorageFileStream fileStream, bool encrypted)
        {
            if (fileStream.Length == 0)
            {
                return null;
            }
            byte[] fieldBytes = new byte[fileStream.Length];
            fileStream.Read(fieldBytes, 0, fieldBytes.Length);
            if (encrypted)
            {
                fieldBytes = DecryptValue(fieldBytes);
            }
            object fieldValue = SerializationUtility.ToObject(fieldBytes);
            return fieldValue;
        }
        private byte[] EncryptValue(byte[] valueBytes)
        {
            if (encryptionProvider != null)
            {
                valueBytes = encryptionProvider.Encrypt(valueBytes);
            }
            return valueBytes;
        }
        private byte[] DecryptValue(byte[] fieldBytes)
        {
            if (encryptionProvider != null)
            {
                fieldBytes = encryptionProvider.Decrypt(fieldBytes);
            }
            return fieldBytes;
        }
    }
}
