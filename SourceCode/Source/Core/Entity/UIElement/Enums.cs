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
namespace Sheng.SailingEase.Core
{
    public enum EnumContentAlignment
    {
        [LocalizedDescription("EnumContentAlignment_TopLeft")]
        TopLeft = 1,
        [LocalizedDescription("EnumContentAlignment_TopCenter")]
        TopCenter = 2,
        [LocalizedDescription("EnumContentAlignment_TopRight")]
        TopRight = 4,
        [LocalizedDescription("EnumContentAlignment_MiddleLeft")]
        MiddleLeft = 16,
        [LocalizedDescription("EnumContentAlignment_MiddleCenter")]
        MiddleCenter = 32,
        [LocalizedDescription("EnumContentAlignment_MiddleRight")]
        MiddleRight = 64,
        [LocalizedDescription("EnumContentAlignment_BottomLeft")]
        BottomLeft = 256,
        [LocalizedDescription("EnumContentAlignment_BottomCenter")]
        BottomCenter = 512,
        [LocalizedDescription("EnumContentAlignment_BottomRight")]
        BottomRight = 1024
    }
    public enum EnumTextImageRelation
    {
        [LocalizedDescription("EnumTextImageRelation_Overlay")]
        Overlay = 0,
        [LocalizedDescription("EnumTextImageRelation_ImageAboveText")]
        ImageAboveText = 1,
        [LocalizedDescription("EnumTextImageRelation_TextAboveImage")]
        TextAboveImage = 2,
        [LocalizedDescription("EnumTextImageRelation_ImageBeforeText")]
        ImageBeforeText = 4,
        [LocalizedDescription("EnumTextImageRelation_TextBeforeImage")]
        TextBeforeImage = 8
    }
}
