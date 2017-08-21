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
using System.Xml.Linq;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Core.Development.Localisation;
namespace Sheng.SailingEase.Core.Development
{
    public abstract class EventEditorAdapterAbstract
    {
        private EventEditorNode _defaultNode;
        internal EventEditorNode DefaultNode
        {
            get
            {
                if (_defaultNode == null)
                {
                    if (EditorNode != null && EditorNode.ChildNodes.Count > 0)
                        return EditorNode.ChildNodes[0];
                    else
                        return null;
                }
                else
                {
                    return this._defaultNode;
                }
            }
            private set
            {
                this._defaultNode = value;
            }
        }
        private List<IEventEditorPanel> _eventEditorPanelList;
        public List<IEventEditorPanel> EventEditorPanelList
        {
            get
            {
                return this._eventEditorPanelList;
            }
            protected set
            {
                this._eventEditorPanelList = value;
            }
        }
        private EventCollection _eventCollection;
        public EventCollection EventCollection
        {
            get { return _eventCollection; }
            set { _eventCollection = value; }
        }
        private EventBase _hostEvent;
        public EventBase HostEvent
        {
            get { return _hostEvent; }
        }
        public WindowEntity HostFormEntity
        {
            get { return _hostEvent.HostFormEntity as WindowEntity; }
        }
        private EventEditorNode _editorNode;
        public EventEditorNode EditorNode
        {
            get { return _editorNode; }
            protected set { _editorNode = value; }
        }
        public EventEditorAdapterAbstract(EventBase hostEvent)
        {
            _hostEvent = hostEvent;
            CreateEditorNode();
            CreateEventEditorNode();
        }
        protected virtual void CreateRootNode()
        {
            _editorNode = new EventEditorNode(Language.Current.EventEditorAdapterAbstract_EventEditorTreeNodeRoot);
            _editorNode.ImageIndex = EditorTreeNodeIcons.Property;
            _editorNode.SelectedImageIndex = EditorTreeNodeIcons.Property;
        }
        protected abstract void CreateEditorNode();
        private void CreateEventEditorNode()
        {
            CreateRootNode();
            if (_eventEditorPanelList == null)
                return;
            foreach (IEventEditorPanel panel in _eventEditorPanelList)
            {
                _editorNode.ChildNodes.Add(
                    AddEventParameterTreeNode(panel.PanelTitle, panel, panel.DefaultPanel));
            }
        }
        private EventEditorNode AddEventParameterTreeNode(string name, IEventEditorPanel panel, bool defaultNode)
        {
            EventEditorNode eventParameterTreeNode = new EventEditorNode(name);
            eventParameterTreeNode.ImageIndex = EditorTreeNodeIcons.EmptyIcon;
            eventParameterTreeNode.SelectedImageIndex = EditorTreeNodeIcons.Right;
            eventParameterTreeNode.Panel = panel;
            if (defaultNode)
            {
                this.DefaultNode = eventParameterTreeNode;
            }
            return eventParameterTreeNode;
        }
        public XElement GetParameterSetXml()
        {
            if (String.IsNullOrEmpty(this.HostEvent.Id))
            {
                this.HostEvent.Id = Guid.NewGuid().ToString();
            }
            XElement element = new XElement("Event",
                new XAttribute("Id", this.HostEvent.Id),
                new XAttribute("EventCode", EventDevTypes.Instance.GetProvideAttribute(this.HostEvent.GetType()).Code),
                new XAttribute("EventTime", 0),
                new XAttribute("Sys", false));
            foreach (IEventEditorPanel parameterPanel in this.EventEditorPanelList)
            {
                element.Add(parameterPanel.GetXml());
            }
            return element;
        }
        public void BeginEdit()
        {
            if (this.EventEditorPanelList != null)
            {
                foreach (IEventEditorPanel parameterPanel in this.EventEditorPanelList)
                {
                    parameterPanel.SetParameter(this.HostEvent);
                }
            }
        }
    }
}
