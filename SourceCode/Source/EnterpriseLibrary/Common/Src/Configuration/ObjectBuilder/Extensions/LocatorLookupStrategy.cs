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
using System.Threading;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder
{
    public class LocatorLookupStrategy : BuilderStrategy
    {
        public override void PreBuildUp(IBuilderContext context)
        {
            if (context.Locator != null)
            {
                Monitor.Enter(context.Locator);
                context.RecoveryStack.Add(new LockReleaser(context.Locator));
                object result = context.Locator.Get(context.BuildKey);
                if (result != null)
                {
                    context.Existing = result;
                    context.BuildComplete = true;
                    Monitor.Exit(context.Locator);
                }
            }
        }
        public override void PostBuildUp(IBuilderContext context)
        {
            if(context.Locator != null)
            {
                Monitor.Exit(context.Locator);
            }
        }
        private class LockReleaser : IRequiresRecovery
        {
            private readonly object lockToRelease;
            public LockReleaser(object lockToRelease)
            {
                this.lockToRelease = lockToRelease;
            }
            public void Recover()
            {
                try
                {
                    Monitor.Exit(lockToRelease);
                }
                catch (SynchronizationLockException)
                {
                }
            }
        }
    }
}
