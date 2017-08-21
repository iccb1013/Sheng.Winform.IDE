/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
namespace Sheng.SailingEase.Controls.TextEditor.Document
{
	public class AbstractSegment : ISegment
	{
		protected int offset = -1;
		protected int length = -1;
		public virtual int Offset {
			get {
				return offset;
			}
			set {
				offset = value;
			}
		}
		public virtual int Length {
			get {
				return length;
			}
			set {
				length = value;
			}
		}
		public override string ToString()
		{
			return String.Format("[AbstractSegment: Offset = {0}, Length = {1}]",
			                     Offset,
			                     Length);
		}
	}
}
