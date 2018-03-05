using System.Linq.Expressions;
using ExpressionBuilder.Common;

namespace ExpressionBuilder.Operations
{
    /// <summary>
    /// Operation representing an inequality comparison.
    /// </summary>
    public class NotEqualTo : Operation
    {
        /// <inheritdoc />
        public NotEqualTo()
            : base("NotEqualTo", 1, TypeGroup.Default | TypeGroup.Boolean | TypeGroup.Date | TypeGroup.Number | TypeGroup.Text) { }

        /// <inheritdoc />
        public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2)
        {
            if (member.Type == typeof(string))
            {
                return Expression.NotEqual(member.TrimToLower(), constant1)
                       .AddNullCheck(member);
            }

            return Expression.NotEqual(member, constant1);
        }
    }
}