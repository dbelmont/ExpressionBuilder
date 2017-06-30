using ExpressionBuilder.Common;
using ExpressionBuilder.Interfaces;

namespace ExpressionBuilder.Generics
{
    /// <summary>
    /// Connects to FilterStatement together.
    /// </summary>
	public class FilterStatementConnection<TClass> : IFilterStatementConnection where TClass : class
	{
		readonly IFilter _filter;
		readonly IFilterStatement _statement;
		
		internal FilterStatementConnection(IFilter filter, IFilterStatement statement)
		{
			_filter = filter;
			_statement = statement;
		}

        /// <summary>
		/// Defines that the last filter statement will connect to the next one using the 'AND' logical operator.
		/// </summary>
		public IFilter And
		{
			get
			{
				_statement.Connector = FilterStatementConnector.And;
				return _filter;
			}
		}

        /// <summary>
        /// Defines that the last filter statement will connect to the next one using the 'OR' logical operator.
        /// </summary>
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
