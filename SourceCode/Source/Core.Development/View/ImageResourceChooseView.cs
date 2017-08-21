/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Practices.Composite.Events;
using Sheng.SailingEase.Controls.Extensions;
using Sheng.SailingEase.Core.Development.Localisation;
using Sheng.SailingEase.Drawing;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Kernal;
namespace Sheng.SailingEase.Core.Development
{
    partial class ImageResourceChooseView : FormViewBase
    {
        private IResourceComponentService _resourceService = ServiceUnity.ResourceService;
        private IEventAggregator _eventAggregator = ServiceUnity.EventAggregator;
        private Dictionary<ImageResourceInfo, Image> _imageCache = new Dictionary<ImageResourceInfo, Image>();
        private ListBoxController _listBoxController;
        private ImageResourceInfo _selectedImageResource;
        public ImageResourceInfo SelectedImageResource
        {
            get
            {
                return _selectedImageResource;
            }
        }
        public ImageResourceChooseView()
        {
            InitializeComponent();
            Unity.ApplyResource(this);
            InitController();
            SubscribeEvent();
            this.Disposed += (sender, e) =>
            {
                foreach (var item in _imageCache)
                {
                    item.Value.Dispose();
                }
            };
        }
        private void FormImageResourceChoose_Load(object sender, EventArgs e)
        {
            LoadImageResource();
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void btnImport_Click(object sender, EventArgs e)
        {
            _resourceService.ImportImageResource();
        }
        private void _listBoxController_SelectedItemChanged(object sender, EventArgs e)
        {
            _selectedImageResource = _listBoxController.GetSelectedItem<ImageResourceInfo>();
            if (_selectedImageResource == null)
            {
                if (this.pictureBoxPreview.Image != null)
                {
                    this.pictureBoxPreview.Image = null;
                }
                return;
            }
            if (_imageCache.Keys.Contains(_selectedImageResource) == false)
            {
                Image thumbnailImage = DrawingTool.GetAutoScaleThumbnailImage(
                    _selectedImageResource.GetImage(), this.pictureBoxPreview.Width, this.pictureBoxPreview.Height);
                _imageCache.Add(_selectedImageResource, thumbnailImage);
            }
            this.pictureBoxPreview.Image = _imageCache[_selectedImageResource];
        }
        private void SubscribeEvent()
        {
            SubscriptionToken resourceAddedEventToken =
                _eventAggregator.GetEvent<ResourceAddedEvent>().Subscribe((e) =>
                {
                    _listBoxController.Add(_resourceService.GetImageResource(e.Name));
                });
            SubscriptionToken resourceUpdatedEventToken =
                _eventAggregator.GetEvent<ResourceUpdatedEvent>().Subscribe((e) =>
                {
                    ImageResourceInfo imageResourceInfo =
                        _listBoxController.Find<ImageResourceInfo>((resource) => {return resource.Name == e.Name; });
                    if(imageResourceInfo!=null)
                        _listBoxController.Replace(imageResourceInfo,_resourceService.GetImageResource(e.Name));
                });
            SubscriptionToken resourceRemovedEventToken =
                _eventAggregator.GetEvent<ResourceRemovedEvent>().Subscribe((e) =>
                {
                    ImageResourceInfo imageResourceInfo =
                        _listBoxController.Find<ImageResourceInfo>((resource) => { return resource.Name == e.Name; });
                    _listBoxController.Remove(imageResourceInfo);
                });
            this.Disposed += (sender, e) =>
            {
                _eventAggregator.GetEvent<ResourceAddedEvent>().Unsubscribe(resourceAddedEventToken);
                _eventAggregator.GetEvent<ResourceUpdatedEvent>().Unsubscribe(resourceUpdatedEventToken);
                _eventAggregator.GetEvent<ResourceRemovedEvent>().Unsubscribe(resourceRemovedEventToken);
            };
        }
        private void InitController()
        {
            _listBoxController = new ListBoxController(listBoxImageResourceList);
            _listBoxController.AllowNullSelection = true;
            _listBoxController.SelectedItemChanged += 
                new ListBoxController.OnSelectedItemChangedHandler(_listBoxController_SelectedItemChanged);
        }
        private void LoadImageResource()
        {
            this.listBoxImageResourceList.Items.Clear();
            this.pictureBoxPreview.Image = null;
            List<ImageResourceInfo> resources = _resourceService.GetImageResoruceList();
            _listBoxController.DataBind<ImageResourceInfo>(resources);
        }
    }
}
