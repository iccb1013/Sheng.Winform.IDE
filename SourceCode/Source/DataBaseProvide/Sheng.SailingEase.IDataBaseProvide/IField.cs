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
namespace Sheng.SailingEase.IDataBaseProvide
{
    public interface IField
    {
        string TypeName { get; }
        bool LengthEnable { get; }
        bool AllowMaxLength { get; }
        int LengthMax { get; }
        int LengthMin { get; }
        bool DecimalDigitsEnable { get; }
        int DecimalDigitsMax { get; }
        int DecimalDigitsMin { get; }
        string Name { get; set; }
        FieldLength Length { get; set; }
        bool AllowEmpty { get; set; }
        string DefaultValue { get; set; }
        string ToSql();
    }
}
