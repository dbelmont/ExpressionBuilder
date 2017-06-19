using System;
using System.Collections.Generic;

namespace ExpressionBuilder.Builder.Generic
{
	/// <summary>
	/// Defines the order by clause that will be used to sort the data.
	/// </summary>
	public interface IOrderBy<TClass> where TClass : class
	{
		/// <summary>
		/// List of the order by elements defining the pair property/direction
		/// </summary>
		IEnumerable<OrderByElement<TClass>> Elements { get; }
		/// <summary>
		/// Adds an element to the order by clause.
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="direction"></param>
		/// <returns></returns>
		IOrderBy<TClass> By(string propertyName, OrderByDirection direction);
	}
}