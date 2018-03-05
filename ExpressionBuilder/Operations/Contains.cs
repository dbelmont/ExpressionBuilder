using System;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using ExpressionBuilder.Common;

namespace ExpressionBuilder.Operations
{
    /// <summary>
    /// Operation representing a string "Contains" method call.
    /// </summary>
    public class Contains : Operation
    {
        readonly MethodInfo stringContainsMethod = typeof(string).GetMethod("Contains");

        /// <inheritdoc />
        public Contains()
            : base("Contains", 1, TypeGroup.Text) { }

        /// <inheritdoc />
        public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2)
        {
            return Expression.Call(member.TrimToLower(), stringContainsMethod, constant1)
                   .AddNullCheck(member);
        }
    }
}