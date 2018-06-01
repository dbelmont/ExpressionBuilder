using System;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionBuilder.Common
{
    public static class CommonExtensionMethods
    {
        private static readonly MethodInfo trimMethod = typeof(string).GetMethod("Trim", new Type[0]);
        private static readonly MethodInfo toLowerMethod = typeof(string).GetMethod("ToLower", new Type[0]);

        /// <summary>
        /// Gets a member expression for an specific property
        /// </summary>
        /// <param name="param"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static MemberExpression GetMemberExpression(this ParameterExpression param, string propertyName)
        {
            return GetMemberExpression((Expression)param, propertyName);
        }

        private static MemberExpression GetMemberExpression(Expression param, string propertyName)
        {
            if (!propertyName.Contains("."))
            {
                return Expression.PropertyOrField(param, propertyName);
            }

            var index = propertyName.IndexOf(".");
            var subParam = Expression.PropertyOrField(param, propertyName.Substring(0, index));
            return GetMemberExpression(subParam, propertyName.Substring(index + 1));
        }

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
        /// Applies the string Trim and ToLower methods to an ExpressionMember.
        /// </summary>
        /// <param name="constant">Constant to which to methods will be applied.</param>
        /// <returns></returns>
        public static Expression TrimToLower(this ConstantExpression constant)
        {
            var trimMemberCall = Expression.Call(constant, trimMethod);
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

        /// <summary>
        /// Checks if an object is a generic list.
        /// </summary>
        /// <param name="o">Object to be tested.</param>
        /// <returns>TRUE if the object is a generic list.</returns>
        public static bool IsGenericList(this object o)
        {
            var oType = o.GetType();
            return (oType.IsGenericType && (oType.GetGenericTypeDefinition() == typeof(System.Collections.Generic.List<>)));
        }
    }
}