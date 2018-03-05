using System.Linq.Expressions;
using ExpressionBuilder.Common;

namespace ExpressionBuilder.Operations
{
    /// <summary>
    /// Operation representing a null check.
    /// </summary>
    public class IsNull : Operation
    {
        /// <inheritdoc />
        public IsNull()
            : base("IsNull", 1, TypeGroup.Text | TypeGroup.Nullable, expectNullValues: true) { }

        /// <inheritdoc />
        public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2)
        {
            return Expression.Equal(member, Expression.Constant(null));
        }
    }
}