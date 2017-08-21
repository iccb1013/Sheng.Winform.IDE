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
	[SuppressMessage("Microsoft.Design", "CA1005",
		Justification = "Three types are needed here to achieve type safety.")]
	public interface IMapProperty<TTarget, TSource, TProperty>
	{
		[SuppressMessage("Microsoft.Design", "CA1006",
			Justification = "Signatures dealing with Expression trees use nested generics (see http://msdn2.microsoft.com/en-us/library/bb534754.aspx).")]
		IPropertyAndFinishPolicyBuilding<TTarget, TSource> To(
			Expression<Func<TSource, TProperty>> mappingExpression);
	}
}
