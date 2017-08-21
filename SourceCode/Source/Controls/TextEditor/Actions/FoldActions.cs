/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using Sheng.SailingEase.Controls.TextEditor.Document;
namespace Sheng.SailingEase.Controls.TextEditor.Actions
{
	public class ToggleFolding : AbstractEditAction
	{
		public override void Execute(TextArea textArea)
		{
			List<FoldMarker> foldMarkers = textArea.Document.FoldingManager.GetFoldingsWithStart(textArea.Caret.Line);
			if (foldMarkers.Count != 0) {
				foreach (FoldMarker fm in foldMarkers)
					fm.IsFolded = !fm.IsFolded;
			} else {
				foldMarkers = textArea.Document.FoldingManager.GetFoldingsContainsLineNumber(textArea.Caret.Line);
				if (foldMarkers.Count != 0) {
					FoldMarker innerMost = foldMarkers[0];
					for (int i = 1; i < foldMarkers.Count; i++) {
						if (new TextLocation(foldMarkers[i].StartColumn, foldMarkers[i].StartLine) >
						    new TextLocation(innerMost.StartColumn, innerMost.StartLine))
						{
							innerMost = foldMarkers[i];
						}
					}
					innerMost.IsFolded = !innerMost.IsFolded;
				}
			}
			textArea.Document.FoldingManager.NotifyFoldingsChanged(EventArgs.Empty);
		}
	}
	public class ToggleAllFoldings : AbstractEditAction
	{
		public override void Execute(TextArea textArea)
		{
			bool doFold = true;
			foreach (FoldMarker fm in  textArea.Document.FoldingManager.FoldMarker) {
				if (fm.IsFolded) {
					doFold = false;
					break;
				}
			}
			foreach (FoldMarker fm in  textArea.Document.FoldingManager.FoldMarker) {
				fm.IsFolded = doFold;
			}
			textArea.Document.FoldingManager.NotifyFoldingsChanged(EventArgs.Empty);
		}
	}
	public class ShowDefinitionsOnly : AbstractEditAction
	{
		public override void Execute(TextArea textArea)
		{
			foreach (FoldMarker fm in  textArea.Document.FoldingManager.FoldMarker) {
				fm.IsFolded = fm.FoldType == FoldType.MemberBody || fm.FoldType == FoldType.Region;
			}
			textArea.Document.FoldingManager.NotifyFoldingsChanged(EventArgs.Empty);
		}
	}
}
