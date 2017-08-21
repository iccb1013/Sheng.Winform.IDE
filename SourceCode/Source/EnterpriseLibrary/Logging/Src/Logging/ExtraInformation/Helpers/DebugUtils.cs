/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation.Helpers
{
    internal class DebugUtils : IDebugUtils
    {
        public string GetStackTraceWithSourceInfo(StackTrace stackTrace)
        {
            string atString = Properties.Resources.DebugInfo_SchemaHelperAtString;
            string unknownTypeString = Properties.Resources.DebugInfo_SchemaHelperUnknownType;
            string newLine = Environment.NewLine;
            StringBuilder stringBuilder = new StringBuilder(255);
            for (int i = 0; i < stackTrace.FrameCount; i++)
            {
                StackFrame stackFrame = stackTrace.GetFrame(i);
                stringBuilder.Append(atString);
                MethodBase method = stackFrame.GetMethod();
                Type t = method.DeclaringType;
                if (t != null)
                {
                    String nameSpace = t.Namespace;
                    if (nameSpace != null)
                    {
                        stringBuilder.Append(nameSpace);
                        if (stringBuilder != null)
                        {
                            stringBuilder.Append(".");
                        }
                    }
                    stringBuilder.Append(t.Name);
                    stringBuilder.Append(".");
                }
                stringBuilder.Append(method.Name);
                stringBuilder.Append("(");
                ParameterInfo[] arrParams = method.GetParameters();
                for (int j = 0; j < arrParams.Length; j++)
                {
                    String typeName = unknownTypeString;
                    if (arrParams[j].ParameterType != null)
                    {
                        typeName = arrParams[j].ParameterType.Name;
                    }
                    stringBuilder.Append((j != 0 ? ", " : "") + typeName + " " + arrParams[j].Name);
                }
                stringBuilder.Append(")");
                if (stackFrame.GetILOffset() != -1)
                {
                    String fileName = stackFrame.GetFileName();
                    if (fileName != null)
                    {
						stringBuilder.Append(
							String.Format(
								Properties.Resources.Culture, 
								Properties.Resources.DebugInfo_SchemaHelperLine, 
								fileName, 
								stackFrame.GetFileLineNumber()));
                    }
                }
                if (i != stackTrace.FrameCount - 1)
                {
                    stringBuilder.Append(newLine);
                }
            }
            return stringBuilder.ToString();
        }
    }
}
