/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
namespace Sheng.SailingEase.Controls.TextEditor.Util
{
	static class LoggingService
	{
		public static void Debug(string text)
		{
			#if DEBUG
			Console.WriteLine(text);
			#endif
		}
	}
}
