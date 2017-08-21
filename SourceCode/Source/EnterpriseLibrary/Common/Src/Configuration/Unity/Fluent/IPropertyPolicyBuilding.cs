/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity.Fluent
{
	public interface IPropertyPolicyBuilding<TTarget, TSource>
	{
		[SuppressMessage("Microsoft.Design", "CA1006",
			Justification = "Signatures dealing with Expression trees use nested generics (see http://msdn2.microsoft.com/en-us/library/bb534754.aspx).")]
		IMapProperty<TTarget, TSource, TProperty> SetProperty<TProperty>(Expression<Func<TTarget, TProperty>> propertyAccessExpression);
	}
}
