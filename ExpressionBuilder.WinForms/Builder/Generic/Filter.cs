using System;
using System.Collections.Generic;

namespace ExpressionBuilder.Builder.Generic
{
	public class Filter<TClass> : IFilter<TClass> where TClass : class
	{
		private readonly List<IFilterStatement> _statements;
		
		public IEnumerable<IFilterStatement> Statements
		{
			get
			{
				return _statements.ToArray();
			}
		}
		
		public Filter()
		{
			_statements = new List<IFilterStatement>();
		}

		public IFilterStatementConnection<TClass> By<TPropertyType>(string propertyName, Operation operation, TPropertyType value, FilterStatementConnector connector = FilterStatementConnector.And)
		{
			IFilterStatement statement = null;
			statement = new FilterStatement<TPropertyType>(propertyName, operation, value, connector);
			_statements.Add(statement);
			return new FilterStatementConnection<TClass>(this, statement);
		}	
		
		public void Clear()
		{
			_statements.Clear();
		}

		public System.Linq.Expressions.Expression<Func<TClass, bool>> BuildExpression()
		{
			return Builder.GetExpression<TClass>(this);
		}
		
		public static implicit operator Func<TClass, bool>(Filter<TClass> filter)
		{
			return filter.BuildExpression().Compile();
		}
		
		public override string ToString()
		{
			var result = "";
			FilterStatementConnector lastConector = FilterStatementConnector.And;
			foreach (var statement in _statements)
			{
				if (!string.IsNullOrWhiteSpace(result)) result += " " + lastConector + " ";
				result += statement.ToString();
				lastConector = statement.Connector;
			}
			
			return result.Trim();
		}
	}
}
