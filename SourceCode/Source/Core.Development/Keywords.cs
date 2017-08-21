/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Sheng.SailingEase.Core.Development
{
    public static class Keywords
    {
        private static List<string> _csharpKeywords = new List<string>();
        static Keywords()
        {
            InitCSharpKeywords();
        }
        static void InitCSharpKeywords()
        {
            _csharpKeywords.Clear();
            _csharpKeywords.AddRange(new string[] { 
                "abstract","event","new","struct","as","explicit","null","switch","base","extern","object","this","bool","false",
                "operator","throw","break","finally","out","true","byte","fixed","override","try","case","float","params","typeof",
                "catch","for","private","uint","char","foreach","protected","ulong","checked","goto","public","unchecked","class",
                "if","readonly","unsafe","const","implicit","ref","ushort","continue","in","return","using","decimal","int","sbyte","virtual",
                "default","interface","sealed","volatile","delegate","internal","short","void","do","is","sizeof","while","double","lock",
                "stackalloc","else","long","static","enum","namespace","string"});
            _csharpKeywords.AddRange(new string[] { 
                 "from","get","group","into","join","let","orderby","partial","partial","select","set","value","where","where","yield" });
        }
        public static bool Container(string word)
        {
            return _csharpKeywords.Contains(word);
        }
    }
}
