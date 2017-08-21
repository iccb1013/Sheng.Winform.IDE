//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity.Fluent;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity
{
	/// <summary>
	/// Helps specifying build plan policies for objects, using Linq expressions to achieve type safety.
	/// </summary>
	/// <typeparam name="TTarget">The concrete type for which policies will be built.</typeparam>
	/// <typeparam name="TSource">The type of the object from which the arguments for the policies will be extracted.</typeparam>
	/// <remarks>
	/// The policy builder will interprete Linq expresions and translate them into suitable build plan policies. These expressions will
	/// typically consist of accesing property values from an instance of <typeparamref name="TSource"/>, but that is not strictly necessary.
	/// Top-level expressions involving method calls to the static class <see cref="Resolve"/> will be interpreted as requests to resolve
	/// references using the container.
	/// </remarks>
	/// <seealso cref="Resolve"/>
	public class PolicyBuilder<TTarget, TSource> :
		IPropertyPolicyBuilding<TTarget, TSource>,
		IFinishPoliciesBuilding,
		IPropertyAndFinishPolicyBuilding<TTarget, TSource>
	{
		private readonly NamedTypeBuildKey instanceKey;
		private readonly TSource source;

		private SelectedConstructor selectedConstructor;
		private readonly List<SelectedProperty> selectedProperties;
		private readonly Dictionary<string, IDependencyResolverPolicy> resolverPolicies;
		private int propertyWaitCount;
		private bool finished;

		/// <summary>
		/// Initializes a new instance of the class <see cref="PolicyBuilder{TTarget, TSource}"/>.
		/// </summary>
		/// <paparam name="name">The instance name to use for the keys associated to the created polices.</paparam>
		/// <parparam name="source">The object from which values will extracted.</parparam>
		/// <remarks>
		/// No constructor policy will be created by instances created with this constructor.
		/// </remarks>
		public PolicyBuilder(string name, TSource source)
			: this(NamedTypeBuildKey.Make<TTarget>(name), source)
		{ }

		/// <summary>
		/// Initializes a new instance of the class <see cref="PolicyBuilder{TTarget, TSource}"/>.
		/// </summary>
		/// <paparam name="instanceKey">The key to associate to the created polices.</paparam>
		/// <parparam name="source">The object from which values will extracted.</parparam>
		/// <remarks>
		/// No constructor policy will be created by instances created with this constructor.
		/// </remarks>
		public PolicyBuilder(NamedTypeBuildKey instanceKey, TSource source)
		{
			this.instanceKey = instanceKey;
			this.source = source;

			this.selectedProperties = new List<SelectedProperty>();
			this.resolverPolicies = new Dictionary<string, IDependencyResolverPolicy>();
		}

		/// <summary>
		/// Initializes a new instance of the class <see cref="PolicyBuilder{TSource, TTarget}"/> with a creation expression.
		/// </summary>
		/// <param name="name">The instance name to use for the keys associated to the created polices.</param>
		/// <param name="source">The object from which values will extracted.</param>
		/// <param name="creationExpression">The expression that specifies the <see cref="IConstructorSelectorPolicy"/>
		/// that should be created by the policy builder.</param>
		[SuppressMessage("Microsoft.Design", "CA1006",
			Justification = "Signatures dealing with Expression trees use nested generics (see http://msdn2.microsoft.com/en-us/library/bb534754.aspx).")]
		public PolicyBuilder(string name, TSource source, Expression<Func<TSource, TTarget>> creationExpression)
			: this(NamedTypeBuildKey.Make<TTarget>(name), source, creationExpression)
		{ }

		/// <summary>
		/// Initializes a new instance of the class <see cref="PolicyBuilder{TSource, TTarget}"/> with a creation expression.
		/// </summary>
		/// <param name="instanceKey">The key to associate to the created polices.</param>
		/// <param name="source">The object from which values will extracted.</param>
		/// <param name="creationExpression">The expression that specifies the <see cref="IConstructorSelectorPolicy"/>
		/// that should be created by the policy builder.</param>
		[SuppressMessage("Microsoft.Design", "CA1006",
			Justification = "Signatures dealing with Expression trees use nested generics (see http://msdn2.microsoft.com/en-us/library/bb534754.aspx).")]
		public PolicyBuilder(NamedTypeBuildKey instanceKey, TSource source, Expression<Func<TSource, TTarget>> creationExpression)
		{
			Guard.ArgumentNotNull(creationExpression, "creationExpression");
			ParameterExpression parameter = creationExpression.Parameters[0];

			this.instanceKey = instanceKey;
			this.source = source;

			this.selectedProperties = new List<SelectedProperty>();
			this.resolverPolicies = new Dictionary<string, IDependencyResolverPolicy>();

			NewExpression newExpression = creationExpression.Body as NewExpression;
			if (newExpression != null)
			{
				InitializeForCreationWithConstructor(newExpression, parameter);
				return;
			}

			throw new ArgumentException(
				string.Format(
					CultureInfo.CurrentCulture,
					Resources.ExceptionSuppliedCreationExpressionIsNotNewExpression, 
					creationExpression.ToString()),
				"creationExpression");
		}

		private void InitializeForCreationWithConstructor(NewExpression newExpression, ParameterExpression parameter)
		{
			ConstructorInfo constructor = newExpression.Constructor;
			// TODO check accessibility?

			// keep a local reference to the selected constructor until it's done building
			SelectedConstructor tempSelectedConstructor = new SelectedConstructor(constructor);

			SetUpCallParameters(parameter,
				newExpression.Arguments,
				constructor, s => tempSelectedConstructor.AddParameterKey(s));

			// by now the selected constructor is sane
			this.selectedConstructor = tempSelectedConstructor;
		}

		private delegate void AddParameterKey(string key);

		private void SetUpCallParameters(ParameterExpression parameter,
			IEnumerable<Expression> argumentExpressions,
			MethodBase method,
			AddParameterKey addParameterKey)
		{
			ParameterInfo[] constructorParameters = method.GetParameters();

			// get a resolver policy for each argument
			int i = 0;
			foreach (var argumentExpression in argumentExpressions)
			{
				Type formalParameterType = constructorParameters[i++].ParameterType;
				string key = AddResolverPolicy(formalParameterType, argumentExpression, parameter);
				addParameterKey(key);
			};
		}

		/// <summary>
		/// Specify a property mapping.
		/// </summary>
		/// <typeparam name="TProperty">The type of the target property.</typeparam>
		/// <param name="propertyAccessExpression">The expresion that specifies the property to map to. Should look like "o => o.Property".</param>
		[SuppressMessage("Microsoft.Design", "CA1006",
			Justification = "Signatures dealing with Expression trees use nested generics (see http://msdn2.microsoft.com/en-us/library/bb534754.aspx).")]
		public IMapProperty<TTarget, TSource, TProperty> SetProperty<TProperty>(
			Expression<Func<TTarget, TProperty>> propertyAccessExpression)
		{
			CheckFinished();

			this.propertyWaitCount++;

			PropertyInfo property = ExtractProperty(propertyAccessExpression);

			return new WaitingForPropertyValueExpression<TProperty>(this, property);
		}

		/// <summary>
		/// Finish the creation process by adding all the collected policies to <paramref name="policyList"/>.
		/// </summary>
		/// <param name="policyList">The destination <see cref="IPolicyList"/>.</param>
		public void AddPoliciesToPolicyList(IPolicyList policyList)
		{
			CheckFinished();

			if (this.propertyWaitCount > 0)
			{
				throw new InvalidOperationException(Resources.ExceptionPolicyBuilderStillWaitingForPropertyPolicy);
			}

			this.finished = true;

			if (this.selectedConstructor != null)
			{
				policyList.Set<IConstructorSelectorPolicy>(
					new FixedConstructorSelectorPolicy(this.selectedConstructor),
					this.instanceKey);
			}

			if (this.selectedProperties.Count > 0)
			{
				policyList.Set<IPropertySelectorPolicy>(
					new FixedPropertySelectorPolicy(this.selectedProperties),
					this.instanceKey);
			}

			foreach (var item in this.resolverPolicies)
			{
				policyList.Set(item.Value, item.Key);
			}
		}

		internal IPropertyAndFinishPolicyBuilding<TTarget, TSource> SerPropertyMapping<TProperty>(
			PropertyInfo property,
			Expression<Func<TSource, TProperty>> mappingExpression)
		{
			this.propertyWaitCount--;

			// the expression is guaranteed to have this parameter
			ParameterExpression parameter = mappingExpression.Parameters[0];

			string key = AddResolverPolicy(property.PropertyType, mappingExpression.Body, parameter);
			this.selectedProperties.Add(new SelectedProperty(property, key));

			return this;
		}

		private void CheckFinished()
		{
			if (this.finished)
			{
				throw new InvalidOperationException(
					string.Format(
						CultureInfo.CurrentCulture, 
						Resources.ExceptionPolicyBuilderFinished, 
						this.instanceKey.Name, 
						instanceKey.Type));
			}
		}

		private string AddResolverPolicy(Type formalParameterType, Expression argumentExpression, ParameterExpression parameter)
		{
			string key = Guid.NewGuid().ToString();

			MethodCallExpression callExpression = argumentExpression as MethodCallExpression;
			if (callExpression != null && callExpression.Method.DeclaringType == typeof(Resolve))
			{
				// evaluate the resolve parameters
				LambdaExpression resolveArgumentExpression = Expression.Lambda(callExpression.Arguments[0], parameter);
				Delegate resolveArgumentDelegate = resolveArgumentExpression.Compile();
				object resolveValue = resolveArgumentDelegate.DynamicInvoke(this.source);

				string resolveMethodName = callExpression.Method.Name;

				switch (resolveMethodName)
				{
					case "Reference":
						// resolveValue must be a string to use as a key
						this.resolverPolicies.Add(
							key,
							new ReferenceResolverPolicy(
								new NamedTypeBuildKey(formalParameterType, (string)resolveValue)));
						break;
					case "ReferenceCollection":
						{
							// need to extract the type parameters from the generic method call to set up the resolver
							// the first parameter is the collection type, and the second parameter is the element type
							Type[] typeParameters = callExpression.Method.GetGenericArguments();

							this.resolverPolicies.Add(
								key,
								new ReferenceCollectionResolverPolicy(
									typeParameters[0],
									typeParameters[1],
									(IEnumerable<string>)resolveValue));
						}
						break;
					case "OptionalReference":
						{
							// resolveValue must be a string to use as a key. If null it's ignored
							string resolveKey = (string)resolveValue;
							if (!string.IsNullOrEmpty(resolveKey))
							{
								this.resolverPolicies.Add(
									key,
									new ReferenceResolverPolicy(new NamedTypeBuildKey(formalParameterType, resolveKey)));
							}
							else
							{
								this.resolverPolicies.Add(key, new ConstantResolverPolicy(null));
							}
						}
						break;
					case "ReferenceDictionary":
						{
							// need to extract the type parameters from the generic method call to set up the resolver
							// the first parameter is the dictionary type, the second parameter is the element type
							// and the third parameter is the key type
							Type[] typeParameters = callExpression.Method.GetGenericArguments();

							// should assert the nature of resolveValue?

							this.resolverPolicies.Add(
								key,
								(IDependencyResolverPolicy)Activator.CreateInstance(
									typeof(ReferenceDictionaryResolverPolicy<,,>).MakeGenericType(
										typeParameters[0],
										typeParameters[1],
										typeParameters[2]),
									resolveValue));
						}
						break;
					default:
						throw new ArgumentException(
							string.Format(
								CultureInfo.CurrentCulture, 
								Resources.ExceptionUnknownResolveMethod, 
								resolveMethodName, 
								argumentExpression.ToString()));
				}
			}
			else
			{
				// evaluate and generate a constant resolver policy
				LambdaExpression argumentEvaluationExpression = Expression.Lambda(argumentExpression, parameter);
				Delegate argumentEvaluationDelegate = argumentEvaluationExpression.Compile();
				object value = argumentEvaluationDelegate.DynamicInvoke(this.source);
				this.resolverPolicies.Add(key, new ConstantResolverPolicy(value));
			}

			return key;
		}

		private static PropertyInfo ExtractProperty<TProperty>(Expression<Func<TTarget, TProperty>> propertyAccessExpression)
		{
			MemberExpression memberExpression = propertyAccessExpression.Body as MemberExpression;
			if (memberExpression == null)
			{
				throw new ArgumentException(
					string.Format(
						CultureInfo.CurrentCulture, 
						Resources.ExceptionPropertyAccessExpressionNotPropertyAccess, 
						propertyAccessExpression.ToString()),
					"propertyAccessExpression");
			}

			PropertyInfo propertyInfo = memberExpression.Member as PropertyInfo;
			if (propertyInfo == null)
			{
				throw new ArgumentException(
					string.Format(
						CultureInfo.CurrentCulture, 
						Resources.ExceptionPropertyAccessExpressionNotPropertyAccess, 
						propertyAccessExpression.ToString()),
					"propertyAccessExpression");
			}

			// TODO should check for valid property or does Unity take care of it?
			return propertyInfo;

		}

		private class WaitingForPropertyValueExpression<TProperty>
			: IMapProperty<TTarget, TSource, TProperty>
		{
			[SuppressMessage("Microsoft.Performance", "CA1823",
				Justification = "False positive, it is used in the To() method.")]
			private PolicyBuilder<TTarget, TSource> owner;
			[SuppressMessage("Microsoft.Performance", "CA1823",
				Justification = "False positive, it is used in the To() method.")]
			private PropertyInfo property;

			public WaitingForPropertyValueExpression(PolicyBuilder<TTarget, TSource> owner, PropertyInfo property)
			{
				this.owner = owner;
				this.property = property;
			}

			public IPropertyAndFinishPolicyBuilding<TTarget, TSource> To(
				Expression<Func<TSource, TProperty>> mappingExpression)
			{
				return this.owner.SerPropertyMapping<TProperty>(this.property, mappingExpression);
			}
		}
	}
}
