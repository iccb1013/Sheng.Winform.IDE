/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Drawing;
namespace Sheng.SailingEase.Controls.TextEditor.Util
{
	class TipSpacer: TipSection
	{
		SizeF spacerSize;
		public TipSpacer(Graphics graphics, SizeF size): base(graphics)
		{
			spacerSize = size;
		}
		public override void Draw(PointF location)
		{
		}
		protected override void OnMaximumSizeChanged()
		{
			base.OnMaximumSizeChanged();
			SetRequiredSize(new SizeF
			                (Math.Min(MaximumSize.Width, spacerSize.Width),
			                Math.Min(MaximumSize.Height, spacerSize.Height)));
		}
	}
}
