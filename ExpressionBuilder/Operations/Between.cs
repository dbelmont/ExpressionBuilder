using System;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using ExpressionBuilder.Common;

namespace ExpressionBuilder.Operations
{
    /// <summary>
    /// Operation representing a range comparison.
    /// </summary>
    public class Between : Operation
    {
        /// <inheritdoc />
        public Between()
            : base("Between", 1, TypeGroup.Number | TypeGroup.Date) { }

        /// <inheritdoc />
        public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2)
        {
            var left = Expression.GreaterThanOrEqual(member, constant1);
            var right = Expression.LessThanOrEqual(member, constant1);

            return Expression.AndAlso(left, right);
        }
    }
}