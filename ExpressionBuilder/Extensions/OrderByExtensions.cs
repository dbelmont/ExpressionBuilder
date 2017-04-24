using System;
using System.Collections.Generic;
using System.Linq;
using ExpressionBuilder.Builders;
using ExpressionBuilder.Generics;

namespace ExpressionBuilder.Extensions
{
	public static class OrderByExtensions
	{
		public static IEnumerable<TType> OrderBy<TType>(this IEnumerable<TType> list, SortExpression<TType> sortExpression) where TType : class
		{
			var builder = new SortBuilder(new BuilderHelper());
			return builder.BuildOrderBys(list, sortExpression.Elements.ToList());
		}
	}
}