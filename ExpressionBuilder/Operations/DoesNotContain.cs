using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using ExpressionBuilder.Common;

namespace ExpressionBuilder.Operations
{
    /// <summary>
    /// Operation that checks for the non-existence of a substring within another string.
    /// </summary>
    public class DoesNotContain : Operation
    {
        readonly MethodInfo stringContainsMethod = typeof(string).GetMethod("Contains");

        /// <inheritdoc />
        public DoesNotContain()
            : base("DoesNotContain", 1, TypeGroup.Text) { }

        /// <inheritdoc />
        public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2)
        {
            return Expression.Not(Expression.Call(member.TrimToLower(), stringContainsMethod, constant1))
                   .AddNullCheck(member);
        }
    }
}