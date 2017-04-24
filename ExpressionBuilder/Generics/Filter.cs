using System;
using System.Collections.Generic;
using ExpressionBuilder.Builders;
using ExpressionBuilder.Interfaces;
using ExpressionBuilder.Interfaces.Generics;

namespace ExpressionBuilder.Generics
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

		[Obsolete("\r\nThere is no need to use this anymore, because the Filter can be passed directly to the 'Where' LINQ method (as it is being implicitly converted).\r\ne.g.: People.Where(filter);")]
		public System.Linq.Expressions.Expression<Func<TClass, bool>> BuildExpression()
		{
			var builder = new FilterBuilder(new BuilderHelper());
			return builder.GetExpression(this);
		}
		
		public static implicit operator Func<TClass, bool>(Filter<TClass> filter)
		{
			var builder = new FilterBuilder(new BuilderHelper());
			return builder.GetExpression(filter).Compile();
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
