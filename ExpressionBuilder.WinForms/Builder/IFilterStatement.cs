using System;

namespace ExpressionBuilder.Builder
{
	/// <summary>
	/// Defines how a property should be filtered.
	/// </summary>
	public interface IFilterStatement
	{
		/// <summary>
		/// Establishes how this filter statement will connect to the next one. 
		/// </summary>
		FilterStatementConnector Connector { get; set; }
		/// <summary>
		/// Name of the property (or property chain).
		/// </summary>
		string PropertyName { get; set; }
		/// <summary>
		/// Express the interaction between the property and the constant value defined in this filter statement.
		/// </summary>
		Operation Operation { get; set; }
		/// <summary>
		/// Constant value that will interact with the property defined in this filter statement.
		/// </summary>
		object Value { get; set; }
	}
}