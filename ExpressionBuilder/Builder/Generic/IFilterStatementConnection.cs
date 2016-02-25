using System;

namespace ExpressionBuilder.Builder.Generic
{
	public interface IFilterStatementConnection<TClass> where TClass : class
	{
		/// <summary>
		/// Defines that the last filter statement will connect to the next one using the 'AND' logical operator.
		/// </summary>
        IFilter<TClass> And { get; }
        /// <summary>
        /// Defines that the last filter statement will connect to the next one using the 'OR' logical operator.
        /// </summary>
        IFilter<TClass> Or { get; }
	}
}