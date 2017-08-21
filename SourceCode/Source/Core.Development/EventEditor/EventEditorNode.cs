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
    public class EventEditorNode
    {
        private string _name = String.Empty;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private IEventEditorPanel _panel;
        public IEventEditorPanel Panel
        {
            get { return _panel; }
            set { _panel = value; }
        }
        private List<EventEditorNode> _childNodes = new List<EventEditorNode>();
        public List<EventEditorNode> ChildNodes
        {
            get { return _childNodes; }
            set { _childNodes = value; }
        }
        private int _imageIndex = 0;
        public int ImageIndex
        {
            get { return _imageIndex; }
            set { _imageIndex = value; }
        }
        private int _selectedImageIndex = 0;
        public int SelectedImageIndex
        {
            get { return _selectedImageIndex; }
            set { _selectedImageIndex = value; }
        }
        public EventEditorNode(string name)
        {
            Name = name;
        }
    }
}
