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
namespace Sheng.SailingEase.Controls
{
    public class SEListViewDescriptiveMembers : ISEListViewExtendMember
    {
        public const string DescriptioinMember = "Descriptioin";
        public string Descriptioin { get; set; }
        public Dictionary<string, string> GetExtendMembers()
        {
            Dictionary<string, string> members = new Dictionary<string, string>();
            if (String.IsNullOrEmpty(Descriptioin) == false)
                members.Add(DescriptioinMember, Descriptioin);
            return members;
        }
    }
}
