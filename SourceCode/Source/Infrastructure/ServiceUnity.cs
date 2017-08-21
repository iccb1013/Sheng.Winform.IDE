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
using Microsoft.Practices.Unity;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Composite.Events;
namespace Sheng.SailingEase.Infrastructure
{
    public class ServiceUnity
    {
        private static IUnityContainer _container;
        public static IUnityContainer Container
        {
            get
            {
                if (_container == null)
                {
                    _container = (IUnityContainer)ServiceLocator.Current.GetService(typeof(IUnityContainer));
                }
                return _container;
            }
        }
        private static ICachingService _cachingService;
        public static ICachingService CachingService
        {
            get
            {
                if (_cachingService == null)
                {
                    _cachingService = Container.Resolve<ICachingService>();
                }
                return _cachingService;
            }
        }
        private static IEventAggregator _eventAggregator;
        public static IEventAggregator EventAggregator
        {
            get
            {
                if (_eventAggregator == null)
                {
                    _eventAggregator = Container.Resolve<IEventAggregator>();
                }
                return _eventAggregator;
            }
        }
        private static IPackageService _packageService;
        public static IPackageService PackageService
        {
            get
            {
                if (_packageService == null)
                {
                    _packageService = Container.Resolve<IPackageService>();
                }
                return _packageService;
            }
        }
        private static IProjectService _projectService;
        public static IProjectService ProjectService
        {
            get
            {
                if (_projectService == null)
                {
                    _projectService = Container.Resolve<IProjectService>();
                }
                return _projectService;
            }
        }
        private static IWorkbenchService _workbenchService;
        public static IWorkbenchService WorkbenchService
        {
            get
            {
                if (_workbenchService == null)
                {
                    _workbenchService = Container.Resolve<IWorkbenchService>();
                }
                return _workbenchService;
            }
        }
        private static IArchiveServiceUnity _archiveServiceUnity;
        public static IArchiveServiceUnity ArchiveServiceUnity
        {
            get
            {
                if (_archiveServiceUnity == null)
                {
                    _archiveServiceUnity = Container.Resolve<IArchiveServiceUnity>();
                }
                return _archiveServiceUnity;
            }
        }
        private static IDictionaryComponentService _dictionaryComponentService;
        public static IDictionaryComponentService DictionaryComponentService
        {
            get
            {
                if (_dictionaryComponentService == null)
                {
                    _dictionaryComponentService = Container.Resolve<IDictionaryComponentService>();
                }
                return _dictionaryComponentService;
            }
        }
        private static IDataEntityComponentService _dataEntityComponentService;
        public static IDataEntityComponentService DataEntityComponentService
        {
            get
            {
                if (_dataEntityComponentService == null)
                {
                    _dataEntityComponentService = Container.Resolve<IDataEntityComponentService>();
                }
                return _dataEntityComponentService;
            }
        }
        private static IResourceComponentService _resourceService;
        public static IResourceComponentService ResourceService
        {
            get
            {
                if (_resourceService == null)
                {
                    _resourceService = Container.Resolve<IResourceComponentService>();
                }
                return _resourceService;
            }
        }
        private static IWindowComponentService _windowComponentService;
        public static IWindowComponentService WindowComponentService
        {
            get
            {
                if (_windowComponentService == null)
                {
                    _windowComponentService = Container.Resolve<IWindowComponentService>();
                }
                return _windowComponentService;
            }
        }
        private static IWindowCompontsContainer _windowCompontsContainer;
        public static IWindowCompontsContainer WindowCompontsContainer
        {
            get
            {
                if (_windowCompontsContainer == null)
                {
                    _windowCompontsContainer = Container.Resolve<IWindowCompontsContainer>();
                }
                return _windowCompontsContainer;
            }
        }
        private static IWindowElementContainer _windowElementContainer;
        public static IWindowElementContainer WindowElementContainer
        {
            get
            {
                if (_windowElementContainer == null)
                {
                    _windowElementContainer = Container.Resolve<IWindowElementContainer>();
                }
                return _windowElementContainer;
            }
        }
        private static IWindowDesignService _windowDesignService;
        public static IWindowDesignService WindowDesignService
        {
            get
            {
                if (_windowDesignService == null)
                {
                    _windowDesignService = Container.Resolve<IWindowDesignService>();
                }
                return _windowDesignService;
            }
        }
    }
}
