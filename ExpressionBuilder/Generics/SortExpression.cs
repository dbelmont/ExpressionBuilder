using System;
using System.Collections.Generic;
using ExpressionBuilder.Builders;
using ExpressionBuilder.Interfaces.Generics;

namespace ExpressionBuilder.Generics
{
	public class SortExpression<TClass> : ISortExpression<TClass> where TClass : class
	{
		private readonly List<SortElement<TClass>> _elements;
		
		public IEnumerable<SortElement<TClass>> Elements
		{
			get
			{
				return _elements.ToArray();
			}
		}
		
		public SortExpression()
		{
			_elements = new List<SortElement<TClass>>();
		}
		
		public ISortExpression<TClass> By(string propertyName, SortDirection direction = SortDirection.Ascending)
		{
			_elements.Add(new SortElement<TClass>(propertyName, direction));
			return this;
		}
		
		public void Clear()
		{
			_elements.Clear();
		}
	}
}