/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Xml;
namespace Sheng.SailingEase.Controls.TextEditor.Document
{
	public interface ISyntaxModeFileProvider
	{
		ICollection<SyntaxMode> SyntaxModes {
			get;
		}
		XmlTextReader GetSyntaxModeFile(SyntaxMode syntaxMode);
		void UpdateSyntaxModeList();
	}
}
