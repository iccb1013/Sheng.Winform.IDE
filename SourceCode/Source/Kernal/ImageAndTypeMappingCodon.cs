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
using System.Drawing;
namespace Sheng.SailingEase.Kernal
{
    public class ImageAndTypeMappingCodon
    {
        private Type _dataBoundType;
        public Type DataBoundType
        {
            get { return _dataBoundType; }
            set { _dataBoundType = value; }
        }
        private bool _actOnSubClass = false;
        public bool ActOnSubClass
        {
            get { return _actOnSubClass; }
            set { _actOnSubClass = value; }
        }
        private Image _image;
        public Image Image
        {
            get { return _image; }
            set { _image = value; }
        }
        public ImageAndTypeMappingCodon(Type dataBoundType, Image image)
            : this(dataBoundType, image, false)
        {
        }
        public ImageAndTypeMappingCodon(Type dataBoundType, Image image, bool actOnSubClass)
        {
            _dataBoundType = dataBoundType;
            _image = image;
            _actOnSubClass = actOnSubClass;
        }
    }
}
