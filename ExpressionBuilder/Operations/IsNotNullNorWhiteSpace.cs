using System;
using System.Linq.Expressions;
using System.Reflection;
using ExpressionBuilder.Common;

namespace ExpressionBuilder.Operations
{
    /// <summary>
    /// Operation representing a "not null nor whitespace" check.
    /// </summary>
    public class IsNotNullNorWhiteSpace : Operation
    {
        public readonly MethodInfo trimMethod = typeof(string).GetMethod("Trim", new Type[0]);

        /// <inheritdoc />
        public IsNotNullNorWhiteSpace()
            : base("IsNotNullNorWhiteSpace", 1, TypeGroup.Text, expectNullValues: true) { }

        /// <inheritdoc />
        public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2)
        {
            Expression exprNull = Expression.Constant(null);
            var trimMemberCall = Expression.Call(member, trimMethod);
            Expression exprEmpty = Expression.Constant(string.Empty);
            return Expression.AndAlso(
                Expression.NotEqual(member, exprNull),
                Expression.NotEqual(trimMemberCall, exprEmpty));
        }
    }
}