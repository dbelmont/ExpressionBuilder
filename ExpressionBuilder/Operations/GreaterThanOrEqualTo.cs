using System;
using System.Linq.Expressions;
using ExpressionBuilder.Common;

namespace ExpressionBuilder.Operations
{
    /// <summary>
    /// Operation representing an "greater than or equal" comparison.
    /// </summary>
    public class GreaterThanOrEqualTo : Operation
    {
        /// <inheritdoc />
        public GreaterThanOrEqualTo()
            : base("GreaterThanOrEqualTo", 1, TypeGroup.Number | TypeGroup.Date) { }

        /// <inheritdoc />
        public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2)
        {
            return Expression.GreaterThanOrEqual(member, constant1);
        }
    }
}