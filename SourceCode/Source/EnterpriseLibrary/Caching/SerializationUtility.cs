/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
    public static class SerializationUtility
    {
        public static byte[] ToBytes(object value)
        {
            if (value == null)
            {
                return null;
            }
            byte[] inMemoryBytes;
            using (MemoryStream inMemoryData = new MemoryStream())
            {
                new BinaryFormatter().Serialize(inMemoryData, value);
                inMemoryBytes = inMemoryData.ToArray();
            }
            return inMemoryBytes;
        }
        public static object ToObject(byte[] serializedObject)
        {
            if (serializedObject == null)
            {
                return null;
            }
            using (MemoryStream dataInMemory = new MemoryStream(serializedObject))
            {
                return new BinaryFormatter().Deserialize(dataInMemory);
            }
        }
    }
}
