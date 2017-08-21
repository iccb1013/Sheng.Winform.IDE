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
using System.IO;
using System.Diagnostics;
namespace Sheng.SailingEase.Core
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class ResourceContentAttribute : Attribute
    {
        private List<string> _extensions = new List<string>();
        private string _contentFilter;
        public string ContentFilter
        {
            get { return _contentFilter; }
            set
            {
                _contentFilter = value;
                _extensions = new List<string>(_contentFilter.ToLower().Split(';'));
            }
        }
        public ResourceContentAttribute(string contentFilter)
        {
            ContentFilter = contentFilter;
        }
        public bool CanHandle(string file)
        {
            if (String.IsNullOrEmpty(file))
            {
                Debug.Assert(false, "文件名为空");
                throw new ArgumentException();
            }
            string extension = Path.GetExtension(file);
            return _extensions.Contains(extension.ToLower());
        }
    }
}
