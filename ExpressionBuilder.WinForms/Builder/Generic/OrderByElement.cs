using System;

namespace ExpressionBuilder.Builder.Generic
{
	public class OrderByElement<TClass> where TClass : class
	{
		public string PropertyName { get; private set; }
		public OrderByDirection Direction { get; private set; }
		
		public OrderByElement(string propertyName, OrderByDirection direction)
		{
			PropertyName = propertyName;
			Direction = direction;
		}
	}
}