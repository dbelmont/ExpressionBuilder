using System;
using System.Collections.Generic;
using ExpressionBuilder.Builder.Generic;

namespace ExpressionBuilder.Builder.Generic
{
	public class OrderBy<TClass> : IOrderBy<TClass> where TClass : class
	{
		private readonly List<OrderByElement<TClass>> _elements;
		
		public IEnumerable<OrderByElement<TClass>> Elements
		{
			get
			{
				return _elements.ToArray();
			}
		}
		
		public OrderBy()
		{
			_elements = new List<OrderByElement<TClass>>();
		}
		
		public IOrderBy<TClass> By(string propertyName, OrderByDirection direction)
		{
			_elements.Add(new OrderByElement<TClass>(propertyName, direction));
			return this;
		}
		
		public void Clear()
		{
			_elements.Clear();
		}
	}
}