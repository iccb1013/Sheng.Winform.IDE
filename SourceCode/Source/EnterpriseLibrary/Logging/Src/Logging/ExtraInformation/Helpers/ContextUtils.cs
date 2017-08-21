/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.EnterpriseServices;
using System.Runtime.InteropServices;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation.Helpers
{
	internal class ContextUtils : IContextUtils
	{
		private SecurityCallContext currentCall;
		public string GetActivityId()
		{
			return ContextUtil.ActivityId.ToString();
		}
		public string GetApplicationId()
		{
			return ContextUtil.ApplicationId.ToString();
		}
		public string GetTransactionId()
		{
			return ContextUtil.TransactionId.ToString();
		}
		public string GetDirectCallerAccountName()
		{
			string directCallerAccountName;
			SecurityCallContext currentCall = this.CurrentCall;
			if (currentCall != null)
			{
				if (currentCall.IsSecurityEnabled) 
                {
					directCallerAccountName = currentCall.DirectCaller.AccountName;
                }
				else
                {
					directCallerAccountName = string.Empty;
			}
            }
			else
            {
				directCallerAccountName = string.Empty;
            }
			return directCallerAccountName;
		}
		public string GetOriginalCallerAccountName()
		{
			string originalCallerAccountName;
			SecurityCallContext currentCall = this.CurrentCall;
			if (currentCall != null)
			{
				if (currentCall.IsSecurityEnabled) 
                {
					originalCallerAccountName = currentCall.OriginalCaller.AccountName;
                }
				else
                {
					originalCallerAccountName = string.Empty;
			}
            }
			else
            {
				originalCallerAccountName = string.Empty;
            }
			return originalCallerAccountName;
		}
		private SecurityCallContext CurrentCall
		{
			get 
			{
				NativeMethods.IObjectContext objectContext = ObjectContext;
				if (objectContext != null)
				{
					if (objectContext.IsSecurityEnabled())
                    {
						currentCall = SecurityCallContext.CurrentCall;
                    }
					else
                    {
						currentCall = null;
				}
                }
				return currentCall;
			}
		}
		private NativeMethods.IObjectContext ObjectContext
		{
			get 
			{
				NativeMethods.IObjectContext objectContext;
				int hr = NativeMethods.GetObjectContext(out objectContext);
				if ( ! (hr == 0 || hr == NativeMethods.E_NOINTERFACE || hr == NativeMethods.CONTEXT_E_NOCONTEXT) )
				{
					Marshal.ThrowExceptionForHR(hr);
				}
				return objectContext;
			}
		}
	}
}
