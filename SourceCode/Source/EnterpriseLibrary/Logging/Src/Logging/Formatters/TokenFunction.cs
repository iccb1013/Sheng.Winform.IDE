/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Text;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Formatters
{
	[Obsolete("Use the TokenHandler delegate instead.")]
    public abstract class TokenFunction
    {
        private string startDelimiter = string.Empty;
        private string endDelimiter = ")}";
        protected TokenFunction(string tokenStartDelimiter)
        {
            if (tokenStartDelimiter == null || tokenStartDelimiter.Length == 0)
            {
                throw new ArgumentNullException("tokenStartDelimiter");
            }
            this.startDelimiter = tokenStartDelimiter;
        }
        protected TokenFunction(string tokenStartDelimiter, string tokenEndDelimiter)
        {
            if (tokenStartDelimiter == null || tokenStartDelimiter.Length == 0)
            {
                throw new ArgumentNullException("tokenStartDelimiter");
            }
            if (tokenEndDelimiter == null || tokenEndDelimiter.Length == 0)
            {
                throw new ArgumentNullException("tokenEndDelimiter");
            }
            this.startDelimiter = tokenStartDelimiter;
            this.endDelimiter = tokenEndDelimiter;
        }
        public virtual void Format(StringBuilder messageBuilder, LogEntry log)
        {
            int pos = 0;
            while (pos < messageBuilder.Length)
            {
                string messageString = messageBuilder.ToString();
                if (messageString.IndexOf(this.startDelimiter) == -1)
                {
                    break;
                }
                string tokenTemplate = GetInnerTemplate(pos, messageString);
                string tokenToReplace = this.startDelimiter + tokenTemplate + this.endDelimiter;
                pos = messageBuilder.ToString().IndexOf(tokenToReplace);
                string tokenValue = FormatToken(tokenTemplate, log);
                messageBuilder.Replace(tokenToReplace, tokenValue);
            }
        }
        public abstract string FormatToken(string tokenTemplate, LogEntry log);
        protected virtual string GetInnerTemplate(int startPos, string message)
        {
            int tokenStartPos = message.IndexOf(this.startDelimiter, startPos) + this.startDelimiter.Length;
            int endPos = message.IndexOf(this.endDelimiter, tokenStartPos);
            return message.Substring(tokenStartPos, endPos - tokenStartPos);
        }
    }
}
