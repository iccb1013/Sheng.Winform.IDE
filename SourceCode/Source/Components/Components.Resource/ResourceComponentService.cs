/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Kernal;
using Sheng.SailingEase.Localisation;
using Sheng.SailingEase.Components.ResourceComponent.Localisation;
using System.Diagnostics;
namespace Sheng.SailingEase.Components.ResourceComponent
{
    public class ResourceComponentService : IResourceComponentService
    {
        private ResourceArchive _resourceArchive = ResourceArchive.Instance;
        private static InstanceLazy<ResourceComponentService> _instance =
            new InstanceLazy<ResourceComponentService>(() => new ResourceComponentService());
        public static ResourceComponentService Instance
        {
            get { return _instance.Value; }
        }
        public bool Container(string resourceName)
        {
            if (String.IsNullOrEmpty(resourceName))
            {
                Debug.Assert(false, "resourceName 为空");
                throw new ArgumentNullException();
            }
            return _resourceArchive.Container(resourceName);
        }
        public void ImportImageResource()
        {
            string file;
            if (DialogUnity.OpenFile(Constant.RESOURCE_IMAGE_DIALOG_FILTER, out file) == false)
                return;
            string fileName = Path.GetFileName(file);
            if (_resourceArchive.Container(fileName))
            {
                if (MessageBox.Show(Language.Current.ResourceComponentService_OverrideConfirm,
                        CommonLanguage.Current.MessageCaption_Notice,
                        MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                {
                    return;
                }
            }
            _resourceArchive.Add(file);
        }
        public List<ImageResourceInfo> GetImageResoruceList()
        {
            return _resourceArchive.GetImageResoruceList();
        }
        public ImageResourceInfo GetImageResource(string name)
        {
            return _resourceArchive.GetImageResoruce(name);
        }
    }
}
