/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Reflection;
using System.Security;
using System.Security.Principal;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
	public class ExceptionFormatter
	{
		public static string Header = Properties.Resources.ExceptionFormatterHeader;
		private const string LineSeparator = "======================================";
		private NameValueCollection additionalInfo;
		private string applicationName;
		public ExceptionFormatter()
			: this(new NameValueCollection(), string.Empty)
		{
		}
		public ExceptionFormatter(NameValueCollection additionalInfo, string applicationName)
		{
			this.additionalInfo = additionalInfo;
			this.applicationName = applicationName;
		}
		public string GetMessage(Exception exception)
		{
			StringBuilder eventInformation = new StringBuilder();
			CollectAdditionalInfo();
			eventInformation.AppendFormat("{0}\n\n", this.additionalInfo.Get(Header));
			eventInformation.AppendFormat("\n{0} {1}:\n{2}",
										  Resources.ExceptionSummary, this.applicationName, LineSeparator);
			foreach (string info in this.additionalInfo)
			{
				if (info != Header)
				{
					eventInformation.AppendFormat("\n--> {0}", this.additionalInfo.Get(info));
				}
			}
			if (exception != null)
			{
				Exception currException = exception;
				int exceptionCount = 1;
				do
				{
					eventInformation.AppendFormat("\n\n{0}\n{1}", Resources.ExceptionDetails, LineSeparator);
					eventInformation.AppendFormat("\n{0}: {1}", Resources.ExceptionType, currException.GetType().FullName);
					ReflectException(currException, eventInformation);
					if (currException.StackTrace != null)
					{
						eventInformation.AppendFormat("\n\n{0} \n{1}",
													  Resources.ExceptionStackTraceDetails, LineSeparator);
						eventInformation.AppendFormat("\n{0}", currException.StackTrace);
					}
					currException = currException.InnerException;
					exceptionCount++;
				} while (currException != null);
			}
			return eventInformation.ToString();
		}
		private static void ReflectException(Exception currException, StringBuilder strEventInfo)
		{
			PropertyInfo[] arrPublicProperties = currException.GetType().GetProperties();
			foreach (PropertyInfo propinfo in arrPublicProperties)
			{
				if (propinfo.Name != "InnerException" && propinfo.Name != "StackTrace")
				{
                    if (propinfo.CanRead && propinfo.GetIndexParameters().Length == 0)
                    {
                        object propValue = null;
                        try
                        {
                            propValue = propinfo.GetValue(currException, null);
                        }
                        catch (TargetInvocationException)
                        {
                            propValue = Resources.PropertyAccessFailed;
                        }
                        if (propValue == null)
                        {
                            strEventInfo.AppendFormat("\n{0}: NULL", propinfo.Name);
                        }
                        else
                        {
                            ProcessAdditionalInfo(propinfo, propValue, strEventInfo);
                        }
                    }
				}
			}
		}
		private static void ProcessAdditionalInfo(PropertyInfo propinfo, object propValue, StringBuilder stringBuilder)
		{
			NameValueCollection currAdditionalInfo;
			if (propinfo.Name == "AdditionalInformation")
			{
				if (propValue != null)
				{
					currAdditionalInfo = (NameValueCollection)propValue;
					if (currAdditionalInfo.Count > 0)
					{
						stringBuilder.AppendFormat("\nAdditionalInformation:");
						for (int i = 0; i < currAdditionalInfo.Count; i++)
						{
							stringBuilder.AppendFormat("\n{0}: {1}", currAdditionalInfo.GetKey(i), currAdditionalInfo[i]);
						}
					}
				}
			}
			else
			{
				stringBuilder.AppendFormat("\n{0}: {1}", propinfo.Name, propValue);
			}
		}
		private void CollectAdditionalInfo()
		{
			if (this.additionalInfo["MachineName:"] != null)
			{
				return;
			}
			this.additionalInfo.Add("MachineName:", "MachineName: " + GetMachineName());
			this.additionalInfo.Add("TimeStamp:", "TimeStamp: " + DateTime.UtcNow.ToString(CultureInfo.CurrentCulture));
			this.additionalInfo.Add("FullName:", "FullName: " + Assembly.GetExecutingAssembly().FullName);
			this.additionalInfo.Add("AppDomainName:", "AppDomainName: " + AppDomain.CurrentDomain.FriendlyName);
			this.additionalInfo.Add("WindowsIdentity:", "WindowsIdentity: " + GetWindowsIdentity());
		}
		private static string GetWindowsIdentity()
		{
			try
			{
				return WindowsIdentity.GetCurrent().Name;
			}
			catch (SecurityException)
			{
				return "Permission Denied";
			}
		}
		private static string GetMachineName()
		{
			try
			{
				return Environment.MachineName;
			}
			catch (SecurityException)
			{
				return "Permission Denied";
			}
		}
	}
}
