namespace ExpressionBuilder.Interfaces
{
    /// <summary>
    /// Connects to FilterStatement together.
    /// </summary>
	public interface IFilterStatementConnection
	{
		/// <summary>
		/// Defines that the last filter statement will connect to the next one using the 'AND' logical operator.
		/// </summary>
        IFilter And { get; }
        /// <summary>
        /// Defines that the last filter statement will connect to the next one using the 'OR' logical operator.
        /// </summary>
        IFilter Or { get; }
	}
}