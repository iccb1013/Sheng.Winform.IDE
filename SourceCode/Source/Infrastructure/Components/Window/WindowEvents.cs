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
using IDesign.Composite.Events;
namespace Sheng.SailingEase.Infrastructure
{
    public class WindowFolderAddedEvent : CompositeEvent<WindowFolderEventArgs>
    {
    }
    public class WindowFolderRemovedEvent : CompositeEvent<WindowFolderEventArgs>
    {
    }
    public class WindowFolderUpdatedEvent : CompositeEvent<WindowFolderEventArgs>
    {
    }
    public class WindowAddedEvent : CompositeEvent<WindowEventArgs>
    {
    }
    public class WindowRemovedEvent : CompositeEvent<WindowRemovedEventArgs>
    {
    }
    public class WindowUpdatedEvent : CompositeEvent<WindowEventArgs>
    {
    }
    public class WindowMovedEvent : CompositeEvent<WindowMovedEventArgs>
    {
    }
    public class WindowMovedEventArgs
    {
        private string _windowId;
        public string WindowId
        {
            get { return _windowId; }
        }
        private string _folderId;
        public string FolderId
        {
            get { return _folderId; }
        }
        private string _originalFolderId;
        public string OriginalFolderId
        {
            get { return _originalFolderId; }
        }
        public WindowMovedEventArgs(string windowId, string folderId, string originalFolderId)
        {
            _windowId = windowId;
            _folderId = folderId;
            _originalFolderId = originalFolderId;
        }
    }
    public class WindowFolderMovedEvent : CompositeEvent<WindowFolderMovedEventArgs>
    {
    }
    public class WindowFolderMovedEventArgs
    {
        private string _folderId;
        public string FolderId
        {
            get { return _folderId; }
        }
        private string _parentId;
        public string ParentId
        {
            get { return _parentId; }
        }
        private string _originalParentId;
        public string OriginalParentId
        {
            get { return _originalParentId; }
        }
        public WindowFolderMovedEventArgs(string folderId, string parentId, string originalParentId)
        {
            _folderId = folderId;
            _parentId = parentId;
            _originalParentId = originalParentId;
        }
    }
}
