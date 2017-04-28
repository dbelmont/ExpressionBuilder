using System;
using ExpressionBuilder.Builders;
using ExpressionBuilder.Interfaces;
using ExpressionBuilder.Interfaces.Generics;

namespace ExpressionBuilder.Generics
{
	public class FilterStatementConnection<TClass> : IFilterStatementConnection where TClass : class
	{
		readonly IFilter _filter;
		readonly IFilterStatement _statement;
		
		public FilterStatementConnection(IFilter filter, IFilterStatement statement)
		{
			_filter = filter;
			_statement = statement;
		}

		public IFilter And
		{
			get
			{
				_statement.Connector = FilterStatementConnector.And;
				return _filter;
			}
		}

		public IFilter Or
		{
			get
			{
				_statement.Connector = FilterStatementConnector.Or;
				return _filter;
			}
		}
	}
}
