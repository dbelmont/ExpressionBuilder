using System;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionBuilder.Common
{
    public static class CommonExtensionMethods
    {
        public static readonly MethodInfo trimMethod = typeof(string).GetMethod("Trim", new Type[0]);
        public static readonly MethodInfo toLowerMethod = typeof(string).GetMethod("ToLower", new Type[0]);

        /// <summary>
        /// Applies the string Trim and ToLower methods to an ExpressionMember.
        /// </summary>
        /// <param name="member">Member to which to methods will be applied.</param>
        /// <returns></returns>
        public static Expression TrimToLower(this MemberExpression member)
        {
            var trimMemberCall = Expression.Call(member, trimMethod);
            return Expression.Call(trimMemberCall, toLowerMethod);
        }

        /// <summary>
        /// Adds a "null check" to the expression (before the original one).
        /// </summary>
        /// <param name="expression">Expression to which the null check will be pre-pended.</param>
        /// <param name="member">Member that will be checked.</param>
        /// <returns></returns>
        public static Expression AddNullCheck(this Expression expression, MemberExpression member)
        {
            Expression memberIsNotNull = Expression.NotEqual(member, Expression.Constant(null));
            return Expression.AndAlso(memberIsNotNull, expression);
        }
    }
}