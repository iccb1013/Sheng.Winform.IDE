/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.IO;
using System.Text;
namespace Sheng.SailingEase.Controls.TextEditor.Util
{
	public static class FileReader
	{
		public static bool IsUnicode(Encoding encoding)
		{
			int codepage = encoding.CodePage;
			return codepage == 65001 || codepage == 65000 || codepage == 1200 || codepage == 1201;
		}
		public static string ReadFileContent(Stream fs, ref Encoding encoding)
		{
			using (StreamReader reader = OpenStream(fs, encoding)) {
				reader.Peek();
				encoding = reader.CurrentEncoding;
				return reader.ReadToEnd();
			}
		}
		public static string ReadFileContent(string fileName, Encoding encoding)
		{
			using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
				return ReadFileContent(fs, ref encoding);
			}
		}
		public static StreamReader OpenStream(Stream fs, Encoding defaultEncoding)
		{
			if (fs == null)
				throw new ArgumentNullException("fs");
			if (fs.Length >= 2) {
				int firstByte = fs.ReadByte();
				int secondByte = fs.ReadByte();
				switch ((firstByte << 8) | secondByte) {
					case 0x0000: 
					case 0xfffe: 
					case 0xfeff: 
					case 0xefbb: 
						fs.Position = 0;
						return new StreamReader(fs);
					default:
						return AutoDetect(fs, (byte)firstByte, (byte)secondByte, defaultEncoding);
				}
			} else {
				if (defaultEncoding != null) {
					return new StreamReader(fs, defaultEncoding);
				} else {
					return new StreamReader(fs);
				}
			}
		}
		static StreamReader AutoDetect(Stream fs, byte firstByte, byte secondByte, Encoding defaultEncoding)
		{
			int max = (int)Math.Min(fs.Length, 500000); 
			const int ASCII = 0;
			const int Error = 1;
			const int UTF8  = 2;
			const int UTF8Sequence = 3;
			int state = ASCII;
			int sequenceLength = 0;
			byte b;
			for (int i = 0; i < max; i++) {
				if (i == 0) {
					b = firstByte;
				} else if (i == 1) {
					b = secondByte;
				} else {
					b = (byte)fs.ReadByte();
				}
				if (b < 0x80) {
					if (state == UTF8Sequence) {
						state = Error;
						break;
					}
				} else if (b < 0xc0) {
					if (state == UTF8Sequence) {
						--sequenceLength;
						if (sequenceLength < 0) {
							state = Error;
							break;
						} else if (sequenceLength == 0) {
							state = UTF8;
						}
					} else {
						state = Error;
						break;
					}
				} else if (b >= 0xc2 && b < 0xf5) {
					if (state == UTF8 || state == ASCII) {
						state = UTF8Sequence;
						if (b < 0xe0) {
							sequenceLength = 1; 
						} else if (b < 0xf0) {
							sequenceLength = 2; 
						} else {
							sequenceLength = 3; 
						}
					} else {
						state = Error;
						break;
					}
				} else {
					state = Error;
					break;
				}
			}
			fs.Position = 0;
			switch (state) {
				case ASCII:
				case Error:
					if (IsUnicode(defaultEncoding)) {
						defaultEncoding = Encoding.Default; 
					}
					return new StreamReader(fs, defaultEncoding);
				default:
					return new StreamReader(fs);
			}
		}
	}
}
