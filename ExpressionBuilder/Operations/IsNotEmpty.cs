using ExpressionBuilder.Common;
using System.Linq.Expressions;

namespace ExpressionBuilder.Operations
{
    /// <summary>
    /// Operation representing a check for a non-empty string.
    /// </summary>
    public class IsNotEmpty : OperationBase
    {
        /// <inheritdoc />
        public IsNotEmpty()
            : base("IsNotEmpty", 0, TypeGroup.Text) { }

        /// <inheritdoc />
        public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2)
        {
            return Expression.NotEqual(member.TrimToLower(), Expression.Constant(string.Empty))
                   .AddNullCheck(member);
        }
    }
}