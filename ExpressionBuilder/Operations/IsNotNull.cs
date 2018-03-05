using System.Linq.Expressions;
using ExpressionBuilder.Common;

namespace ExpressionBuilder.Operations
{
    /// <summary>
    /// Operation representing a "not-null" check.
    /// </summary>
    public class IsNotNull : Operation
    {
        /// <inheritdoc />
        public IsNotNull()
            : base("IsNotNull", 1, TypeGroup.Text | TypeGroup.Nullable, expectNullValues: true) { }

        /// <inheritdoc />
        public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2)
        {
            return Expression.NotEqual(member, Expression.Constant(null));
        }
    }
}