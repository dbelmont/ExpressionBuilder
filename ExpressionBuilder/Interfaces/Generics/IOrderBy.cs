using System;
using System.Collections.Generic;
using ExpressionBuilder.Builders;
using ExpressionBuilder.Generics;

namespace ExpressionBuilder.Interfaces.Generics
{
	/// <summary>
	/// Defines the order by clause that will be used to sort the data.
	/// </summary>
	public interface ISortExpression<TClass> where TClass : class
	{
		/// <summary>
		/// List of the order by elements defining the pair property/direction
		/// </summary>
		IEnumerable<SortElement<TClass>> Elements { get; }
		/// <summary>
		/// Adds an element to the order by clause.
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="direction"></param>
		/// <returns></returns>
		ISortExpression<TClass> By(string propertyName, SortDirection direction = SortDirection.Ascending);
	}
}