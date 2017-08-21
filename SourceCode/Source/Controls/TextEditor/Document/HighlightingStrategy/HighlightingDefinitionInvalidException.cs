/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Runtime.Serialization;
namespace Sheng.SailingEase.Controls.TextEditor.Document
{
	[Serializable()]
	public class HighlightingDefinitionInvalidException : Exception
	{
		public HighlightingDefinitionInvalidException() : base()
		{
		}
		public HighlightingDefinitionInvalidException(string message) : base(message)
		{
		}
		public HighlightingDefinitionInvalidException(string message, Exception innerException) : base(message, innerException)
		{
		}
		protected HighlightingDefinitionInvalidException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
