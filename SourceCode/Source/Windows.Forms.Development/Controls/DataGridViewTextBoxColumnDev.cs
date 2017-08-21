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
using System.Windows.Forms;
using Sheng.SailingEase.Core.Development;
namespace Sheng.SailingEase.Windows.Forms.Development
{
    class DataGridViewTextBoxColumnDev : DataGridViewTextBoxColumn
    {
        private FormElementDataListTextBoxColumnEntityDev _entity;
        public DataGridViewTextBoxColumnDev(FormElementDataListTextBoxColumnEntityDev entity)
        {
            this._entity = entity;
            this.Name = this._entity.Code;
            this.HeaderText = this._entity.Text;
            this.Visible = this._entity.Visible;
            this.Width = this._entity.Width;
            IFormElementEntityDev entityDev = entity as IFormElementEntityDev;
            if (entityDev != null)
            {
                entityDev.Component = this;
            }
        }
    }
}
