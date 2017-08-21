/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
namespace Sheng.SailingEase.Controls.TextEditor.Document
{
	public class ResourceSyntaxModeProvider : ISyntaxModeFileProvider
	{
		List<SyntaxMode> syntaxModes = null;
		public ICollection<SyntaxMode> SyntaxModes {
			get {
				return syntaxModes;
			}
		}
		public ResourceSyntaxModeProvider()
		{
			Assembly assembly = typeof(SyntaxMode).Assembly;
			Stream syntaxModeStream = assembly.GetManifestResourceStream("Sheng.SailingEase.Controls.TextEditor.Resources.SyntaxModes.xml");
			if (syntaxModeStream != null) {
				syntaxModes = SyntaxMode.GetSyntaxModes(syntaxModeStream);
			} else {
				syntaxModes = new List<SyntaxMode>();
			}
		}
		public XmlTextReader GetSyntaxModeFile(SyntaxMode syntaxMode)
		{
			Assembly assembly = typeof(SyntaxMode).Assembly;
			return new XmlTextReader(assembly.GetManifestResourceStream("Sheng.SailingEase.Controls.TextEditor.Resources." + syntaxMode.FileName));
		}
		public void UpdateSyntaxModeList()
		{
		}
	}
}
