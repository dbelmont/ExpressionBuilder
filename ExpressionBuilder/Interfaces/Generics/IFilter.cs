using System.Collections.Generic;
using ExpressionBuilder.Builders;
using System.Xml;

namespace ExpressionBuilder.Interfaces.Generics
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
		/// <param name="value">Constant value that will interact with the property.</param>
		/// <param name="connector">Establishes how this filter statement will connect to the next one.</param>
		/// <returns>A FilterStatementConnection object that defines how this statement will be connected to the next one.</returns>
		IFilterStatementConnection By<TPropertyType>(string propertyName, Operation operation, TPropertyType value, FilterStatementConnector connector = FilterStatementConnector.And);
		/// <summary>
		/// Removes all statements from this filter.
		/// </summary>
		void Clear();
    }
}