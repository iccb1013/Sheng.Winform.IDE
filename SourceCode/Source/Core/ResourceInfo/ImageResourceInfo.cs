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
using System.Diagnostics;
namespace Sheng.SailingEase.Core
{
    [ResourceContent(".jpg;.png;.gif;.bmp;.ico")]
    public class ImageResourceInfo : ResourceInfo
    {
        public Image GetImage()
        {
            Debug.Assert(this.ResourceStream != null);
            System.IO.Stream stream = this.ResourceStream;
            Image image = Image.FromStream(stream);
            return image;
        }
    }
}
