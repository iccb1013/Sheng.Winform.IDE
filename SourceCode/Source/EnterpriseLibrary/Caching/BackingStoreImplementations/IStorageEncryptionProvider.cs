/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations
{
	[CustomFactory(typeof(StorageEncryptionProviderCustomFactory))]
    public interface IStorageEncryptionProvider 
    {
        byte[] Encrypt(byte[] plaintext);
        byte[] Decrypt(byte[] ciphertext);
    }
}
