using System.Collections.Generic;
using ExpressionBuilder.Common;

namespace ExpressionBuilder.Interfaces
{
	/// <summary>
	/// Defines a filter from which a expression will be built.
	/// </summary>
	public interface IFilter
	{
		/// <summary>
		/// Group of statements that compose this filter.
		/// </summary>
		IEnumerable<IFilterStatement> Statements { get; }
        /// <summary>
        /// Add a statement, that doesn't need value, to this filter.
        /// </summary>
        /// <param name="propertyId">Property identifier conventionalized by for the Expression Builder.</param>
        /// <param name="operation">Express the interaction between the property and the constant value.</param>
        /// <param name="connector">Establishes how this filter statement will connect to the next one.</param>
        /// <returns>A FilterStatementConnection object that defines how this statement will be connected to the next one.</returns>
        IFilterStatementConnection By(string propertyId, Operation operation, FilterStatementConnector connector = FilterStatementConnector.And);
        /// <summary>
        /// Adds another statement to this filter.
        /// </summary>
        /// <param name="propertyId">Name of the property that will be filtered.</param>
        /// <param name="operation">Express the interaction between the property and the constant value.</param>
        /// <param name="value">Constant value that will interact with the property, required by operations that demands one value or more.</param>
        /// <param name="value2">Constant value that will interact with the property, required by operations that demands two values.</param>
        /// <param name="connector">Establishes how this filter statement will connect to the next one.</param>
        /// <returns>A FilterStatementConnection object that defines how this statement will be connected to the next one.</returns>
        IFilterStatementConnection By<TPropertyType>(string propertyId, Operation operation, TPropertyType value, TPropertyType value2 = default(TPropertyType), FilterStatementConnector connector = FilterStatementConnector.And);
		/// <summary>
		/// Removes all statements from this filter.
		/// </summary>
		void Clear();
    }
}