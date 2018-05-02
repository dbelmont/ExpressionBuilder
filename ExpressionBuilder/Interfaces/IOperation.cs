using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using ExpressionBuilder.Common;

namespace ExpressionBuilder.Interfaces
{
    /// <summary>
    /// Define an operation.
    /// </summary>
    public interface IOperation
    {
        /// <summary>
        /// The operations name (works as an identifier as well).
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Type group were the operation fall into.
        /// </summary>
        TypeGroup TypeGroup { get; }

        /// <summary>
        /// Number of values supported by the operation.
        /// </summary>
        [Range(0, 2)]
        int NumberOfValues { get; }

        /// <summary>
        /// Determines if the operation is active (default is true).
        /// </summary>
        bool Active { get; set; }

        /// <summary>
        /// Determines if the operations supports lists.
        /// </summary>
        bool SupportsLists { get; }

        /// <summary>
        /// Determines if this operation expects nulls to happen within its values.
        /// </summary>
        bool ExpectNullValues { get; }

        /// <summary>
        /// Returns the expression generated through this operation.
        /// </summary>
        /// <param name="member">Member access expression</param>
        /// <param name="constant1">The operation's first constant value.</param>
        /// <param name="constant2">The operation's second constant value.</param>
        /// <returns></returns>
        Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2);

        /// <summary>
        /// Returns a string representation of the operation.
        /// </summary>
        string ToString();
    }
}