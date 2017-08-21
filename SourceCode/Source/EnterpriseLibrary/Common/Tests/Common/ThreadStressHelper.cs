/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections;
using System.Threading;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests
{
    public static class ThreadStressHelper
    {
        public static void ThreadStress(ThreadStart testMethod, int threadCount)
        {
            ArrayList threads = new ArrayList();
            for (int i = 0; i < threadCount; i++)
            {
                threads.Add(new Thread(testMethod));
            }
            foreach (Thread thread in threads)
            {
                thread.Start();
            }
            foreach (Thread thread in threads)
            {
                thread.Join();
            }
        }
    }
}
