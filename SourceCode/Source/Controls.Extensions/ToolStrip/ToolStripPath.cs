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
using System.Text.RegularExpressions;
using System.Diagnostics;
namespace Sheng.SailingEase.Controls.Extensions
{
    public class ToolStripPath
    {
        private List<ToolStripPathPoint> _pathPoints = new List<ToolStripPathPoint>();
        public List<ToolStripPathPoint> PathPoints
        {
            get { return _pathPoints; }
        }
        public bool IsEmpty
        {
            get { return _pathPoints == null || _pathPoints.Count == 0; }
        }
        public ToolStripPath()
        {
        }
        public ToolStripPath(string path)
        {
            ParsePath(path);
        }
        private void ParsePath(string path)
        {
            if (String.IsNullOrEmpty(path))
                return;
            _pathPoints.Clear();
            string[] paths = path.Split('/');
            foreach (string pathPoint in paths)
            {
                _pathPoints.Add(new ToolStripPathPoint(pathPoint));
            }
        }
        public ToolStripPath Cut(int index)
        {
            ToolStripPath newPath = new ToolStripPath();
            for (int i = index; i < _pathPoints.Count; i++)
            {
                newPath.PathPoints.Add(_pathPoints[i]);
            }
            return newPath;
        }
    }
    public class ToolStripPathPoint
    {
        /*
         * 使用了零宽断言
         *  \w	匹配字母或数字或下划线或汉字
         */
        static Regex regexPath = new Regex(@"\w*(?=\[\d*\])|^\w*$", RegexOptions.Singleline);
        static Regex regexIndex = new Regex(@"(?<=\[)\d*(?=\])", RegexOptions.Singleline);
        public string Name { get; set; }
        public Nullable<int> Index { get; set; }
        public ToolStripPathPoint(string pathPoint)
        {
            Debug.Assert(String.IsNullOrEmpty(pathPoint) == false, "pathPoint为null或为空");
            if (String.IsNullOrEmpty(pathPoint) == false)
            {
                Match match;
                match = regexPath.Match(pathPoint);
                if (match.Success)
                {
                    Name = match.Value;
                }
                match = regexIndex.Match(pathPoint);
                if (match.Success)
                {
                    Index = Int32.Parse(match.Value);
                }
            }
        }
    }
}
