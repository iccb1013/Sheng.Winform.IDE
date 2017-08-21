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
using System.Security.Cryptography;
namespace Sheng.SailingEase.Kernal
{
    public static class AESEncryption
    {
        const string PasswordSalt = "PasswordSalt";
        public static void Encrypt(Stream inStream, Stream outStream, string password)
        {
            byte[] buffer = new byte[2048];
            int readLength;
            outStream.SetLength(0);
            inStream.Seek(0, SeekOrigin.Begin);
            using (Aes aes = new AesManaged())
            {
                Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(PasswordSalt));
                aes.Key = deriveBytes.GetBytes(128 / 8);
                outStream.Write(BitConverter.GetBytes(aes.IV.Length), 0, sizeof(int));
                outStream.Write(aes.IV, 0, aes.IV.Length);
                CryptoStream cryptoStream = new CryptoStream(outStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
                while (true)
                {
                    readLength = inStream.Read(buffer, 0, buffer.Length);
                    cryptoStream.Write(buffer, 0, readLength);
                    if (readLength == 0)
                        break;
                }
                cryptoStream.FlushFinalBlock();
            }
            outStream.Seek(0, SeekOrigin.Begin);
        }
        public static void Decrypt(Stream inStream, Stream outStream, string password)
        {
            byte[] buffer = new byte[2048];
            int readLength;
            outStream.SetLength(0);
            inStream.Seek(0, SeekOrigin.Begin);
            using (Aes aes = new AesManaged())
            {
                Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(PasswordSalt));
                aes.Key = deriveBytes.GetBytes(128 / 8);
                aes.IV = ReadByteArray(inStream);
                CryptoStream cryptoStream = new CryptoStream(inStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
                while (true)
                {
                    readLength = cryptoStream.Read(buffer, 0, buffer.Length);
                    outStream.Write(buffer, 0, readLength);
                    if (readLength == 0)
                        break;
                }
            }
            outStream.Seek(0, SeekOrigin.Begin);
        }
        private static byte[] ReadByteArray(Stream s)
        {
            byte[] rawLength = new byte[sizeof(int)];
            if (s.Read(rawLength, 0, rawLength.Length) != rawLength.Length)
            {
                throw new SystemException("Stream did not contain properly formatted byte array");
            }
            byte[] buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
            if (s.Read(buffer, 0, buffer.Length) != buffer.Length)
            {
                throw new SystemException("Did not read byte array properly");
            }
            return buffer;
        }
    }
}
