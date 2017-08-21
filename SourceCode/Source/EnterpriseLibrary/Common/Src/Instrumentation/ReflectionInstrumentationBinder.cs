/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation
{
	public class ReflectionInstrumentationBinder
	{
		public void Bind(object eventSource, object eventListener)
		{
			EventBinder binder = new EventBinder(eventSource, eventListener);
			Dictionary<string, List<EventInfo>> eventMap = GetInstrumentationEvents(eventSource);
			Dictionary<string, List<MethodInfo>> listenerMap = GetInstrumentationListeners(eventListener);
			foreach (string topic in eventMap.Keys)
			{
				if (listenerMap.ContainsKey(topic) == false) continue;
				List<EventInfo> sourceEvents = eventMap[topic];
				List<MethodInfo> targetMethods = listenerMap[topic];
				foreach (EventInfo sourceEvent in sourceEvents)
				{
					foreach (MethodInfo targetMethod in targetMethods)
					{
						binder.Bind(sourceEvent, targetMethod);
					}
				}
			}
		}
		delegate T[] MemberFinder<T>(Type t);
		EventInfo[] GetEventInfo(Type t) { return t.GetEvents(); }
		Dictionary<string, List<EventInfo>> GetInstrumentationEvents(object eventSource)
		{
			return GetAttributedMembers<EventInfo,
				InstrumentationProviderAttribute>(eventSource, delegate(Type t) { return GetEventInfo(t); });
		}
		MethodInfo[] GetMethodInfo(Type t) { return t.GetMethods(); }
		Dictionary<string, List<MethodInfo>> GetInstrumentationListeners(object eventListener)
		{
			return GetAttributedMembers<MethodInfo,
				InstrumentationConsumerAttribute>(eventListener, delegate(Type t) { return GetMethodInfo(t); });
		}
		Dictionary<string, List<TMemberInfoType>>
	GetAttributedMembers<TMemberInfoType, TAttributeType>(object target, MemberFinder<TMemberInfoType> memberFinder)
			where TMemberInfoType : MemberInfo
			where TAttributeType : InstrumentationBaseAttribute
		{
			Dictionary<string, List<TMemberInfoType>> memberInfoList =
				new Dictionary<string, List<TMemberInfoType>>();
			Type type = target.GetType();
			MemberInfo[] memberInfos = memberFinder(type);
			foreach (MemberInfo memberInfo in memberInfos)
			{
				object[] attributes = memberInfo.GetCustomAttributes(typeof(TAttributeType), false);
				foreach (InstrumentationBaseAttribute attribute in attributes)
				{
					if (memberInfoList.ContainsKey(attribute.SubjectName) == false)
					{
						memberInfoList.Add(attribute.SubjectName, new List<TMemberInfoType>());
					}
					List<TMemberInfoType> list = memberInfoList[attribute.SubjectName];
					list.Add((TMemberInfoType)memberInfo);
				}
			}
			return memberInfoList;
		}
	}
}
