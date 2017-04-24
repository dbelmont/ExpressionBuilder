using System;
using ExpressionBuilder.Builders;

namespace ExpressionBuilder.Interfaces.Generics
{
	/// <summary>
	/// Defines one of the elements of the 'order by' clause.
	/// </summary>
	public interface IOrderByElement
	{
		/// <summary>
		/// Name of the property
		/// </summary>
		string PropertyName { get; }
		/// <summary>
		/// Direction in which the specified property will be sorted
		/// </summary>
		SortDirection Direction { get; }
	}
}