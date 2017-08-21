/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder
{
	public sealed class ConfigurationReflectionCache
	{
		private Dictionary<PairKey<Type, Type>, Attribute> typeAttributes;
		private object typeAttributesLock = new object();
		private Dictionary<PairKey<Type, Type>, Attribute> typeInheritedAttributes;
		private object typeInheritedAttributesLock = new object();
		private Dictionary<Type, ICustomFactory> typeCustomFactories;
		private object typeCustomFactoriesLock = new object();
		private Dictionary<Type, IConfigurationNameMapper> typeNameMappers;
		private object typeNameMappersLock = new object();
		public ConfigurationReflectionCache()
		{
			typeAttributes = new Dictionary<PairKey<Type, Type>, Attribute>();
			typeInheritedAttributes = new Dictionary<PairKey<Type, Type>, Attribute>();
			typeCustomFactories = new Dictionary<Type, ICustomFactory>();
			typeNameMappers = new Dictionary<Type, IConfigurationNameMapper>();
		}
		public TAttribute GetCustomAttribute<TAttribute>(Type type)
			where TAttribute : Attribute
		{
			return GetCustomAttribute<TAttribute>(type, false);
		}
		public bool HasCachedCustomAttribute<TAttribute>(Type type)
		{
			PairKey<Type, Type> key = new PairKey<Type, Type>(type, typeof(TAttribute));
			return this.typeAttributes.ContainsKey(key);
		}
		public TAttribute GetCustomAttribute<TAttribute>(Type type, bool inherit)
			where TAttribute : Attribute
		{
			TAttribute attribute
				= DoGetCustomAttribute<TAttribute>(
					type,
					inherit ? typeInheritedAttributes : typeAttributes,
					inherit ? typeInheritedAttributesLock : typeAttributesLock,
					inherit);			
			return attribute;
		}
		public ICustomFactory GetCustomFactory(Type type)
		{
			ICustomFactory storedObject;
			bool exists = false;
			lock (typeCustomFactoriesLock)
			{
				exists = typeCustomFactories.TryGetValue(type, out storedObject);
			}
			if (!exists)
			{
				storedObject = CreateCustomFactory(type);
				lock (typeCustomFactoriesLock)
				{
					typeCustomFactories[type] = storedObject;
				}
			}
			return storedObject;
		}
		public IConfigurationNameMapper GetConfigurationNameMapper(Type type)
		{
			IConfigurationNameMapper storedObject;
			bool exists = false;
			lock (typeNameMappersLock)
			{
				exists = typeNameMappers.TryGetValue(type, out storedObject);
			}
			if (!exists)
			{
				storedObject = CreateConfigurationNameMapper(type);
				lock (typeNameMappersLock)
				{
					typeNameMappers[type] = storedObject;
				}
			}
			return storedObject;
		}
		private TAttribute DoGetCustomAttribute<TAttribute>(Type type, Dictionary<PairKey<Type, Type>, Attribute> cache, object lockObject, bool inherit)
			where TAttribute : Attribute
		{
			PairKey<Type, Type> key = new PairKey<Type, Type>(type, typeof(TAttribute));
			Attribute storedObject;
			bool exists = false;
			lock (lockObject)
			{
				exists = cache.TryGetValue(key, out storedObject);
			}
			if (!exists)
			{
				storedObject = RetrieveAttribute<TAttribute>(key, inherit);
				lock (lockObject)
				{
					cache[key] = storedObject;
				}
			}
			return storedObject as TAttribute;
		}
		private TAttribute RetrieveAttribute<TAttribute>(PairKey<Type, Type> key, bool inherit)
			where TAttribute : Attribute
		{
			TAttribute attribute = (TAttribute)Attribute.GetCustomAttribute(key.Left, typeof(TAttribute), inherit);
			return attribute;
		}
		private ICustomFactory CreateCustomFactory(Type type)
		{
			CustomFactoryAttribute attribute
			   = GetCustomAttribute<CustomFactoryAttribute>(type);
			if (attribute != null)
			{
				return (ICustomFactory)Activator.CreateInstance(attribute.FactoryType);
			}
			else
			{
				return null;
			}
		}
		private IConfigurationNameMapper CreateConfigurationNameMapper(Type type)
		{
			ConfigurationNameMapperAttribute attribute
			   = GetCustomAttribute<ConfigurationNameMapperAttribute>(type);
			if (attribute != null)
			{
				return (IConfigurationNameMapper)Activator.CreateInstance(attribute.NameMappingObjectType);
			}
			else
			{
				return null;
			}
		}
	}
	internal class PairKey<TLeft, TRight>
	{
		private TLeft left;
		private TRight right;
		internal PairKey(TLeft left, TRight right)
		{
			this.left = left;
			this.right = right;
		}
		public override bool Equals(object obj)
		{
			PairKey<TLeft, TRight> other = obj as PairKey<TLeft, TRight>;
			if (other == null)
				return false;
			return (Equals(left, other.left) && Equals(right, other.right));
		}
		public override int GetHashCode()
		{
			int hashForType = left == null ? 0 : left.GetHashCode();
			int hashForID = right == null ? 0 : right.GetHashCode();
			return hashForType ^ hashForID;
		}
		public TLeft Left 
		{ 
			get { return left; } 
		}
		public TRight Right
		{
			get { return right; }
		}
	}
}
