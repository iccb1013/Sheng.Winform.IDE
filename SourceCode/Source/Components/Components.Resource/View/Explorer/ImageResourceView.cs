/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sheng.SailingEase.Controls.Extensions;
using Microsoft.Practices.Composite.Events;
using Sheng.SailingEase.Infrastructure;
namespace Sheng.SailingEase.Components.ResourceComponent.View
{
    public partial class ImageResourceView : UserControl
    {
        private ResourceArchive _resourceArchive = ResourceArchive.Instance;
        private ResourceComponentService _resourceService = ResourceComponentService.Instance;
        private IEventAggregator _eventAggregator = ServiceUnity.EventAggregator;
        private ImageListViewController _controller;
        private ImageListViewContextMenuStrip _contextMenuStrip = new ImageListViewContextMenuStrip();
        private ImageListViewToolStrip _toolStrip = new ImageListViewToolStrip();
        private AddImageResourceCommand _addImageResourceCommand;
        private RemoveImageResourceCommand _removeImageResourceCommand;
        public ImageResourceView()
        {
            InitializeComponent();
            InitCommands();
            InitToolStrip();
            InitContextMenuStrip();
            InitController();
            SubscribeEvent();
        }
        private void ImageResourceView_Load(object sender, EventArgs e)
        {
            LoadResources();
        }
        private void InitCommands()
        {
            _addImageResourceCommand = new AddImageResourceCommand()
            {
                PreExcuteHandler = () => { imageListView.SuspendLayout(); },
                AfterExcuteHandler = () => { imageListView.ResumeLayout(); }
            };
            _removeImageResourceCommand = new RemoveImageResourceCommand()
            {
                CanExcuteHandler = () => { return _controller.SelectedCount > 0; },
                GetArgumentHandler = () => { return _controller.GetSelectedItems(); },
                PreExcuteHandler = () => { imageListView.SuspendLayout(); },
                AfterExcuteHandler = () => { imageListView.ResumeLayout(); }
            };
        }
        private void InitToolStrip()
        {
            _toolStrip.AddCommand = _addImageResourceCommand;
            _toolStrip.RemoveCommand = _removeImageResourceCommand;
            _toolStrip.View.Dock = DockStyle.Fill;
            this.topPanel.Controls.Add(_toolStrip.View);
        }
        private void InitContextMenuStrip()
        {
            _contextMenuStrip.AddCommand = _addImageResourceCommand;
            _contextMenuStrip.DeleteCommand = _removeImageResourceCommand;
        }
        private void InitController()
        {
            _controller = new ImageListViewController(this.imageListView);
            imageListView.ContextMenuStrip = _contextMenuStrip.View;
        }
        private void LoadResources()
        {
            _controller.DataBind(_resourceArchive.GetImageResoruceList());
        }
        private void SubscribeEvent()
        {
            SubscriptionToken resourceAddedEvent =
                _eventAggregator.GetEvent<ResourceAddedEvent>().Subscribe((e) =>
                {
                    _controller.Add(_resourceService.GetImageResource(e.Name));
                });
            SubscriptionToken resourceRemovedEvent =
                _eventAggregator.GetEvent<ResourceRemovedEvent>().Subscribe((e) =>
                {
                    _controller.Remove(e.Name);
                });
            SubscriptionToken resourceUpdatedEvent =
                _eventAggregator.GetEvent<ResourceUpdatedEvent>().Subscribe((e) =>
                {
                    _controller.Update(_resourceService.GetImageResource(e.Name));
                });
            this.Disposed += (sender, e) =>
            {
                _eventAggregator.GetEvent<ResourceAddedEvent>().Unsubscribe(resourceAddedEvent);
                _eventAggregator.GetEvent<ResourceRemovedEvent>().Unsubscribe(resourceRemovedEvent);
                _eventAggregator.GetEvent<ResourceUpdatedEvent>().Unsubscribe(resourceUpdatedEvent);
            };
        }
    }
}
