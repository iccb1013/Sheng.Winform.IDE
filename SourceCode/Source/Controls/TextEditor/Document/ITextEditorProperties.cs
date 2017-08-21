/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Drawing;
using System.Text;
namespace Sheng.SailingEase.Controls.TextEditor.Document
{
	public interface ITextEditorProperties
	{
		bool CaretLine
		{
			get;
			set;
		}
		bool AutoInsertCurlyBracket { 
			get;
			set;
		}
		bool HideMouseCursor { 
			get;
			set;
		}
		bool IsIconBarVisible { 
			get;
			set;
		}
		bool AllowCaretBeyondEOL {
			get;
			set;
		}
		bool ShowMatchingBracket { 
			get;
			set;
		}
		bool CutCopyWholeLine {
			get;
			set;
		}
		System.Drawing.Text.TextRenderingHint TextRenderingHint { 
			get;
			set;
		}
		bool MouseWheelScrollDown {
			get;
			set;
		}
		bool MouseWheelTextZoom {
			get;
			set;
		}
		string LineTerminator {
			get;
			set;
		}
		LineViewerStyle LineViewerStyle { 
			get;
			set;
		}
		bool ShowInvalidLines { 
			get;
			set;
		}
		int VerticalRulerRow { 
			get;
			set;
		}
		bool ShowSpaces { 
			get;
			set;
		}
		bool ShowTabs { 
			get;
			set;
		}
		bool ShowEOLMarker { 
			get;
			set;
		}
		bool ConvertTabsToSpaces { 
			get;
			set;
		}
		bool ShowHorizontalRuler { 
			get;
			set;
		}
		bool ShowVerticalRuler { 
			get;
			set;
		}
		Encoding Encoding {
			get;
			set;
		}
		bool EnableFolding { 
			get;
			set;
		}
		bool ShowLineNumbers { 
			get;
			set;
		}
		int TabIndent { 
			get;
			set;
		}
		int IndentationSize {
			get;
			set;
		}
		IndentStyle IndentStyle { 
			get;
			set;
		}
		DocumentSelectionMode DocumentSelectionMode {
			get;
			set;
		}
		Font Font { 
			get;
			set;
		}
		FontContainer FontContainer {
			get;
		}
		BracketMatchingStyle  BracketMatchingStyle { 
			get;
			set;
		}
		bool SupportReadOnlySegments {
			get;
			set;
		}
	}
}
