using System;

namespace ExpressionBuilder.Common
{
    /// <summary>
    /// Defines how the filter statements will be connected to each other.
    /// </summary>
    public enum Connector
    {
        /// <summary>
        /// Determines that both the current AND the next filter statement needs to be satisfied.
        /// </summary>
        And,

        /// <summary>
        /// Determines that the current OR the next filter statement needs to be satisfied.
        /// </summary>
        Or
    }

    /// <summary>
    /// Groups types into simple groups and map the supported operations to each group.
    /// </summary>
    [Flags]
    public enum TypeGroup
    {
        /// <summary>
        /// Default type group, only supports EqualTo and NotEqualTo.
        /// </summary>
        Default = -1,

        /// <summary>
        /// Supports all text related operations.
        /// </summary>
        Text = 1,

        /// <summary>
        /// Supports all numeric related operations.
        /// </summary>
        Number = 2,

        /// <summary>
        /// Supports boolean related operations.
        /// </summary>
        Boolean = 4,

        /// <summary>
        /// Supports all date related operations.
        /// </summary>
        Date = 8,

        /// <summary>
        /// Supports nullable related operations.
        /// </summary>
        Nullable = 16
    }
}