using System;
using ExpressionBuilder.Builders;

namespace ExpressionBuilder.Generics
{
	public class SortElement<TClass> where TClass : class
	{
		public string PropertyName { get; private set; }
		public SortDirection Direction { get; private set; }
		
		public SortElement(string propertyName, SortDirection direction)
		{
			PropertyName = propertyName;
			Direction = direction;
		}
	}
}