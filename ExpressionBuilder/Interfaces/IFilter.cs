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
		/// Adds another statement to this filter.
		/// </summary>
		/// <param name="propertyName">Name of the property that will be filtered.</param>
		/// <param name="operation">Express the interaction between the property and the constant value.</param>
		/// <param name="value">Constant value that will interact with the property, required by operations that demands one value or more.</param>
        /// <param name="value2">Constant value that will interact with the property, required by operations that demands two values.</param>
		/// <param name="connector">Establishes how this filter statement will connect to the next one.</param>
		/// <returns>A FilterStatementConnection object that defines how this statement will be connected to the next one.</returns>
		IFilterStatementConnection By<TPropertyType>(string propertyName, Operation operation, TPropertyType value, TPropertyType value2 = default(TPropertyType), FilterStatementConnector connector = FilterStatementConnector.And);
		/// <summary>
		/// Removes all statements from this filter.
		/// </summary>
		void Clear();
    }
}