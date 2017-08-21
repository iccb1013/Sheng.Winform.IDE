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
using Sheng.SailingEase.Infrastructure;
using System.Diagnostics;
using Microsoft.Practices.Composite.Events;
namespace Sheng.SailingEase.Shell
{
    class PackageService : IPackageService
    {
        private IEventAggregator _eventAggregator = ServiceUnity.EventAggregator;
        public PackageService()
        {
            _eventAggregator.GetEvent<ProjectPreCloseEvent>().Subscribe((e) =>
            {
                _current.Close();
            });
        }
        private Package _current;
        public IPackage Current
        {
            get
            {
                Debug.Assert(_current != null, "没有打开任何档案");
                if (_current == null)
                {
                    throw new Exception("没有打开任何档案");
                }
                return _current;
            }
        }
        public IPackage Create(string path)
        {
            Package package = new Package(path);
            package.Create();
            return package;
        }
        public IPackage Open(string path)
        {
            if (_current != null)
                _current.Close();
            _current = new Package(path);
            return this.Current;
        }
    }
}
