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
namespace Sheng.SailingEase.ComponentModel
{
    [Serializable]
    public class PropertyNumericUpDownEditorAttribute : PropertyEditorAttribute
    {
        private int _numericMin = 1;
        public int NumericMin
        {
            get
            {
                return this._numericMin;
            }
            set
            {
                this._numericMin = value;
            }
        }
        private int _numericMax = 10000;
        public int NumericMax
        {
            get
            {
                return this._numericMax;
            }
            set
            {
                this._numericMax = value;
            }
        }
    }
}
